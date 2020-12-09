<?php

namespace App\Middleware;
use Slim\Http\Request;
use Slim\Http\Response;
use Slim\App;
use App\Common\ApiException;
use Psr\Log\LoggerInterface;
use App\Repository\EmployeesRepository;
use App\Models\EmployeeHire;
use App\Models\Employee;
use App\Models\Company;

use JsonMapper;

/**
 * Description of HireValidator
 *
 * @author slavko
 */
class HireValidator {
    /**
     * @var Slim\App
     */
    private $app;    
    private $settings;
    protected $logger;
    protected $repository;
    protected $mapper;

    public function __construct($settings, LoggerInterface $logger, EmployeesRepository $repository) {
        //$this->app = $app;
        $this->settings = $settings;
        $this->logger = $logger;
        $this->repository = $repository;
        $this->mapper = new JsonMapper();
        $this->mapper->setLogger($logger);
        $this->mapper->bStrictNullTypes = FALSE;
    }
    
    public function __invoke(Request $request, Response $response, callable $next)
    {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}            

        try {
            $requestjson = json_decode($request->getBody());       
            $employee = new EmployeeHire();  
            $this->mapper->map($requestjson, $employee);

            $employee->setEmployeeId($requestjson->{"uuid"});
            $employee->getHireemployer()->setCompanyId($requestjson->{"hireemployer"}->{'uuid'});
                        
            $state = $this->repository->getCurrentWorkPeriod($employee->getUuid(), 1);
            
            $current = $state->getCurrentPeriod();
            $last = $state->getLastPeriod();
            
            if($current !== NULL){
                $request = $request->withAttribute('currentperiod', $current->getUuid());
            }
            
            if ($current === NULL) {
                $current = $last;
            }
            
            if ($current === NULL) {
                return $next($request, $response); 
            }

            if($current->getEnd() === NULL && $current->getCompany()->getUuid() !== $employee->getHireemployer()->getUuid())
            {
                    throw ApiException::serverError("Zaposleni je je še zmeraj prijavljen na podjetju "
                            . "" . $current->getCompany()->getShortname() ."prijavljen je od "
                            . "" . $current->getStartDate()->format("d.m.Y") ." prijava na dan "
                            . "" . $employee->getDateHire()->format("d.m.Y") ." ni možna!");
            }

            if($current->getEndDate() > $employee->getDateHire()){
                throw ApiException::serverError("Zaposleni je bil na podjetju "
                    . "" . $current->getCompany()->getShortname() ." prijavljen do "
                    . "" . $current->getEndDate()->format("d.m.Y") . ". Prijava na podjetju "
                    . "" . $employee->getHireemployer()->getShortname() . " z dnem "
                    . "" . $employee->getDateHire()->format("d.m.Y") . " ni možna!");
            }
   
            if($last !== NULL && $last->getEndDate() > $employee->getDateHire()){
                throw ApiException::serverError("Zaposleni je bil na podjetju "
                    . "" . $current->getCompany()->getShortname() ." prijavljen do "
                    . "" . $current->getEndDate()->format("d.m.Y") . ". Prijava na podjetju "
                    . "" . $employee->getHireemployer()->getShortname() . " z dnem "
                    . "" . $employee->getDateHire()->format("d.m.Y") . " ni možna!");
            }     
            
            $docs = $this->repository->getPromissoryNotes($employee->getUuid());
            if(!empty($docs)){
                
                foreach ($docs as $docs) {
                    $msg .= $docs->getName().PHP_EOL;
                }
                throw ApiException::serverError("Zaposleni mora razdolžiti opremo:". PHP_EOL . $msg);
            }            
            return $next($request, $response); 
        } 
        catch (ApiException $exc) {
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {

            var_dump($exc->getMessage());
            die;
            return $next($request, $response);
        }
    }
}

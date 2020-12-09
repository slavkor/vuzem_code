<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Middleware;
use Slim\Http\Request;
use Slim\Http\Response;
use Slim\App;
use App\Common\ApiException;
use Psr\Log\LoggerInterface;
use App\Repository\DepartureArrivalRepository;
use App\Models\Departure;

use JsonMapper;

class DepartureValidator {
    /**
     * @var Slim\App
     */
    private $app;
    private $settings;
    protected $logger;
    protected $repository;
    protected $mapper;

    public function __construct($settings, LoggerInterface $logger, DepartureArrivalRepository $repository) {
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
            $unvalids = $this->repository->getUnvalidInDeparture($request, $args);

            if(NULL === $unvalids){
                return $next($request, $response); 
            }
            if(is_array($unvalids)){
                if(count($unvalids) === 0){
                    return $next($request, $response); 
                }
            }
     
            $msg = "Odhoda ni možno potrditi.". PHP_EOL ." Zaposleni še niso na startu:". PHP_EOL;
            foreach ($unvalids as $emp) {
                $occ = $emp->getLastOccupancy()->getOccupancy()->getCompany() === NULL ? $emp->getLastOccupancy()->getOccupancy()->getProject()->getSite()->getCustomer()->getname()
                        . " " . $emp->getLastOccupancy()->getOccupancy()->getProject()->getSite()->getname() 
                        . " " . $emp->getLastOccupancy()->getOccupancy()->getProject()->getname() 
                        : $emp->getLastOccupancy()->getOccupancy()->getCompany()->getName();
                $msg .= $emp->getlastname() . " " . $emp->getname() ." (". $occ .")" . PHP_EOL;
            }
            throw ApiException::serverError($msg);
            
            
        } 
        catch (ApiException $exc) {
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(json_encode(array(
                'error' => array(
                    'msg' => $exc->getMessage(),
                    'code' => $exc->getCode(),
                ),
            )), 500);
        }
    }
}

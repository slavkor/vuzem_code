<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Action;

use Psr\Log\LoggerInterface;
use Slim\Http\Request;
use Slim\Http\Response;
use Slim\App;

use App\Repository\EmployeesRepository;
use App\Common\ApiException;
final class EmployeesAction {
    /**
     * @var Slim\App
     */
    private $app;

    /**
     * @var \Psr\Log\LoggerInterface
     */
    private $logger;

    /**
     * @var \App\Repository\EmployeesRepository
     */
    private $repository;

    /**
     * @param \Psr\Log\LoggerInterface       $logger
     * @param \App\Factory\NoteFactory       $factory
     * @param \App\Repository\EmployeesRepository $repository
     */
//    public function __construct(App $app, LoggerInterface $logger,  EmployeesRepository $repository)
    public function __construct(LoggerInterface $logger,  EmployeesRepository $repository)
    {
        //$this->app = $app;
        $this->logger = $logger;
        $this->repository = $repository;
    }  
    
    public function getActivecount(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->getActivecount($request, $args), 200, JSON_PRETTY_PRINT);  
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }    
    public function getByUser(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->getByUser($args['username']), 200, JSON_PRETTY_PRINT);
            
            
            
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    
    public function addEmployee(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->addEmployee($request, $args));        
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    public function hireEmployee(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->hireEmployee($request,$args));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    public function fireEmployee(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->fireEmployee($request, $args));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    public function getWorkPeriods(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

            $firmid = NULL;
            if(array_key_exists('firmid', $args))
            {
                $firmid = $args['firmid'];
            }
            if (NULL === $firmid) {
                throw ApiException::serverError('No Company provided');
            }

            return $response->withJson($this->repository->getEmployeeWorkPeriods($firmid, $args['id']));
       
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }    
    public function getCurrentWorkPeriod(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            
            return $response->withJson($this->repository->getCurrentWorkPeriod($args['id'], 1));
       
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }    
    public function getCoutryDuration(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            
            return $response->withJson($this->repository->getCoutryDuration($request, $args));
       
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }   
    public function getAbsentEmployees(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            
            return $response->withJson($this->repository->getAbsentEmployees($request, $args));
       
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }       
    
    public function getCoutrySiteDuration(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            
            return $response->withJson($this->repository->getCoutrySiteDuration($request, $args));
       
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }         
    
    public function updateEmployee(Request $request, Response $response, $args){
        try {
            
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            $jsonData = json_decode($request->getBody());
            if (json_last_error() !== JSON_ERROR_NONE) {
                throw ApiException::json_decodeError();
            }
            if (NULL === $jsonData) {
                throw ApiException::serverError('No data povided');
            }
            return $response->withJson($this->repository->updateEmployee($jsonData));        
            //return $response->withJson($this->repository->updateEmployee($request));        
        } catch (ApiException $exc){
            $this->logger->error($exc->getMessage());
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            $this->logger->error($exc->getMessage());
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    public function addLoaner(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            $jsonData = json_decode($request->getBody());
            if (json_last_error() !== JSON_ERROR_NONE) {
                throw ApiException::json_decodeError();
            }
            if (NULL === $jsonData) {
                throw ApiException::serverError('No data povided');
            }
            return $response->withJson($this->repository->addLoaner($request, $args));        
        } catch (ApiException $exc){
            $this->logger->error($exc->getMessage());
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            $this->logger->error($exc->getMessage());
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    public function deleteLoaner(Request $request, Response $response, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        try {
            return $response->withJson($this->repository->deleteLoaner($request, $args));        
        } catch (ApiException $exc){
            $this->logger->error($exc->getMessage());
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            $this->logger->error($exc->getMessage());
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    public function workplace(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->workplace($request, $args));        
        } catch (ApiException $exc){
            $this->logger->error($exc->getMessage());
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            $this->logger->error($exc->getMessage());
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }    
    
    public function getEmployed(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            
            return $response->withJson($this->repository->getEmployed($args['firmid']), 200, JSON_PRETTY_PRINT);
            
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    public function getLoaned(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            
            return $response->withJson($this->repository->getLoaned($args['firmid']), 200, JSON_PRETTY_PRINT);
            
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }    
    
    public function getFired(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            
            return $response->withJson($this->repository->getFired($args['firmid']), 200, JSON_PRETTY_PRINT);
            
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }    
        
    
    public function getAllEmployees(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            $firmid = NULL;
            if(array_key_exists('firmid', $args))
            {
                $firmid = $args['firmid'];
            }
            
            $date = $request->getParam("date", NULL);
            

            
            if(NULL !== $date){

                return $response->withJson($this->repository->getEmployeesForEmployerOnDate($firmid, $date), 200, JSON_PRETTY_PRINT);
            }

            if(NULL !== $firmid)
            {
                return $response->withJson($this->repository->getEmployeesForEmployer($firmid), 200, JSON_PRETTY_PRINT);
            }
            else
            {
                return $response->withJson($this->repository->getAllEmployees(), 200, JSON_PRETTY_PRINT);
            }
            return $response;   
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }

    public function getAllEmployees2(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            
            return $response->withJson($this->repository->getAllEmployees2($request, $args), 200, JSON_PRETTY_PRINT);
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    
    public function getAllEmployeesNotRented(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
//            $firmid = NULL;
//            if(array_key_exists('firmid', $args))
//            {
//                $firmid = $args['firmid'];
//            }
//            
//            $date = $request->getParam("date", NULL);
//            
//
//            
//            if(NULL !== $date){
//
//                return $response->withJson($this->repository->getEmployeesForEmployerOnDate($firmid, $date), 200, JSON_PRETTY_PRINT);
//            }
            return $response->withJson($this->repository->getEmployeesForEmployerNotRented($args['firmid']), 200, JSON_PRETTY_PRINT);

        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    public function getRentedEmployees(Request $request, Response $response, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        try {
            return $response->withJson($this->repository->getRentedEmployees($request, $args));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }

    public function startOccupancy(Request $request, Response $response, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        try {
            return $response->withJson($this->repository->startOccupancy($request, $args));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    
    public function endOccupancy(Request $request, Response $response, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        try {
            return $response->withJson($this->repository->endOccupancy($request, $args));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
        
    public function getEmployeesWithHistory(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

            return $response->withJson($this->repository->getEmployeesWithHistory($request, $args), 200, JSON_PRETTY_PRINT);
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    public function getEmployeeWithHistory(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

            return $response->withJson($this->repository->getEmployeeWithHistory($request, $args), 200, JSON_PRETTY_PRINT);
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }    
    public function getPodlage(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

            return $response->withJson($this->repository->getPodlage($args['firmid'], $args['year'], $args['month']), 200, JSON_PRETTY_PRINT);
             
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    public function getHiredFiredEmployees(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

            return $response->withJson($this->repository->getHiredFiredEmployees($args['firmid'], $args['year'], $args['month']), 200, JSON_PRETTY_PRINT);
             
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }    
    public function getStateHiredFiredEmployees(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

            return $response->withJson($this->repository->getStateHiredFiredEmployees($args['firmid'], $args['year'], $args['month']), 200, JSON_PRETTY_PRINT);
             
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }      
    
    public function getHomeEmployees(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->getHomeEmployees(), 200, JSON_PRETTY_PRINT);
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    public function getHomeEmployees2(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

            return $response->withJson($this->repository->getHomeEmployees2(), 200, JSON_PRETTY_PRINT);
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }

    
    public function getHomeEmployeesForReport(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->getHomeEmployeesForReport(), 200, JSON_PRETTY_PRINT);
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }    
    
    public function getHomeEmployeesForReport2(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->getHomeEmployeesForReport2(), 200, JSON_PRETTY_PRINT);
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }        
    
    
    public function getAwayEmployees(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->getAwayEmployees(), 200, JSON_PRETTY_PRINT);
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    } 
    public function getActiveEmployees(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->getActiveEmployees(), 200, JSON_PRETTY_PRINT);
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }     
//    
//    
    public function getDocumnetsToExpire(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->getDocumnetsToExpire($request, $args), 200, JSON_PRETTY_PRINT);
            
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }    
    
    public function addDocument(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            $jsonData = json_decode($request->getBody());   
            if (json_last_error() !== JSON_ERROR_NONE) {
                throw ApiException::json_decodeError();
            }
            $data = $this->repository->addEmployeeDocument($jsonData);
            //$data = $this->repository->addEmployeeDocument($request);
            return $response->withJson($data);
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    public function addAddress(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

            $json = json_decode($request->getBody());
            if (json_last_error() !== JSON_ERROR_NONE) {
                throw ApiException::json_decodeError();
            }

            $employee = $this->repository->addEmployeeAddress($json);
            //$employee = $this->repository->addEmployeeAddress($request);
            return $response->withJson($employee);
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    public function addContact(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            $data = $this->repository->addEmployeeContact(json_decode($request->getBody()));
            //$data = $this->repository->addEmployeeContact($request);
            
            if (json_last_error() !== JSON_ERROR_NONE) {
                throw ApiException::json_decodeError();
            }
            return $response->withJson($data);
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    public function getAddresses(Request $request, Response $response, $args)   {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        try {
            return $response->withJson($this->repository->getAddresses($args['id']), 200, JSON_PRETTY_PRINT);
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    public function getContacts(Request $request, Response $response, $args)   {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        try {
            return $response->withJson($this->repository->getContacts($args['id']));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }        
    }
    public function getContactsEx(Request $request, Response $response, $args)   {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        try {
            return $response->withJson($this->repository->getContactsEx($args['id']));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }        
    }    
    public function getDocuments(Request $request, Response $response, $args)   {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        try {
           
            $firmid = NULL;
            if(array_key_exists('firmid', $args))
            {
                $firmid = $args['firmid'];
            }

            return $response->withJson($this->repository->getDocuments($firmid, $args['id']));
            
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }        
    }  
    public function getDocumentsOfType(Request $request, Response $response, $args)   {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        try {
            return $response->withJson($this->repository->getDocumentsOfType($args["id"], $args["type"]));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }        
    }      
    public function getEmployeesForEmployer(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            if (json_last_error() !== JSON_ERROR_NONE) {
                throw ApiException::json_decodeError();
            }
            $jsonData = json_decode($request->getBody());
            if (NULL === $jsonData) {
                throw ApiException::serverError('No data povided');
            }

            return $response->withJson($this->repository->getAllEmployees());
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }        
    }
    public function updateEmployeeAddress(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            $jsonData = json_decode($request->getBody());
            if (json_last_error() !== JSON_ERROR_NONE) {
                throw ApiException::json_decodeError();
            }
            if (NULL === $jsonData) {
                throw ApiException::serverError('No data povided');
            }
            $employee = $this->repository->updateEmployeeAddress($jsonData);
            //$employee = $this->repository->updateEmployeeAddress($request);
            if (!empty($employee)) {
                $response = $response->withJson($employee);
            } else {
                throw ApiException::serverError('Unable to update address');
            }
            return $response;        
        } catch (ApiException $exc){
            $this->logger->error($exc->getMessage());
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            $this->logger->error($exc->getMessage());
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    public function updateEmployeeContact(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            $jsonData = json_decode($request->getBody());
            if (json_last_error() !== JSON_ERROR_NONE) {
                throw ApiException::json_decodeError();
            }
            if (NULL === $jsonData) {
                throw ApiException::serverError('No data povided');
            }
            $employee = $this->repository->updateEmployeeContact($jsonData);
            //$employee = $this->repository->updateEmployeeContact($request);
            if (!empty($employee)) {
                $response = $response->withJson($employee);
            } else {
                throw ApiException::serverError('Unable to update contact');
            }
            return $response;        
        } catch (ApiException $exc){
            $this->logger->error($exc->getMessage());
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            $this->logger->error($exc->getMessage());
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }   
    public function deleteEmployee(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            $jsonData = json_decode($request->getBody());
            if (json_last_error() !== JSON_ERROR_NONE) {
                throw ApiException::json_decodeError();
            }
            if (NULL === $jsonData) {
                throw ApiException::serverError('No data povided');
            }
            
            return $response->withJson($this->repository->deleteEmployee($jsonData));
            //return $response->withJson($this->repository->deleteEmployee($request));
        } catch (ApiException $exc){
            $this->logger->error($exc->getMessage());
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            $this->logger->error($exc->getMessage());
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    
    }
    public function deleteEmployeeAddress(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            $jsonData = json_decode($request->getBody());
            if (json_last_error() !== JSON_ERROR_NONE) {
                throw ApiException::json_decodeError();
            }
            if (NULL === $jsonData) {
                throw ApiException::serverError('No data povided');
            }
            $employee = $this->repository->deleteEmployeeAddress($jsonData);
            //$employee = $this->repository->deleteEmployeeAddress($request);
            if (!empty($employee)) {
                $response = $response->withJson($employee);
            } else {
                throw ApiException::serverError('Unable to delete employee');
            }
            return $response;        
        } catch (ApiException $exc){
            $this->logger->error($exc->getMessage());
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            $this->logger->error($exc->getMessage());
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    public function deleteEmployeeContact(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            $jsonData = json_decode($request->getBody());
            if (json_last_error() !== JSON_ERROR_NONE) {
                throw ApiException::json_decodeError();
            }
            if (NULL === $jsonData) {
                throw ApiException::serverError('No data povided');
            }
            $employee = $this->repository->deleteEmployeeContact($jsonData);
            //$employee = $this->repository->deleteEmployeeContact($request);
            if (!empty($employee)) {
                $response = $response->withJson($employee);
            } else {
                throw ApiException::serverError('Unable to delete employee');
            }
            return $response;        
        } catch (ApiException $exc){
            $this->logger->error($exc->getMessage());
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            $this->logger->error($exc->getMessage());
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }    
    public function deleteEmployeeDocument(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            $jsonData = json_decode($request->getBody());
            if (json_last_error() !== JSON_ERROR_NONE) {
                throw ApiException::json_decodeError();
            }
            if (NULL === $jsonData) {
                throw ApiException::serverError('No data povided');
            }
            $employee = $this->repository->deleteEmployeeDocument($jsonData);
            //$employee = $this->repository->deleteEmployeeDocument($request);
            if (!empty($employee)) {
                $response = $response->withJson($employee);
            } else {
                throw ApiException::serverError('Unable to delete employee');
            }
            return $response;        
        } catch (ApiException $exc){
            $this->logger->error($exc->getMessage());
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            $this->logger->error($exc->getMessage());
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }   
    public function getSpokenLanguage(Request $request, Response $response, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        try {
            return $response->withJson($this->repository->getSpokenLanguage($args['id']));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }    
    } 
    public function getHireHistory(Request $request, Response $response, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        try {
            return $response->withJson($this->repository->getHireHistory($args['id']));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }    
    }    
    public function getWorkHistory(Request $request, Response $response, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        try {
            return $response->withJson($this->repository->getWorkHistory($args['id']));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }    
    }        
    public function addSpokenLanguage(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

            $json = json_decode($request->getBody());
            if (json_last_error() !== JSON_ERROR_NONE) {
                throw ApiException::json_decodeError();
            }
            return $response->withJson($this->repository->addSpokenLanguage($args['id'],$json));
            //return $response->withJson($this->repository->addSpokenLanguage($request));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    } 
    public function deleteSpokenLanguage(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

            $json = json_decode($request->getBody());
            if (json_last_error() !== JSON_ERROR_NONE) {
                throw ApiException::json_decodeError();
            }
            return $response->withJson($this->repository->deleteSpokenLanguage($args['id'],$json));
            //return $response->withJson($this->repository->deleteSpokenLanguage($request));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }  
    
    public function addAbsence(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->addAbsence($request, $args));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }  
    public function updateAbsence(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->updateAbsence($request, $args));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }  
    public function deleteAbsence(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->deleteAbsence($request, $args));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    } 
    public function listAbsence(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->listAbsence($request, $args));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }    
    public function getEmployeeByUuid(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->getEmployeeByUuid($args['id']));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }        
}

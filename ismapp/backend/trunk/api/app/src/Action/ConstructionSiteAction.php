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

use App\Repository\ConstructionSiteRepository;
use App\Common\ApiException;

/**
 * Description of ConstructionSiteAction
 *
 * @author slavko
 */
class ConstructionSiteAction {
    /**
     * @var Slim\App
     */
    private $app;

    /**
     * @var \Psr\Log\LoggerInterface
     */
    private $logger;

    /**
     * @var \App\Repository\ConstructionSiteRepository
     */
    private $repository;

    /**
     * @param \Psr\Log\LoggerInterface       $logger
     * @param \App\Factory\NoteFactory       $factory
     * @param \App\Repository\ConstructionSiteRepository $repository
     */
    //public function __construct(App $app, LoggerInterface $logger,  ConstructionSiteRepository $repository)
    public function __construct(LoggerInterface $logger,  ConstructionSiteRepository $repository)
    {
        //$this->app = $app;
        $this->logger = $logger;
        $this->repository = $repository;
    }
    
    // <editor-fold defaultstate="collapsed" desc="Construction site related ">
    public function getAllSites(Request $request, Response $response, $args) {
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            
            return($response->withJson($this->repository->getAllSites()));
            
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    public function getByEmployee(Request $request, Response $response, $args) {
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            
            return($response->withJson($this->repository->getByEmployee($args["id"])));
            
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    public function getWorkShiftsByDay(Request $request, Response $response, $args) {
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            $jsonData = json_decode($request->getBody());   
            if (json_last_error() !== JSON_ERROR_NONE) {
                throw ApiException::json_decodeError();
            }
            return $response->withJson($this->repository->getWorkShiftsByDay($args['id'], $jsonData));
            
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    public function getWorkShifts(Request $request, Response $response, $args) {
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

            return $response->withJson($this->repository->getWorkShifts($args['id']));
            
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }    
    public function getAllCompanySites(Request $request, Response $response, $args) {
        
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

            $jsonData = json_decode($request->getBody());
            if (json_last_error() !== JSON_ERROR_NONE) {
                throw ApiException::json_decodeError();
            }
            if (NULL === $jsonData) {
                throw ApiException::serverError('No data povided');
            }            
     
            return($response->withJson($this->repository->getAllCompanySites($args['firmid'], $jsonData)));
            
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }                
    }
    public function addCSite(Request $request, Response $response, $args) {
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            $jsonData = json_decode($request->getBody());   
            if (json_last_error() !== JSON_ERROR_NONE) {
                throw ApiException::json_decodeError();
            }
         
            return $response->withJson($this->repository->addCSide($args['firmid'], $jsonData));
            
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    public function updateCSite(Request $request, Response $response, $args) {
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            $jsonData = json_decode($request->getBody());   
            if (json_last_error() !== JSON_ERROR_NONE) {
                throw ApiException::json_decodeError();
            }
            return $response->withJson($this->repository->updateCSite($jsonData));
            
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }    
    public function getActiveSites(Request $request, Response $response, $args) {
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->getActiveSites($request, $args));
            
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }    
    
    public function getActiveSitesOverview(Request $request, Response $response, $args) {
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->getActiveSitesOverview($request, $args));
            
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }    
        
    
    // </editor-fold>

    // <editor-fold defaultstate="collapsed" desc="Documents related">
    public function getDocuments(Request $request, Response $response, $args) {
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

            return $response->withJson($this->repository->getDocuments($args['id']));
            
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
            $data = $this->repository->addDocument($jsonData);
            return $response->withJson($data);
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }  
    public function deleteDocument(Request $request, Response $response, $args){
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
    
    // </editor-fold>

    // <editor-fold defaultstate="collapsed" desc="Customer related">
    public function addCSiteCustomer(Request $request, Response $response, $args) {
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            $jsonData = json_decode($request->getBody());   
            if (json_last_error() !== JSON_ERROR_NONE) {
                throw ApiException::json_decodeError();
            }
         
            return $response->withJson($this->repository->addCSide($args['firmid'], $jsonData));
            
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }    
    // </editor-fold>

    // <editor-fold defaultstate="collapsed" desc="Address related">
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
    public function addAddress(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

            $json = json_decode($request->getBody());
            if (json_last_error() !== JSON_ERROR_NONE) {
                throw ApiException::json_decodeError();
            }

            $employee = $this->repository->addEmployeeAddress($json);
            return $response->withJson($employee);
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }    
    public function updateAddress(Request $request, Response $response, $args){
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
    public function deleteAddress(Request $request, Response $response, $args){
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
    // </editor-fold>
    
    // <editor-fold defaultstate="collapsed" desc="Contacts related">
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
    public function addContact(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            $data = $this->repository->addContact(json_decode($request->getBody()));
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
    public function updateContact(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            $jsonData = json_decode($request->getBody());
            if (json_last_error() !== JSON_ERROR_NONE) {
                throw ApiException::json_decodeError();
            }
            if (NULL === $jsonData) {
                throw ApiException::serverError('No data povided');
            }
            $employee = $this->repository->updateContact($jsonData);
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
    public function deleteContact(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            $jsonData = json_decode($request->getBody());
            if (json_last_error() !== JSON_ERROR_NONE) {
                throw ApiException::json_decodeError();
            }
            if (NULL === $jsonData) {
                throw ApiException::serverError('No data povided');
            }
            $employee = $this->repository->deleteContact($jsonData);
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
    // </editor-fold>


    public function getActiveCars(Request $request, Response $response, $args) {
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

            return $response->withJson($this->repository->getActiveCars($args['id']));
            
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }   

    public function getActiveEmployees(Request $request, Response $response, $args) {
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

            return $response->withJson($this->repository->getActiveEmployees($args['id']));
            
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }   
    
    public function getDepartureStats(Request $request, Response $response, $args) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        try {
            return $response->withJson($this->repository->getDepartureStats($request, $args));
            
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }   
        
    
}

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
     
use App\Repository\ProjectRepository;
use App\Common\ApiException;

class ProjectAction {
    /**
     * @var Slim\App
     */
    private $app;
    
    /**
     * @var \Psr\Log\LoggerInterface
     */
    private $logger;

    /**
     * @var \App\Repository\ProjectRepository
     */
    private $repository;

    /**
     * @param \Psr\Log\LoggerInterface       $logger
     * @param \App\Repository\ProjectRepository $repository
     */
//    public function __construct(App $app, LoggerInterface $logger, ProjectRepository $repository)
    public function __construct(LoggerInterface $logger, ProjectRepository $repository)
    {
        //$this->app = $app;
        $this->logger = $logger;
        $this->repository = $repository;
    }
    
    // <editor-fold defaultstate="collapsed" desc="Project related">
    public function getAllProjects(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->getAllProjects());
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    public function getProjects(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->getProjectsByStatus($request, $args));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }

    public function getEmployeeProjects(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->getEmployeeProjectsByStatus($request, $args));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    
    public function getUserSites(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->getUserSites($request, $args));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    
    public function getUserProjects(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->getUserProjects($request, $args));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }        
    
    public function getCsiteProjects(Request $request, Response $response, $args){
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
            return $response->withJson($this->repository->getCsiteProjects($firmid, $args['id']));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    public function addProject(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            $jsonData = json_decode($request->getBody());
            if (json_last_error() !== JSON_ERROR_NONE) {
                throw ApiException::json_decodeError();
            }
            if (NULL === $jsonData) {
                throw ApiException::serverError('No data povided');
            }
            $firmid = NULL;
            if(array_key_exists('firmid', $args))
            {
                $firmid = $args['firmid'];
            }
            if (NULL === $firmid) {
                throw ApiException::serverError('No Company provided');
            }
            return $response->withJson($this->repository->addProject($firmid, $args['id'], $jsonData));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }  
    public function updateProject(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            $jsonData = json_decode($request->getBody());
            if (json_last_error() !== JSON_ERROR_NONE) {
                throw ApiException::json_decodeError();
            }
            if (NULL === $jsonData) {
                throw ApiException::serverError('No data povided');
            }
            return $response->withJson($this->repository->updateProject($jsonData));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    public function deleteProject(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            
            return $response->withJson($this->repository->deleteProject($request, $args));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    
    public function addShift(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            $jsonData = json_decode($request->getBody());
            if (json_last_error() !== JSON_ERROR_NONE) {
                throw ApiException::json_decodeError();
            }
            if (NULL === $jsonData) {
                throw ApiException::serverError('No data povided');
            }
            return $response->withJson($this->repository->addShift($args['id'], $args['eid'], $jsonData));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }  
    public function deleteShift(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            $jsonData = json_decode($request->getBody());
            if (json_last_error() !== JSON_ERROR_NONE) {
                throw ApiException::json_decodeError();
            }
            if (NULL === $jsonData) {
                throw ApiException::serverError('No data povided');
            }
            return $response->withJson($this->repository->deleteShift($args['id'], $args['eid'], $jsonData));
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
    public function getMonthSummary(Request $request, Response $response, $args) {
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

            return $response->withJson($this->repository->getMonthSummary($args['id'], $args['year'], $args['month']));
            
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }  
    public function getProjectSiteData(Request $request, Response $response, $args) {
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

            return $response->withJson($this->repository->getProjectSiteData($args['id']));
            
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }  
    public function getEwrData(Request $request, Response $response, $args) {
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

            return $response->withJson($this->repository->getEwrData($request, $args));
            
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }  

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
            
            return $response->withJson($this->repository->deleteDocument($jsonData));
            
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
            return $response->withJson($this->repository->addAddress($json));
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
            return $response->withJson($this->repository->updateAddress($jsonData));
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
            return $response->withJson($this->repository->deleteAddress($jsonData));
        } catch (ApiException $exc){
            $this->logger->error($exc->getMessage());
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            $this->logger->error($exc->getMessage());
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    public function bindAddress(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            $jsonData = json_decode($request->getBody());
            if (json_last_error() !== JSON_ERROR_NONE) {
                throw ApiException::json_decodeError();
            }
            if (NULL === $jsonData) {
                throw ApiException::serverError('No data povided');
            }
            return $response->withJson($this->repository->bindAddress($jsonData));
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
            return $response->withJson($this->repository->updateContact($jsonData));
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
            return $response->withJson($this->repository->deleteContact($jsonData));
                    
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

    // <editor-fold defaultstate="collapsed" desc="EWR related">
    public function getAllEwrs(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->getAllEwrs($request, $args));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    public function getProjectEwrs(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->getProjectEwrs($request, $args));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    public function addEwr(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->addEwr($request, $args));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }  
    public function updateEwr(Request $request, Response $response, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        try {
            return $response->withJson($this->repository->updateEwr($request, $args));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    public function addEwrEmployee(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->addEwrEmployee($request, $args));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }  
    public function deleteEwrEmployee(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->deleteEwrEmployee($request, $args));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }  
    public function addEwrDocument(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->addEwrDocument($request, $args));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    public function deleteEwrDocument(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->deleteEwrDocument($request, $args));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    public function getEwrDocuments(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->getEwrDocuments($request, $args));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    
    
    // </editor-fold>

    public function addWp(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->addWp($request, $args));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }  
    
    
    public function deleteWp(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->deleteWp($request, $args));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }  
    
    public function addWpPlan(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->addWpPlan($request, $args));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }  
}

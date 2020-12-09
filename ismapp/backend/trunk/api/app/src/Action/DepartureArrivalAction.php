<?php

namespace App\Action;

use Psr\Log\LoggerInterface;
use Slim\Http\Request;
use Slim\Http\Response;
use Slim\App;

use App\Repository\DepartureArrivalRepository;
use App\Common\ApiException;

class DepartureArrivalAction {
    /**
     * @var Slim\App
     */
    private $app;

    /**
     * @var \Psr\Log\LoggerInterface
     */
    private $logger;

    /**
     * @var \App\Repository\DepartureArrivalRepository 
     */
    private $repository;

    /**
     * @param \Psr\Log\LoggerInterface       $logger
     * @param \App\Repository\DepartureArrivalRepository  $repository
     */
    //public function __construct(App $app, LoggerInterface $logger,  DepartureArrivalRepository $repository)
    public function __construct(LoggerInterface $logger,  DepartureArrivalRepository $repository)
    {
        //$this->app = $app;
        $this->logger = $logger;
        $this->repository = $repository;
    }
    
    // <editor-fold defaultstate="collapsed" desc="Departure related">
    public function listDepartures(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            $jsonData = json_decode($request->getBody());
            if (json_last_error() !== JSON_ERROR_NONE) {
                throw ApiException::json_decodeError();
            }
            if (NULL === $jsonData) {
                throw ApiException::serverError('No data povided');
            }            
            return $response->withJson($this->repository->listDepartures($jsonData));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => $exc->getCode(),'message' => $exc->getMessage()], 500);
        }          
    }
    public function addDeparture(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            $jsonData = json_decode($request->getBody());
            if (json_last_error() !== JSON_ERROR_NONE) {
                throw ApiException::json_decodeError();
            }
            if (NULL === $jsonData) {
                throw ApiException::serverError('No data povided');
            }
            
            return $this->repository->addDeparture($jsonData);
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => $exc->getCode(),'message' => $exc->getMessage()], 500);
        }        
    }

    public function addConfirmDeparture(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            $jsonData = json_decode($request->getBody());
            if (json_last_error() !== JSON_ERROR_NONE) {
                throw ApiException::json_decodeError();
            }
            if (NULL === $jsonData) {
                throw ApiException::serverError('No data povided');
            }
            
            return $this->repository->addConfirmDeparture($jsonData);
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => $exc->getCode(),'message' => $exc->getMessage()], 500);
        }        
    }
    public function updateDeparture(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            $jsonData = json_decode($request->getBody());
            if (json_last_error() !== JSON_ERROR_NONE) {
                throw ApiException::json_decodeError();
            }
            if (NULL === $jsonData) {
                throw ApiException::serverError('No data povided');
            }
            
            return $this->repository->updateDeparture($jsonData);
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => $exc->getCode(),'message' => $exc->getMessage()], 500);
        }        
    }

    public function cancelDeparture(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            $jsonData = json_decode($request->getBody());
            if (json_last_error() !== JSON_ERROR_NONE) {
                throw ApiException::json_decodeError();
            }
            if (NULL === $jsonData) {
                throw ApiException::serverError('No data povided');
            }            
            return $this->repository->cancelDeparture($jsonData);
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => $exc->getCode(),'message' => $exc->getMessage()], 500);
        }        
    }
    
    public function confirmDeparture(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            $jsonData = json_decode($request->getBody());
            if (json_last_error() !== JSON_ERROR_NONE) {
                throw ApiException::json_decodeError();
            }
            if (NULL === $jsonData) {
                throw ApiException::serverError('No data povided');
            }            
            return $this->repository->confirmDeparture($jsonData);
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => $exc->getCode(),'message' => $exc->getMessage()], 500);
        }                
    }

    public function getDepartureReport(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->getDepartureReport($request, $args));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => $exc->getCode(),'message' => $exc->getMessage()], 500);
        }          
    }
    public function getPlanedDeparturesReport(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->getPlanedDeparturesReport($request, $args));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => $exc->getCode(),'message' => $exc->getMessage()], 500);
        }          
    }    

    public function getDeparturesInRangeReport(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->getDeparturesInRangeReport($request, $args));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => $exc->getCode(),'message' => $exc->getMessage()], 500);
        }          
    }    
    
    public function getDeparturesInRangeReport2(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->getDeparturesInRangeReport2($request, $args));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => $exc->getCode(),'message' => $exc->getMessage()], 500);
        }          
    }        

    public function getPlaned(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->getPlaned($request, $args));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => $exc->getCode(),'message' => $exc->getMessage()], 500);
        }          
    }    

    public function getDepartureEmployees(Request $request, Response $response, $args){
//        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->getDepartureEmployees($request, $args));
//        } catch (ApiException $exc){
//            return $exc->generateHttpResponse($response);
//        }
//        catch (\Exception $exc) {
//            return $response->withJson(['error' => $exc->getCode(),'message' => $exc->getMessage()], 500);
//        }          
    }    

    public function getDepartureCars(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->getDepartureCars($request, $args));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => $exc->getCode(),'message' => $exc->getMessage()], 500);
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
            return $response->withJson(['error' => $exc->getCode(),'message' => $exc->getMessage()], 500);
        }          
    }
    
    public function addManyEmployee(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->addManyEmployee($request, $args));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => $exc->getCode(),'message' => $exc->getMessage()], 500);
        }          
    }    
   
    
    public function removeEmployee(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->removeEmployee($request, $args));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => $exc->getCode(),'message' => $exc->getMessage()], 500);
        }          
    }   
    public function notifyEmployee(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->notifyEmployee($request, $args));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => $exc->getCode(),'message' => $exc->getMessage()], 500);
        }          
    }   
        public function confirmEmployee(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->confirmEmployee($request, $args));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => $exc->getCode(),'message' => $exc->getMessage()], 500);
        }          
    }   
    public function addCar(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->addCar($request, $args));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => $exc->getCode(),'message' => $exc->getMessage()], 500);
        }          
    }   
    public function removeCar(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->deleteCar($request, $args));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => $exc->getCode(),'message' => $exc->getMessage()], 500);
        }          
    } 
    
    public function getDepartureDocumentsToPrint(Request $request, Response $response, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        try {
            return $response->withJson($this->repository->getDepartureDocumentsToPrint($request, $args));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => $exc->getCode(),'message' => $exc->getMessage()], 500);
        }          
    }
    
    public function setDriver(Request $request, Response $response, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        try {
            return $response->withJson($this->repository->setDriver($request, $args));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => $exc->getCode(),'message' => $exc->getMessage()], 500);
        }          
    } 
    public function setPassenger(Request $request, Response $response, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        try {
            return $response->withJson($this->repository->setPassenger($request, $args));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => $exc->getCode(),'message' => $exc->getMessage()], 500);
        }          
    } 
    
    public function getPlandeSites(Request $request, Response $response, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        try {
            return $response->withJson($this->repository->getPlandeSites($request, $args));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => $exc->getCode(),'message' => $exc->getMessage()], 500);
        }          
    }     
    
    
    
    // </editor-fold>

    // <editor-fold defaultstate="collapsed" desc="Arrival related">

    public function listArrivals(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return null;
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => $exc->getCode(),'message' => $exc->getMessage()], 500);
        }    
    }
    public function addArrival(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return null;
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => $exc->getCode(),'message' => $exc->getMessage()], 500);
        }        
    }

    public function cancelArrival(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return null;
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => $exc->getCode(),'message' => $exc->getMessage()], 500);
        }        
    }
    
    public function confirmArrival(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return null;
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => $exc->getCode(),'message' => $exc->getMessage()], 500);
        }                
    }    
    
    public function setCarMilage(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            $this->repository->setCarMilage($request, $args);
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => $exc->getCode(),'message' => $exc->getMessage()], 500);
        }                
    }    
    // </editor-fold>


}

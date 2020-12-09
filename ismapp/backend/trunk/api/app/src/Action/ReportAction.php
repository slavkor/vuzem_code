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

use App\Repository\ReportRepository;
use App\Common\ApiException;


class ReportAction {
    /**
     * @var Slim\App
     */
    private $app;

     /**
     * @var \Psr\Log\LoggerInterface
     */
    private $logger;

    /**
     * @var \App\Repository\ReportRepository
     */
    private $repository;
    
    /**
     * @param \Psr\Log\LoggerInterface       $logger
     * @param \App\Repository\ReportRepository $repository
     */
//    public function __construct(App $app, LoggerInterface $logger,  ReportRepository $repository)
    public function __construct(LoggerInterface $logger,  ReportRepository $repository)
    {
        //$this->app = $app;
        $this->logger = $logger;
        $this->repository = $repository;
    }
    
    public function gethdrurl(Request $request, Response $response, $args) {
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            
//            var_dump($request->getUri()->getScheme()).PHP_EOL;
//            var_dump($request->getUri()->getAuthority()).PHP_EOL;
//            var_dump($request->getUri()->getUserInfo()).PHP_EOL;
//            var_dump($request->getUri()->getHost()).PHP_EOL;
//            var_dump($request->getUri()->getPort()).PHP_EOL;
//            var_dump($request->getUri()->getPath()).PHP_EOL;
//            var_dump($request->getUri()->getBasePath()).PHP_EOL;
//            var_dump($request->getUri()->getQuery()).PHP_EOL;
//            var_dump($request->getUri()->getFragment()).PHP_EOL;
//            var_dump($request->getUri()->getBaseUrl()).PHP_EOL;
//            die;
            
            return $response->withJson(["url" =>$request->getUri()->getBaseUrl()."/rpt/hdr/". $args["fid"]."?". $request->getUri()->getQuery()]);
            
    
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }        
    }
    
    public function listreports(Request $request, Response $response, $args) {
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            
            return $response->withJson($this->repository->listreports($args["firmid"]));
            
    
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }        
    }
    public function listuserreports(Request $request, Response $response, $args) {
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            
            return $response->withJson($this->repository->listuserreports($args["firmid"], $args["user"]));
            
    
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }        
    }    

    public function listcontextreports(Request $request, Response $response, $args) {
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            
            return $response->withJson($this->repository->listcontextreports($args["firmid"], $args["context"], $args["user"]));
            
    
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }        
    }    
    
    
    public function addreport(Request $request, Response $response, $args) {
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
            
            return $response->withJson($this->repository->addreport($args["firmid"], $jsonData));
        
    
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }        
    }
    public function updatereport(Request $request, Response $response, $args) {
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

            $jsonData = json_decode($request->getBody());
            if (json_last_error() !== JSON_ERROR_NONE) {
                throw ApiException::json_decodeError();
            }
            if (NULL === $jsonData) {
                throw ApiException::serverError('No data povided');
            }            
            return $response->withJson($this->repository->updatereport($jsonData));
            
    
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }        
    }
    public function deletereport(Request $request, Response $response, $args) {
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            
            return $response->withJson($this->repository->deletereport($args["id"]));
            
    
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }        
    }    
    
    public function binduser(Request $request, Response $response, $args) {
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            
            return $response->withJson($this->repository->binduser($args["id"],$args["user"]));
            
    
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }        
    }    
    public function unbinduser(Request $request, Response $response, $args) {
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            
            return $response->withJson($this->repository->unbinduser($args["id"],$args["user"]));
            
    
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }        
    }  
   public function getReportIsoData(Request $request, Response $response, $args) {
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            
            return $response->withJson($this->repository->getReportIsoData($args["id"]));
            
    
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }        
    }      
    
    
    
    
}

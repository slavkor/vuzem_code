<?php

namespace App\Action;

use Psr\Log\LoggerInterface;
use Slim\Http\Request;
use Slim\Http\Response;
use Slim\App;
use App\Repository\CommonRepository;
use App\Common\ApiException;


class CommonAction {
    /**
     * @var Slim\App
     */
    private $app;

     /**
     * @var \Psr\Log\LoggerInterface
     */
    private $logger;

    /**
     * @var \App\Repository\CommonRepository
     */
    private $repository;
    
    /**
     * @param \Psr\Log\LoggerInterface       $logger
     * @param \App\Repository\NoteRepository $repository
     */
//    public function __construct(App $app, LoggerInterface $logger,  CommonRepository $repository)
    public function __construct(LoggerInterface $logger,  CommonRepository $repository)
    {
        //$this->app = $app;
        $this->logger = $logger;
        $this->repository = $repository;
    }
    
    
    public function getNfo(Request $request, Response $response, $args) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        try {
            $this->logger->debug("info");
            phpinfo();
            
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }        
    }

    public function getAllLanguages(Request $request, Response $response, $args) {
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->getAllLanguages());    
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }        
    }

    public function getAllAddresses(Request $request, Response $response, $args) {
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}            
            return $response->withJson($this->repository->getAllAddresses());       
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }        
    }      
    
    public function getAllCountries(Request $request, Response $response, $args) {
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}            
            return $response->withJson($this->repository->getAllCountries());       
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }        
    }    
    
    public function addLanguage(Request $request, Response $response, $args) {
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            $jsonData = json_decode($request->getBody());
            if (json_last_error() !== JSON_ERROR_NONE) {
                throw ApiException::json_decodeError();
            }
            
            if (NULL === $jsonData) {
                throw ApiException::serverError('No data povided');
            }            
            return $response->withJson($this->repository->addLanguage($jsonData));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }        
    } 

    // <editor-fold defaultstate="collapsed" desc="WorkPlace related">
    public function addWorkplace(Request $request, Response $response, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        try {
            return $response->withJson($this->repository->addWorkplace($request, $args), 200, JSON_PRETTY_PRINT);
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }           
    }
    public function deleteWorkplace(Request $request, Response $response, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        try {
            return $response->withJson($this->repository->deleteWorkplace($request, $args), 200, JSON_PRETTY_PRINT);
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }    
    }
    public function getAllWorkplaces(Request $request, Response $response, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        try {
            return $response->withJson($this->repository->getAllWorkplaces($request, $args), 200, JSON_PRETTY_PRINT);
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }    
    }
    public function getWorkplace(Request $request, Response $response, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        try {
            return $response->withJson($this->repository->getWorkplace($request, $args), 200, JSON_PRETTY_PRINT);
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }    
    }
    // </editor-fold>

}

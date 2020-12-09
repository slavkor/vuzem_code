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

use App\Repository\CompanyRepository;
use App\Common\ApiException;

class CompanyAction {
    /**
     * @var Slim\App
     */
    private $app;

    /**
     * @var \Psr\Log\LoggerInterface
     */
    private $logger;

    /**
     * @var \App\Repository\CompanyRepository
     */
    private $repository;

    /**
     * @param \Psr\Log\LoggerInterface       $logger
     * @param \App\Repository\NoteRepository $repository
     */
    //public function __construct(App $app, LoggerInterface $logger,  CompanyRepository $repository)
    public function __construct(LoggerInterface $logger,  CompanyRepository $repository)
    {
        //$this->app = $app;
        $this->logger = $logger;
        $this->repository = $repository;
    }
    
    public function getAllCompanies(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->getAllCompanies());
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    public function getCompanyDocumens(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            
            $id = NULL;
            if(array_key_exists('id', $args))
            {
                $id = $args['id'];
            }
            if (NULL === $id) {
                throw ApiException::serverError('No Company provided');
            }
            
            return $response->withJson($this->repository->getCompanyDocumens($id));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    public function getCompanyDocumensByType(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            
            return $response->withJson($this->repository->getCompanyDocumensByType($args['id'], $args['type']));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }    

    
    public function addCompany(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            $jsonData = json_decode($request->getBody());
            if (json_last_error() !== JSON_ERROR_NONE) {
                throw ApiException::json_decodeError();
            }
          
            if (NULL === $jsonData) {
                throw ApiException::serverError('No data povided');
            }
            $company = $this->repository->addCompany($jsonData);
            if (!empty($company)) {
                $response = $response->withJson($company);
            } else {
                throw ApiException::serverError('Unable to create company');
            }
            return $response;        
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }  
   public function updateCompany(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
         
            return $response->withJson($this->repository->updateCompany($request, $args));        
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }  
        
    
    public function addlogo(Request $request, Response $response, $args) {
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

            $json = json_decode($request->getBody());
            if (json_last_error() !== JSON_ERROR_NONE) {
                throw ApiException::json_decodeError();
            }

            $this->repository->addCompanyLogo($json);
            return $response->withStatus(200);
            
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }      
    } 
  
    public function getCompany(Request $request, Response $response, $args) {
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

            $id = NULL;
            if(array_key_exists('id', $args))
            {
                $id = $args['id'];
            }
            if (NULL === $id) {
                throw ApiException::serverError('No Company provided');
            }

            return $response->withJson($this->repository->getCompanyByUuid($id), 200) ;
            
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }           
    }
    public function getCompanyData(Request $request, Response $response, $args) {
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

            return $response->withJson($this->repository->getCompanyByUuid($args['fid']), 200) ;
            
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }           
    }    
}

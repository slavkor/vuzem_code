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
use App\Repository\BusinessPartnersRepository;
use App\Common\ApiException;

final class BusinessPartnersAction {

    /**
     * @var Slim\App
     */
    private $app;

    /**
     * @var \Psr\Log\LoggerInterface
     */
    private $logger;

    /**
     * @var \App\Repository\BusinessPartnersRepository
     */
    private $repository;

    /**
     * @param \Psr\Log\LoggerInterface              $logger
     * @param \App\Repository\BusinessPartnersRepository    $repository
     */
    //public function __construct(App $app, LoggerInterface $logger,  BusinessPartnersRepository $repository)
    public function __construct(LoggerInterface $logger,  BusinessPartnersRepository $repository)
    {
        //$this->app = $app;
        $this->logger = $logger;
        $this->repository = $repository;
    }
    
    public function addPartner(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            
            $jsonData = json_decode($request->getBody());
            if (json_last_error() !== JSON_ERROR_NONE) {
                throw ApiException::json_decodeError();
            }
            
            if (NULL === $jsonData) {
                throw ApiException::serverError('No data povided');
            }
            $partner = $this->repository->addPartner($jsonData);
            if (!empty($partner)) {
                $response = $response->withJson($partner);
            } else {
                throw ApiException::serverError('Unable to create partner');
            }
            return $response;        
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    } 
    public function updatePartner(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            $jsonData = json_decode($request->getBody());
            if (json_last_error() !== JSON_ERROR_NONE) {
                throw ApiException::json_decodeError();
            }
            
            if (NULL === $jsonData) {
                throw ApiException::serverError('No data povided');
            }
            
            $partner = $this->repository->updatePartner($jsonData);
            if (!empty($partner)) {
                $response = $response->withJson($partner);
            } else {
                throw ApiException::serverError('Unable to create partner');
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
    public function getAll(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            
            $firmid = NULL;
            if(array_key_exists('firmid', $args))
            {
                $firmid = $args['firmid'];
            }
            
            return $response->withJson($this->repository->getAll($firmid), 200, JSON_PRETTY_PRINT);
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
            $data = $this->repository->addPartnerDocument($jsonData);
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

            $partner = $this->repository->addPartnerAddress($json);
            return $response->withJson($partner);
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
            $data = $this->repository->addPartnerContact(json_decode($request->getBody()));
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
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->getAddresses($args['id']));
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
    public function getDocuments(Request $request, Response $response, $args)   {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        try {
            return $response->withJson($this->repository->getDocuments($args['id']));
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
            return $response->withJson($this->repository->getDocumentsOfType($request, $args));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }        
    } 
}

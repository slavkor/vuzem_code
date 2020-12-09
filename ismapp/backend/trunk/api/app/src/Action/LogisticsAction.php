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

use App\Repository\LogisticsRepository;
use App\Common\ApiException;

class LogisticsAction {
    /**
     * @var Slim\App
     */
    private $app;

   /**
     * @var \Psr\Log\LoggerInterface
     */
    private $logger;

    /**
     * @var \App\Repository\LogisticsRepository
     */
    private $repository;

    /**
     * @param \Psr\Log\LoggerInterface       $logger
     * @param \App\Repository\LogisticsRepository $repository
     */
    //public function __construct(App $app, LoggerInterface $logger,  LogisticsRepository $repository)
    public function __construct(LoggerInterface $logger,  LogisticsRepository $repository)
    {
        //$this->app = $app;
        $this->logger = $logger;
        $this->repository = $repository;
    }

// <editor-fold defaultstate="collapsed" desc="public functions - car related">
    public function addCar(Request $request, Response $response, $args){
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
            return $response->withJson($this->repository->addCar($firmid, $jsonData));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    public function updateCar(Request $request, Response $response, $args){
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
            return $response->withJson($this->repository->updateCar($jsonData));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    public function deleteCar(Request $request, Response $response, $args){
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
            return $response->withJson($this->repository->deleteCar($jsonData));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    public function listCars(Request $request, Response $response, $args){
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
            return $response->withJson($this->repository->listCars($firmid));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    public function listhomeCars(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            

            return $response->withJson($this->repository->listhomeCars($args['firmid']));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }    

    
// </editor-fold>



}

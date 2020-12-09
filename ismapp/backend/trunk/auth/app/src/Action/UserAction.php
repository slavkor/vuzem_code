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

use App\Repository\UserRepository;
use App\Common\ApiException;

/**
 * Description of UserAction
 *
 * @author slavko
 */
class UserAction {
    /**
     * @var \Psr\Log\LoggerInterface
     */
    private $logger;

    /**
     * @var \App\Repository\UserRepository
     */
    private $userRepository;

    public function __construct(LoggerInterface $logger, UserRepository $userRepository) {
        $this->logger = $logger;
        $this->userRepository = $userRepository;
    }
    
    public function GetUserByToken(Request $request, Response $response){
        try {
            return $response->withJson($this->userRepository->getUserEntityByToken($request->getAttribute('oauth_access_token_id')));
        } catch (\Exception $exc) {
            throw ApiException::serverError($exc->getMessage());
        }
    }
   
    public function getAllUsers(Request $request, Response $response, $args){
        $this->logger->info(__CLASS__.':'.__FUNCTION__);
        
        
        return $response->withJson($this->userRepository->getAllUsers());
    }
    
    public function addUser(Request $request, Response $response, $args){
        $this->logger->info(__CLASS__.':'.__FUNCTION__);

        if (empty($request->getAttribute('oauth_user_id')) || $request->getAttribute('oauth_user_id') === NULL) {
            return $response;
        }
        $jsonData = json_decode($request->getBody());
        if (json_last_error() !== JSON_ERROR_NONE) {
            throw ApiException::json_decodeError();
        }
        $user = $this->userRepository->createUser($jsonData);
        return $response->withJson($user);
    }
    
    public function updateUser(Request $request, Response $response, $args){
        $this->logger->info(__CLASS__.':'.__FUNCTION__);

        if (empty($request->getAttribute('oauth_user_id')) || $request->getAttribute('oauth_user_id') === NULL) {
            return $response;
        }
        
        $jsonData = json_decode($request->getBody());
        if (json_last_error() !== JSON_ERROR_NONE) {
            throw ApiException::json_decodeError();
        }
        $user = $this->userRepository->updateUser($jsonData, $request->getAttribute('oauth_user_id'));
        return $response->withJson($user);
    }
    public function addScope(Request $request, Response $response, $args){
        $this->logger->info(__CLASS__.':'.__FUNCTION__);

        if (empty($request->getAttribute('oauth_user_id')) || $request->getAttribute('oauth_user_id') === NULL) {
            return $response;
        }
        
        $jsonData = json_decode($request->getBody());
        if (json_last_error() !== JSON_ERROR_NONE) {
            throw ApiException::json_decodeError();
        }
        $user = $this->userRepository->addScope($jsonData, $args['id']);
        return $response->withJson($user);
    }
    public function removeScope(Request $request, Response $response, $args){
        $this->logger->info(__CLASS__.':'.__FUNCTION__);

        if (empty($request->getAttribute('oauth_user_id')) || $request->getAttribute('oauth_user_id') === NULL) {
            return $response;
        }
        
        $jsonData = json_decode($request->getBody());
        if (json_last_error() !== JSON_ERROR_NONE) {
            throw ApiException::json_decodeError();
        }
        $user = $this->userRepository->removeScope($jsonData, $args['id']);
        return $response->withJson($user);
    }
    

}
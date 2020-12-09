<?php

namespace App\Action;

use Psr\Log\LoggerInterface;
use Slim\Http\Request;
use Slim\Http\Response;
use Slim\App;

use App\Repository\UsersRepository;

/**
 * Description of UsersAction
 *
 * @author slavko
 */
final class UsersAction {
    /**
     * @var Slim\App
     */
    private $app;

    /**
     * @var \Psr\Log\LoggerInterface
     */
    private $logger;

    /**
     * @var \App\Repository\UsersRepository
     */
    private $repository;

    /**
     * @param \Psr\Log\LoggerInterface       $logger
     * @param \App\Factory\NoteFactory       $factory
     * @param \App\Repository\NoteRepository $repository
     */
//    public function __construct(App $app, LoggerInterface $logger,  UsersRepository $repository){
    public function __construct(LoggerInterface $logger,  UsersRepository $repository){
        //$this->app = $app;
        $this->logger = $logger;
        $this->repository = $repository;
    }
    
    public function getAllUsers(Request $request, Response $response, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        
        return $response->withJson($this->repository->getAllUsers());
    }
    
    public function addUser(Request $request, Response $response, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        if (empty($request->getAttribute('oauth_user_id')) || $request->getAttribute('oauth_user_id') === NULL) {
            return $response;
        }
        $jsonData = json_decode($request->getBody());
        if (json_last_error() !== JSON_ERROR_NONE) {
            throw ApiException::json_decodeError();
        }
        $user = $this->repository->createUser($jsonData, $request->getAttribute('oauth_user_id'));
        return $response->withJson($user);
    }
    
    public function getUserByToken(Request $request, Response $response, $args) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $user = $this->repository->getUserByToken($request->getAttribute('oauth_access_token_id', NULL));
        return $response->withJson($user);
    }
}

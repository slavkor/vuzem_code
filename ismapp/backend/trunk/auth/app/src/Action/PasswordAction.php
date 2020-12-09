<?php

namespace App\Action;

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
use Psr\Log\LoggerInterface;
use Slim\Http\Request;
use Slim\Http\Response;
use League\OAuth2\Server\AuthorizationServer;
use League\OAuth2\Server\Grant\PasswordGrant;
use League\OAuth2\Server\Exception\OAuthServerException;
use App\Repository\UserRepository;
use App\Repository\RefreshTokenRepository;
use App\Common\ApiException;

/**
 * Description of PasswordAction
 *
 * @author slavko
 */
class PasswordAction {
    /**
     * @var \Psr\Log\LoggerInterface
     */
    private $logger;

    /**
     * @var \App\Repository\UserRepository
     */
    private $userRepository;    
    
    /**
     * @var \App\Repository\RefreshTokenRepository
     */
    private $refreshRepository;
    
    /**
     * @var League\OAuth2\Server\AuthorizationServer
     */
    private $server;

    /**
     * @param \Psr\Log\LoggerInterface          $logger
     * @param \App\Repository\UserRepository    $userRepository
     * @param \App\Repository\RefreshTokenRepository        $refreshRepository
     * @param League\OAuth2\Server\AuthorizationServer    $server
     */
    public function __construct(LoggerInterface $logger, UserRepository $userRepository, RefreshTokenRepository $refreshRepository, AuthorizationServer $server)
    {
        $this->logger = $logger;
        $this->userRepository = $userRepository;
        $this->refreshRepository = $refreshRepository;
        $this->server = $server;
    }

    public function RespondToAccessRequest(Request $request, Response $response){
        try {
            $grant = new PasswordGrant($this->userRepository, $this->refreshRepository);
            $grant->setRefreshTokenTTL(new \DateInterval('P1M')); // refresh tokens will expire after 1 month
            $this->server->enableGrantType($grant, new \DateInterval('PT10H')); // access tokens will expire after 1 hour
 
            return $this->server->respondToAccessTokenRequest($request, $response);   
        } catch (OAuthServerException $exc) {
            return $exc->generateHttpResponse($response);
        } catch (\Exception $exc){
            throw ApiException::serverError($exc->getMessage());
        }
    }
}

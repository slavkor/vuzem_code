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
use League\OAuth2\Server\AuthorizationServer;
use League\OAuth2\Server\Grant\RefreshTokenGrant;
use League\OAuth2\Server\Exception\OAuthServerException;
use App\Repository\RefreshTokenRepository;
use App\Common\ApiException;

class RefreshAction {
    /**
     * @var \Psr\Log\LoggerInterface
     */
    private $logger;

    /**
     * @var \App\Repository\RefreshTokenRepository
     */
    private $refreshRepository;
    
    /**
     * @var League\OAuth2\Server\AuthorizationServer
     */
    private $server;

    
    public function __construct(LoggerInterface $logger, RefreshTokenRepository $refreshRepository, AuthorizationServer $server)
    {
        $this->logger = $logger;
        $this->refreshRepository = $refreshRepository;
        $this->server = $server;
    }
    
    public function RespondToAccessRequest(Request $request, Response $response){
        try {
            $grant = new RefreshTokenGrant($this->refreshRepository);
            $grant->setRefreshTokenTTL(new \DateInterval('P1M')); // refresh tokens will expire after 1 month
            $this->server->enableGrantType($grant, new \DateInterval('PT1H')); // access tokens will expire after 1 hour
            return $this->server->respondToAccessTokenRequest($request, $response);   
        } catch (OAuthServerException $exc) {
            return $exc->generateHttpResponse($response);
        } catch (\Exception $exc){
            throw ApiException::serverError($exc->getMessage());
        }
    }
}

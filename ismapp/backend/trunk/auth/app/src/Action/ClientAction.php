<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Action;

/**
 * Description of ClientAction
 *
 * @author sl<?php

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
use League\OAuth2\Server\Grant\ClientCredentialsGrant;
use app\Common\ApiException;
/**
 * Description of PasswordAction
 *
 * @author slavko
 */
class ClientAction {
    /**
     * @var \Psr\Log\LoggerInterface
     */
    private $logger;

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
    public function __construct(LoggerInterface $logger, AuthorizationServer $server)
    {
        $this->logger = $logger;
        $this->server = $server;
    }
    
    public function RespondToAccessRequest(Request $request, Response $response){

        try {
            $grant = new ClientCredentialsGrant();

            // Enable the password grant on the server with a token TTL of 1 hour
            $this->server->enableGrantType(
                $grant,
                new \DateInterval('PT10H') // access tokens will expire after 10 hours
            );
            // Try to respond to the access token request
            return $this->server->respondToAccessTokenRequest($request, $response);        
        } catch (\Exception $exc) {
            throw ApiException::serverError($exc->getMessage());
        }
    }
    
}
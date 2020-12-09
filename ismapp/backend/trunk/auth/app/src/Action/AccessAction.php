<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Action;

use Psr\Log\LoggerInterface;
use League\OAuth2\Server\Exception\OAuthServerException;
use Slim\Http\Request;
use Slim\Http\Response;
use App\Common\ApiException;

class AccessAction {
    /**
     * @var \Psr\Log\LoggerInterface
     */
    private $logger;

    /**
     * @var \App\Action\ClientAction
     */
    private $clientAction;

    /**
     * @var \App\Action\PasswordAction
     */
    private $passwordAction;
    
    /**
     * @var \App\Action\PasswordAction
     */
    private $refreshAction; 
    
    /**
     * @var \App\Action\UserAction
     */
    private $userAction;
    
    /**
     * @var \App\Action\ScopeAction
     */
    private $scopeAction;

    
    public function __construct(LoggerInterface $logger, ClientAction $clientAction, PasswordAction $passwordAction, RefreshAction $refreshAction,  UserAction $userAction, ScopeAction $scopeAction)
    {
        $this->logger = $logger;
        $this->clientAction = $clientAction;
        $this->passwordAction = $passwordAction;
        $this->refreshAction = $refreshAction;
        $this->userAction = $userAction;
        $this->scopeAction = $scopeAction;
    }
    
    public function RespondToAccessRequest(Request $request, Response $response){
        try {
            
            $json = $request->getParsedBody();

            if (null === $json) {
                return $response;
            }
            if (empty($json)) {
                return $response;
            }
            
            $grant_type = $json['grant_type'];
            
            switch ($grant_type) {
                case 'password':
                    return $this->passwordAction->RespondToAccessRequest($request, $response);
                case 'client_credentials':
                    return $this->clientAction->RespondToAccessRequest($request, $response);
                case 'refresh_token':
                    return $this->refreshAction->RespondToAccessRequest($request, $response);
                default:
                    break;
            }
        } catch (OAuthServerException $exc) {
            return $exc->generateHttpResponse($response);
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }   
    }
    
    public function RespondToAccessTokenInfo(Request $request, Response $response){
        try {
            return  $this->userAction->GetUserByToken($request, $response);
            
        } catch (OAuthServerException $exc) {
            return $exc->generateHttpResponse($response);
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
    } 
    
    public function RespondToUserInfo(Request $request, Response $response){
        try {
            return  $this->userAction->GetUserByToken($request, $response);
            
        } catch (OAuthServerException $exc) {
            return $exc->generateHttpResponse($response);
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
    }     
    
    public function RespondToScopeInfo(Request $request, Response $response){
        try {
            return  $this->scopeAction->GetScopeByToken($request, $response);
        } catch (OAuthServerException $exc) {
            return $exc->generateHttpResponse($response);
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
    }        
    
}

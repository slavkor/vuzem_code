<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Middleware;

use League\OAuth2\Server\AuthorizationValidators\AuthorizationValidatorInterface;
use League\OAuth2\Server\Exception\OAuthServerException;
use Psr\Http\Message\ServerRequestInterface;
use Slim\App;

class DummyValidator implements AuthorizationValidatorInterface {
    /**
     * @var Slim\App
     */
    private $app;
    /**
     * @var AccessTokenRepositoryInterface
     */
    private $accessTokenRepository;

    /**
     * @param AccessTokenRepositoryInterface $accessTokenRepository
     */
    public function __construct($accessTokenRepository)
    {
        //$this->app = $app;
        $this->accessTokenRepository = $accessTokenRepository;
    }
    
    public function validateAuthorization(ServerRequestInterface $request): ServerRequestInterface {
  
       
        try {
            // Return the request with additional attributes
            return $request
                ->withAttribute('oauth_access_token_id', "dummy:)")
                ->withAttribute('oauth_client_id', "dummy:)")
                ->withAttribute('oauth_user_id', "dummy:)")
                ->withAttribute('oauth_scopes', "dummy:)");
        } catch (\InvalidArgumentException $exception) {
            // JWT couldn't be parsed so return the request as is
            throw OAuthServerException::accessDenied($exception->getMessage());
        } catch (\RuntimeException $exception) {
            //JWR couldn't be parsed so return the request as is
            throw OAuthServerException::accessDenied('Error while decoding to JSON');
        }        
    }
    
    /**
     * Set the private key
     *
     * @param \League\OAuth2\Server\CryptKey $key
     */
    public function setPublicKey($key)
    {
        
    }
}

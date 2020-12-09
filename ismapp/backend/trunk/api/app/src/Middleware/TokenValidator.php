<?php
namespace App\Middleware;

use Slim\App;

use League\OAuth2\Server\AuthorizationValidators\AuthorizationValidatorInterface;
use League\OAuth2\Server\Exception\OAuthServerException;
use Psr\Http\Message\ServerRequestInterface;




/**
 * Description of TokenValidator
 *
 * @author slavko
 */

class TokenValidator implements AuthorizationValidatorInterface {
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
        $params = $request->getQueryParams();
        if(!is_array($params)){
            throw OAuthServerException::accessDenied('Missing token');
        }
        $token = $params['token'];
        if ($token === NULL) {
            throw OAuthServerException::accessDenied('Missing token');
        }
        try {
            /*
            // Attempt to parse and validate the JWT
            $token = (new Parser())->parse($jwt);
            if ($token->verify(new Sha256(), $this->publicKey->getKeyPath()) === false) {
                throw OAuthServerException::accessDenied('Access token could not be verified');
            }

            // Ensure access token hasn't expired
            $data = new ValidationData();
            $data->setCurrentTime(time());

            if ($token->validate($data) === false) {
                throw OAuthServerException::accessDenied('Access token is invalid');
            }
            */
            
            //var_dump($this->accessTokenRepository->isAccessTokenRevoked($token));

            // Check if token has been revoked
            if ($this->accessTokenRepository->isAccessTokenRevoked($token)) {
                throw OAuthServerException::accessDenied('Access token has been revoked');
            }

            
            // Return the request with additional attributes
            return $request
                ->withAttribute('oauth_access_token_id', $token)
                ->withAttribute('oauth_client_id', $token)
                ->withAttribute('oauth_user_id', $token)
                ->withAttribute('oauth_scopes', $token);
            
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

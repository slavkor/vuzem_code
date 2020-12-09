<?php

namespace App\Action;
use Psr\Log\LoggerInterface;
use Slim\Http\Request;
use Slim\Http\Response;

use App\Repository\ScopeRepository;
use App\Common\ApiException;

class ScopeAction {
    /**
     * @var \Psr\Log\LoggerInterface
     */
    private $logger;

    /**
     * @var \App\Repository\ScopeRepository
     */
    private $scopeRepository;

    public function __construct(LoggerInterface $logger, ScopeRepository $scopeRepository) {
        $this->logger = $logger;
        $this->scopeRepository = $scopeRepository;
    }

    public function AddScope(Request $request, Response $response){
        try {

            $this->logger->info(__CLASS__.':'.__FUNCTION__);
        
            $jsonData = json_decode($request->getBody());
            if (NULL === $jsonData) {
                return;
            }
            $scope = $this->scopeRepository->AddScope($jsonData);
            return $response->withJson($scope);
        } catch (\Exception $exc) {
            throw ApiException::serverError($exc->getMessage());
        }        
    } 
    public function GetScopeByToken(Request $request, Response $response){
        try {

            $this->logger->info(__CLASS__.':'.__FUNCTION__);

            $tokenId = $request->getAttribute('oauth_access_token_id');
            if (null === $tokenId) {
                throw ApiException::serverError('Access token not provided');
            }
            if (empty($tokenId)) {
                throw ApiException::serverError('Access token not provided');
            }
            
            $scope = $this->scopeRepository->GetScopeByToken($tokenId);
            return $response->withJson($scope);
        } catch (\Exception $exc) {
            throw ApiException::serverError($exc->getMessage());
        }        
    }  
    public function GetAllScopes(Request $request, Response $response){
        try {

            $this->logger->info(__CLASS__.':'.__FUNCTION__);

            $tokenId = $request->getAttribute('oauth_access_token_id');
            if (null === $tokenId) {
                throw ApiException::serverError('Access token not provided');
            }
            if (empty($tokenId)) {
                throw ApiException::serverError('Access token not provided');
            }
            return $response->withJson($this->scopeRepository->GetAllScopes());
        } catch (\Exception $exc) {
            throw ApiException::serverError($exc->getMessage());
        }        
    }      
}

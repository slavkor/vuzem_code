<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Middleware;

use Psr\Log\LoggerInterface;
use Slim\Http\Request;
use Slim\Http\Response;
use Slim\App;

use App\Repository\AuditRepository;
use Psr\Http\Message\ServerRequestInterface;
use App\Common\Enum;
use App\Common\AuditType;
/**
 * Description of AuditMiddleware
 *
 * @author slavko
 */
class AuditMiddleware {
    /**
     * @var Slim\App
     */
    private $app;
    
    private $settings;
    protected $logger;    
    /**
     *
     * @var App\Repository\AuditRepository
     */
    private $repository;
    
    public function __construct($settings, AuditRepository $repository, LoggerInterface $logger) {
        //$this->app = $app;
        $this->settings = $settings;
        $this->repository = $repository;
        $this->logger = $logger;
    }
    
    /**
     * @param Slim\Http\Request $request
     * @param Slim\Http\Response $response
     * @param callable               $next
     *
     * @return Slim\Http\Response
     */
    public function __invoke(Request $request, Response $response, callable $next)
    {
        
        return $next($request, $response);
        
        try {
            $uuid = NULL;
            $auditType = Enum::Label(AuditType::class, AuditType::NONE);
            $route = $request->getAttribute('route');
            if (empty($route)) { return $next($request, $response); }

            $name = $route->getName();
            if (empty($name)) { return $next($request, $response); }
            if (null === $name) { return $next($request, $response); }

            $reuqestType = $request->getHeader('Content-type');

            switch ($reuqestType[0]) {
                case 'application/json;charset=utf-8':
                case 'application/json':
                    $requestjson = json_decode($request->getBody(), TRUE);
                    $requestdata = json_encode($requestjson);
                    if (isset($requestjson['uuid'])) {
                        $uuid = $requestjson['uuid'];
                    }
                    break;
                default:
                    $requestdata = $request->getBody();
                    break;
            }        

            $method = $request->getMethod();
            switch ($method) {
                case 'GET':
                    return $next($request, $response);
                    //$auditType = Enum::Label(AuditType::class, AuditType::GET);
                    break;
                case 'PUT':
                case 'POST':
                    if (empty($requestjson)) { break; }
                    if(empty($requestjson['uuid'])){ $auditType = Enum::Label(AuditType::class, AuditType::CREATE);}
                    else { 
                        $auditType = Enum::Label(AuditType::class, AuditType::UPDATE);
                        if (isset($requestjson['uuid'])) {
                            $uuid = $requestjson['uuid'];
                        }
                    }
                    break;
                default:
                    break;
            }
            
            if($auditType === Enum::Label(AuditType::class, AuditType::NONE))
            { 
                return $next($request, $response);
            }
    
            $response =  $next($request, $response);
            
            $responseType = $response->getHeader('Content-type');
            switch ($responseType[0]) {
                case 'application/json;charset=utf-8':
                case 'application/json':
                    $responsejson = json_decode($response->getBody(), TRUE);
                    $responsedata = json_encode($responsejson);
                    if (NULL !== $uuid) {
                        if (isset($responsejson['uuid'])) {
                            $uuid = $responsejson['uuid'];
                        }
                    }
                    break;
                default:
                    $responsedata = json_encode($response->getBody());
                    break;
            }

            $this->repository->AuditAction($auditType, $name, $uuid,$requestdata, $responsedata, $request->getAttribute('oauth_user_id', NULL));   
            return $response; 
            
        } catch (\Exception $exc) {
            return $next($request, $response);
        }
    }
}

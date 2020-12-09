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

/**
 * Description of LogMiddleware
 *
 * @author slavko
 */
class LogMiddleware {

        
    private $settings;
    /**
     *
     * @var LoggerInterface 
     */
    protected $logger;    
    public function __construct($settings, LoggerInterface $logger) {

        $this->settings = $settings;
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
        try {
            $response =  $next($request, $response);
            
            $this->logger->info((string)$request->getUri() ."->".$request->getHeaderLine("voodoo") ."->".$request->getBody());
            return $response;
        } catch (\Exception $exc) {
            return $next($request, $response);
        }
    }
}

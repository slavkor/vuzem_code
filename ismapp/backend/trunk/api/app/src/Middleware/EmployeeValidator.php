<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Middleware;
use Slim\Http\Request;
use Slim\Http\Response;
use Slim\App;
use App\Common\ApiException;
use Psr\Log\LoggerInterface;

class EmployeeValidator {
    /**
     * @var Slim\App
     */
    private $app;    
    private $settings;
    protected $logger;
    public function __construct($settings, LoggerInterface $logger) {
        //$this->app = $app;
        $this->settings = $settings;
        $this->logger = $logger;
    }
    
    public function __invoke(Request $request, Response $response, callable $next)
    {
        try {
            /// TO-DO implementa Employee fields validation
            
            return $next($request, $response); 
            
        } catch (\Exception $exc) {
            return $next($request, $response);
        }
    }
}

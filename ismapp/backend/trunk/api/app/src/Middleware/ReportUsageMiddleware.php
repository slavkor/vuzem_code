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
use Psr\Http\Message\ServerRequestInterface;
use App\Common\Enum;
use App\Common\AuditType;
use App\Repository\ReportRepository;

class ReportUsageMiddleware {
  
    /**
     * @var Slim\App
     */
    private $app;    

    private $settings;
    protected $logger;    
    /**
     *
     * @var App\Repository\ReportRepository
     */
    private $repository;
    
    public function __construct($settings,  ReportRepository$repository, LoggerInterface $logger) {
        //$this->app = $app;
        $this->settings = $settings;
        $this->repository = $repository;
        $this->logger = $logger;
    }
    
    /**
     * @param Slim\Http\Request     $request
     * @param Slim\Http\Response    $response
     * @param callable              $next
     *
     * @return Slim\Http\Response
     */
    public function __invoke(Request $request, Response $response, callable $next)
    {
        try {
            $response = $next($request, $response);
          
            return $response; 
            
        } catch (\Exception $exc) {
            return $next($request, $response);
        }
    }
}

<?php

namespace App\Middleware;
use Slim\App;

use \Psr\Http\Message\ServerRequestInterface as Request;
use \Psr\Http\Message\ResponseInterface as Response;

use App\Common\ExecutionTime;

class StatisticsMiddleware {
    /**
     * @var Slim\App
     */
    private $app;    
    
    private $settings;
    
    public function __construct($settings) {
        //$this->app = $app;
        $this->settings = $settings;
    }
    /**
     * @param ServerRequestInterface $request
     * @param ResponseInterface      $response
     * @param callable               $next
     *
     * @return \Psr\Http\Message\ResponseInterface
     */
    public function __invoke(Request $request, Response $response, callable $next)
    {
        $executionTime = new ExecutionTime();
        $executionTime->Start();
        
        $response =  $next($request, $response);

        $executionTime->End();

        $json = json_decode($response->getBody());
        if (NULL === $json) {
            return $response;
        }

        $json['serverstats'] = $executionTime->__toString();
        
        $response = $response->withJson($json);
        
        return $response;
    }
}

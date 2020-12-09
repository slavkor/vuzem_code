<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Middleware;

use Slim\Http\Request;
use Slim\Http\Response;
use PhpAmqpLib\Connection\AMQPStreamConnection;
use Lcobucci\JWT\Parser;


/**
 * Description of AuditMiddleware
 *
 * @author slavko
 */
class RabbitMQ {
    
    private $settings;
    public function __construct($settings) {
        //$this->app = $app;
        $this->settings = $settings;
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
        $response =  $next($request, $response);
        try {

            

            $jwt = trim(preg_replace('/^(?:\s+)?Bearer\s/', '', json_decode($response->getBody())->{"access_token"}));
            $token = (new Parser())->parse($jwt);

            $connection = new AMQPStreamConnection($this->settings['rabbitmq']['host'], $this->settings['rabbitmq']['port'], $this->settings['rabbitmq']['username'], $this->settings['rabbitmq']['password']);
            $channel = $connection->channel();
            $channel->exchange_declare("ism.sistem", "fanout", FALSE, TRUE, FALSE);
            $channel->queue_declare($token->getClaim('sub'), FALSE, TRUE, FALSE, FALSE);
            $channel->queue_bind($token->getClaim('sub'), "ism.sistem");

            $channel->close();
            $connection->close();
            
            return $response;
            
        } catch (\Exception $exc) {
            return $response;
        }
    }
}

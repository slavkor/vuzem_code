<?php

use App\Action\AccessAction;
use App\Action\ScopeAction;
use App\Action\UserAction;

use App\Middleware\RabbitMQ;

use League\OAuth2\Server\Middleware\ResourceServerMiddleware;


$app->group('/access', function() use($app){
    // route to request a token 
    $app->post('', AccessAction::class.':RespondToAccessRequest')->add($app->getContainer()->get(RabbitMQ::class));
    
    // route to get token or user information protected by the same token (if token invalid call fails)
    $app->group('/get', function() use($app){
        $app->get('/token', AccessAction::class.':RespondToAccessTokenInfo');
        $app->get('/user', AccessAction::class.':RespondToUserInfo');
        $app->get('/scope', AccessAction::class.':RespondToScopeInfo');
    })->add($app->getContainer()->get(ResourceServerMiddleware::class));
});

$app->group('/users', function() use($app){ 
    $app->post('/add', UserAction::class.':addUser');
    $app->post('/update', UserAction::class.':updateUser');
    $app->get('/list', UserAction::class.':getAllUsers');
    $app->get('/me', UserAction::class.':getUserByToken');    
    $app->post('/{id}/addscope', UserAction::class.':addScope');    
    $app->post('/{id}/removescope', UserAction::class.':removeScope');    
})->add($app->getContainer()->get(ResourceServerMiddleware::class));


$app->group('/scope', function() use($app){
    $app->post('/add', ScopeAction::class.':AddScope');
    $app->get('/list', ScopeAction::class.':GetAllScopes');
})->add($app->getContainer()->get(ResourceServerMiddleware::class));
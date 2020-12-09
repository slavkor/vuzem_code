<?php

/* 
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
use League\OAuth2\Server\Middleware\ResourceServerMiddleware;
use League\OAuth2\Server\ResourceServer;

use App\Middleware\AuditMiddleware;
use App\Middleware\LogMiddleware;
use App\Action\ProjectAction;

$app->group('/mobile', function() use($app){ 
    $app->get('/user/{user}/csite/list', ProjectAction::class.':getUserSites');
    $app->get('/user/{user}/project/list', ProjectAction::class.':getUserProjects');

    $app->post('/user/{user}/project/{project}/shift/add', ProjectAction::class.':addShift');
    $app->post('/user/{user}/project/{project}/shift/update', ProjectAction::class.':updateShift');
    $app->post('/user/{user}/project/{project}/shift/delete', ProjectAction::class.':deleteShift');
    $app->post('/user/{user}/project/{project}/shift/addmany', ProjectAction::class.':addManyShifts');

    $app->post('/user/{user}/project/{project}/btb/list', ProjectAction::class.':listBtb');
    $app->post('/user/{user}/project/{project}/btb/add', ProjectAction::class.':adBtb');
    $app->post('/user/{user}/project/{project}/btb/update', ProjectAction::class.':updateBtb');
    $app->post('/user/{user}/project/{project}/btb/delete', ProjectAction::class.':deleteBtb');
    
})->add($app->getContainer()->get(LogMiddleware::class))->add($app->getContainer()->get(AuditMiddleware::class))->add(new ResourceServerMiddleware($app->getContainer()->get(ResourceServer::class)));

<?php

return [
    'settings' => [
        // Slim Settings
        'determineRouteBeforeAppMiddleware' => true,
        'displayErrorDetails' => true,
        
        // monolog settings
        'logger' => [
            'name' => 'app',
            'path' => __DIR__.'/log/app.log',
        ],
        
        //neo4j clien settings
        'dbclient' => [
            'name' => 'bolt',
            'host' => '127.0.0.1',
            'port' => '7687',
            'username' => 'neo4j',
            'password' => 'r1htar1c', 
            'type' => 'bolt'
        ],
        
        //neo4j clien settings
        'mailer' => [
            'host' => 'webmail.ismvuzem.si',
            'port' => '26',
            'username' => 'alert@ismvuzem.si',
            'password' => '!#alert1alert#!', 
            'from' => 'alert@ismvuzem.si'
        ],        
        
        //oauth2 resource server settings
        'resourceserver' => [
            'url' => 'auth.ismvuzem.si',
            'publickey' => 'file://' . __DIR__ . '/key/public.key'
        ],
    ],
];

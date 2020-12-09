<?php
return [
    'settings' => [
        // Slim Settings
        'determineRouteBeforeAppMiddleware' => false,
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
        'rabbitmq' => [
            'host' => 'api.ismvuzem.si',
            'port' => '5672',
            'username' => 'ism',
            'password' => '%hz6u#yZ5@Q#Gqqm'
        ],
        
        
        //oauth2 authorization server settings
        'authorizationserver' => [
            'privatekey' => 'file://' . __DIR__ . '/key/private.key',
            'encryptionkey' => 'lxZFUEsBCJ2Yb14IF2ygAHI5N4+ZAUXXaSeeJm6+twsUmIen'
        ],
        //oauth2 resource server settings
        'resourceserver' => [
            'url' => 'auth.ismvuzem.si',
            'publickey' => 'file://' . __DIR__ . '/key/public.key'
        ]        
    ],
];

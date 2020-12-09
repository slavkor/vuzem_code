<?php
use App\Repository\UserRepository;
use App\Repository\RefreshTokenRepository;
use App\Repository\ScopeRepository;
use App\Repository\ClientRepository;
use App\Repository\AuthCodeRepository;
use App\Repository\AccessTokenRepository;

use App\Action\PasswordAction;
use App\Action\ClientAction;
use App\Action\UserAction;
use App\Action\AccessAction;
use App\Action\RefreshAction;
use App\Action\ScopeAction;
use App\Middleware\RabbitMQ;

use GraphAware\Neo4j\Client\ClientBuilder;
use GraphAware\Neo4j\OGM\EntityManager;
use League\OAuth2\Server\AuthorizationServer;
use League\OAuth2\Server\ResourceServer;
use League\OAuth2\Server\Middleware\ResourceServerMiddleware;


$container = $app->getContainer();

//<editor-fold  desc="LOGGER">
$container['logger'] = function ($c) {
    $settings = $c->get('settings');
    $logger = new Monolog\Logger($settings['logger']['name']);
    $logger->pushProcessor(new Monolog\Processor\UidProcessor());
    $logger->pushHandler(new Monolog\Handler\StreamHandler($settings['logger']['path'], Monolog\Logger::DEBUG));
    
    return $logger;
};
//</editor-fold>

//<editor-fold desc="DATABASE">
$container['dbclinet'] = function($c){
    $settings = $c->get('settings');
    $connection = 'http://'.$settings['dbclient']['username'].':'.$settings['dbclient']['password'].'@'. $settings['dbclient']['host'].':'.$settings['dbclient']['port'];
    //echo $connection;
    $client = ClientBuilder::create()
    ->addConnection( $settings['dbclient']['name'], $connection)
    ->build();
    return $client;
};
$container['dbentity'] = function($c){
    $settings = $c->get('settings');
    
    $connection = 'http://'.$settings['dbclient']['username'].':'.$settings['dbclient']['password'].'@'. $settings['dbclient']['host'].':'.$settings['dbclient']['port'];

    $entitymanager = EntityManager::create($connection);
   
    return $entitymanager;
};


$container['entitymaper'] = function($c){
    $mapper = new \JsonMapper();
    $mapper->setLogger($c->get('logger'));
    return $mapper;
};
//</editor-fold>

//<editor-fold desc="AUTHORIZATION SERVER">
$container[UserRepository::class] = function ($c) {
    return new UserRepository($c->get('logger'),  $c->get('settings'));
};
$container[RefreshTokenRepository::class] = function ($c) {
    return new RefreshTokenRepository($c->get('logger'), $c->get('settings'));
};
$container[ScopeRepository::class] = function ($c) {
    return new ScopeRepository($c->get('logger'), $c->get('settings'));
};
$container[ClientRepository::class] = function ($c) {
    return new ClientRepository($c->get('logger'), $c->get('settings'));
};
$container[AuthCodeRepository::class] = function ($c) {
    return new AuthCodeRepository($c->get('logger'), $c->get('settings'));
};
$container[AccessTokenRepository::class] = function ($c) {
    return new AccessTokenRepository($c->get('logger'), $c->get('settings'));
};


$container[AuthorizationServer::class] = function ($c) {
    $settings = $c->get('settings');
    
    //var_dump($settings);
    
    $server = new AuthorizationServer(
        $c->get(ClientRepository::class),       // instance of ClientRepositoryInterface
        $c->get(AccessTokenRepository::class),  // instance of AccessTokenRepositoryInterface
        $c->get(ScopeRepository::class),        // instance of ScopeRepositoryInterface
        $settings['authorizationserver']['privatekey'],    // path to private key
        $settings['authorizationserver']['encryptionkey']      // encryption key
    );
    
    return $server;
};
//</editor-fold>

// <editor-fold desc="RESOURCE SERVER" defaultstate="collapsed"> 
$container[ResourceServer::class] = function ($c) {
    $settings = $c->get('settings');
    
    $server = new ResourceServer(
        $c->get(AccessTokenRepository::class),// instance of AccessTokenRepositoryInterface
        $settings['resourceserver']['publickey']  // the authorization server's public key
    );
    return $server;
};

$container[ResourceServerMiddleware::class] = function ($c) {
    $middleware = new ResourceServerMiddleware($c->get(ResourceServer::class));
    return $middleware;
};

// </editor-fold>

//<editor-fold desc="ACTIONS">
$container[PasswordAction::class] = function ($c) {
    return new PasswordAction($c->get('logger'), $c->get(UserRepository::class), $c->get(RefreshTokenRepository::class), $c->get(AuthorizationServer::class));
};
$container[UserAction::class] = function ($c) {
    return new UserAction($c->get('logger'), $c->get(UserRepository::class));
};
$container[ClientAction::class] = function ($c) {
    return new ClientAction($c->get('logger'), $c->get(AuthorizationServer::class));
};
$container[RefreshAction::class] = function ($c) {
    return new RefreshAction($c->get('logger'), $c->get(RefreshTokenRepository::class), $c->get(AuthorizationServer::class));
};
$container[AccessAction::class] = function ($c) {
    return new AccessAction($c->get('logger'), $c->get(ClientAction::class), $c->get(PasswordAction::class), $c->get(RefreshAction::class), $c->get(UserAction::class), $c->get(ScopeAction::class));
};
$container[ScopeAction::class] = function ($c) {
    return new ScopeAction($c->get('logger'), $c->get(ScopeRepository::class));
};


$container[RabbitMQ::class] = function ($c) {
    return new RabbitMQ($c->get('settings'));
};

//</editor-fold>

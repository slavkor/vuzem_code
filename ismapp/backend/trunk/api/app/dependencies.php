<?php
use PHPMailer\PHPMailer;

use Slim\App;

use League\OAuth2\Server\ResourceServer;
use App\Middleware\TokenValidator;
use App\Middleware\DummyValidator;

use App\Repository\AccessTokenRepository;
use App\Repository\UsersRepository;
use App\Repository\EmployeesRepository;
use App\Repository\DocumentRepository;
use App\Repository\ConstructionSiteRepository;
use App\Repository\CompanyRepository;
use App\Repository\AuditRepository;
use App\Repository\BusinessPartnersRepository;
use App\Repository\CommonRepository;
use App\Repository\ReportRepository;
use App\Repository\ProjectRepository;
use App\Repository\LogisticsRepository;
use App\Repository\DepartureArrivalRepository;


use App\Action\UsersAction;
use App\Action\DocumentAction;
use App\Action\EmployeesAction;
use App\Action\ConstructionSiteAction;
use App\Action\CompanyAction;
use App\Action\BusinessPartnersAction;
use App\Action\CommonAction;
use App\Action\ReportAction;
use App\Action\ProjectAction;
use App\Action\LogisticsAction;
use App\Action\DepartureArrivalAction;

use App\Middleware\AuditMiddleware;
use App\Middleware\EmployeeValidator;
use App\Middleware\HireValidator;
use App\Middleware\FireValidator;
use App\Middleware\LogMiddleware;
use App\Middleware\DepartureValidator;
use App\Middleware\DepartureCarValidator;
use App\Middleware\DeparturePlanMailer;
use App\Middleware\OccupancyMiddleware;


$container = $app->getContainer();
$container['slim'] = function ($c) {
   global $app;
   return $app;
};

// <editor-fold desc="LOGGER" defaultstate="collapsed"> 

$container['logger'] = function ($c) {
    $settings = $c->get('settings');
    $logger = new Monolog\Logger($settings['logger']['name']);
    //$logger->pushProcessor(new Monolog\Processor\UidProcessor());
    $logger->pushProcessor(new Monolog\Processor\IntrospectionProcessor());
    $logger->pushProcessor(new Monolog\Processor\WebProcessor());
    $logger->pushHandler(new Monolog\Handler\StreamHandler($settings['logger']['path'],Monolog\Logger::DEBUG));

    return $logger;
};

// </editor-fold>

// <editor-fold desc="DATABASE" defaultstate="collapsed"> 

$container['entitymaper'] = function($c){
    $mapper = new \JsonMapper();
    $mapper->setLogger($c->get('logger'));
    return $mapper;
};

// </editor-fold>

// <editor-fold desc="RESOURCE SERVER" defaultstate="collapsed"> 



$container[AccessTokenRepository::class] = function ($c) {
    return new AccessTokenRepository($c->get('logger'), $c->get('settings'));
};

$container[TokenValidator::class] = function ($c) {
    return new TokenValidator($c->get(AccessTokenRepository::class));
};

$container[DummyValidator::class] = function ($c) {
    return new DummyValidator($c->get(AccessTokenRepository::class));
};


$container[ResourceServer::class] = function ($c) {
    $settings = $c->get('settings');
    
//    $server = new ResourceServer(
//        $c->get(AccessTokenRepository::class),// instance of AccessTokenRepositoryInterface
//        $settings['resourceserver']['publickey'],  // the authorization server's public key
//        $c->get(DummyValidator::class)
//    );
//    return $server;
    
    $server = new ResourceServer(
        $c->get(AccessTokenRepository::class),// instance of AccessTokenRepositoryInterface
        $settings['resourceserver']['publickey']  // the authorization server's public key
    );
    
    
    return $server;
};

$container["token"] = function ($c) {
    $settings = $c->get('settings');
    
    $server = new ResourceServer(
        $c->get(AccessTokenRepository::class),// instance of AccessTokenRepositoryInterface
        $settings['resourceserver']['publickey'],  // the authorization server's public key
        $c->get(TokenValidator::class)
    );
    return $server;
};
// </editor-fold>

// <editor-fold desc="REPOSITORIES" defaultstate="collapsed"> 


$container[UsersRepository::class] = function ($c) { 
    return new UsersRepository($c->get('logger'), $c->get('settings'));
};
$container[EmployeesRepository::class] = function ($c) {
    return new EmployeesRepository($c->get('logger'), $c->get('settings'));
};
$container[DocumentRepository::class] = function ($c) {
    return new DocumentRepository($c->get('logger'), $c->get('settings'));
};
$container[ConstructionSiteRepository::class] = function ($c) {
    return new ConstructionSiteRepository($c->get('logger'), $c->get('settings'));
};
$container[CompanyRepository::class] = function ($c) {
    return new CompanyRepository($c->get('logger'), $c->get('settings'));
};
$container[AuditRepository::class] = function ($c) {
    return new AuditRepository($c->get('logger'), $c->get('settings'));
};
$container[BusinessPartnersRepository::class] = function ($c) {
    return new BusinessPartnersRepository($c->get('logger'), $c->get('settings'));
};
$container[CommonRepository::class] = function ($c) {
    return new CommonRepository($c->get('logger'), $c->get('settings'));
};
$container[ReportRepository::class] = function ($c) {
    return new ReportRepository($c->get('logger'), $c->get('settings'));
};
$container[ProjectRepository::class] = function ($c) {
    return new ProjectRepository($c->get('logger'), $c->get('settings'));
};
$container[LogisticsRepository::class] = function ($c) {
    return new LogisticsRepository($c->get('logger'), $c->get('settings'));
};
$container[DepartureArrivalRepository::class] = function ($c) {
    return new DepartureArrivalRepository($c->get('logger'), $c->get('settings'));
};


// </editor-fold>

// <editor-fold desc="ACTIONS" defaultstate="collapsed"> 



$container[UsersAction::class] = function ($c) {
    return new UsersAction($c->get('logger'), $c->get(UsersRepository::class));
};
$container[DocumentAction::class] = function ($c) {
    return new DocumentAction($c->get('logger'), $c->get(DocumentRepository::class));
};
$container[EmployeesAction::class] = function ($c) {
    return new EmployeesAction($c->get('logger'), $c->get(EmployeesRepository::class));
};
$container[ConstructionSiteAction::class] = function ($c) {
    return new ConstructionSiteAction($c->get('logger'), $c->get(ConstructionSiteRepository::class));
};
$container[CompanyAction::class] = function ($c) {
    return new CompanyAction($c->get('logger'), $c->get(CompanyRepository::class));
};
$container[BusinessPartnersAction::class] = function ($c)  {
    return new BusinessPartnersAction($c->get('logger'), $c->get(BusinessPartnersRepository::class));
};

$container[CommonAction::class] = function ($c) {
    return new CommonAction($c->get('logger'), $c->get(CommonRepository::class));
};

$container[ReportAction::class] = function ($c) {
    return new ReportAction($c->get('logger'), $c->get(ReportRepository::class));
};

$container[ProjectAction::class] = function ($c) {
    return new ProjectAction($c->get('logger'), $c->get(ProjectRepository::class));
};

$container[LogisticsAction::class] = function ($c) {
    return new LogisticsAction($c->get('logger'), $c->get(LogisticsRepository::class));
};

$container[DepartureArrivalAction::class] = function ($c) {
    return new DepartureArrivalAction($c->get('logger'), $c->get(DepartureArrivalRepository::class));
};


// </editor-fold>

// <editor-fold desc="MIDDLEWARE" defaultstate="collapsed"> 
$container[AuditMiddleware::class] = function ($c) {
    return new AuditMiddleware($c->get('settings'), $c->get(AuditRepository::class), $c->get('logger'));
};
$container[LogMiddleware::class] = function ($c) {
    return new LogMiddleware($c->get('settings'), $c->get('logger'));
};


$container[EmployeeValidator::class] = function ($c) {
    return new EmployeeValidator($c->get('settings'), $c->get('logger'));
};

$container[HireValidator::class] = function ($c) {
    return new HireValidator($c->get('settings'), $c->get('logger'), $c->get(EmployeesRepository::class));
};
$container[FireValidator::class] = function ($c) {
    return new FireValidator($c->get('settings'), $c->get('logger'), $c->get(EmployeesRepository::class));
};
$container[DepartureValidator::class] = function ($c) {
    return new DepartureValidator($c->get('settings'), $c->get('logger'), $c->get(DepartureArrivalRepository::class));
};
$container[DepartureCarValidator::class] = function ($c) {
    return new DepartureCarValidator($c->get('settings'), $c->get('logger'), $c->get(DepartureArrivalRepository::class));
};
$container[DeparturePlanMailer::class] = function ($c) {
    return new DeparturePlanMailer($c->get('settings'), $c->get('logger'), $c->get(DepartureArrivalRepository::class));
};
$container[OccupancyMiddleware::class] = function ($c) {
    return new OccupancyMiddleware($c->get('settings'), $c->get('logger'), $c->get(EmployeesRepository::class));
};
// </editor-fold>


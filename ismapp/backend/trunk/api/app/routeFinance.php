<?php

use League\OAuth2\Server\Middleware\ResourceServerMiddleware;
use League\OAuth2\Server\ResourceServer;
use PHPMailer\PHPMailer;
use App\Action\BusinessPartnersAction;
use App\Action\CompanyAction;
use App\Action\ConstructionSiteAction;
use App\Action\DocumentAction;
use App\Action\EmployeesAction;
use App\Action\UsersAction;
use App\Middleware\EmployeeValidator;
use App\Action\CommonAction;
use App\Action\ReportAction;
use App\Action\ProjectAction;
use App\Action\DepartureArrivalAction;
use App\Action\LogisticsAction;
use App\Middleware\HireValidator;
use App\Middleware\FireValidator;
use App\Middleware\AuditMiddleware;
use App\Middleware\LogMiddleware;
use App\Middleware\DepartureValidator;
use App\Middleware\DepartureCarValidator;
use App\Middleware\DeparturePlanMailer;
use App\Middleware\OccupancyMiddleware;


$app->group('/f/{firmid}', function() use($app){ 
    $app->group('/invoice/out', function() use($app){
        $app->post('/add', FinanceAction::class.':addInvoice')->setname(FinanceAction::class.':addInvoice');
        $app->post('/update', FinanceAction::class.':updateInvoice')->setname(FinanceAction::class.':updateInvoice');
        $app->post('/delete', FinanceAction::class.':deleteInvoice')->setname(FinanceAction::class.':deleteInvoice');
        $app->post('{id}/book', FinanceAction::class.':bookInvoice')->setname(FinanceAction::class.':bookInvoice');
    });
    $app->group('/invoice/in', function() use($app){
        $app->post('/add', FinanceAction::class.':addInvoice')->setname(FinanceAction::class.':addInvoice');
        $app->post('/update', FinanceAction::class.':updateInvoice')->setname(FinanceAction::class.':updateInvoice');
        $app->post('/delete', FinanceAction::class.':deleteInvoice')->setname(FinanceAction::class.':deleteInvoice');
        $app->post('{id}/book', FinanceAction::class.':bookInvoice')->setname(FinanceAction::class.':bookInvoice');
    });
})->add($app->getContainer()->get(LogMiddleware::class))->add($app->getContainer()->get(AuditMiddleware::class))->add(new ResourceServerMiddleware($app->getContainer()->get(ResourceServer::class)));


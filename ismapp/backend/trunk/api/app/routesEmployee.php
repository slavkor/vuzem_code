<?php
use League\OAuth2\Server\Middleware\ResourceServerMiddleware;
use League\OAuth2\Server\ResourceServer;

use App\Action\EmployeesAction;
use App\Middleware\EmployeeValidator;
use App\Middleware\HireValidator;
use App\Middleware\FireValidator;
use App\Middleware\AuditMiddleware;
use App\Middleware\LogMiddleware;
use App\Middleware\OccupancyMiddleware;

$app->group('/employees', function() use($app){
    $app->get('/list', EmployeesAction::class.':getAllEmployees'); 
    $app->get('/list2', EmployeesAction::class.':getAllEmployees2'); 
    $app->get('/listh', EmployeesAction::class.':getEmployeesWithHistory'); 
    $app->get('/listhome', EmployeesAction::class.':getHomeEmployees');
    $app->get('/listhome2', EmployeesAction::class.':getHomeEmployees2');
    $app->get('/listaway', EmployeesAction::class.':getAwayEmployees');   
    $app->get('/listactive', EmployeesAction::class.':getActiveEmployees');
    $app->get('/activecount', EmployeesAction::class.':getActivecount'); 
    $app->get('/{username}', EmployeesAction::class.':getByUser'); 
    $app->get('/{id}/workperiods/current', EmployeesAction::class.':getCurrentWorkPeriod');
})->add($app->getContainer()->get(LogMiddleware::class))->add($app->getContainer()->get(AuditMiddleware::class))->add(new ResourceServerMiddleware($app->getContainer()->get(ResourceServer::class)));

$app->group('/f/{firmid}', function() use($app){ 
    $app->group('/employees', function() use($app){
        $app->post('/add', EmployeesAction::class.':addEmployee')->setname(EmployeesAction::class.':addEmployee')->add($app->getContainer()->get(EmployeeValidator::class));
        $app->post('/hire', EmployeesAction::class.':hireEmployee')->setname(EmployeesAction::class.':hireEmployee')->add($app->getContainer()->get(HireValidator::class));
        $app->post('/fire', EmployeesAction::class.':fireEmployee')->setname(EmployeesAction::class.':fireEmployee')->add($app->getContainer()->get(FireValidator::class));
        $app->post('/addDocument', EmployeesAction::class.':addDocument')->setname(EmployeesAction::class.':addDocument');
        $app->post('/addAddress', EmployeesAction::class.':addAddress')->setname(EmployeesAction::class.':addAddress');
        $app->post('/addContact', EmployeesAction::class.':addContact')->setname(EmployeesAction::class.':addContact');
        $app->post('/update', EmployeesAction::class.':updateEmployee')->setname(EmployeesAction::class.':updateEmployee');  
        $app->post('/delete', EmployeesAction::class.':deleteEmployee')->setname(EmployeesAction::class.':deleteEmployee');
        $app->post('/documentstoexpire', EmployeesAction::class.':getDocumnetsToExpire'); 

        $app->get('/listemployed', EmployeesAction::class.':getEmployed'); 
        $app->get('/listloaned', EmployeesAction::class.':getLoaned'); 
        $app->get('/listfired', EmployeesAction::class.':getFired'); 

        $app->get('/list', EmployeesAction::class.':getAllEmployees'); 
        
        $app->get('/listh', EmployeesAction::class.':getEmployeesWithHistory'); 
        $app->get('/activecount', EmployeesAction::class.':getActivecount'); 
        $app->get('/listhome', EmployeesAction::class.':getHomeEmployees');
        $app->get('/listhome2', EmployeesAction::class.':getHomeEmployees2');
        $app->get('/listaway', EmployeesAction::class.':getAwayEmployees');
        $app->get('/listactive', EmployeesAction::class.':getActiveEmployees');
        $app->get('/listduration', EmployeesAction::class.':getCoutryDuration');
        $app->get('/listabsences', EmployeesAction::class.':getAbsentEmployees');
    
        $app->get('/{username}', EmployeesAction::class.':getByUser'); 
        $app->get('/{id}/address', EmployeesAction::class.':getAddresses'); 
        $app->get('/{id}/contact', EmployeesAction::class.':getContacts'); 
        $app->get('/{id}/document', EmployeesAction::class.':getDocuments');
        $app->get('/{id}/workperiods', EmployeesAction::class.':getWorkPeriods');
        $app->get('/{id}/workperiods/current', EmployeesAction::class.':getCurrentWorkPeriod');
        $app->get('/{id}/document/{type}', EmployeesAction::class.':getDocumentsOfType');
        $app->get('/{id}/spokenlang', EmployeesAction::class.':getSpokenLanguage');
        $app->get('/{id}/hirehistory', EmployeesAction::class.':getHireHistory');
        $app->get('/{id}/workhistory', EmployeesAction::class.':getWorkHistory');
        
        $app->post('/{id}/address/update', EmployeesAction::class.':updateEmployeeAddress')->setname(EmployeesAction::class.':updateEmployeeAddress');
        $app->post('/{id}/contact/update', EmployeesAction::class.':updateEmployeeContact')->setname(EmployeesAction::class.':updateEmployeeContact');
        $app->post('/{id}/address/delete', EmployeesAction::class.':deleteEmployeeAddress')->setname(EmployeesAction::class.':deleteEmployeeAddress');
        $app->post('/{id}/contact/delete', EmployeesAction::class.':deleteEmployeeContact')->setname(EmployeesAction::class.':deleteEmployeeContact');
        $app->post('/{id}/document/delete', EmployeesAction::class.':deleteEmployeeDocument')->setname(EmployeesAction::class.':deleteEmployeeDocument');
        $app->post('/{id}/spokenlang/add', EmployeesAction::class.':addSpokenLanguage')->setname(EmployeesAction::class.':addSpokenLanguage');
        $app->post('/{id}/spokenlang/delete', EmployeesAction::class.':deleteSpokenLanguage')->setname(EmployeesAction::class.':deleteSpokenLanguage');
        
        $app->post('/{id}/absence/add',EmployeesAction::class.':addAbsence')->setname(EmployeesAction::class.':addAbsence'); 
        $app->post('/{id}/absence/update',EmployeesAction::class.':updateAbsence')->setname(EmployeesAction::class.':updateAbsence'); 
        $app->post('/{id}/absence/delete',EmployeesAction::class.':deleteAbsence')->setname(EmployeesAction::class.':deleteAbsence'); 
        $app->post('/{id}/absence/list',EmployeesAction::class.':listAbsence'); 

        $app->post('/{id}/addloaner', EmployeesAction::class.':addLoaner')->setname(EmployeesAction::class.':addLoaner');
        $app->post('/{id}/deleteloaner', EmployeesAction::class.':deleteLoaner')->setname(EmployeesAction::class.':deleteLoaner');
        $app->post('/{id}/workplace', EmployeesAction::class.':workplace')->setname(EmployeesAction::class.':workplace');
        
    });
})->add($app->getContainer()->get(LogMiddleware::class))->add($app->getContainer()->get(AuditMiddleware::class))->add(new ResourceServerMiddleware($app->getContainer()->get(ResourceServer::class)));

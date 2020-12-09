<?php

use League\OAuth2\Server\Middleware\ResourceServerMiddleware;
use League\OAuth2\Server\ResourceServer;

use App\Action\ProjectAction;
use App\Middleware\AuditMiddleware;
use App\Middleware\LogMiddleware;

$app->group('/f/{firmid}', function() use($app){ 

    $app->group('/project', function() use($app){
        
        $app->get('/list/{status}', ProjectAction::class.':getProjects');
        $app->get('/list/{status}/{employee}', ProjectAction::class.':getEmployeeProjects');

        $app->get('/list', ProjectAction::class.':getAllProjects');
        $app->post('/add', ProjectAction::class.':addProject')->setname(ProjectAction::class.':addProject');
        $app->post('/addDocument', ProjectAction::class.':addDocument')->setname(ProjectAction::class.':addDocument');    
        $app->post('/addAddress', ProjectAction::class.':addAddress')->setname(ProjectAction::class.':addAddress');    
        $app->post('/addContact', ProjectAction::class.':addContact')->setname(ProjectAction::class.':addContact');
        $app->post('/{id}/delete', ProjectAction::class.':deleteProject')->setname(ProjectAction::class.':deleteProject');

        $app->post('/{id}/addExpense', ProjectAction::class.':deleteProject')->setname(ProjectAction::class.':addExpense');
        $app->post('/{id}/addIncome', ProjectAction::class.':deleteProject')->setname(ProjectAction::class.':addIncome');
        $app->post('/{id}/updateExpense', ProjectAction::class.':deleteProject')->setname(ProjectAction::class.':addExpense');
        $app->post('/{id}/updateIncome', ProjectAction::class.':deleteProject')->setname(ProjectAction::class.':addIncome');
        $app->post('/{id}/deleteExpense', ProjectAction::class.':deleteProject')->setname(ProjectAction::class.':addExpense');
        $app->post('/{id}/deleteIncome', ProjectAction::class.':deleteProject')->setname(ProjectAction::class.':addIncome');

                
        $app->post('/{id}/address/update', ProjectAction::class.':updateAddress')->setname(ProjectAction::class.':updateAddress');  
        $app->post('/{id}/address/delete', ProjectAction::class.':deleteAddress')->setname(ProjectAction::class.':deleteAddress');  
        $app->post('/{id}/address/bind', ProjectAction::class.':bindAddress')->setname(ProjectAction::class.':bindAddress');  
        $app->post('/{id}/contact/update', ProjectAction::class.':updateContact')->setname(ProjectAction::class.':updateContact');  
        $app->post('/{id}/contact/delete', ProjectAction::class.':deleteContact')->setname(ProjectAction::class.':deleteContact');  
        $app->post('/{id}/document/delete', ProjectAction::class.':deleteDocument')->setname(ProjectAction::class.':deleteDocument');  

        
        $app->post('/{id}/ewr/list', ProjectAction::class.':getProjectEwrs');  
        $app->post('/{id}/ewr/add', ProjectAction::class.':addEwr')->setname(ProjectAction::class.':addEwr');  
        $app->post('/{id}/ewr/update', ProjectAction::class.':updateEwr')->setname(ProjectAction::class.':updateEwr');  
        $app->post('/{id}/ewr/delete', ProjectAction::class.':deleteEwr')->setname(ProjectAction::class.':deleteEwr');  
        $app->post('/{id}/ewr/{ewr}/worker/{employee}/add/{workplace}', ProjectAction::class.':addEwrEmployee')->setname(ProjectAction::class.':addEwrEmployee');  
        $app->post('/{id}/ewr/{ewr}/worker/{employee}/delete', ProjectAction::class.':deleteEwrEmployee')->setname(ProjectAction::class.':deleteEwrEmployee');  

        $app->post('/{id}/wp/add', ProjectAction::class.':addWp')->setname(ProjectAction::class.':addWp');  
        $app->post('/{id}/wp/update', ProjectAction::class.':deleteWp')->setname(ProjectAction::class.':deleteWp');  

        $app->post('/{id}/wp/{wp}/add', ProjectAction::class.':addWpPlan')->setname(ProjectAction::class.':addWpPlan');  
        $app->post('/{id}/wp/{wp}/delete', ProjectAction::class.':deleteWp')->setname(ProjectAction::class.':deleteWp');  
        
        $app->post('/{id}/employee/{eid}/shiftadd', ProjectAction::class.':addShift')->setname(ProjectAction::class.':addShift');  
        $app->post('/{id}/employee/{eid}/shiftdelete', ProjectAction::class.':deleteShift')->setname(ProjectAction::class.':deleteShift');  
        $app->get('/{id}/employee/list', ProjectAction::class.':getActiveEmployees');  


        $app->get('/{id}/car/list', ProjectAction::class.':getActiveCars');  
        $app->get('/{id}/document', ProjectAction::class.':getDocuments');
        $app->get('/{id}/address', ProjectAction::class.':getAddresses'); 
        $app->get('/{id}/contact', ProjectAction::class.':getContacts'); 

        $app->post('/{id}/workdayshifts', ProjectAction::class.':getWorkShiftsByDay');
        $app->post('/{id}/allshifts', ProjectAction::class.':getWorkShifts');
        
    });    

    $app->group('/ewr', function() use($app){
        $app->get('/list', ProjectAction::class.':getAllEwrs');
        $app->post('/addDocument', ProjectAction::class.':addEwrDocument')->setname(ProjectAction::class.':addEwrDocument');    
        //$app->post('/addContact', ProjectAction::class.':addContact')->setname(ProjectAction::class.':addContact');

//        $app->post('/{id}/contact/update', ProjectAction::class.':updateContact')->setname(ProjectAction::class.':updateContact');  
//        $app->post('/{id}/contact/delete', ProjectAction::class.':deleteContact')->setname(ProjectAction::class.':deleteContact');  
        $app->get('/{id}/document', ProjectAction::class.':getEwrDocuments');  
        $app->post('/{id}/document/delete', ProjectAction::class.':deleteEwrDocument')->setname(ProjectAction::class.':deleteEwrDocument');  
    });    
})->add($app->getContainer()->get(LogMiddleware::class))->add($app->getContainer()->get(AuditMiddleware::class))->add(new ResourceServerMiddleware($app->getContainer()->get(ResourceServer::class)));
 



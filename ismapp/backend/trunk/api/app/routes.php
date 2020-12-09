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


$app->group('/nfo', function() use($app){ 
    $app->get('/', CommonAction::class.':getNfo');
    $app->post('/car/milage', DepartureArrivalAction::class.':setCarMilage')->setname(DepartureArrivalAction::class.':setCarMilage');
});

//$app->get('/nfo', CommonAction::class.':getNfo');
$app->group('/files', function() use($app){ 
    $app->get('/{fid}', DocumentAction::class.':downloadFile');
})->add($app->getContainer()->get(LogMiddleware::class))->add(new ResourceServerMiddleware($app->getContainer()->get("token")));


$app->group('/shrd', function() use($app){ 
    $app->get('/lang/list', CommonAction::class.':getAllLanguages');
    $app->post('/lang/add', CommonAction::class.':addLanguage')->setname(CommonAction::class.':addLanguage');
    $app->get('/country/list', CommonAction::class.':getAllCountries');
    $app->get('/address/list', CommonAction::class.':getAllAddresses');
    $app->get('/workplace/list', CommonAction::class.':getAllWorkPlaces');
    $app->post('/workplace/add', CommonAction::class.':addWorkPlace')->setname(CommonAction::class.':addWorkPlace');
    $app->post('/car/milage', CommonAction::class.':addWorkPlace')->setname(CommonAction::class.':addWorkPlace');
    
})->add($app->getContainer()->get(LogMiddleware::class))->add($app->getContainer()->get(AuditMiddleware::class))->add(new ResourceServerMiddleware($app->getContainer()->get(ResourceServer::class)));

$app->group('/company', function() use($app){
    $app->get('/list', CompanyAction::class.':getAllCompanies');
    $app->get('/{id}', CompanyAction::class.':getCompany');
    $app->post('/add', CompanyAction::class.':addCompany')->setname(CompanyAction::class.':addCompany');
    $app->post('/update', CompanyAction::class.':updateCompany')->setname(CompanyAction::class.':updateCompany');

    $app->get('/{id}/documents', CompanyAction::class.':getCompanyDocumens');
    $app->get('/{id}/documents/{type}', CompanyAction::class.':getCompanyDocumensByType');
    $app->post('/{id}/addlogo', CompanyAction::class.':addlogo')->setname(CompanyAction::class.':addlogo');
})->add($app->getContainer()->get(LogMiddleware::class))->add($app->getContainer()->get(AuditMiddleware::class))->add(new ResourceServerMiddleware($app->getContainer()->get(ResourceServer::class)));

$app->group('/documents', function() use($app){
    $app->post('/notifyexpireondate', DocumentAction::class.':notifyExpireableOnDate'); 
    $app->get('/types/list', DocumentAction::class.':getAllTypes');
    $app->post('/types/add', DocumentAction::class.':addDocumentType')->setname(DocumentAction::class.':addDocumentType');
    $app->post('/types/update', DocumentAction::class.':updateDocumentType')->setname(DocumentAction::class.':updateDocumentType');
    $app->get('/{id}/files/{fid}', DocumentAction::class.':download');
})->add($app->getContainer()->get(LogMiddleware::class))->add($app->getContainer()->get(AuditMiddleware::class))->add(new ResourceServerMiddleware($app->getContainer()->get(ResourceServer::class)));

$app->group('/csite', function() use($app){
    $app->get('/byemployee/{id}', ConstructionSiteAction::class.':getByEmployee');
    $app->get('/listactive', ConstructionSiteAction::class.':getActiveSites');
    $app->get('/listoverview', ConstructionSiteAction::class.':getActiveSitesOverview');
    
})->add($app->getContainer()->get(LogMiddleware::class))->add($app->getContainer()->get(AuditMiddleware::class))->add(new ResourceServerMiddleware($app->getContainer()->get(ResourceServer::class)));

$app->group('/f/{firmid}', function() use($app){ 
    $app->group('/documents', function() use($app){
        $app->post('/types/add', DocumentAction::class.':addDocumentType')->setname(DocumentAction::class.':addDocumentType');
        $app->post('/types/update', DocumentAction::class.':updateDocumentType')->setname(DocumentAction::class.':updateDocumentType');
        $app->post('/initUpload', DocumentAction::class.':initUpload')->setname(DocumentAction::class.':initUpload');
        $app->post('/uploadFile', DocumentAction::class.':uploadFile');
        $app->post('/finishUpload', DocumentAction::class.':finishUpload')->setname(DocumentAction::class.':finishUpload');
        $app->post('/update', DocumentAction::class.':updateDocument')->setname(DocumentAction::class.':updateDocument');               
        $app->get('/list', DocumentAction::class.':getAll');
        $app->get('/list/e/{type}', DocumentAction::class.':getByTypeForEmployee');
        $app->post('/{id}/activate', DocumentAction::class.':activate')->setname(DocumentAction::class.':activate');
        $app->post('/{id}/files/{fid}/delete', DocumentAction::class.':deleteFile')->setname(DocumentAction::class.':deleteFile');
        $app->get('/{id}/files/{fid}', DocumentAction::class.':download');
        $app->get('/{id}/notify', DocumentAction::class.':getDocument');
        $app->get('/{id}', DocumentAction::class.':getDocument');

        
    });
    $app->group('/csite', function() use($app){
        $app->post('/list', ConstructionSiteAction::class.':getAllCompanySites');
        $app->get('/listactive', ConstructionSiteAction::class.':getActiveSites');
        $app->post('/add', ConstructionSiteAction::class.':addCSite')->setname(ConstructionSiteAction::class.':addCSite');
        $app->post('/update', ConstructionSiteAction::class.':updateCSite')->setname(ProjectAction::class.':updateCSite');
        $app->post('/addDocument', ConstructionSiteAction::class.':addDocument')->setname(ConstructionSiteAction::class.':addDocument');    
        $app->post('/addAddress', ConstructionSiteAction::class.':addAddress')->setname(ConstructionSiteAction::class.':addAddress');    
        $app->post('/addContact', ConstructionSiteAction::class.':addContact')->setname(ConstructionSiteAction::class.':addContact');  

        $app->post('/{id}/document/delete', ConstructionSiteAction::class.':deleteDocument')->setname(ConstructionSiteAction::class.':deleteDocument');  

        $app->get('/{id}/project/list', ProjectAction::class.':getCsiteProjects');
        $app->post('/{id}/project/add', ProjectAction::class.':addProject')->setname(ProjectAction::class.':addProject');
        $app->post('/{id}/project/update', ProjectAction::class.':updateProject')->setname(ProjectAction::class.':updateProject');

        $app->get('/{id}/contact', ConstructionSiteAction::class.':getContacts'); 
        $app->post('/{id}/contact/update', ConstructionSiteAction::class.':updateContact')->setname(ConstructionSiteAction::class.':updateContact');  
        $app->post('/{id}/contact/delete', ConstructionSiteAction::class.':deleteContact')->setname(ConstructionSiteAction::class.':deleteContact');  

        $app->get('/{id}/document', ConstructionSiteAction::class.':getDocuments');
        $app->get('/byemployee/{id}', ConstructionSiteAction::class.':getByEmployee');

        $app->post('/{id}/workdayshifts', ConstructionSiteAction::class.':getWorkShiftsByDay');
        $app->post('/{id}/allshifts', ConstructionSiteAction::class.':getWorkShifts');
        $app->get('/{id}/departurestats', ConstructionSiteAction::class.':getDepartureStats');
        
        $app->get('/{id}/car/list', ConstructionSiteAction::class.':getActiveCars');  
        $app->get('/{id}/employee/list', ConstructionSiteAction::class.':getActiveEmployees');  
        
    });
    
    $app->group('/partners', function() use($app){
        $app->get('/list', BusinessPartnersAction::class.':getAll');
        $app->post('/add', BusinessPartnersAction::class.':addPartner')->setname(BusinessPartnersAction::class.':addPartner');
        $app->post('/update', BusinessPartnersAction::class.':updatePartner')->setname(BusinessPartnersAction::class.':updatePartner');
        $app->get('/{id}/address', BusinessPartnersAction::class.':getAddresses'); 
        $app->get('/{id}/contact', BusinessPartnersAction::class.':getContacts'); 
        $app->get('/{id}/document', BusinessPartnersAction::class.':getDocuments'); 
        $app->get('/{id}/document/{type}', BusinessPartnersAction::class.':getDocumentsOfType');        
        $app->post('/addDocument', BusinessPartnersAction::class.':addDocument')->setname(BusinessPartnersAction::class.':addDocument');    
        $app->post('/addAddress', BusinessPartnersAction::class.':addAddress')->setname(BusinessPartnersAction::class.':addAddress');    
        $app->post('/addContact', BusinessPartnersAction::class.':addContact')->setname(BusinessPartnersAction::class.':addContact');  
    });
    
    $app->group('/departures', function() use($app){
        $app->get('/listpladed', DepartureArrivalAction::class.':getPlandeSites');
        $app->post('/list', DepartureArrivalAction::class.':listDepartures')->setname(DepartureArrivalAction::class.':listDepartures');
        $app->post('/add', DepartureArrivalAction::class.':addDeparture')->setname(DepartureArrivalAction::class.':addDeparture');
        $app->post('/addconfirm', DepartureArrivalAction::class.':addConfirmDeparture')->setname(DepartureArrivalAction::class.':addConfirmDeparture')->add($app->getContainer()->get(DepartureValidator::class));
        $app->post('/update', DepartureArrivalAction::class.':updateDeparture')->setname(DepartureArrivalAction::class.':updateDeparture');
        $app->post('/{id}/confirm', DepartureArrivalAction::class.':confirmDeparture')->setname(DepartureArrivalAction::class.':confirmDeparture')->add($app->getContainer()->get(DepartureValidator::class))->add($app->getContainer()->get(DepartureCarValidator::class))->add($app->getContainer()->get(DeparturePlanMailer::class));
        $app->post('/{id}/cancel', DepartureArrivalAction::class.':cancelDeparture')->setname(DepartureArrivalAction::class.':confirmDeparture')->add($app->getContainer()->get(DeparturePlanMailer::class));
        $app->get('/{id}/employees', DepartureArrivalAction::class.':getDepartureEmployees');
        $app->get('/{id}/cars', DepartureArrivalAction::class.':getDepartureCars');
        $app->get('/{id}/printabledocuments', DepartureArrivalAction::class.':getDepartureDocumentsToPrint');
        
        $app->post('/{id}/employee/add', DepartureArrivalAction::class.':addEmployee')->setname(DepartureArrivalAction::class.':addEmployee')->add($app->getContainer()->get(DeparturePlanMailer::class));
        $app->post('/{id}/employee/addmany', DepartureArrivalAction::class.':addManyEmployee')->setname(DepartureArrivalAction::class.':addManyEmployee')->add($app->getContainer()->get(DeparturePlanMailer::class));
        $app->post('/{id}/employee/remove', DepartureArrivalAction::class.':removeEmployee')->setname(DepartureArrivalAction::class.':removeEmployee')->add($app->getContainer()->get(DeparturePlanMailer::class));
        $app->post('/{id}/employee/notify', DepartureArrivalAction::class.':notifyEmployee')->setname(DepartureArrivalAction::class.':notifyEmployee')->add($app->getContainer()->get(DeparturePlanMailer::class));
        $app->post('/{id}/employee/confirm', DepartureArrivalAction::class.':confirmEmployee')->setname(DepartureArrivalAction::class.':confirmEmployee')->add($app->getContainer()->get(DeparturePlanMailer::class));
        $app->post('/{id}/employee/{employeeId}/driver', DepartureArrivalAction::class.':setDriver')->setname(DepartureArrivalAction::class.':setDriver')->add($app->getContainer()->get(DeparturePlanMailer::class));
        $app->post('/{id}/employee/{employeeId}/passenger', DepartureArrivalAction::class.':setPassenger')->setname(DepartureArrivalAction::class.':setPassenger')->add($app->getContainer()->get(DeparturePlanMailer::class));

        $app->post('/{id}/car/add', DepartureArrivalAction::class.':addCar')->setname(DepartureArrivalAction::class.':addCar')->add($app->getContainer()->get(DeparturePlanMailer::class));
        $app->post('/{id}/car/remove', DepartureArrivalAction::class.':removeCar')->setname(DepartureArrivalAction::class.':removeCar')->add($app->getContainer()->get(DeparturePlanMailer::class));
    });

    $app->group('/cars', function() use($app){
        $app->post('/add', LogisticsAction::class.':addCar')->setname(LogisticsAction::class.':addCar');
        $app->post('/addDocument', LogisticsAction::class.':addDocument')->setname(LogisticsAction::class.':addDocument');    
        $app->post('/update', LogisticsAction::class.':updateCar')->setname(LogisticsAction::class.':updateCar');  
        $app->post('/delete', LogisticsAction::class.':deleteCar')->setname(LogisticsAction::class.':deleteCar');  
        $app->get('/list', LogisticsAction::class.':listCars'); 
        $app->get('/listhome', LogisticsAction::class.':listhomeCars'); 
        $app->get('/{id}/document', LogisticsAction::class.':getDocuments');
        $app->get('/{id}/document/{type}', LogisticsAction::class.':getDocumentsOfType');
        $app->post('/{id}/document/delete', LogisticsAction::class.':deleteEmployeeDocument')->setname(LogisticsAction::class.':deleteEmployeeDocument');  
    }); 

    $app->group('/reports', function() use($app){
        
        $app->group('/list', function() use($app){
            $app->get('/all', ReportAction::class.':listreports'); 
            $app->get('/user/{user}', ReportAction::class.':listuserreports');
             $app->get('/context/{context}/{user}', ReportAction::class.':listcontextreports');
        }); 
    
        //$app->get('/list', ReportAction::class.':listreports'); 
        $app->post('/add', ReportAction::class.':addreport')->setname(ReportAction::class.':addreport');
        $app->post('/update', ReportAction::class.':updatereport')->setname(ReportAction::class.':updatereport');  
        $app->post('/delete', ReportAction::class.':deletereport')->setname(ReportAction::class.':deletereport');  
        $app->post('/bind/{id}/{user}', ReportAction::class.':binduser')->setname(ReportAction::class.':binduser');  
        $app->post('/unbind/{id}/{user}', ReportAction::class.':unbinduser')->setname(ReportAction::class.':unbinduser');  
    }); 
    
})->add($app->getContainer()->get(LogMiddleware::class))->add($app->getContainer()->get(AuditMiddleware::class))->add(new ResourceServerMiddleware($app->getContainer()->get(ResourceServer::class)));
 



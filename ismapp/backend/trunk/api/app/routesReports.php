<?php


use League\OAuth2\Server\Middleware\ResourceServerMiddleware;


use App\Action\CompanyAction;
use App\Action\ConstructionSiteAction;
use App\Action\DocumentAction;
use App\Action\EmployeesAction;
use App\Action\ReportAction;
use App\Action\ProjectAction;
use App\Action\DepartureArrivalAction;
use App\Middleware\LogMiddleware;
use App\Action\LogisticsAction;

$app->group('/rpt', function() use($app){

    $app->get('/employees/doctoexpire', EmployeesAction::class.':getDocumnetsToExpire');
    $app->get('/csite/listactive', ConstructionSiteAction::class.':getActiveSites');
    $app->get('/csite/listoverview', ConstructionSiteAction::class.':getActiveSitesOverview');
    
    $app->get('/hdr/{fid}/url', ReportAction::class.':gethdrurl');
    $app->get('/hdr/{fid}/data', CompanyAction::class.':getCompanyData');
    $app->get('/hdr/{fid}', DocumentAction::class.':downloadFile');
    $app->get('/ftr/{fid}', DocumentAction::class.':downloadFile');

    $app->get('/employees/{id}/contact', EmployeesAction::class.':getContactsEx');
    $app->get('/employees/{id}/data', EmployeesAction::class.':getEmployeeByUuid');
    $app->get('/employees/{id}/hirehistory', EmployeesAction::class.':getHireHistory');
    $app->get('/employees/{id}/workhistory', EmployeesAction::class.':getWorkHistory');

    $app->get('/{firmid}/employees/listh', EmployeesAction::class.':getEmployeesWithHistory');
    
    $app->get('/{firmid}/employees/list', EmployeesAction::class.':getAllEmployees');
    $app->get('/{firmid}/employees/listemployed', EmployeesAction::class.':getAllEmployeesNotRented');
    
    $app->get('/{firmid}/employees/listhome', EmployeesAction::class.':getHomeEmployeesForReport');
    $app->get('/{firmid}/employees/listhome2', EmployeesAction::class.':getHomeEmployeesForReport2');
    $app->get('/{firmid}/employees/listaway', EmployeesAction::class.':getAwayEmployees');
    $app->get('/{firmid}/employees/listduration', EmployeesAction::class.':getCoutryDuration');
    $app->get('/{firmid}/employees/listdurationsite', EmployeesAction::class.':getCoutrySiteDuration');
    $app->get('/{firmid}/employees/list/{employeeid}', EmployeesAction::class.':getEmployeeWithHistory');
    $app->get('/{firmid}/employees/listrented/{status}', EmployeesAction::class.':getRentedEmployees');
    $app->get('/{firmid}/employees/listhiredfired/{year}/{month}', EmployeesAction::class.':getHiredFiredEmployees');
    $app->get('/{firmid}/employees/liststatehiredfired/{year}/{month}', EmployeesAction::class.':getStateHiredFiredEmployees');
    $app->get('/{firmid}/employees/podlage/{year}/{month}', EmployeesAction::class.':getPodlage');

    $app->get('/project/{id}/sitedata', ProjectAction::class.':getProjectSiteData');
    $app->get('/project/{id}/ewr/{eid}', ProjectAction::class.':getEwrData');
    $app->get('/project/{id}/hours/{year}/{month}', ProjectAction::class.':getMonthSummary');


    $app->get('/csite/{id}/departurestats', ConstructionSiteAction::class.':getDepartureStats');
    $app->get('/csite/{id}/departurehistory', ConstructionSiteAction::class.':getDepartureHistory');
    $app->get('/csite/{id}/car/list', ConstructionSiteAction::class.':getActiveCars');
    $app->get('/csite/{id}/employee/list', ConstructionSiteAction::class.':getActiveEmployees');
    
    $app->get('/departure/getplan', DepartureArrivalAction::class.':getPlanedDeparturesReport');
    $app->get('/departure/listpladed', DepartureArrivalAction::class.':getPlaned');
    $app->get('/departure/getinrange', DepartureArrivalAction::class.':getDeparturesInRangeReport');
    $app->get('/departure/getinrange2', DepartureArrivalAction::class.':getDeparturesInRangeReport2');
    $app->get('/departure/{id}/get', DepartureArrivalAction::class.':getDepartureReport');
    $app->get('/departure/{id}/employees', DepartureArrivalAction::class.':getDepartureEmployees');
    $app->get('/departure/{id}/cars', DepartureArrivalAction::class.':getDepartureCars');
    
    $app->get('/iso/{id}', ReportAction::class.':getReportIsoData');
    
    $app->group('/documents', function() use($app){
        $app->get('/list/e/{type}', DocumentAction::class.':getByTypeForEmployee');
    });

    $app->group('/{firmid}/cars', function() use($app){
        $app->get('/list', LogisticsAction::class.':listCars');
    }); 
})->add($app->getContainer()->get(LogMiddleware::class))->add(new ResourceServerMiddleware($app->getContainer()->get("token")));

<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Repository;


use Psr\Log\LoggerInterface;
use GraphAware\Neo4j\Client\Client;
use GraphAware\Neo4j\Client\ClientBuilder;
use GraphAware\Neo4j\OGM\EntityManager;


use App\Models\Company;
use App\Models\Address;
use App\Models\Document;
use App\Models\Hdr;
use App\Common\ApiException;
use App\Models\Report;

class ReportRepository {
    protected $logger;
    protected $dbclient;
    protected $dbentity;
    protected $mapper;
    protected $settings;
    
    private $connection;
    private $connalias;
    
    private $reports;
    
    public function __construct(LoggerInterface $logger, \Slim\Collection $settings){
        $this->logger = $logger;
        $this->mapper = new \JsonMapper();
        //$this->mapper->setLogger($this->logger);
        $this->settings = $settings;
        
        $this->connection = $this->settings['dbclient']['type'].'://'.$this->settings['dbclient']['username'].':'.$this->settings['dbclient']['password'].'@'. $this->settings['dbclient']['host'].':'.$this->settings['dbclient']['port'];
        $this->connalias = $this->settings['dbclient']['name'];
        
        $this->dbclient = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
        $this->dbentity  = EntityManager::create($this->connection);

        $this->reports = $this->dbentity->getRepository(Report::class); 
    }      
    
    public function gethdr($cmpny, $cnst, $prjct) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        $hdr = new Hdr();
        
        $company = $this->companies->findOneBy(["uuid" => $cmpny]);
        $company->getAddress();
        
        $hdr->setCompany($company);

        return $hdr;    
    }
    
    public function listreports($company) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        $cql = "match (company:Company{uuid:{company}})-[:HAS_REPORT]-(report:Report)
                return report 
                order by report.reportid";

        $query = $this->dbentity->createQuery($cql);
        
        $query->addEntityMapping('report', Report::class);
        $query->setParameter("company", $company);
                
        return $query->execute();
    }
    
    public function listuserreports($company, $user) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        $cql = "match (user:User{username:{user}})-[:HAS_REPORT]-(report:Report)-[:HAS_REPORT]-(:Company{uuid:{company}})
                return report";

        $query = $this->dbentity->createQuery($cql);
        
        $query->addEntityMapping('report', Report::class);
        $query->setParameter("user", $user);
        $query->setParameter("company", $company);
                
        return $query->execute();
    }    

    public function listcontextreports($company, $context, $user) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        $cql = "match (exi:User{username:{user}})-[:HAS_REPORT]->(report:Report{module:{context}})<-[:HAS_REPORT]-(:Company{uuid:{company}})
                return report ";

        $query = $this->dbentity->createQuery($cql);
        
        $query->addEntityMapping('report', Report::class);
        $query->setParameter("user", $user);
        $query->setParameter("company", $company);
        $query->setParameter("context", $context);
                
        return $query->execute();
    }    
    
    
    
    public function addreport($company, $json) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $report = new Report();
                
        $this->mapper->map($json, $report);
        
        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
        $query = "create (r:Report{".$report->getQueryFields()."})
                    with r
                    match(c:Company{uuid:{company}}) 
                    with r, c
                    create (c)-[:HAS_REPORT]->(r)";

        $client->run($query,  array_merge( $report->getQueryFieldsParams(), ["company" => $company]));
        $client->transaction()->commit();

        return $this->getReportByUuid($report->getUuid());
    
    }
    public function updatereport($json) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $report = new Report();
        $this->mapper->map($json, $report);

        $report->setUuid($json->{'uuid'});
        
        $existing = $this->getReportByUuid($report->getUuid());
        if (NULL == $existing) {
            throw ApiException::serverError('Report not found');
        }
        $existing->import($report);
        $existing->setmfdate('');

        $this->dbentity->persist($existing);
        $this->dbentity->flush();
        return $existing;
    }
    
    public function deletereport($reportId) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
    }    
    
    public function binduser($reportId, $user) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
   
        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
        $query = "match(r:Report{uuid:{report}})
                    with r
                    match(u:User{username:{user}}) 
                    with r, u
                    create (u)-[:HAS_REPORT]->(r)";

        $client->run($query,   ["report" => $reportId, "user" => $user]);
        $client->transaction()->commit();

        return null;
    }   
    public function unbinduser($reportId, $user) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
   
        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
        $query = "match(:User{username:{user}})-[r:HAS_REPORT]->(:Report{uuid:{report}})
                  delete r";

        $client->run($query,   ["report" => $reportId, "user" => $user]);
        $client->transaction()->commit();
    }       

    public function getReportIsoData($reportId) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        return $this->getReportByUuid($reportId);
    }   
    
    private function getReportByUuid($uuid){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        return $this->reports->findOneBy(["uuid" => $uuid]);
    }    
    
    private function getReport($criteria){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        return $this->reports->findOneBy($criteria);
    }
    
    
}

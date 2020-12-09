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
use Slim\Http\Request;
use App\Models\Company;
use App\Models\Address;
use App\Models\Document;
//use App\Models\DocumentType;
use App\Common\ApiException;

class CompanyRepository {
    protected $logger;
    protected $dbclient;
    protected $dbentity;
    protected $mapper;
    protected $settings;
        
    private $connection;
    private $connalias;
    private $companies;
    private $addresses;
    private $documents;
    
    public function __construct(LoggerInterface $logger, \Slim\Collection $settings){
        $this->logger = $logger;
        $this->mapper = new \JsonMapper();
        //$this->mapper->setLogger($this->logger);
        $this->mapper->bStrictNullTypes = FALSE;
        $this->settings = $settings;
        
        $this->connection = $this->settings['dbclient']['type'].'://'.$this->settings['dbclient']['username'].':'.$this->settings['dbclient']['password'].'@'. $this->settings['dbclient']['host'].':'.$this->settings['dbclient']['port'];
        $this->connalias = $this->settings['dbclient']['name'];
        
        $this->dbclient = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
        $this->dbentity  = EntityManager::create($this->connection);

        $this->companies = $this->dbentity->getRepository(Company::class); 
        $this->addresses = $this->dbentity->getRepository(Address::class);   
        $this->documents = $this->dbentity->getRepository(Document::class);
    }      
    
    public function getAllCompanies(){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        $repository = $this->dbentity->getRepository(Company::class);
        $companies = $repository->findBy(["active" => 1, "deleted" => 0]);
        
        return $companies;
    }
    public function getCompanyDocumens($companyId){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $c = "match(t:DocumentType)-[rt:IS_OF_TYPE]-(d:Document)-[rd:ASSOCIATED_WITH]-(c:Company{uuid:{companyId}})
            match(d)-[rf:ATTACHED]-(f:File)
            return d";
        
        $query = $this->dbentity->createQuery($c);
        $query->addEntityMapping("d", Document::class);
        $query->setParameter("companyId", $companyId);
        
        $documents = $query->getResult();

        foreach ($documents as $doc) {
            $doc->getType();
        }
        
        return $documents;
    }
    
    public function getCompanyDocumensByType($companyId, $type){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $c = "match(t:DocumentType{name:{type}})-[rt:IS_OF_TYPE]-(d:Document)-[rd:ASSOCIATED_WITH]-(c:Company{uuid:{companyId}})
            match(d)-[rf:ATTACHED]-(f:File)
            return d";
        
        $query = $this->dbentity->createQuery($c);
        $query->addEntityMapping("d", Document::class);
        $query->setParameter("companyId", $companyId);
        $query->setParameter("type", $type);
        
        $documents = $query->getResult();

        foreach ($documents as $doc) {
            $doc->getType();
        }
        
        return $documents;
    }    
    
    public function addCompany($jsonData){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $company = $this->mapper->map($jsonData, new Company());
        

        $existing = $this->getCompanyByTaxNumber($company->getTaxnumber());
        if (NULL != $existing) {
            throw ApiException::serverError('Company allredy exists');
        }
        
        $this->dbentity->persist($company);
        $this->dbentity->flush();
        return $company;
    }
    public function updateCompany(Request $request, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            
        $jsonData = json_decode($request->getBody());
        if (json_last_error() !== JSON_ERROR_NONE) {
            throw ApiException::json_decodeError();
        }

        if (NULL === $jsonData) {
            throw ApiException::serverError('No data povided');
        }
        
        $company = $this->mapper->map($jsonData, new Company());
        
        $existing = $this->dbentity->getRepository(Company::class)->findOneBy(["uuid" => $company->getUuid()]);
        
        if (NULL === $existing) {
            throw ApiException::serverError('Company not found ');
        }

        $existing->import($company);

        $this->dbentity->persist($existing);
        $this->dbentity->flush();
        return $existing;
    }    
    
    public function addCompanyLogo($jsonData){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
        $query = 'match(d:Document{uuid:{docId}})-[r:IS_OF_TYPE]-(t:DocumentType{name:{type}}) return d';

        $result = $client->run($query,['docId' => $jsonData->{'document'}->{'uuid'}, 'type' => "LOGO"]);
        $rec = $result->firstRecordOrDefault(NULL);
        
        if (NULL === $rec) {
            return NULL;
        }

        $this->addDocument($jsonData->{'parentuuid'}, $jsonData->{'document'}->{'uuid'});
        return $company;
    }    
    
    public function getCompanyByTaxNumber($taxnumber) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        return $this->getCompany(['taxnumber' => $taxnumber]);
    }
    public function getCompanyByUuid($uuid) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        return $this->getCompany(['uuid' => $uuid]);
    }
    public function getCompanyByIdNumber($idnumber) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        return $this->getCompany(['idnumber' => $idnumbers]);
    }

    private function getCompany($criteria) {     
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $company = $this->companies->findOneBy($criteria);
        return $company;
    }   
    
    private function addDocument($companyId, $documentId){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
        $query = 'match(c:Company{uuid:{companyId}})
            match(d:Document{uuid:{documentId}})
            create unique (c)-[r:ASSOCIATED_WITH]->(d)';

        $client->run($query,['documentId' => $documentId, 'companyId' => $companyId]);
        $client->transaction()->commit(); 
    }    
    
}

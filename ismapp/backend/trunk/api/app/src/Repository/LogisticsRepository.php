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
use GraphAware\Neo4j\OGM\Query;
use App\Common\ApiException;
use App\Repository\CompanyRepository;

use App\Models\Employee;
use App\Models\EmployeeHire;
use App\Models\EmployeeFire;
use App\Models\Document;
use App\Models\Address;
use App\Models\Contact;
use App\Models\CSite;
use App\Models\ModelType;
use App\Models\EmployeeAddresses;
use App\Models\EmployeeContacts;
use App\Models\EmployeeDocuments;
use App\Models\EmployeeList;
use App\Models\Company;
use App\Models\Day;
use App\Models\Month;
use App\Models\Year;
use App\Models\WorkPeriod;
use App\Models\Language;
use App\Models\DocumentType;
use App\Models\CurrentWorkPeriod;
use App\Models\Occupancy;
use App\Models\Car;
use App\Models\Project;

class LogisticsRepository {
    protected $logger;
    protected $dbclient;
    protected $dbentity;
    protected $mapper;
    protected $settings;


    private $employees;
    private $contacts;
    private $addresses;
    private $documents;
    private $csites;
    private $companies;
    private $connection;
    private $connalias;
    private $periods;
    private $cars;

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

        
        $this->employees = $this->dbentity->getRepository(Employee::class);
        $this->contacts = $this->dbentity->getRepository(Contact::class);
        $this->addresses = $this->dbentity->getRepository(Address::class);
        $this->documents = $this->dbentity->getRepository(Document::class);
        $this->csites = $this->dbentity->getRepository(CSite::class);
        $this->companies = $this->dbentity->getRepository(Company::class);
        $this->periods = $this->dbentity->getRepository(WorkPeriod::class);
        $this->cars = $this->dbentity->getRepository(Car::class);
        
        $this->dbentity->clear();
        $this->dbentity->flush();
    }
     
    public function __destruct() {
        $this->dbentity->clear();
    }
    
    
    // <editor-fold defaultstate="collapsed" desc="public functions">

    public function listCars($companyId){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        $company = $this->companies->findOneBy(['uuid' => $companyId]);
        if (NULL === $company) {
            return;
        }
        $cql = " match (company:Company{uuid:{companyId}})<-[:BELONGS_TO]-(car:Car{active:1, deleted:0})
            with company, car 
            optional match (car)-[:OCCUPIES]->(o:Occupancy{active:1})-[:OCCUPIES]->(project:Project)-[:HAS]-(site:CSite)
            return company, car, project, site ";

        $query = $this->dbentity->createQuery($cql);
        
        $query->addEntityMapping('company', Company::class);
        $query->addEntityMapping('car', Car::class);
        $query->addEntityMapping('project', Project::class);
        $query->addEntityMapping('site', CSite::class);
        $query->setParameter('companyId', $companyId);

        return $query->execute();
    }
    public function listhomeCars($companyId){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        $company = $this->companies->findOneBy(['uuid' => $companyId]);
        if (NULL === $company) {
            return;
        }
        $cql = " match (company:Company/*{uuid:{companyId}}*/)<-[:BELONGS_TO]-(car:Car{active:1, deleted:0})
            with company, car
            match (car)-[:OCCUPIES]->(o:Occupancy{active:1})-[:OCCUPIES]->(company)
            where not ((car)-[:TRANSPORT]-(:Departure{status:0}))
            with company, car
            return company, car ";

        $query = $this->dbentity->createQuery($cql);
        
        $query->addEntityMapping('company', Company::class);
        $query->addEntityMapping('car', Car::class);
        $query->setParameter('companyId', $companyId);

        return $query->execute();
    }
    
    
    public function addCar($companyId, $jsonData){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $car = new Car();
        $this->mapper->map($jsonData, $car);
        
        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
        $query = "match(company:Company{uuid:{company}}) with company "
                . " match (year:Year{value:{y}})-[:CONTAINS]->(month:Month {value:{m}})-[:CONTAINS]->(today:Day{value:{d}}) with today, company "
                . " create (c:Car{".$car->getQueryFields()."}) "
                . " create (c)-[:BELONGS_TO]->(company)"
                . " create (o:Occupancy{active:1})-[:START]->(today) "
                . " create (c)-[:OCCUPIES]->(o)-[:OCCUPIES]->(company) ";

        $client->run($query, array_merge([
                            'y' => idate("Y"),
                            'm' => idate("m"),
                            'd' => idate("d"),
                            'company' => $companyId ], $car->getQueryFieldsParams()));
        $client->transaction()->commit();

        return $this->getCarByUuid($car->getUuid());
    }

    public function updateCar($jsonData){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $car = new Car();
        $this->mapper->map($jsonData, $car);
        
        $existing = $this->getCarByUuid($car->getUuid());
        if (NULL == $existing) {
            throw ApiException::serverError('Car not found');
        }
        $existing->import($car);
        $existing->setmfdate('');

        $this->dbentity->persist($existing);
        $this->dbentity->flush();
        return $existing;
    }
    public function deleteCar($jsonData){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $e = $this->cars->findOneBy(["uuid" => $jsonData->{"uuid"}]);
        $e->setDeleted(1);
        $this->dbentity->flush();
        return $e;
    }     
    
    public function getCarByRegistration($emso) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        return $this->getCar(['registration' => $emso]);
    }
    public function getCarByUuid($uuid) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        return $this->getCar(['uuid' => $uuid]);
    }

    
// </editor-fold>

// <editor-fold defaultstate="collapsed" desc="private functions">
    private function getCar($criteria) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        return $this->cars->findOneBy($criteria);
    }    
// </editor-fold>

}

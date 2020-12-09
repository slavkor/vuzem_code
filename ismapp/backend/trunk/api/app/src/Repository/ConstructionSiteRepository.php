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
use Slim\Http\Request;

use App\Models\CSite;
use App\Models\Company;
use App\Models\Range;
use App\Models\Project;
use App\Models\BusinessPartner;
use App\Models\Document;
use App\Models\Address;
use App\Models\Contact;
use App\Models\Day;
use App\Models\Employee;
use App\Models\BaseShift;
use App\Models\ShiftDay;
use App\Models\Car;
use App\Models\CSiteOverview;
use App\Models\EmployeeEx;
use App\Models\Departure;
use App\Models\DepartureList;
use App\Models\EmployeeOverview;
/**
 * Description of ConstructionSiteRepositorz
 *
 * @author slavko
 */
class ConstructionSiteRepository {
    protected $logger;
    protected $dbclient;
    protected $dbentity;
    protected $mapper;
    protected $settings;
    
    private $csites;
    private $companies;
    private $connection;
    private $connalias;
    private $documents;
    private $contacts;
    private $addresses;
    private $partners;
    
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

        $this->csites = $this->dbentity->getRepository(CSite::class);
        $this->companies = $this->dbentity->getRepository(Company::class);
        $this->documents = $this->dbentity->getRepository(Document::class);
        $this->addresses = $this->dbentity->getRepository(Address::class);
        $this->contacts = $this->dbentity->getRepository(Contact::class);
        $this->partners = $this->dbentity->getRepository(BusinessPartner::class);
        
    }
    
    // <editor-fold defaultstate="collapsed" desc="public functions Construction site  related">
    public function getAllSites() {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $sites = $this->csites->findAll();
        return $sites;
    }
    public function getAllCompanySites($companyId, $json) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $this->mapper->bStrictNullTypes =FALSE;
        $range = new Range();
        $this->mapper->map($json, $range);

        $cql = "match(:Year{value:{y}})-[:CONTAINS]->(:Month{value:{m}})-[:CONTAINS]->(ds:Day{value:{d}})-[:NEXT*0..". $range->getDuration()."]->(de:Day) with ds, de
                match (:Company/*{uuid:{company}}*/)-[:IS_FOR]-(site:CSite)-[:HAS]-(project:Project)-[:START|END]-(de) 
                return site, collect(distinct project) as projects ";   

        $query = $this->dbentity->createQuery($cql);
        $query->addEntityMapping('site', CSite::class, \GraphAware\Neo4j\OGM\Query::HYDRATE_SINGLE);
        $query->addEntityMapping('projects', Project::class, \GraphAware\Neo4j\OGM\Query::HYDRATE_COLLECTION);
        $query->setParameter('company', $companyId);
        $query->setParameter('y', $range->getFrom()->getMonth()->getYear()->getValue());
        $query->setParameter('m', $range->getFrom()->getMonth()->getValue());
        $query->setParameter('d', $range->getFrom()->getValue());

        $res =  $query->execute();
        return $res;        
    }
    public function getByEmployee($employee) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $cql = "match (employee:Employee{uuid:{employee}})-[:OCCUPIES]->(o:Occupancy)-[:OCCUPIES]->(project:Project)
            with project ,o, employee
            match(o)-[:START]-(d:Day)
            with d, o, employee, project
            order by d.strrep desc
            with employee, project, collect(o) as occus unwind occus as oc
            match(site:CSite)-[:HAS]->(project) 
            return site, project";           

        $query = $this->dbentity->createQuery($cql);
        $query->addEntityMapping('site', CSite::class, \GraphAware\Neo4j\OGM\Query::HYDRATE_SINGLE);
        $query->addEntityMapping('project', Project::class, \GraphAware\Neo4j\OGM\Query::HYDRATE_SINGLE);
        $query->setParameter('employee', $employee);

        $res =  $query->execute()[0];
        return $res;        
    }    
    public function getWorkShiftsByDay($site, $jsonData) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $this->mapper->bStrictNullTypes = FALSE;

        $day = new Day();
        $this->mapper->map($jsonData, $day);

        $cql = "match(site:CSite{uuid:{site}})-[:HAS]-(project:Project)<-[:CONFIRMED_ARRIVAL{year:{y}, month:{m}}]-(employee:Employee)
            with employee, project
            match(:Year{value:{y}})-[:CONTAINS]->(:Month {value:{m}})-[:CONTAINS]->(day:Day {value:{d}})
            with employee, project, day
            optional match(employee)-[:WORKS]->(dayshift:Shift{shifttype:0})-[:WORKDAY]->(day)  
            optional  match(dayshift)-[:WORKS]->(project)
            optional match(employee)-[:WORKS]->(nightshift:Shift{shifttype:1})-[:WORKDAY]->(day)  
            optional  match(nightshift)-[:WORKS]->(project)            
            return employee, dayshift, nightshift";
        
        $query = $this->dbentity->createQuery($cql);
        $query->addEntityMapping('employee', Employee::class );
        $query->addEntityMapping('dayshift', BaseShift::class );
        $query->addEntityMapping('nightshift', BaseShift::class);
        $query->setParameter('site', $site);
        $query->setParameter('y', $day->getYearValue());
        $query->setParameter('m', $day->getMonthValue());
        $query->setParameter('d', $day->getValue());

        return $query->getResult();
        
    }
    public function getWorkShifts($site) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
       
        $cql = "match (day:Day)<-[:WORKDAY]-(shift:Shift)-[:WORKS]->(project:Project)<-[:HAS]-(site:CSite{uuid:{site}}) "
                . " return day.strrep, shift.shifttype, sum(shift.hours) as hours ";
   
        $result = $this->dbclient->run($cql, ['site' => $site]);
  
        
        
        foreach ($result->records() as $record) {
            $dd= new ShiftDay();
            $dd->setStrrep($record->get("day.strrep"));
            $dd->setShifttype($record->get("shift.shifttype"));
            $dd->setHours($record->get("hours"));
            $rec[] = $dd;
        }
 
        return $rec;
        
    }    
    public function addCSide($companyId, $jsonData) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $this->mapper->bStrictNullTypes = FALSE;

        $site = new CSite();
        $this->mapper->map($jsonData, $site);


        $company = $this->companies->findOneBy(["uuid" => $companyId]);
        $site->setCompany($company);
        
        if(NULL !==$site->getCustomer()){
            $site->setCustomer($this->partners->findOneBy(["uuid" => $site->getCustomer()->getUuid()]));
        }
        
        $this->dbentity->persist($site);
        $this->dbentity->flush();

        return $site;
    }
    public function updateCSite($jsonData) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $this->mapper->bStrictNullTypes = FALSE;
        $site = new CSite();
        $this->mapper->map($jsonData, $site);
        $existing = $this->getCsiteByUuid($jsonData->{'uuid'});
        if (NULL == $existing) {
            throw ApiException::serverError('ConstructionSite not found');
        }

        $existing->import($site);

        $customerJsonData = $jsonData->{'customer'};
        if(NULL !==$customerJsonData){
            $customer = new BusinessPartner();
            $this->mapper->map($customerJsonData, $customer);   
            $this->addCust($existing, $customer);            
        }
         
        $this->dbentity->persist($existing);
        $this->dbentity->flush();
        return $existing;
    }
    public function getActiveSites(Request $request, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
//        $cql = "match(c:CSite)-[]-(p:Project)-[]-(o:Occupancy{active:1})-[]-(e:Employee)
//                return c as site, collect(e) as employee";
//   
//        $result = $this->dbclient->run($cql);
//        
//        foreach ($result->records() as $record) {
//            $site = new \App\Models\CSiteReport();
//            
//            foreach ($record->get("employee") as $employee) {
//                $e= new Employee();
//                $e->setname($employee->get("name"));
//                $e->setlastname($employee->get("lastname"));
//                $emps[] = $e;
//            }
//
//            $site->setEmployees($emps);
//            $rec[] = $site;
//        }
// 
//        return $rec;

        $cql = "match(c:CSite)-[:HAS]->(p:Project)<-[:OCCUPIES]-(o:Occupancy{active:1})
                return distinct c as site ";         

        $query = $this->dbentity->createQuery($cql);
        $query->addEntityMapping('site', CSite::class);

        $res =  $query->getResult();
        return $res;          
    }
    public function getActiveSitesOverview(Request $request, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $cql = "match(site:CSite)-[:HAS]->(p:Project)<-[:OCCUPIES]-(o:Occupancy{active:1})<-[:OCCUPIES]-(employee:Employee) with site, employee
            match(employee)-[:WORKS_IN]->(:WorkPeriod{active:1})-[:WORKS_IN]->(company:Company) with site, employee, company
            optional match(site)-[:IS_FOR]->(partner:BusinessPartner) with site, employee, company, partner
            return distinct site.uuid as siteid, site.name as sitename, partner.name as partner, 
            employee.lastname as lastname, employee.name as employeename, employee.uuid as employeeid, 
            company.color as color
            order by sitename, partner, lastname,employeename ";  
        
        $records = $this->dbclient->run($cql)->records();
        foreach ($records as $record) {
            
            $rec[] = [                
                "sitename" => $record->get("sitename"),
                "partner"=> null, //$record->get("partner"),
                "siteid" => $record->get("siteid"),                
                "lastname" => $record->get("lastname"),
                "employeename" => $record->get("employeename"),
                "employeeid" => $record->get("employeeid"),
                "color" => $record->get("color"),
                "departures" => $this->getActiveSitesOverviewEmployeesDepartures($record->get("employeeid")),
                ];
        }
        return $rec;    
    }


    public function getActiveSitesOverviewEmployeesDepartures($employee){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

//       $cql = "match(employee:Employee{uuid:{employee}})-[:DEPARTS_IN]->(departure:Departure{status:0, deleted:0, active:1})-[:TO]->(destination) with departure
//            match(departure)-[:FROM]->(origin) with departure, origin
//            match(departure)-[:TO]->(destination) with departure, origin, destination
//            optional match (origin)<-[:HAS]-(osite:CSite)-[:IS_FOR]->(opartner:BusinessPartner)
//            optional match (destination)<-[:HAS]-(dsite:CSite)-[:IS_FOR]->(dpartner:BusinessPartner)
//            return departure.uuid as uuid, departure.departtime as departtime, origin.uuid as ouuid, destination.uuid as duuid,
//            case when osite is null then origin.name else osite.name + '(' + opartner.name + ')' end as origin,
//            case when dsite is null then destination.name else dsite.name + '(' + dpartner.name + ')' end as destination";        
       $cql = "match(employee:Employee{uuid:{employee}})-[:DEPARTS_IN]->(departure:Departure{status:0, deleted:0, active:1})-[:TO]->(destination) with departure
            match(departure)-[:FROM]->(origin) with departure, origin
            match(departure)-[:TO]->(destination) with departure, origin, destination
            optional match (origin)<-[:HAS]-(osite:CSite)-[:IS_FOR]->(opartner:BusinessPartner)
            optional match (destination)<-[:HAS]-(dsite:CSite)-[:IS_FOR]->(dpartner:BusinessPartner)
            return departure.uuid as uuid, departure.departtime as departtime, origin.uuid as ouuid, destination.uuid as duuid,
            case when osite is null then origin.name else osite.name end as origin,
            case when dsite is null then destination.name else dsite.name end as destination";        


        $records = $this->dbclient->run($cql,['employee' => $employee])->records();
        foreach ($records as $record) {
            $rec[] = [                
                "uuid" => $record->get("uuid"),
                "departtime" => $record->get("departtime"),
                "origin" => $record->get("origin"),
                "destination" => $record->get("destination"),
                ];
        }
        return $rec; 
    }    



    
    // </editor-fold>
    
    // <editor-fold defaultstate="collapsed" desc="public functions Documents related">
    public function getDocuments($uuid)   {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $q = "match (dt:DocumentType{deleted:0})
            with dt
            match (d:Document{deleted:0})-[rt:IS_OF_TYPE]->(dt)
            with d
            where not((d)-[:NEXT]->(:Document))
            match (c:CSite{uuid:{uuid}})-[r:ASSOCIATED_WITH{deleted:0}]->(d) return c, collect(d) as documents";
        
        $query = $this->dbentity->createQuery($q);
        $query->addEntityMapping("c", CSite::class);
        $query->addEntityMapping("documents", Document::class, Query::HYDRATE_COLLECTION);
        $query->setParameter("uuid", $uuid);
        
        $e = $query->getResult()[0];       
        foreach ($e["documents"] as $doc) {
            $doc->getType();           
            foreach ($doc->getFiles() as $file) {
                $file->getLanguage();
            }
        }  
        return $e;
    }
    public function addDocument($jsonData){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $site = $this->getCsiteByUuid($jsonData->{'parentuuid'});
        if (NULL === $site) {
            throw ApiException::serverError('Construction site not found');
        }
        $document = $this->documents->findOneBy(['uuid' => $jsonData->{'document'}->{'uuid'}]);
        
        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
        $query = 'match(s:CSite{uuid:{site}})
            match(d:Document{uuid:{documentId}})
            create (s)-[r:ASSOCIATED_WITH{active:1, deleted:0}]->(d)';

        $client->run($query,['documentId' => $document->getUuid(),
            'site' => $site->getUuid()]);
        $client->transaction()->commit(); 
        return $this->getCsiteByUuid($jsonData->{'parentuuid'});
    } 
    public function deleteDocument($jsonData){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $existing = $this->documents->findOneBy(['uuid' => $jsonData->{'uuid'}]);
        if (NULL == $existing) {
            throw ApiException::serverError('Document not found');
        }
        $existing->setDeleted(1);
        $this->dbentity->flush();
        return $existing;
    } 
    
    // </editor-fold>
    
    // <editor-fold defaultstate="collapsed" desc="public functions Customer related">
    public function addCustomer($jsonData){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $site = $this->getCsiteByUuid($jsonData->{'parentuuid'});
        if (NULL === $site) {
            throw ApiException::serverError('Construction site not found');
        }

        $customerJsonData = $jsonData->{'customer'};
        $customer = $this->mapper->map($customerJsonData, new BusinessPartner());

        $this->addCust($site, $customer);
        
        return $this->getCsiteByUuid($jsonData->{'parentuuid'});        
    }
    // </editor-fold>

    // <editor-fold defaultstate="collapsed" desc="public functions Contacts related">
    public function getContacts($uuid)   {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $query = $this->dbentity->createQuery("match(s:CSite{uuid:{site}})-[r:CONTACT{deleted:0}]-(c:Contact{deleted:0})
                return s, collect(c) as contacts");
        $query->addEntityMapping("s", CSite::class);
        $query->addEntityMapping("contacts", Contact::class, Query::HYDRATE_COLLECTION);
        $query->setParameter("site", $uuid);
        return $query->getResult()[0];        
       
    }
    public function addContact($jsonData){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $site = $this->getCsiteByUuid($jsonData->{'parentuuid'});
        if (NULL === $site) {
            throw ApiException::serverError('Construction site not found');
        }
        $contactJsonData = $jsonData->{'contact'};
        $contact = new Contact();
        $this->mapper->map($contactJsonData, $contact);

        $existsing = $this->contacts->findOneBy(['uuid'=>$contact->getUuid()]);
        if(NULL !== $existsing)
        {
            $existsing->import($contact);
            $this->dbentity->persist($existsing);
        }
        else{
            $this->persistContact($site, $contact);
        }
        return $this->getCsiteByUuid($site->getUuid());
    }
    public function updateContact($jsonData){
        
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        $contactJsonData = $jsonData->{'contact'};

        $contact = $this->mapper->map($contactJsonData, new Contact());

        
        $existing = $this->contacts->findOneBy(['uuid'=>$contactJsonData->{'uuid'}]);
        if (NULL == $existing) {
            throw ApiException::serverError('Contact not found');
        }

        $existing->import($contact);
        $existing->setmfdate('');

        $this->dbentity->persist($existing);
        $this->dbentity->flush();

        return $existing;
    }
    public function deleteContact($jsonData){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $existing = $this->contacts->findOneBy(['uuid' => $jsonData->{'uuid'}]);
        if (NULL == $existing) {
            throw ApiException::serverError('Contact not found');
        }
        $existing->setDeleted(1);
        $this->dbentity->flush();
        return $existing;
    }
    // </editor-fold>

    // <editor-fold defaultstate="collapsed" desc="public functions Address related">
    public function getAddresses($uuid)   {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $query = $this->dbentity->createQuery("match(s:CSite{uuid:{site}})-[r:LIVES{deleted:0}]-(a:Address{deleted:0}) 
            return s, collect(a) as addresses");
        $query->addEntityMapping("s", CSite::class);
        $query->addEntityMapping("addresses", Address::class, Query::HYDRATE_COLLECTION);
        $query->setParameter("site", $uuid);
        return $query->getResult()[0];
    }
    public function addAddress($jsonData){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        $site = $this->getCsiteByUuid($jsonData->{'parentuuid'});

        if (NULL === $site) {
            throw ApiException::serverError('Construction site not found');
        }
        
        $addressJsonData = $jsonData->{'address'};
        $address = $this->mapper->map($addressJsonData, new Address());
        $this->persistAddress($site, $address);
        
        return $this->getCsiteByUuid($jsonData->{'parentuuid'});
    }
    public function updateAddress($jsonData){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $addressJsonData = $jsonData->{'address'};
        $address = $this->mapper->map($addressJsonData, new Address());
        
        $existing = $this->addresses->findOneBy(['uuid' => $addressJsonData->{'uuid'}]);
        
        if (NULL == $existing) {
            throw ApiException::serverError('Address not found');
        }

        $existing->import($address);
        $existing->setmfdate('');

        $this->dbentity->persist($existing);
        $this->dbentity->flush();

        return $existing;
    } 
    public function deleteAddress($jsonData){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $existing = $this->addresses->findOneBy(['uuid' => $jsonData->{'uuid'}]);
        if (NULL == $existing) {
            throw ApiException::serverError('Address not found');
        }
        $existing->setDeleted(1);
        $this->dbentity->flush();
        return $existing;
    } 

// </editor-fold>
        
    // <editor-fold defaultstate="collapsed" desc="private functions Construction site related">
    private function getCsiteByUuid($id){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        return $this->csites->findOneBy(['uuid' => $id]);
    }
    // </editor-fold>

    // <editor-fold defaultstate="collapsed" desc="private functions Customer related">
    private function addCust(CSite $site, BusinessPartner $customer) {
        
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        if (NULL === $site) {
            throw ApiException::serverError('Construction site  not foud');
        }
        if (NULL === $customer) {
            throw ApiException::serverError('Businesspartner not foud');
        }
        
        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();

        $stack = $client->stack();
        
        $cql = 'match(e:CSite{uuid:{site}})-[r:IS_FOR]-(:BusinessPartner) delete r';
        $stack->push($cql, ["site" => $site->getUuid()]);
        
        $cql = 'match(e:CSite{uuid:{site}}) match(b:BusinessPartner{uuid:{customer}}) create (e)-[:IS_FOR]->(b)';
        $stack->push($cql, ["site" => $site->getUuid(), "customer" => $customer->getUuid()]);

        $client->runStack($stack);
        $client->transaction()->commit();
    } 
    // </editor-fold>

    // <editor-fold defaultstate="collapsed" desc="private functions Contact related">
    private function persistContact(CSite $site, Contact $contact) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        if (NULL === $site) {
            throw ApiException::serverError('Construction site not foud');
        }
        if (NULL === $contact) {
            throw ApiException::serverError('Contact not foud');
        }
        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
        $existing = $this->contacts->findOneBy(['name' => $contact->getname(), 'lastname' => $contact->getlastname(), 'phone' => $contact->getphone()]);

        if ($existing !== NULL) { return $this->bindContact($site, $existing); }

        $query = 'match(s:CSite{uuid:{site}})
            create (c:Contact{'. $contact->getQueryFields() .'})
            create (s)-[r:CONTACT{active:1, deleted:0}]->(c)';

        $client->run($query,array_merge(['site' => $site->getUuid()], $contact->getQueryFieldsParams()));
        $client->transaction()->commit(); 
            
        return $site;
    }
    private function bindContact(CSite $site, Contact $contact) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        if (NULL === $site) {
            throw ApiException::serverError('Construction site not foud');
        }
        if (NULL === $contact) {
            throw ApiException::serverError('Contact not foud');
        }
        $query = 'match(s:CSite{uuid:{site}}) match(c:Contact{uuid:{contactId}}) create (s)-[r:CONTACT{active:1, deleted:0}]->(c)';
        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
        $client->run($query,array_merge(['contactId' => $contact->getUuid(), 'site' => $site->getUuid()], $contact->getQueryFieldsParams()));
        $client->transaction()->commit(); 
        return $site;
    }  
    // </editor-fold>

    // <editor-fold defaultstate="collapsed" desc="private functions Address related">
    private function persistAddress(CSite $site, Address $address) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        if (NULL === $site) {
            throw ApiException::serverError('Constriction site not foud');
        }
        if (NULL === $address) {
            throw ApiException::serverError('Address not foud');
        }
        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
        $query = 'match(s:CSite{uuid:{site}})
                create (a:Address{'.$address->getQueryFields(). '})
                create UNIQUE (e)-[r:LIVES{active:1, deleted:0}]->(a)';

        $client->run($query, array_merge($address->getQueryFieldsParams(), ['site'=>$site->getUuid()]));
        $client->transaction()->commit();   
        return $site;
    }      
    // </editor-fold>
    
    public function getActiveCars($site){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $cql = "match(car:Car)-[:OCCUPIES]->(o:Occupancy{active:1})-[:OCCUPIES]->(project:Project)<-[:HAS]-(site:CSite{uuid:{site}})
            with car
            match (car)-[:BELONGS_TO]->(c:Company)
            return c as company, car";
        $query = $this->dbentity->createQuery($cql);
        $query->setParameter("site", $site);
        $query->addEntityMapping('car', Car::class);
        $query->addEntityMapping('company', Company::class);
        
        return $query->execute();
    }
    public function getActiveEmployees($site){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $cql = "match(employee:Employee)-[:OCCUPIES]->(o:Occupancy{active:1})-[:OCCUPIES]->(project:Project)<-[:HAS]-(site:CSite{uuid:{site}})
            return employee";
        $query = $this->dbentity->createQuery($cql);
        $query->setParameter("site", $site);
        $query->addEntityMapping('employee', \App\Models\EmployeeList::class);
        $res =  $query->getResult();
        return $res;  
    }       
    public function getDepartureStats(Request $request, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $cql = "match (b:BusinessPartner)-[]-(c:CSite{uuid:{site}})-[]-(p:Project)-[r]-(d:Departure{status:1, deleted:0})-[]-(e:Employee) return b.name + ' ' + c.name as site, count(e) as cnt, d.departtime as dep, type(r) as dir order by dep desc";
        $query = $this->dbentity->createQuery($cql);
        $query->setParameter("site", $args["id"]);
        return $query->getResult();
    }        
    public function getDepartureHistory(Request $request, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $cql = "match (b:BusinessPartner)-[]-(c:CSite{uuid:{site}})-[]-(p:Project)-[r]-(d:Departure{status:1, deleted:0})-[]-(e:Employee)
            optional match(d)-[]-(car:Car)
            return b.name + ' ' + c.name as site, e.lastname + ' ' + e.name as employee, d.departtime as dep, collect(car.registration) as cars, type(r) as dir 
            order by dep desc";
        $query = $this->dbentity->createQuery($cql);
        $query->setParameter("site", $args["id"]);
        return $query->getResult();
    }    
    
}

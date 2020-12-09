<?php

namespace App\Repository;
use Slim\Http\Request;
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
use App\Models\Departure;
use App\Models\DepartureList;
use App\Models\Range;
use App\Models\Project;
use App\Models\Car;
use App\Models\DepartureReport;
use App\Models\DepartureEx;
use App\Models\EmployeeEx;
use App\Models\DepartureRelationEx;
use App\Models\DepartureCarRelation;


class DepartureArrivalRepository {
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

    private $departures;
    private $days;

    public function __construct(LoggerInterface $logger, \Slim\Collection $settings){
        $this->logger = $logger;
        $this->mapper = new \JsonMapper();
        $this->mapper->bStrictNullTypes = FALSE;
        //$this->mapper->setLogger($this->logger);
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
        $this->departures = $this->dbentity->getRepository(Departure::class);
        $this->days = $this->dbentity->getRepository(Day::class);
        $this->dbentity->clear();
        $this->dbentity->flush();
    }
     
    public function __destruct() {
        $this->dbentity->clear();
    }
    
    
    // <editor-fold defaultstate="collapsed" desc="public functions - departure replated">
    public function listDepartures($jsonData) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        $range = new Range();
        $this->mapper->bStrictNullTypes = FALSE;
        $this->mapper->map($jsonData, $range);
  
        
        if(NULL !== $range->getOrigin()){
            $cql = "match(year:Year{value:{y}})-[:CONTAINS]->(m:Month{value:{m}})-[:CONTAINS]->(day:Day{value:{d}})
                with day
                match(day)-[:NEXT*0..". $range->getDuration() ."]->(dayend:Day)
                with day, dayend
                match(depart:Departure{deleted:0, active:1, internal:false})-[:DEPARTDATE]->(dayend)
                with collect(distinct depart) as departures unwind departures as departure 
                match(departure)-[:FROM]->(fromproject:Project)-[HAS]-(:CSite{uuid:{origin}})
                match(departure)-[:TO]->(tocompany:Company)
                with departure, fromproject, tocompany
                optional match(e:Employee)-[:DEPARTS_IN]->(departure)
                with departure, fromproject, tocompany, collect(e) as employees 
                optional match(c:Car)-[:TRANSPORT]->(departure)
                with departure,fromproject, tocompany, employees,collect(c) as cars
                return departure, employees, cars, null as fromcompany, fromproject, tocompany,  null as toproject
                union
                match(year:Year{value:{y}})-[:CONTAINS]->(m:Month{value:{m}})-[:CONTAINS]->(day:Day{value:{d}})
                with day
                match(day)-[:NEXT*0..". $range->getDuration() ."]->(dayend:Day)
                with day, dayend
                match(depart:Departure{deleted:0, active:1, internal:false})-[:DEPARTDATE]->(dayend)
                with collect(distinct depart) as departures unwind departures as departure 
                match(departure)-[:FROM]->(fromproject:Project)-[HAS]-(:CSite{uuid:{origin}})
                match(departure)-[:TO]->(toproject:Project)
                with departure, fromproject, toproject
                optional match(e:Employee)-[:DEPARTS_IN]->(departure)
                with departure, fromproject, toproject, collect(e) as employees 
                optional match(c:Car)-[:TRANSPORT]->(departure)
                with departure,fromproject, toproject, employees,collect(c) as cars
                return departure, employees, cars, null as fromcompany, fromproject, null as tocompany,  toproject
                union
                match(year:Year{value:{y}})-[:CONTAINS]->(m:Month{value:{m}})-[:CONTAINS]->(day:Day{value:{d}})
                with day
                match(day)-[:NEXT*0..". $range->getDuration() ."]->(dayend:Day)
                with day, dayend
                match(depart:Departure{deleted:0, active:1, internal:false})-[:DEPARTDATE]->(dayend)
                with collect(distinct depart) as departures unwind departures as departure 
                match(departure)-[:TO]->(toproject:Project)-[HAS]-(:CSite{uuid:{origin}})
                with departure, toproject
                match(departure)-[:FROM]->(fromcompany:Company)
                with departure, toproject, fromcompany
                optional match(e:Employee)-[:DEPARTS_IN]->(departure)
                with departure,  toproject, fromcompany, collect(e) as employees 
                optional match(c:Car)-[:TRANSPORT]->(departure)
                with departure, toproject, fromcompany, employees,collect(c) as cars
                return departure, employees, cars, fromcompany, null as fromproject, null as tocompany,  toproject
                union
                match(year:Year{value:{y}})-[:CONTAINS]->(m:Month{value:{m}})-[:CONTAINS]->(day:Day{value:{d}})
                with day
                match(day)-[:NEXT*0..". $range->getDuration() ."]->(dayend:Day)
                with day, dayend
                match(depart:Departure{deleted:0, active:1, internal:false})-[:DEPARTDATE]->(dayend)
                with collect(distinct depart) as departures unwind departures as departure 
                match(departure)-[:TO]->(toproject:Project)-[HAS]-(:CSite{uuid:{origin}})
                with departure, toproject
                match(departure)-[:FROM]->(fromproject:Project)
                with departure, toproject, fromproject
                optional match(e:Employee)-[:DEPARTS_IN]->(departure)
                with departure,  toproject, fromproject, collect(e) as employees 
                optional match(c:Car)-[:TRANSPORT]->(departure)
                with departure, toproject, fromproject, employees,collect(c) as cars
                return departure, employees, cars, null as fromcompany,  fromproject, null as tocompany,  toproject";
        }
        else{            
        $cql = "match(year:Year{value:{y}})-[:CONTAINS]->(m:Month{value:{m}})-[:CONTAINS]->(day:Day{value:{d}})
                with day
                match(day)-[:NEXT*0..". $range->getDuration() ."]->(dayend:Day)
                with day, dayend
                match(depart:Departure{deleted:0, active:1, internal:false})-[:DEPARTDATE]->(dayend)
                with collect(distinct depart) as departures unwind departures as departure 
                optional match(e:Employee)-[:DEPARTS_IN]->(departure)
                with departure, collect(e) as employees 
                optional match(c:Car)-[:TRANSPORT]->(departure)
                with departure,employees,collect(c) as cars
                optional match(departure)-[:FROM]->(fromcompany:Company)
                optional match(departure)-[:FROM]->(fromproject:Project)
                optional match(departure)-[:TO]->(tocompany:Company)
                optional match(departure)-[:TO]->(toproject:Project)                
                return departure, employees, cars, fromcompany, fromproject, tocompany, toproject ";        
        }

        $query = $this->dbentity->createQuery($cql);
        $query->addEntityMapping('departure', Departure::class);
        $query->addEntityMapping('employees', Employee::class, Query::HYDRATE_COLLECTION);
        $query->addEntityMapping('cars', Car::class, Query::HYDRATE_COLLECTION);
        $query->addEntityMapping('fromcompany', Company::class);
        $query->addEntityMapping('fromproject', Project::class );
        $query->addEntityMapping('tocompany', Company::class);
        $query->addEntityMapping('toproject', Project::class);
        $query->setParameter('y', $range->getFrom()->getMonth()->getYear()->getValue());
        $query->setParameter('m', $range->getFrom()->getMonth()->getValue());
        $query->setParameter('d', $range->getFrom()->getValue());
        if($range->getOrigin() !== NULL){
            $query->setParameter('origin', $range->getOrigin()->getUuid());
            $query->setParameter('destination', $range->getDestination()->getUuid());
        }
        
        $res =  $query->execute();

        return $res;       
        
    }
      
    public function addDeparture($jsonData) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $departure = new Departure();
        $this->mapper->map($jsonData, $departure);

        $d = \DateTime::createFromFormat('YmdHis', $departure->getDeparttime());
        
//        $addEmployees = $this->mapper->mapArray($jsonData->{"employeesadd"}, array(), Employee::class);
//        $addCars = $this->mapper->mapArray($jsonData->{"carsadd"}, array(), Car::class);
//        
        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
        $stack = $client->stack();
        
        $query = "match(y:Year{value:{y}})-[:CONTAINS]->(m:Month{value:{m}})-[:CONTAINS]->(d:Day{value:{d}})
            create (dp:Departure{" . $departure->getQueryFields() . "})
            create (dp)-[:DEPARTDATE]->(d)";

        $stack->push($query, array_merge($departure->getQueryFieldsParams(), 
                [
                    'y' => (int)$d->format('Y'),
                    'm' => (int)$d->format('m'),
                    'd' => (int)$d->format('d')]
                ));
        
       
        $query="match(dp:Departure{uuid:{departure}}) "
                . " match(o:".$departure->getOrigin()->NodeName()."{uuid:{origin}}) "
                . " create (dp)-[:FROM]->(o) ";
        $stack->push($query, ["departure" => $departure->getUuid(), "origin" => $departure->getOrigin()->getUuid()]);

        $query="match(dp:Departure{uuid:{departure}}) "
                . " match(d:".$departure->getDestination()->NodeName()."{uuid:{destination}}) "
                . " create (dp)-[:TO]->(d) ";
        $stack->push($query, ["departure" => $departure->getUuid(), "destination" => $departure->getDestination()->getUuid()]);
//
//        foreach ($addEmployees as $employee) {
//            $query = "match(e:Employee{uuid:{employee}}) 
//                //where not ((e)-[:DEPARTS_IN]-(:Departure{status:0}))
//                with e
//                match(dp:Departure{uuid:{departure}})
//                create (e)-[:DEPARTS_IN]->(dp)";
//            $stack->push($query, ['employee' => $employee->getUuid(), "departure" => $departure->getUuid()]);
//        }
//        
//        foreach ($addCars as $car) {
//            $query = "match(c:Car{uuid:{car}}) 
//                where not ((c)-[:TRANSPORT]-(:Departure{status:0}))
//                match(dp:Departure{uuid:{departure}})
//                create (c)-[:TRANSPORT]->(dp)";
//            $stack->push($query, ['car' => $car->getUuid(), "departure" => $departure->getUuid()]);
//        }        
//        
        $client->runStack($stack);
        $client->transaction()->commit();
        return $this->getDepartureByUuid($departure->getUuid());        
    }
    public function updateDeparture($jsonData) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $departure = new Departure();
        $this->mapper->map($jsonData, $departure);

        $d = \DateTime::createFromFormat('YmdHis', $departure->getDeparttime());

//        $removeEmployees = $this->mapper->mapArray($jsonData->{"employeesremove"}, array(), Employee::class);
//        $addEmployees = $this->mapper->mapArray($jsonData->{"employeesadd"}, array(), Employee::class);
//
//        $removeCars = $this->mapper->mapArray($jsonData->{"carsremove"}, array(), Car::class);
//        $addCars = $this->mapper->mapArray($jsonData->{"carsadd"}, array(), Car::class);
        
        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
        $stack = $client->stack();
        
        $query="match(dp:Departure{uuid:{departure}})-[r:DEPARTDATE]->() delete r";
        $stack->push($query, ["departure" => $departure->getUuid()]);
        
        $query = "match(y:Year{value:{y}})-[:CONTAINS]->(m:Month{value:{m}})-[:CONTAINS]->(d:Day{value:{d}})
            match(dp:Departure{uuid:{departure}})
            create (dp)-[:DEPARTDATE]->(d)";

        $stack->push($query,  
                [   
                    "departure" => $departure->getUuid(),
                    'y' => (int)$d->format('Y'),
                    'm' => (int)$d->format('m'),
                    'd' => (int)$d->format('d')]
                );

        $query="match(dp:Departure{uuid:{departure}})-[r:FROM]->() delete r";
        $stack->push($query, ["departure" => $departure->getUuid()]);

        $query="match(dp:Departure{uuid:{departure}}) "
                . " match(o:".$departure->getOrigin()->NodeName()."{uuid:{origin}}) "
                . " create (dp)-[:FROM]->(o) ";
        $stack->push($query, ["departure" => $departure->getUuid(), "origin" => $departure->getOrigin()->getUuid()]);

        $query="match(dp:Departure{uuid:{departure}})-[r:TO]->() delete r ";
        $stack->push($query, ["departure" => $departure->getUuid()]);

        $query="match(dp:Departure{uuid:{departure}}) "
                . " match(d:".$departure->getDestination()->NodeName()."{uuid:{destination}}) "
                . " create (dp)-[:TO]->(d) ";
        $stack->push($query, ["departure" => $departure->getUuid(), "destination" => $departure->getDestination()->getUuid()]);
        
//        foreach ($removeEmployees as $employee) {
//            $query = "match(e:Employee{uuid:{employee}})-[r:DEPARTS_IN]->(dp:Departure{uuid:{departure}})
//                delete r";
//            $stack->push($query, ['employee' => $employee->getUuid(), "departure" => $departure->getUuid()]);
//        }
//        
//        foreach ($addEmployees as $employee) {
//            $query = "match(e:Employee{uuid:{employee}}) 
//                match(dp:Departure{uuid:{departure}})
//                create (e)-[:DEPARTS_IN]->(dp)";
//            $stack->push($query, ['employee' => $employee->getUuid(), "departure" => $departure->getUuid()]);
//        }
//
//        foreach ($removeCars as $car) {
//            $query = "match(c:Car{uuid:{car}})-[r:TRANSPORT]->(dp:Departure{uuid:{departure}})
//                delete r";
//            $stack->push($query, ['car' => $car->getUuid(), "departure" => $departure->getUuid()]);
//        }
//        
//        foreach ($addCars as $car) {
//            $query = "match(c:Car{uuid:{car}}) 
//                match(dp:Departure{uuid:{departure}})
//                create (c)-[:TRANSPORT]->(dp)";
//            $stack->push($query, ['car' => $car->getUuid(), "departure" => $departure->getUuid()]);
//        }        
//       
        $client->runStack($stack);
        $client->transaction()->commit();
        
        
        $existing = $this->getDepartureByUuid($departure->getUuid());
        $existing->import($departure);
        $existing->setmfdate('');

        $this->dbentity->persist($existing);
        $this->dbentity->flush();
        
        return $existing;
    }

    public function addConfirmDeparture($jsonData){
       if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $departure = new Departure();
        $this->mapper->map($jsonData, $departure);

        $d = \DateTime::createFromFormat('YmdHis', $departure->getDeparttime());

        $type = $departure->getOrigin()->getDeparttype() == "COMPANY" ? 0 : 1;
        
        $addEmployees = $this->mapper->mapArray($jsonData->{"employeesadd"}, array(), Employee::class);
        $addCars = $this->mapper->mapArray($jsonData->{"carsadd"}, array(), Car::class);
        
        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
        $stack = $client->stack();
        
        $query = "match(y:Year{value:{y}})-[:CONTAINS]->(m:Month{value:{m}})-[:CONTAINS]->(d:Day{value:{d}})
            create (dp:Departure{" . $departure->getQueryFields() . "})
            create (dp)-[:DEPARTDATE]->(d)";

        $stack->push($query, array_merge($departure->getQueryFieldsParams(), 
                [
                    'y' => (int)$d->format('Y'),
                    'm' => (int)$d->format('m'),
                    'd' => (int)$d->format('d')]
                ));
        
       
        $query="match(dp:Departure{uuid:{departure}}) "
                . " match(o:".$departure->getOrigin()->NodeName()."{uuid:{origin}}) "
                . " create (dp)-[:FROM]->(o) ";
        $stack->push($query, ["departure" => $departure->getUuid(), "origin" => $departure->getOrigin()->getUuid()]);

        $query="match(dp:Departure{uuid:{departure}}) "
                . " match(d:".$departure->getDestination()->NodeName()."{uuid:{destination}}) "
                . " create (dp)-[:TO]->(d) ";
        $stack->push($query, ["departure" => $departure->getUuid(), "destination" => $departure->getDestination()->getUuid()]);

        foreach ($addEmployees as $employee) {
            $query = "match(e:Employee{uuid:{employee}}) 
                //where not ((e)-[:DEPARTS_IN]-(:Departure{status:0}))
                with e
                match(dp:Departure{uuid:{departure}})
                create (e)-[:DEPARTS_IN]->(dp)";
            $stack->push($query, ['employee' => $employee->getUuid(), "departure" => $departure->getUuid()]);
        }
        
        foreach ($addCars as $car) {
            $query = "match(c:Car{uuid:{car}}) 
                where not ((c)-[:TRANSPORT]-(:Departure{status:0}))
                match(dp:Departure{uuid:{departure}})
                create (c)-[:TRANSPORT]->(dp)";
            $stack->push($query, ['car' => $car->getUuid(), "departure" => $departure->getUuid()]);
        }        
        
        $query = "match(e:Employee)-[:DEPARTS_IN]->(dp:Departure{uuid:{departure}})-[:DEPARTDATE]-(d:Day) 
                with e, d, dp 
                match(dp)-[:FROM]-(origin) 
                with e, origin, d 
                match(e)-[:OCCUPIES]->(o:Occupancy {active:1}) 
                where not((o)-[:END]->(:Day)) 
                create (o)-[:END]->(d) 
                set o.end = d.strrep";
        $stack->push($query, ["departure" => $departure->getUuid()]);
        
        $query = "match(e:Employee)-[r:DEPARTS_IN]->(dp:Departure{uuid:{departure}})-[:TO]->(dst) 
                match (dp)-[:DEPARTDATE]-(d:Day)<-[:CONTAINS]-(m:Month)<-[:CONTAINS]-(y:Year) 
                match(e)-[:OCCUPIES]->(ao:Occupancy {active:1})-[:END]->(:Day) 
                set dp.status = 1, e.status = 1, ao.active = 0 
                create (o:Occupancy{active:1, start:d.strrep})-[:START]->(d) 
                create (e)-[:OCCUPIES]->(o) 
                create (o)-[:OCCUPIES]->(dst)
                create (ao)-[:NEXT]->(o) ";
        $stack->push($query, ["departure" => $departure->getUuid()]);
        
        $query = "match(e:Car)-[:TRANSPORT]->(dp:Departure{uuid:{departure}})-[:DEPARTDATE]-(d:Day) 
                with e, d, dp 
                match(dp)-[:FROM]-(origin) 
                with e, origin, d 
                match(e)-[:OCCUPIES]->(o:Occupancy {active:1}) 
                where not((o)-[:END]->(:Day)) 
                create (o)-[:END]->(d) ";
        $stack->push($query, ["departure" => $departure->getUuid()]);
        
        $query = "match(e:Car)-[r:TRANSPORT]->(dp:Departure{uuid:{departure}})-[:TO]->(dst) 
                match (dp)-[:DEPARTDATE]-(d:Day)<-[:CONTAINS]-(m:Month)<-[:CONTAINS]-(y:Year) 
                match(e)-[:OCCUPIES]->(ao:Occupancy {active:1})-[:END]->(:Day) 
                set dp.status = 1, e.status = 1, ao.active = 0  
                create (o:Occupancy{active:1})-[:START]->(d) 
                create (e)-[:OCCUPIES]->(o) 
                create (o)-[:OCCUPIES]->(dst) 
                create (ao)-[:NEXT]->(o) ";
        $stack->push($query, ["departure" => $departure->getUuid()]);

//        $query = "match(e:Employee)-[r:DEPARTS_IN]->(dp:Departure{uuid:{departure}})
//                with e 
//                match (e)-[r1:DEPARTS_IN]->(:Departure{status:0}) 
//                delete r1 ";
//        $stack->push($query, ["departure" => $departure->getUuid()]);
        
        $client->runStack($stack);
        $client->transaction()->commit();
        return $this->getDepartureByUuid($departure->getUuid());         
    }

    public function cancelDeparture($jsonData) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

//        $departure = new Departure();
//        $this->mapper->map($jsonData, $departure);
        
        $existsing = $this->getDepartureByUuid($jsonData->{"uuid"});
        $existsing->setdeleted(1);
        $existsing->setactive(0);
        $existsing->setStatus(-1);
        
        $this->dbentity->persist($existsing);
        $this->dbentity->flush();
        return $existsing;
                
    }
    public function confirmDeparture($jsonData) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $departure = new Departure();
        $this->mapper->map($jsonData, $departure);
        
        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
        $stack = $client->stack();
        
        
        
        

//        $query="match(dp:Departure{uuid:{departure}}) set dp.status = 1 ";
//        $stack->push($query, ["departure" => $departure->getUuid()]);
        
        $type = $departure->getOrigin()->getDeparttype() == "COMPANY" ? 0 : 1;

        $query = "match(e:Employee)-[:DEPARTS_IN]->(dp:Departure{uuid:{departure}})-[:DEPARTDATE]-(d:Day) 
                with e, d, dp 
                match(dp)-[:FROM]-(origin) 
                with e, origin, d 
                match(e)-[:OCCUPIES]->(o:Occupancy {active:1}) 
                where not((o)-[:END]->(:Day)) 
                create (o)-[:END]->(d)";
        $stack->push($query, ["departure" => $departure->getUuid()]);
        
        $query = "match(e:Employee)-[r:DEPARTS_IN]->(dp:Departure{uuid:{departure}})-[:TO]->(dst) 
                match (dp)-[:DEPARTDATE]-(d:Day)<-[:CONTAINS]-(m:Month)<-[:CONTAINS]-(y:Year) 
                match(e)-[:OCCUPIES]->(ao:Occupancy {active:1})-[:END]->(:Day) 
                set dp.status = 1, e.status = 1, ao.active = 0 
                create (o:Occupancy{active:1})-[:START]->(d) 
                create (e)-[:OCCUPIES]->(o) 
                create (o)-[:OCCUPIES]->(dst)
                create (ao)-[:NEXT]->(o) ";
        $stack->push($query, ["departure" => $departure->getUuid(), "type" => $type]);
        
        $query = "match(e:Car)-[:TRANSPORT]->(dp:Departure{uuid:{departure}})-[:DEPARTDATE]-(d:Day) 
                with e, d, dp 
                match(dp)-[:FROM]-(origin) 
                with e, origin, d 
                match(e)-[:OCCUPIES]->(o:Occupancy {active:1}) 
                where not((o)-[:END]->(:Day)) 
                create (o)-[:END]->(d) ";
        $stack->push($query, ["departure" => $departure->getUuid()]);
        
        $query = "match(e:Car)-[r:TRANSPORT]->(dp:Departure{uuid:{departure}})-[:TO]->(dst) 
                match (dp)-[:DEPARTDATE]-(d:Day)<-[:CONTAINS]-(m:Month)<-[:CONTAINS]-(y:Year) 
                match(e)-[:OCCUPIES]->(ao:Occupancy {active:1})-[:END]->(:Day) 
                set dp.status = 1, e.status = 1, ao.active = 0  
                create (o:Occupancy{active:1})-[:START]->(d) 
                create (e)-[:OCCUPIES]->(o) 
                create (o)-[:OCCUPIES]->(dst) 
                create (ao)-[:NEXT]->(o) ";
        $stack->push($query, ["departure" => $departure->getUuid(), "type" => $type]);

//        $query = "match(e:Employee)-[r:DEPARTS_IN]->(dp:Departure{uuid:{departure}})
//                with e 
//                match (e)-[r1:DEPARTS_IN]->(:Departure{status:0}) 
//                delete r1 ";
//        $stack->push($query, ["departure" => $departure->getUuid()]);

        
        $client->runStack($stack);
        $client->transaction()->commit();
        return $this->getDepartureByUuid($departure->getUuid()); 
    }
    public function getDepartureReport(Request $request, $args) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
//        $cql = "match(departure:Departure{uuid:{departure}})-[:DEPARTS_IN]-(e:Employee) return departure order by e.lastname";
//        
//        $query = $this->dbentity->createQuery($cql);
//        $query->setParameter("departure", $args["id"]);
//        $query->addEntityMapping("departure", DepartureReport::class);
//        
//        return $query->execute();
        $reps = $this->dbentity->getRepository(DepartureReport::class);
        return $reps->findOneBy(["uuid" => $args["id"]]);
        
    }
    public function getDepartureEmployees(Request $request, $args) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        //$departure = $this->dbentity->getRepository(DepartureEx::class)->findOneBy(["uuid" => $args["id"]]);
        
        $cql = "match(departure:Departure{uuid:{departure}})<-[r:DEPARTS_IN]-(employee:Employee) with employee, r, departure
                return employee.lastname as lastname, employee.name as name, employee.uuid as uuid, r.driver as driver,
                departure.state as state, departure.note as note
                order by employee.lastname ";
        
        $query = $this->dbentity->createQuery($cql);
        $query->setParameter("departure", $args["id"]);
        //$query->addEntityMapping("employee", EmployeeEx::class);

        
        return $query->execute();
        
//        $emps = $query->execute();
//        foreach ($emps as $item) {
//            $item->setDeparture($departure);
//        }
//        return $emps;
    }    
    public function setDriver(Request $request, $args) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $cql = "match(departure:Departure{uuid:{departure}})<-[r:DEPARTS_IN]-(employee:Employee{uuid:{employee}}) 
            set r.driver = 1 ";
        $query = $this->dbentity->createQuery($cql);
        $query->setParameter("departure", $args["id"]);
        $query->setParameter("employee", $args["employeeId"]);

        $query->execute();
    }   
    public function setPassenger(Request $request, $args) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $cql = "match(departure:Departure{uuid:{departure}})<-[r:DEPARTS_IN]-(employee:Employee{uuid:{employee}}) 
            set r.driver = 0 ";
        $query = $this->dbentity->createQuery($cql);
        $query->setParameter("departure", $args["id"]);
        $query->setParameter("employee", $args["employeeId"]);

        $query->execute();
    }       
    public function getDepartureCars(Request $request, $args) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        $cql = "match(departure:Departure{uuid:{departure}})<-[:TRANSPORT]-(car:Car) with car
                return car";
        
        $query = $this->dbentity->createQuery($cql);
        $query->setParameter("departure", $args["id"]);
        $query->addEntityMapping("car", Car::class);
        
        return $query->execute();
//        $reps = $this->dbentity->getRepository(DepartureReport::class);
//        return $reps->findOneBy(["uuid" => $args["id"]]);
        
    }   
    public function getUnvalidInDeparture(Request $request, $args) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $departure = new Departure();
        $this->mapper->map(json_decode($request->getBody()), $departure);
        $project = new Project();
        switch ($departure->getOrigin()->getDeparttype()) {
           case "COMPANY":
                $cql = "match(e:Employee)-[:DEPARTS_IN]->(dp:Departure{uuid:{departure}})
                        with e, dp 
                        match(dp)-[:FROM]-(origin:Company) 
                        with e, origin, dp
                        where not ((e)-[:OCCUPIES]-(:Occupancy{active:1})-[:OCCUPIES]-(:Company))
                        return e";
               break;
           case "PROJECT":
//                $cql = "match(e:Employee)-[:DEPARTS_IN]->(dp:Departure{uuid:{departure}})
//                        with e, dp 
//                        match (dp)-[:FROM]-(origin) 
//                        with e, origin, dp
//                        where not ((e)-[:OCCUPIES]-(:Occupancy{active:1})-[:OCCUPIES]-(origin))
//                        return e";
                
                $this->mapper->map(json_decode($request->getBody())->{"origin"}, $project);
                
                $cql = "match(e:Employee)-[:DEPARTS_IN]->(dp:Departure{uuid:{departure}})
                        with e, dp
                        where not ((e)-[:OCCUPIES]-(:Occupancy{active:1})-[:OCCUPIES]-(:Project)-[:HAS]-(:CSite{uuid:{site}}))
                        return e";
               break;
       }
        
//        $cql = "match(e:Employee)-[:DEPARTS_IN]->(dp:Departure{uuid:{departure}})
//                with e, dp 
//                match(dp)-[:FROM]-(origin) 
//                with e, origin, dp
//                where not ((e)-[:OCCUPIES]-(:Occupancy{active:1})-[:OCCUPIES]-(origin))
//                return e";
        
   
        $query = $this->dbentity->createQuery($cql);
        $query->setParameter("departure", $departure->getUuid());
        if (null !== $project->getSite()) {
            $query->setParameter("site", $project->getSite()->getUuid());
        }
        $query->addEntityMapping("e", EmployeeList::class);
        
        return $query->execute();
        
    }    
    public function getUnvalidCarsInDeparture(Request $request, $args) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $departure = new Departure();
        $this->mapper->map(json_decode($request->getBody()), $departure);

        switch ($departure->getOrigin()->getDeparttype()) {
           case "COMPANY":
                $cql = "match(e:Car)-[:TRANSPORT]->(dp:Departure{uuid:{departure}})
                        with e, dp 
                        match(dp)-[:FROM]-(origin:Company) 
                        with e, origin, dp
                        where not ((e)-[:OCCUPIES]-(:Occupancy{active:1})-[:OCCUPIES]-(:Company))
                        return e";
               break;
           case "PROJECT":
                $cql = "match(e:Car)-[:TRANSPORT]->(dp:Departure{uuid:{departure}})
                        with e, dp 
                        match(dp)-[:FROM]-(:Project)--(s:CSite)  
                        with e, dp
                        //where not ((e)-[:OCCUPIES]-(:Occupancy{active:1})-[:OCCUPIES]-(origin))
                        where not ((e)-[:OCCUPIES]-(:Occupancy{active:1})-[:OCCUPIES]-(:Project)-[:HAS]-(s))
                        return e";
               break;
       }
        
        $query = $this->dbentity->createQuery($cql);
        $query->setParameter("departure", $departure->getUuid());
        $query->addEntityMapping("e", Car::class);
        
        return $query->execute();
        
    }        
    public function getPlanedDeparturesReport(Request $request, $args) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        $cql = "match(departure:Departure{status:0}) with departure "
                . "optional match (departure)<-[:DEPARTS_IN]-(employee:Employee) with employee, departure "
                . " optional match (departure)-[:TO]->(project:Project) "
                . " optional match (departure)-[:TO]->(company:Company) "
                . " return employee, departure, project, company ";
        
        $query = $this->dbentity->createQuery($cql);
        $query->setParameter("status", 0);
        $query->addEntityMapping("employee", EmployeeList::class);
        $query->addEntityMapping("departure", Departure::class);
        $query->addEntityMapping("project", Project::class);
        $query->addEntityMapping("company", Company::class);
        
        return $query->execute();
        
    }
    public function getDeparturesInRangeReport(Request $request, $args) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $params = $request->getQueryParams();

        if(null == $params['date']) return;
        if(null == $params['dateto']) return;
        
        $d = \DateTime::createFromFormat('Ymd',$params['date']);
        $dt = \DateTime::createFromFormat('Ymd',$params['dateto']);
        
        $dif = date_diff($dt,$d, true);

        $cql = "match(:Year{value:{y}})-[:CONTAINS]->(:Month{value:{m}})-[:CONTAINS]->(ds:Day{value:{d}})-[:NEXT*0..". $dif->format('%a') ."]->(de:Day) with ds, de "
                . " match (departure:Departure{deleted:0})-[:DEPARTDATE]->(de)"
                . " match(departure)<-[:DEPARTS_IN]-(employee:Employee) with employee, departure "
                . " optional match (departure)-[:TO]->(project:Project) "
                . " optional match (departure)-[:TO]->(company:Company) "
                . " optional match (departure)-[:FROM]->(fromproject:Project) "
                . " optional match (departure)-[:FROM]->(fromcompany:Company) "
                . " return employee, departure, project, company, fromproject, fromcompany ";
        
        $query = $this->dbentity->createQuery($cql);
        $query->setParameter("y", (int)$d->format('Y'));
        $query->setParameter("m", (int)$d->format('m'));
        $query->setParameter("d", (int)$d->format('d'));
        
        $query->addEntityMapping("employee", Employee::class);
        $query->addEntityMapping("departure", Departure::class);
        $query->addEntityMapping("project", Project::class);
        $query->addEntityMapping("company", Company::class);
        $query->addEntityMapping("fromproject", Project::class);
        $query->addEntityMapping("fromcompany", Company::class);
        
        return $query->execute();
        
    }       
    public function getDeparturesInRangeReport2(Request $request, $args) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $params = $request->getQueryParams();

        if(null == $params['date']) return;
        if(null == $params['dateto']) return;
        
        $d = \DateTime::createFromFormat('Ymd',$params['date']);
        $dt = \DateTime::createFromFormat('Ymd',$params['dateto']);
        
        $dif = date_diff($dt,$d, true);

        $cql = "match(:Year{value:{y}})-[:CONTAINS]->(:Month{value:{m}})-[:CONTAINS]->(ds:Day{value:{d}})-[:NEXT*0..". $dif->format('%a') ."]->(de:Day) with ds, de 
                                 match (departure:Departure{deleted:0})-[:DEPARTDATE]->(de)
                 match(departure)<-[:DEPARTS_IN]-(employee:Employee) with employee, departure 
                 match (departure)-[:FROM]->(source)
                 match (departure)-[:TO]->(destination)
                 optional match (source)<-[:HAS]-(cs:CSite)-[:IS_FOR]->(bs:BusinessPartner)
                 optional match (destination)<-[:HAS]-(cd:CSite)-[:IS_FOR]->(bd:BusinessPartner)
                return departure.uuid as departure_uuid, departure.departtime as departtime , employee.lastname +  ' ' + employee.name as employee
                , case when cs is null then source.name else  bs.name + ' - ' + cs.name  + ' - ' + source.name end as source
                , case when cd is null then destination.name else  bd.name + ' - ' + cd.name  + ' - ' + destination.name end as destination
                , labels(destination)[0] as destination_type
                order by departtime, departure_uuid";
        
        $query = $this->dbentity->createQuery($cql);
        $query->setParameter("y", (int)$d->format('Y'));
        $query->setParameter("m", (int)$d->format('m'));
        $query->setParameter("d", (int)$d->format('d'));
        
        return $query->getResult();
        
    }     
    
    public function getDepartureDocumentsToPrint(Request $request, $args) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $cql = "match(t:DocumentType{csiteprint:true}) with t
                match(d:Document{active:1, deleted:0})-[:IS_OF_TYPE]->(t) with t, d where not((d)-[:NEXT]->(:Document))
                match (d)-[:ATTACHED]->(f:File) with t, d, f where right(f.name, 3) = 'pdf' or right(f.name, 3) = 'PDF'
                match (e:Employee)-[:ASSOCIATED_WITH]->(d) with t, d, e, f
                match (e)-[:DEPARTS_IN]->(dep:Departure{uuid:{departure}}) with t, d, e, f
                optional match(d)-[:VALID_TO]->(to:Day) with t, d, e, f, to where date(to.strrep) >= date() or to is null
                return e.lastname + ' ' + e.name as group, d.uuid as document, t.uuid as typeId, t.name as type, f.uuid as file
                order by e.lastname, e.name, type, document";
        
        
        $query = $this->dbentity->createQuery($cql);
        $query->setParameter("departure", $args["id"]);

        $query->addEntityMapping("e", EmployeeDocuments::class);
        
        return $query->execute();        
    }
    
    public function getPlandeSites(Request $request, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $cql = "match(site:CSite)-[:HAS]->(p:Project)<-[:TO]-(d:Departure{active:1, deleted:0, status:0}) with site, d
            match(employee:Employee)-[:DEPARTS_IN]->(d) with site, employee
            optional match(site)-[:IS_FOR]->(partner:BusinessPartner) with site, employee, partner
            return distinct site.uuid as siteid, site.name as sitename, partner.name as partner, 
            employee.lastname as lastname, employee.name as employeename, employee.uuid as employeeid
            order by sitename, partner, lastname,employeename";         

        $records = $this->dbclient->run($cql)->records();
        foreach ($records as $record) {
            
            $rec[] = [                
                "sitename" => $record->get("sitename"),
                "partner"=> null, // $record->get("partner"),
                "siteid" => $record->get("siteid"),                
                "lastname" => $record->get("lastname"),
                "employeename" => $record->get("employeename"),
                "employeeid" => $record->get("employeeid"),
                "departures" => $this->getPlandeSitesEmployeesDepartures($record->get("employeeid")),
                ];
        }
        return $rec;    
    }
    public function getPlaned(Request $request, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $query = $this->dbentity->createQuery("match(e:Employee)-[:WORKS_IN]->(p:WorkPeriod{active:1})-[:WORKS_IN]->(company:Company) with e, company
match (e)-[:DEPARTS_IN]->(d:Departure{status:0})-[:TO]->(p:Project)<-[:HAS]-(s:CSite) with e, company, d, p, s
                optional match (s)--(b:BusinessPartner) with e, company, d, p, s, b
                return s.name + '('+ case when b.shortname is null then b.name else b.shortname end +')' as sitename, e.lastname as lastname, e.name as name, d.departtime as departtime,
                company.color as color
                order by sitename asc, departtime asc, lastname asc");
        
        return $query->getResult();
        
    }
    

    public function getPlandeSitesEmployeesDepartures($employee){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

//        $cql = "match(employee:Employee{uuid:{employee}})-[:DEPARTS_IN]->(departure:Departure{status:0, deleted:0, active:1})-[:TO]->(destination) with departure
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
            case when osite is null then origin.name else osite.name  end as origin,
            case when dsite is null then destination.name else dsite.name  end as destination";        


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
    
    // <editor-fold defaultstate="collapsed" desc="Employee related">
    public function addEmployee(Request $request, $args) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $json = json_decode($request->getBody());
        
        /** @var DepartureEx $departure */
        $departure = $this->dbentity->getRepository(DepartureEx::class)->findOneBy(["uuid" => $args["id"]]);
        
        /** @var EmployeeEx $employee */
        $employee = $this->dbentity->getRepository(EmployeeEx::class)->findOneBy(["uuid" => $json->{"uuid"}]);
        
        $del;
        foreach ($departure->getEmployees() as $emp) {
            //var_dump(json_encode($emp->getEmployee()));
            if($emp->getEmployee() === $employee){
                $del[]=$emp;
            }
        }

        foreach ($del as $d) {
            $departure->getEmployees()->removeElement($d);
            $employee->getDepartures()->removeElement($d);
            $this->dbentity->remove($d);
        }
        
        $departure->addEmployee($employee, 0, "");
        $this->dbentity->flush();
        return  $employee;
        
    }   
    public function addManyEmployee(Request $request, $args) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $json = json_decode($request->getBody());

        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
        $stack = $client->stack();
        
        $cql = "";
        foreach($json as $item) { //foreach element in $arr
            
            $cql = "match(d:Departure{uuid:{departure}}), (e:Employee{uuid:{employee}}) " . PHP_EOL . "merge (e)-[:DEPARTS_IN{status:0, note:{note}}]->(d)" . PHP_EOL. PHP_EOL;
            $stack->push($cql, [ 'departure' => $args["id"], 'employee' => $item->{"uuid"}, 'note' => ""]);
        }     

        $client->runStack($stack);
        $client->transaction()->commit();
        
        return  null;
        
    }      
    public function removeEmployee(Request $request, $args) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $json = json_decode($request->getBody());
        
        /** @var DepartureEx $departure */
        //$departure = $this->dbentity->getRepository(DepartureEx::class)->findOneBy(["uuid" => $args["id"]]);
        /** @var EmployeeEx $employee */
        $employee = $this->dbentity->getRepository(EmployeeEx::class)->findOneBy(["uuid" => $json->{"uuid"}]);

        $cql = "match(departure:Departure{active:1, status:0, deleted:0, uuid:{departure}})<-[r:DEPARTS_IN]-(employee:Employee{uuid:{employee}}) delete r";
        
        $query = $this->dbentity->createQuery($cql);
        $query->setParameter("departure", $args["id"]);
        $query->setParameter("employee", $json->{"uuid"});
        
        $query->execute();        
        
        /*
        $del;
        foreach ($departure->getEmployees() as $emp) {
            //var_dump(json_encode($emp->getEmployee()));
            if($emp->getEmployee() === $employee){
                $del[]=$emp;
            }
        }

        foreach ($del as $d) {
            $departure->getEmployees()->removeElement($d);
            $employee->getDepartures()->removeElement($d);
            $this->dbentity->remove($d);
        }
        $this->dbentity->flush();
         
         */
        return  $employee;        
    }   
    public function notifyEmployee(Request $request, $args) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
    }   
    public function confirmEmployee(Request $request, $args) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
    }   
    
// </editor-fold>
        
    // <editor-fold defaultstate="collapsed" desc="public functions - car related">
    public function addCar(Request $request, $args) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $json = json_decode($request->getBody());
        
        /** @var DepartureEx $departure */
        $departure = $this->dbentity->getRepository(DepartureEx::class)->findOneBy(["uuid" => $args["id"]]);
        
        /** @var EmployeeEx $employee */
        $car = $this->dbentity->getRepository(Car::class)->findOneBy(["uuid" => $json->{"uuid"}]);
        
        $del;
        foreach ($departure->getCars() as $c) {
            //var_dump(json_encode($emp->getEmployee()));
            if($c->getCar() === $car){
                $del[]=$c;
            }
        }

        foreach ($del as $d) {
            $departure->getCars()->removeElement($d);
            $this->dbentity->remove($d);
        }
        
        $departure->addCar($car);
        $this->dbentity->flush();
        return  $car;        
    }
    public function deleteCar(Request $request, $args) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $json = json_decode($request->getBody());
        
        /** @var DepartureEx $departure */
        $departure = $this->dbentity->getRepository(DepartureEx::class)->findOneBy(["uuid" => $args["id"]]);
        
        /** @var EmployeeEx $employee */
        $car = $this->dbentity->getRepository(Car::class)->findOneBy(["uuid" => $json->{"uuid"}]);
        
        $del;
        foreach ($departure->getCars() as $c) {
            //var_dump(json_encode($emp->getEmployee()));
            if($c->getCar() === $car){
                $del[]=$c;
            }
        }

        foreach ($del as $d) {
            $departure->getCars()->removeElement($d);
            $this->dbentity->remove($d);
        }
        $this->dbentity->flush();
        return  $car;         
    }
    
    public function setCarMilage(Request $request, $args) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $exid = $args['exid'];
        $milage = $args['milage'];
        $this->logger->debug("exid: ". $exid . "milage: " . $milage);
        
        $car = $this->dbentity->getRepository(Car::class)->findOneBy(["uuid" => $exid]);
        
    }    
    
// </editor-fold>
        
    // <editor-fold defaultstate="collapsed" desc="public functions - arrival related">
    public function listArrivals($param) {
        
    }
    public function addArrival($param) {
        
    }
    public function cancelArrival($param) {
        
    }
    public function comfirmArrival($param) {
        
    }
    // </editor-fold>
        
    // <editor-fold defaultstate="collapsed" desc="private funtions - departures related">
    private function getDepartureByUuid($uuid) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        return $this->getDeparture(['uuid' => $uuid]);
    }
    
    private function getDeparture($criteria) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        return $this->departures->findOneBy($criteria);
    } 
    
    private function getDepartures($criteria, $order = null){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        return $this->departures->findBy($criteria, $order);
    }
    // </editor-fold>

    // <editor-fold defaultstate="collapsed" desc="plan related">

    public function planNotifyMailAddresses(Request $request, $args) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
        $stack = $client->stack();
        
        $query = "match(t:DocumentType)-[]-(d:Document)-[]-(e:Employee)-[]-(p:Departure{uuid:{departure}})
            return distinct t.notificationmail";
        
        $res = $client->run($query, ["departure" => $args["id"]]);
        return $res->records();
    }
// </editor-fold>

}

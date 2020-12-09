<?php

namespace App\Repository;
use Slim\Http\Request;

use Psr\Log\LoggerInterface;
use GraphAware\Neo4j\Client\ClientBuilder;
use GraphAware\Neo4j\OGM\EntityManager;
use GraphAware\Neo4j\OGM\Query;
use App\Common\ApiException;
use App\Models\Employee;
use App\Models\EmployeeHire;
use App\Models\EmployeeFire;
use App\Models\Document;
use App\Models\Address;
use App\Models\Contact;
use App\Models\CSite;
use App\Models\EmployeeList;
use App\Models\Company;
use App\Models\Day;
use App\Models\WorkPeriod;
use App\Models\Language;
use App\Models\DocumentType;
use App\Models\CurrentWorkPeriod;
use App\Models\Occupancy;
use App\Models\Podlaga;
use App\Models\Project;
use App\Models\BusinessPartner;
use App\Models\WorkPeriodReport;
use App\Models\WorkPlace;
use App\Models\Absence;
use App\Models\Range;
use App\Models\EmployeeModel;

class EmployeesRepository {
    
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
        
        $this->dbentity->clear();
        $this->dbentity->flush();
    }
     
    public function __destruct() {
        $this->dbentity->clear();
    }

    // <editor-fold defaultstate="collapsed" desc="public functions">

    public function getActivecount(Request $request, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        if($args["firmid"] !== NULL){
            $cql = "match (company:Company{uuid:{company}})<-[:WORKS_IN]-(p:WorkPeriod{active:1})
                with company, p
                match (e:Employee)-[:WORKS_IN]->(p) with e
                    match(e)-[:WORK]-(workplace:WorkPlace) with e, workplace
                    return workplace, count(e) as count";
        }
        else{
            //$cql = "match (e:Employee)-[:WORKS_IN]->(p:WorkPeriod{active:1}) return count(e) as cnt";
            $cql ="match (e:Employee)-[:WORKS_IN]->(p:WorkPeriod{active:1}) with e 
                    optional match(e)-[:WORK]-(workplace:WorkPlace) with e, workplace
                    return workplace, count(e) as count";
        }
//
//        $result =  $this->dbclient->run($cql, ['company'=> $args["firmid"]]);
//        return $result->firstRecord()->get("cnt");
        $query = $this->dbentity->createQuery($cql);
        $query->addEntityMapping('workplace', WorkPlace::class);
        $query->setParameter("company", $args["firmid"]);
        $res =  $query->execute();
        return $res;        
    }
//    public function getActiveEmployees(){
//        //if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
//        return $this->getEmployees([], ['lastname' => 'asc', 'name' => 'asc']);
//    }
    public function getAllEmployees() {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

//        $cql = "match (company:Company)<-[:WORKS_IN]-(p:WorkPeriod{active:1})
//            with company, p
//            match (e:Employee)-[:WORKS_IN]->(p) 
//            with e, company, p
//            order by p.start desc
//            with e, company, collect(p)[..1] as periods unwind periods as per
//            with e, per, company
//            optional match (e)-[:LIVES]->(a:Address) with e, per, company, a
//            order by id(a) desc
//            with e, per, company, collect(a)[..1] as address unwind (CASE address WHEN [] then [null] else address end )as addr
//            optional match (e)-[:CONTACT]->(c:Contact) with e, per, company, addr, c
//            order by id(c) desc
//            with e, per, company, addr, collect(c)[..1] as contact unwind (CASE contact WHEN [] then [null] else contact end )as cont
//            with e, per, company, addr, cont
//            optional match (e)-[:OCCUPIES]->(o:Occupancy{active:1})-[:OCCUPIES]->(project:Project)-[:HAS]-(site:CSite)
//            with e, per, company, addr, cont, project, site
//            optional match (e)-[:ASSOCIATED_WITH]-(doc:Document)-[]-(t:DocumentType)
//            where t.name contains {crt}
//            with e, per, company, addr, cont, project, site, collect(doc) as documents
//            order by e.lastname asc
//            return e as employee, per as workperiod, addr as address, cont as contact, company, project, site, documents";
//        $query = $this->dbentity->createQuery($cql);
//        
//        $query->addEntityMapping('company', Company::class);
//        $query->addEntityMapping('employee', Employee::class);
//        $query->addEntityMapping('workperiod', WorkPeriod::class);
//        $query->addEntityMapping('address', Address::class);
//        $query->addEntityMapping('contact', Contact::class);
//        $query->addEntityMapping('project', Project::class);
//        $query->addEntityMapping('site', CSite::class);
//        $query->addEntityMapping('documents', Document::class, Query::HYDRATE_COLLECTION);
//        $query->setParameter("crt", "CERT_VAR_");
//        $res =  $query->execute();
//        return $res;
        
        $cql = "match (company:Company)<-[:WORKS_IN]-(p:WorkPeriod{active:1})
            with company, p
            match (e:Employee)-[:WORKS_IN]->(p) 
            with e, company, p
            return e as employee";

        
        $query = $this->dbentity->createQuery($cql);
        
        $query->addEntityMapping('employee', Employee::class);
        $res =  $query->execute();
        return $res;
    }
    public function getAllEmployees2(Request $request, $args) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $cql = "match (company:Company)<-[:WORKS_IN]-(p:WorkPeriod{active:1})
            with company, p
            match (e:Employee)-[:WORKS_IN]->(p) 
            with e, company, p
            order by p.start desc
            with e, company, collect(p)[..1] as periods unwind periods as per
            with e, per, company
            optional match (e)-[:LIVES]->(a:Address) with e, per, company, a
            order by id(a) desc
            with e, per, company, collect(a)[..1] as address unwind (CASE address WHEN [] then [null] else address end )as addr
            optional match (e)-[:CONTACT]->(c:Contact) with e, per, company, addr, c
            order by id(c) desc
            with e, per, company, addr, collect(c)[..1] as contact unwind (CASE contact WHEN [] then [null] else contact end )as cont
            with e, per, company, addr, cont
            optional match (e)-[:OCCUPIES]->(o:Occupancy{active:1})-[:OCCUPIES]->(project:Project)-[:HAS]-(site:CSite)
            with e, per, company, addr, cont, project, site
            optional match (e)-[:ASSOCIATED_WITH]-(doc:Document)-[]-(t:DocumentType)
            where t.name contains {crt}
            with e, per, company, addr, cont, project, site, collect(doc) as documents
            order by e.lastname asc
            return e as employee, per as workperiod, addr as address, cont as contact, company, project, site, documents";


        $query = $this->dbentity->createQuery($cql);
        
        $query->addEntityMapping('company', Company::class);
        $query->addEntityMapping('employee', Employee::class);
        $query->addEntityMapping('workperiod', WorkPeriod::class);
        $query->addEntityMapping('address', Address::class);
        $query->addEntityMapping('contact', Contact::class);
        $query->addEntityMapping('project', Project::class);
        $query->addEntityMapping('site', CSite::class);
        $query->addEntityMapping('documents', Document::class, Query::HYDRATE_COLLECTION);
        $query->setParameter("crt", "CERT_VAR_");
        $res =  $query->execute();
        return $res;
        
        //return $this->getEmployees([], ['crdate' => 'desc']);
    }
    
    public function getAllEmployeesNotRented() {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $cql = "match (company:Company)<-[:WORKS_IN]-(p:WorkPeriod{active:1})
            with company, p
            match (e:Employee)-[:WORKS_IN]->(p) 
            where not ((e)<-[:LOANS]-(:BusinessPartner))
            with e, company, p
            order by p.start desc
            with e, company, collect(p)[..1] as periods unwind periods as per
            with e, per, company
            optional match (e)-[:LIVES]->(a:Address) with e, per, company, a
            order by id(a) desc
            with e, per, company, collect(a)[..1] as address unwind (CASE address WHEN [] then [null] else address end )as addr
            optional match (e)-[:CONTACT]->(c:Contact) with e, per, company, addr, c
            order by id(c) desc
            with e, per, company, addr, collect(c)[..1] as contact unwind (CASE contact WHEN [] then [null] else contact end )as cont
            with e, per, company, addr, cont
            optional match (e)-[:OCCUPIES]->(o:Occupancy{active:1})-[:OCCUPIES]->(project:Project)-[:HAS]-(site:CSite)
            with e, per, company, addr, cont, project, site
            optional match (e)-[:ASSOCIATED_WITH]-(doc:Document)-[]-(t:DocumentType)
            where t.name contains {crt}
            with e, per, company, addr, cont, project, site, collect(doc) as documents
            order by e.lastname asc
            return e as employee, per as workperiod, addr as address, cont as contact, company, project, site, documents";

//        var_dump($cql);
//        die;
        $query = $this->dbentity->createQuery($cql);
        
        $query->addEntityMapping('company', Company::class);
        $query->addEntityMapping('employee', Employee::class);
        $query->addEntityMapping('workperiod', WorkPeriod::class);
        $query->addEntityMapping('address', Address::class);
        $query->addEntityMapping('contact', Contact::class);
        $query->addEntityMapping('project', Project::class);
        $query->addEntityMapping('site', CSite::class);
        $query->addEntityMapping('documents', Document::class, Query::HYDRATE_COLLECTION);
        $query->setParameter("crt", "CERT_VAR_");
        $res =  $query->execute();
        return $res;
        
        //return $this->getEmployees([], ['crdate' => 'desc']);
    }    
    public function getEmployeesWithHistory(Request $request, $args) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
//        $cql = "match(employee:Employee)-[:OCCUPIES]->(o:Occupancy) where employee.name is not null  return o";
//        $query = $this->dbentity->createQuery($cql);
//        $query->addEntityMapping('o', OccupancyReport::class);
//
//        $res =  $query->execute();
        $params = $request->getQueryParams();
        $activework = 1;
        if(is_array($params)){
            if($params['activework'] !==NULL){$activework = (int)$params['activework'];}
        }
        if($activework != 0){
        $cql = "match(e:Employee)-[:OCCUPIES]->(o:Occupancy) where e.name is not null with e, o
            match(e)-[:WORKS_IN]-(:WorkPeriod{active:{active}}) with e, o
            match(o)-[:START]->(s:Day) with e, o, s
            optional match (o)-[:END]->(x:Day)  with e, o, s, x
            optional match (o)-[:OCCUPIES]->(c:Company)  with e, o, s, x,c
            optional match (o)-[:OCCUPIES]->(p:Project)-[]-(g:CSite)-[]-(b:BusinessPartner)  with e, o, s, x,c, p, g, b
            return e.uuid as uuid, s.strrep as start, x.strrep as end, e.name as name, e.lastname as lastname, case when c.name is null then  b.name + ' - ' +  g.name + ' - ' + p.name  else c.name end as location";
        }
        else{
        $cql = "match(e:Employee)-[:OCCUPIES]->(o:Occupancy) where e.name is not null with e, o
            match(e)-[:WORKS_IN]-(:WorkPeriod{active:{active}}) where not exists((e)--(:WorkPeriod{active:1})) with e, o
            match(o)-[:START]->(s:Day) with e, o, s
            optional match (o)-[:END]->(x:Day)  with e, o, s, x
            optional match (o)-[:OCCUPIES]->(c:Company)  with e, o, s, x,c
            optional match (o)-[:OCCUPIES]->(p:Project)-[]-(g:CSite)-[]-(b:BusinessPartner)  with e, o, s, x,c, p, g, b
            return e.uuid as uuid, s.strrep as start, x.strrep as end, e.name as name, e.lastname as lastname, case when c.name is null then  b.name + ' - ' +  g.name + ' - ' + p.name  else c.name end as location";
        }
        $records = $this->dbclient->run($cql, ["active" => $activework])->records();
        foreach ($records as $record) {
            $rec[] = [                
                "start" => $record->get("start"),
                "end" => $record->get("end"),
                "name" => $record->get("name"),
                "lastname" => $record->get("lastname"),
                "location" => $record->get("location"),
                "uuid" => $record->get("uuid"),
                ];
        }
        return $rec;
    }


    public function getEmployeeWithHistory(Request $request, $args) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        $emps = $this->dbentity->getRepository(EmployeeList::class);
        
        /** @var EmployeeList[]   $result */
        $result = $emps->findOneBy(["uuid" => $args["employeeid"]]);
        
        return $result;

    }
    public function getHomeEmployees() {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

//        $cql = "match(e:Employee)-[:OCCUPIES]->(o:Occupancy{active:1})-[:OCCUPIES]->(company:Company)
//            //where not ((e)-[:DEPARTS_IN]-(:Departure{status:0}))
//            with e, company
//            optional match (e)-[:LIVES]->(a:Address) with e, company, a
//            order by id(a) desc
//            with e, company, collect(a)[..1] as address unwind (CASE address WHEN [] then [null] else address end )as addr
//            optional match (e)-[:CONTACT]->(c:Contact) with e, company, addr, c
//            order by id(c) desc
//            with e, company, addr, collect(c)[..1] as contact unwind (CASE contact WHEN [] then [null] else contact end )as cont
//            with e, company, addr, cont
//            order by e.lastname asc
//            return e as employee, addr as address, cont as contact, company";
//
//        $query = $this->dbentity->createQuery($cql);
//        
//        $query->addEntityMapping('company', Company::class);
//        $query->addEntityMapping('employee', Employee::class);
//        $query->addEntityMapping('address', Address::class);
//        $query->addEntityMapping('contact', Contact::class);

        $cql = "match(employee:Employee)-[:OCCUPIES]->(o:Occupancy{active:1})-[:OCCUPIES]->(company:Company) with employee
                match(employee)-[:WORKS_IN]->(p:WorkPeriod{active:1})-[:WORKS_IN]->(company)
                where not (employee)-[:WORK]-(:WorkPlace{isadministrative:1})
                return employee";
        $query = $this->dbentity->createQuery($cql);
        
        $query->addEntityMapping('employee', Employee::class);
        //$query->addEntityMapping('o', Occupancy::class);
        
        $res =  $query->execute();
        return $res;
        
        //return $this->getEmployees([], ['crdate' => 'desc']);
    }
    public function getHomeEmployees2() {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $cql = "match(employee:Employee)-[:OCCUPIES]->(o:Occupancy{active:1})-[:OCCUPIES]->(company:Company) 
                /*where not ((employee)-[:DEPARTS_IN]->(:Departure{status:0, active:1, deleted:0}))*/ with employee
                match(employee)-[:WORKS_IN]->(p:WorkPeriod{active:1})-[:WORKS_IN]->(company)
                where not (employee)-[:WORK]-(:WorkPlace{isadministrative:1})
                return employee";
        

        $query = $this->dbentity->createQuery($cql);
        
        
        $query->addEntityMapping('employee', EmployeeList::class);
        
        $res =  $query->execute();
        return $res;
       
    }
    public function getHomeEmployeesForReport() {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $cql = "match(employee:Employee)-[:OCCUPIES]->(o:Occupancy{active:1})-[:OCCUPIES]->(company:Company) with employee, o
            match(o)-[:START]->(fr:Day) with employee, fr 
            match(employee)-[:WORKS_IN]->(p:WorkPeriod{active:1})-[:WORKS_IN]->(cc:Company)
            where not (employee)-[:WORK]-(:WorkPlace{isadministrative:1})
            with employee, fr , cc
            optional match(employee)-[:DEPARTS_IN]->(departure:Departure{status:0, deleted:0,active:1}) with employee, departure, fr, cc
            optional match (departure)-[:TO]-(project:Project)--(s:CSite)--(b:BusinessPartner)
            return employee.lastname as lastname, employee.name as name, fr.strrep as datefrom, departure.departtime as depart, project.name as project, s.name as site, b.name as customer, cc.color as color
            order by datefrom, lastname, name";

                $records = $this->dbclient->run($cql)->records();
        foreach ($records as $record) {
            $rec[] = [                
                "lastname" => $record->get("lastname"),
                "name" => $record->get("name"),
                "datefrom" => $record->get("datefrom"),
                "project" => $record->get("project"),
                "site" => $record->get("site"),
                "customer" => $record->get("customer"),
                "depart" => $record->get("depart"),
                "color" => $record->get("color"),
                ];
        }
        return $rec;
        //return $this->getEmployees([], ['crdate' => 'desc']);
    }
    public function getHomeEmployeesForReport2() {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $date = new \DateTime();
        
        $cql = "match(employee:Employee)-[:OCCUPIES]->(o:Occupancy{active:1})-[:OCCUPIES]->(:Company) 
        with employee, o
        match(o)-[:START]->(fr:Day) with employee, fr 
        match(employee)-[:WORKS_IN]->(p:WorkPeriod{active:1})-[:WORKS_IN]->(cmp:Company)
        where not (employee)-[:WORK]-(:WorkPlace{isadministrative:1})
        with employee, fr, cmp
        optional match(employee)-[:ABSENT]->(ab:Absence) where (ab.from <= {date} and ab.to >= {date}) or ab.from >= {date}  with employee, fr, cmp, ab
        order by id(ab) desc
        with  employee, fr, cmp, collect(ab)[..1] as abs unwind (CASE abs WHEN [] then [null] else abs end )as absence
        optional match (employee)-[:DEPARTS_IN]->(dep:Departure{status:0, deleted:0,active:1}) with employee, fr, cmp, absence, dep
        order by id(dep) desc
        with  employee, fr, cmp, absence, collect(dep)[..1] as depar unwind (CASE depar WHEN [] then [null] else depar end ) as departure
        optional match (departure)-[:TO]-(n) with employee, cmp, fr, absence, departure, n
		optional match (n)-[:HAS]-(c:CSite)-[:IS_FOR]-(b:BusinessPartner) with employee, fr, cmp, absence, n, c, b, departure
        return employee.lastname as lastname, employee.name as name,
        fr.strrep as datefrom, 
        case when absence.type is null then case when c.name is null then null else 'Plan' end else absence.type end as absencetype,
        case when absence.type is null then case when c.name is null then null else   c.name + ' ' + n.name end else 'od '  + apoc.date.format(absence.from, 'ms', 'dd.MM.yyyy', 'PST') + '  do ' +  apoc.date.format(absence.to, 'ms', 'dd.MM.yyyy', 'PST') + ' (' + trim(absence.description)  + ')' end 
          as absence , 
          cmp.color as color
        order by datefrom, lastname, name";

        
        $records = $this->dbclient->run($cql, ["date" => time() * 1000])->records();
        foreach ($records as $record) {
            $rec[] = [                
                "lastname" => $record->get("lastname"),
                "name" => $record->get("name"),
                "datefrom" => $record->get("datefrom"),
                "absencetype" => $record->get("absencetype"),
                "absence" => $record->get("absence"),
                "color" =>$record->get("color")
      
                ];
        }
        return $rec;
        //return $this->getEmployees([], ['crdate' => 'desc']);
    }    
    
    
    
    public function getActiveEmployees() {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $cql = "match(employee:Employee)-[:WORKS_IN]->(p:WorkPeriod{active:1})-[:WORKS_IN]->(company)
                /*where not (employee)-[:WORK]-(:WorkPlace{isadministrative:1})*/
                return employee";
        $query = $this->dbentity->createQuery($cql);
        
        $query->addEntityMapping('employee', EmployeeList::class);
        //$query->addEntityMapping('o', Occupancy::class);
        
        $res =  $query->execute();
        return $res;
        
        //return $this->getEmployees([], ['crdate' => 'desc']);
    }
    public function getAwayEmployees() {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

//        $cql = "match(e:Employee)-[:OCCUPIES]->(o:Occupancy{active:1})-[:OCCUPIES]->(project:Project)<-[:HAS]-(site:CSite)
//            with e, project, site
//            optional match (e)-[:LIVES]->(a:Address) with e, project, site, a
//            order by id(a) desc
//            with e, project, site, collect(a)[..1] as address unwind (CASE address WHEN [] then [null] else address end )as addr
//            optional match (e)-[:CONTACT]->(c:Contact) with e, project, site, addr, c
//            order by id(c) desc
//            with e, project, site, addr, collect(c)[..1] as contact unwind (CASE contact WHEN [] then [null] else contact end )as cont
//            with e, project, site, addr, cont
//            order by e.lastname asc
//            return e as employee, addr as address, cont as contact, project, site";

        $cql = "match(employee:Employee)-[:OCCUPIES]->(o:Occupancy{active:1})-[:OCCUPIES]->(project:Project)<-[:HAS]-(site:CSite) return employee";

//        $query = $this->dbentity->createQuery($cql);
//        
//        $query->addEntityMapping('project', Project::class);
//        $query->addEntityMapping('site', CSite::class);
//        $query->addEntityMapping('employee', Employee::class);
//        //$query->addEntityMapping('workperiod', WorkPeriod::class);
//        $query->addEntityMapping('address', Address::class);
//        $query->addEntityMapping('contact', Contact::class);
//        $res =  $query->execute();
        
        $query = $this->dbentity->createQuery($cql);
        
        $query->addEntityMapping('employee', EmployeeList::class);
        $query->addEntityMapping('o', Occupancy::class);
        
        $res =  $query->execute();        
        
        
        return $res;
        
        //return $this->getEmployees([], ['crdate' => 'desc']);
    } 

    public function getPodlage($company, int $year, int $month) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $cql = "match(:Year{value:{year}})-[:CONTAINS]-(m:Month{value:{month}})-[:CONTAINS]-(fd:Day{value:1}) with m, fd
                match(m)-[:NEXT]-(:Month)-[:CONTAINS]-(ld:Day{value:1}) with fd, ld
                match (:Company{uuid:{company}})-[:ASSOCIATED_WITH]-(doc:Document{deleted:0})-[:IS_OF_TYPE]-(tp:DocumentType) with fd, ld, doc, tp
                where tp.name contains 'ZAV_POD' with fd, ld,doc, tp
                match(e:Employee)-[:ASSOCIATED_WITH]-(doc) with fd, ld,e, doc, tp
                /*match(e)-[:WORKS_IN]-(:WorkPeriod{active:1})-[:WORKS_IN]- (:Company{uuid:{company}})*/
                match (doc)-[:VALID_FROM]-(dayfrom:Day) with fd, ld, e, doc, tp, dayfrom
                where dayfrom.strrep < ld.strrep with fd, ld, e, doc, tp, dayfrom
                optional match (doc)-[:VALID_TO]-(dayto:Day) with fd, ld, e, doc, tp, dayfrom, dayto
                where dayto.strrep >= fd.strrep with   collect({lastname: e.lastname, name: e.name, type: tp.name, fromdate:dayfrom.strrep, todate: dayto.strrep}) as rows
                match(:Year{value:{year}})-[:CONTAINS]-(m:Month{value:{month}})-[:CONTAINS]-(fd:Day{value:1}) with rows, m, fd
                match(m)-[:NEXT]-(:Month)-[:CONTAINS]-(ld:Day{value:1}) with rows,fd, ld
                match (:Company{uuid:{company}})-[:ASSOCIATED_WITH]-(doc:Document{deleted:0})-[:IS_OF_TYPE]-(tp:DocumentType) with rows, fd, ld, doc, tp
                where tp.name contains 'ZAV_POD' with rows, fd, ld, doc, tp
                match(e:Employee)-[:ASSOCIATED_WITH]-(doc) with rows, fd, ld, e, doc, tp
                /*match(e)-[:WORKS_IN]-(:WorkPeriod{active:1})-[:WORKS_IN]- (:Company{uuid:{company}})*/
                match (doc)-[:VALID_FROM]-(dayfrom:Day) with rows, fd, ld, e, doc, tp, dayfrom
                where dayfrom.strrep < ld.strrep and not ((doc)-[:VALID_TO]-()) with rows + collect({lastname: e.lastname, name: e.name, type: tp.name, fromdate:dayfrom.strrep, todate: null}) as allrows
                unwind allrows as row with distinct row
                return row.lastname as lastname,  row.name as name, row.type as type, row.fromdate as fromdate, row.todate as todate
                order by row.lastname, row.name, row.fromdate";

  
        $result =  $this->dbclient->run($cql, ['company'=> $company, 'month' => (int)$month, 'year' => (int)$year]);

        foreach ($result->records() as $record) {
            $dd= new Podlaga();
            $dd->setLastname($record->get("lastname"));
            $dd->setName($record->get("name"));
            $dd->setType($record->get("type"));
            $dd->setFromdate($record->get("fromdate"));
            $dd->setTodate($record->get("todate"));
            $rec[] = $dd;
        }
        return $rec;
        
        //return $this->getEmployees([], ['crdate' => 'desc']);
    }
    
    public function getHiredFiredEmployees($company, $year, $month) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $cql = "match (p:WorkPeriod)-[:WORKS_IN]->(c:Company{uuid:{company}})
                    where (p.start >= {fd} and p.start <= {ld}) or (p.end >= {fd} and p.end <= {ld})
                return p";
        
        $query = $this->dbentity->createQuery($cql);

        
        $query->addEntityMapping('p', WorkPeriodReport::class);
        $query->setParameter('company', $company);
        $query->setParameter('fd', $year.sprintf('%02d', $month)."01");
        $query->setParameter('ld', $year.sprintf('%02d', $month).date('t', strtotime($year.sprintf('%02d', $month)."01")));
        //$query->setParameter('ld', date($year.sprintf('%02d', $month)."t"));
        
        $res =  $query->execute();
        return $res;
    }     
    public function getStateHiredFiredEmployees($company, $year, $month) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $cql = "match (p:WorkPeriod)-[:WORKS_IN]->(c:Company{uuid:{company}})
                where (p.start < {fd} and not exists(p.end)) or (p.start >= {fd} and p.start <= {ld}) or (p.end >= {fd} and p.end <= {ld})
                return p";
        
        $query = $this->dbentity->createQuery($cql);

        $query->addEntityMapping('p', WorkPeriodReport::class);
        $query->setParameter('company', $company);
        $query->setParameter('fd', $year.sprintf('%02d', $month)."01");
        $query->setParameter('ld', $year.sprintf('%02d', $month).date('t', strtotime($year.sprintf('%02d', $month)."01")));
        
//        echo $year.sprintf('%02d', $month)."01".PHP_EOL;
//        echo $year.sprintf('%02d', $month).date('t', strtotime($year.sprintf('%02d', $month)."01"));
//                
        
        //$query->setParameter('ld', date($year.sprintf('%02d', $month)."t"));
        
        $res =  $query->execute();
        return $res;
    }         
    public function getEmployeesForEmployer($companyId){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        $company = $this->companies->findOneBy(['uuid' => $companyId]);
        if (NULL === $company) {
            return;
        }
//        $cql = "match (company:Company{uuid:{companyId}})<-[:WORKS_IN]-(p:WorkPeriod)
//            with company, p
//            match (e:Employee)-[:WORKS_IN]->(p) 
//            with e, company, p
//            order by p.start desc
//            with e, company, collect(p)[..1] as periods unwind periods as per
//            with e, per, company
//            optional match (e)-[:LIVES]->(a:Address) with e, per, company, a
//            order by id(a) desc
//            with e, per, company, collect(a)[..1] as address unwind (CASE address WHEN [] then [null] else address end )as addr
//            optional match (e)-[:CONTACT]->(c:Contact) with e, per, company, addr, c
//            order by id(c) desc
//            with e, per, company, addr, collect(c)[..1] as contact unwind (CASE contact WHEN [] then [null] else contact end )as cont
//            with e, per, company, addr, cont
//            optional match (e)-[:OCCUPIES]->(o:Occupancy{active:1})-[:OCCUPIES]->(project:Project)-[:HAS]-(site:CSite)
//            with e, per, company, addr, cont, project, site
//            optional match (e)-[:ASSOCIATED_WITH]-(doc:Document)-[]-(t:DocumentType)
//            where t.name contains {crt}
//            with e, per, company, addr, cont, project, site, collect(doc) as documents
//            order by e.lastname asc
//            return e as employee, per as workperiod, addr as address, cont as contact, company, project, site, documents";
//
//        $query = $this->dbentity->createQuery($cql);
//        
//        $query->addEntityMapping('company', Company::class);
//        $query->addEntityMapping('employee', Employee::class);
//        $query->addEntityMapping('workperiod', WorkPeriod::class);
//        $query->addEntityMapping('address', Address::class);
//        $query->addEntityMapping('contact', Contact::class);
//        $query->addEntityMapping('project', Project::class);
//        $query->addEntityMapping('site', CSite::class);
//        $query->addEntityMapping('documents', Document::class, Query::HYDRATE_COLLECTION);
//        $query->setParameter('companyId', $companyId);
//        $query->setParameter("crt", "CERT_VAR_");
//        $res =  $query->execute();
//        return $res;

        $cql = "match(e:Employee)--(:WorkPeriod{active:1})--(:Company{uuid:{companyId}})
            return e as employee";

   
        $query = $this->dbentity->createQuery($cql);
        
        $query->addEntityMapping('employee', Employee::class);
        $query->setParameter('companyId', $companyId);
        $res =  $query->execute();
        return $res;
    }
    
    public function getEmployed($companyId){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        $company = $this->companies->findOneBy(['uuid' => $companyId]);
        if (NULL === $company) {
            return;
        }

        $cql = "match(e:Employee)--(:WorkPeriod{active:1})--(:Company{uuid:{companyId}})
            return e as employee";

        $query = $this->dbentity->createQuery($cql);
        
        $query->setParameter('companyId', $companyId);
        $query->addEntityMapping('employee', Employee::class);
        $res =  $query->execute();
        return $res;
    }
    public function getFired($companyId){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        $company = $this->companies->findOneBy(['uuid' => $companyId]);
        if (NULL === $company) {
            return;
        }

        $cql = "match(e:Employee)--(:WorkPeriod{active:0})--(:Company{uuid:{companyId}})
            where not exists ((e)--(:WorkPeriod{active:1})--(:Company{uuid:{companyId}}))
            return e as employee";

        $query = $this->dbentity->createQuery($cql);
        $query->setParameter('companyId', $companyId);
        $query->addEntityMapping('employee', Employee::class);

        $res =  $query->execute();
        return $res;
    }    
    public function getLoaned($companyId){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        $company = $this->companies->findOneBy(['uuid' => $companyId]);
        if (NULL === $company) {
            return;
        }

        $cql = "match (:BusinessPartner)--(e:Employee)--(:WorkPeriod)--(:Company{uuid:{companyId}}) 
            return e as employee";

        $query = $this->dbentity->createQuery($cql);
        $query->setParameter('companyId', $companyId);
        $query->addEntityMapping('employee', Employee::class);
        $res =  $query->execute();
        return $res;
    }        
    
    public function getEmployeesForEmployerNotRented($companyId){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        $company = $this->companies->findOneBy(['uuid' => $companyId]);
        if (NULL === $company) {
            return;
        }
        $cql = "match (company:Company{uuid:{companyId}})<-[:WORKS_IN]-(p:WorkPeriod)
            with company, p
            match (e:Employee)-[:WORKS_IN]->(p) 
            where not ((e)<-[:LOANS]-(:BusinessPartner))
            with e, company, p
            order by p.start desc
            with e, company, collect(p)[..1] as periods unwind periods as per
            with e, per, company
            order by e.lastname asc
            return e as employee, per as workperiod";
        
//        var_dump($cql);
//        var_dump($companyId);
//        die;
        $query = $this->dbentity->createQuery($cql);
        
        $query->addEntityMapping('company', Company::class);
        $query->addEntityMapping('employee', Employee::class);
        $query->addEntityMapping('workperiod', WorkPeriod::class);
        $query->addEntityMapping('address', Address::class);
        $query->addEntityMapping('contact', Contact::class);
        $query->addEntityMapping('project', Project::class);
        $query->addEntityMapping('site', CSite::class);
        $query->addEntityMapping('documents', Document::class, Query::HYDRATE_COLLECTION);
        $query->setParameter('companyId', $companyId);
        $query->setParameter("crt", "CERT_VAR_");
        $res =  $query->execute();
        return $res;
    }    
    public function getRentedEmployees(Request $request, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        $cql = "match (e:Employee)-[:WORKS_IN]->(p:WorkPeriod{active:{active}})-[:WORKS_IN]->(c:Company{uuid:{company}}) 
            with e
            match(e)<-[:LOANS]-(b:BusinessPartner) with e, collect(b)[..1] as loaner unwind loaner as lon
            return e.lastname as lastname, e.name as name, lon.name as loaner order by lastname, name";

        $query = $this->dbentity->createQuery($cql);
        $query->setParameter('company', $args["firmid"]);
        $query->setParameter('active', (int)$args["status"]);

        return $query->execute();
    }
    public function getEmployeesForEmployerOnDate($companyId, $date){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        $company = $this->companies->findOneBy(['uuid' => $companyId]);
        if (NULL === $company) {
            return;
        }
        $cql = "match (company:Company{uuid:{companyId}})<-[:WORKS_IN]-(p:WorkPeriod)
            with company, p
            match (e:Employee)-[:WORKS_IN]->(p)
            where p.start <= {date} and COALESCE(p.end, {date}) >= {date}  and not ((e)-[:LOANS]-(:BusinessPartner)) /*where p.start <= {date} and not ((e)-[:LOANS]-(:BusinessPartner))*/
            with e, company, p
            order by p.start desc
            with e, company, collect(p)[..1] as periods unwind periods as per
            with e, per, company
            optional match (e)-[:LIVES]->(a:Address) with e, per, company, a
            order by id(a) desc
            with e, per, company, collect(a)[..1] as address unwind (CASE address WHEN [] then [null] else address end )as addr
            optional match (e)-[:CONTACT]->(c:Contact) with e, per, company, addr, c
            order by id(c) desc
            with e, per, company, addr, collect(c)[..1] as contact unwind (CASE contact WHEN [] then [null] else contact end )as cont
            with e, per, company, addr, cont
            optional match (e)-[:OCCUPIES]->(o:Occupancy{active:1})-[:OCCUPIES]->(project:Project)-[:HAS]-(site:CSite)
            with e, per, company, addr, cont, project, site
            order by e.lastname asc
            return e as employee, per as workperiod, addr as address, cont as contact, company, project, site";
        
//        var_dump($cql);
//        var_dump($companyId);
//        die;
        $query = $this->dbentity->createQuery($cql);
        
        $query->addEntityMapping('company', Company::class);
        $query->addEntityMapping('employee', Employee::class);
        $query->addEntityMapping('workperiod', WorkPeriod::class);
        $query->addEntityMapping('address', Address::class);
        $query->addEntityMapping('contact', Contact::class);
        $query->addEntityMapping('project', Project::class);
        $query->addEntityMapping('site', CSite::class);
        $query->setParameter('companyId', $companyId);
        $query->setParameter("date", $date);

        
        $res =  $query->execute();
        return $res;
    }    
    public function getDocumnetsToExpire(Request $request, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        $range = new Range();
        $this->mapper->map(json_decode($request->getBody()), $range);

//        
//        $cql = "match p = shortestpath((:Day{strrep:{start}})-[:NEXT*]-(:Day{strrep:{end}}))
//            unwind nodes(p) as day
//            match (typ:DocumentType{expirable:true})<-[:IS_OF_TYPE]-(doc:Document)-[:VALID_TO]->(day) where not ((doc)-[:NEXT]->(:Document)) with day, doc, typ
//            match (e:Employee)-[:WORKS_IN]->(:WorkPeriod{active:1}) with day, doc, typ, e
//            match (e)-[:ASSOCIATED_WITH]->(doc)
//            return day, doc, typ, e
//            order by day.strrep asc";   
        
        $cql = "match(doc:Document{active:1, deleted:0}) where not ((doc)-[:NEXT]->(:Document)) with doc
            match(doc)-[:IS_OF_TYPE]-(typ:DocumentType{expirable:true}) with doc, typ
            match (doc)<-[:ASSOCIATED_WITH]-(e:Employee) with doc,typ, e
            match (doc)-[:VALID_TO]-(day) where day.strrep> {start} and day.strrep < {end}
            return day, doc, typ, e
            order by day.strrep asc";

        $query = $this->dbentity->createQuery($cql);
        $query->addEntityMapping('e', Employee::class);
        $query->addEntityMapping('doc', Document::class);
        $query->addEntityMapping('typ', DocumentType::class);
        $query->addEntityMapping('day', Day::class);
        $query->setParameter("start", $range->getFrom()->getDate()->format("Ymd"));
        $query->setParameter("end", $range->getTo()->getDate()->format("Ymd"));
                
        $res =  $query->execute();
        return $res;
    }    
    public function addEmployee(Request $request, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        $json = json_decode($request->getBody());
        
        if (json_last_error() !== JSON_ERROR_NONE) {throw ApiException::json_decodeError();}
        if (NULL === $json) {throw ApiException::serverError('No data povided');}
        $firmid = NULL;
        if(array_key_exists('firmid', $args)) {$firmid = $args['firmid'];}
        if (NULL === $firmid) {throw ApiException::serverError('No Company provided');}        

//        $employee = new EmployeeAdd();
//        $this->mapper->map($json, $employee);
//
//        $occupancy = new Occupancy();
//        
//        $day = $this->dbentity->getRepository(Day::class)->findOneBy(['strrep' => date("Ymd")]);
//        $company = $this->companies->findOneBy(['uuid' => $firmid]);
//   
//        $occupancy->setStart($day);
//        $occupancy->setBegin(new \DateTime("NOW"));
//        $occupancy->setType(Occupancy::HOME);
//        $occupancy->setactive(1);
//        $occupancy->setCompany($company);
//        
//        $employee->startOccupancy($occupancy,Occupancy::HOME);
//
//        
//        $this->dbentity->persist($employee);
//        $this->dbentity->flush();
//        
//        return $employee;
//        
  

        
        $employee = new Employee();
        $this->mapper->map($json, $employee);

        $employee->setUser($request->getAttribute('oauth_user_id'));

        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
        $query = "match (year:Year{value:{y}})-[:CONTAINS]->(month:Month {value:{m}})-[:CONTAINS]->(today:Day{value:{d}}) with today "
                ." match(company:Company{uuid:{company}}) with today, company "
                . " create (e:Employee{". $employee->getQueryFields()."}) "
                . " create (o:Occupancy{active:1})-[:START]->(today) "
                . " create (e)-[:OCCUPIES]->(o)-[:OCCUPIES]->(company) ";
        
        $client->run($query, 
                array_merge(
                        $employee->getQueryFieldsParams(), 
                        [
                            'y' => idate("Y"),
                            'm' => idate("m"),
                            'd' => idate("d"),
                            'company' => $firmid ]));

        $client->transaction()->commit();

        return $this->getEmployeeByUuid($employee->getUuid());
        
    }
    public function hireEmployee(Request $request,  $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $json = json_decode($request->getBody());
        if (json_last_error() !== JSON_ERROR_NONE) {throw ApiException::json_decodeError();}
        if (NULL === $json) {throw ApiException::serverError('No data povided');}
        $firmid = NULL;
        if(array_key_exists('firmid', $args)){$firmid = $args['firmid'];}
        if (NULL === $firmid) {throw ApiException::serverError('No Company provided');}        
        
        $e = new EmployeeHire();
        $this->mapper->map($json, $e);
        $e->setEmployeeId($json->{"uuid"});

        $e->getHireemployer()->setCompanyId($json->{"hireemployer"}->{"uuid"});
        
        if($request->getAttribute('currentperiod') === NULL){
            $this->addEmployeeWorkPeriod($e);
        }
        else{
            $current = $this->periods->findOneBy(['uuid' => $request->getAttribute('currentperiod')]);
            $period = new WorkPeriod();
            $period->setStart($e->getHiredate());
            $current->import($period);
            $this->updateEmployeeWorkPeriod($current);
        }
        return $this->getEmployeeByUuid($e->getUuid());
    }
    public function addLoaner(Request $request,  $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $json = json_decode($request->getBody());
        if (json_last_error() !== JSON_ERROR_NONE) {throw ApiException::json_decodeError();}
        if (NULL === $json) {throw ApiException::serverError('No data povided');}
        $firmid = NULL;
        if(array_key_exists('firmid', $args)){$firmid = $args['firmid'];}
        if (NULL === $firmid) {throw ApiException::serverError('No Company provided');}        
        
        $p = new BusinessPartner();
        $this->mapper->map($json, $p);
      
        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
 
        $stack = $client->stack();
        
        $cql = "match(e:Employee{uuid:{employee}})-[r:LOANS]-(p:BusinessPartner) delete r";
        $stack->push($cql, ["employee" => $args["id"]]);

        $cql = "match(e:Employee{uuid:{employee}}) with e "
                        . " match(p:BusinessPartner{uuid:{partner}}) with e, p "
                        . " create (p)-[:LOANS]->(e) ";
        $stack->push($cql, ["employee" => $args["id"], "partner" => $p->getUuid()]);
        
        $client->runStack($stack);
        $client->transaction()->commit(); 
        
        return null;          
    } 
    public function deleteLoaner(Request $request,  $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
      
        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
        $stack = $client->stack();
        
        $cql = "match(e:Employee{uuid:{employee}})-[r:LOANS]-(p:BusinessPartner) delete r";
        $stack->push($cql, ["employee" => $args["id"]]);

        $client->runStack($stack);
        $client->transaction()->commit(); 
        
        return null;          
    } 
    public function workplace(Request $request,  $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $json = json_decode($request->getBody());
        if (json_last_error() !== JSON_ERROR_NONE) {throw ApiException::json_decodeError();}
        if (NULL === $json) {throw ApiException::serverError('No data povided');}
        $firmid = NULL;
        if(array_key_exists('firmid', $args)){$firmid = $args['firmid'];}
        if (NULL === $firmid) {throw ApiException::serverError('No Company provided');}        
        
        $p = new WorkPlace();
        $this->mapper->map($json, $p);
      
        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
 
        $stack = $client->stack();
        
        $cql = "match(e:Employee{uuid:{employee}})-[r:WORK]-(p:WorkPlace) delete r";
        $stack->push($cql, ["employee" => $args["id"]]);

        $cql = "match(e:Employee{uuid:{employee}}) with e "
                        . " match(p:WorkPlace{uuid:{workplace}}) with e, p "
                        . " create (e)-[:WORK]->(p)";
        $stack->push($cql, ["employee" => $args["id"], "workplace" => $p->getUuid()]);
        
        $client->runStack($stack);
        $client->transaction()->commit(); 
        
        return null;          
    }     
    
    public function getCurrentWorkPeriod($employeeId, $active){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $current = new CurrentWorkPeriod();

        $cql = "match (company:Company)<-[:WORKS_IN]-(p:WorkPeriod{active:{active}})
            with company, p
            match (e:Employee{uuid:{eid}})-[:WORKS_IN]->(p) with e, company, p
            order by p.start desc
            with e, company, collect(p)[..1] as periods unwind periods as workperiod
            return workperiod";
        
        $query = $this->dbentity->createQuery($cql);
        $query->addEntityMapping('workperiod', WorkPeriod::class);
        $query->setParameter('eid', $employeeId);
        $query->setParameter('active', $active);


        
        $current->setCurrentPeriod(isset($query->getResult()[0]) ? $query->getResult()[0]: null);

        
        if($current->getCurrentPeriod() != NULL){
            $cql = "match (company:Company)<-[:WORKS_IN]-(p:WorkPeriod)
                where p.uuid <> {p}
                with company, p
                match (e:Employee{uuid:{eid}})-[:WORKS_IN]->(p) with e, company, p
                order by p.start desc
                with e, company, collect(p)[..1] as periods unwind periods as workperiod
                return workperiod ";
        
            $query = $this->dbentity->createQuery($cql);
            $query->addEntityMapping('workperiod', WorkPeriod::class);
            $query->setParameter('eid', $employeeId);
            $query->setParameter('p', $current->getCurrentPeriod()->getUuid());
        }
        else
        {
            $cql = "match (company:Company)<-[:WORKS_IN]-(p:WorkPeriod)
                with company, p
                match (e:Employee{uuid:{eid}})-[:WORKS_IN]->(p) with e, company, p
                order by p.start desc
                with e, company, collect(p)[..1] as periods unwind periods as workperiod
                return workperiod ";
        
            $query = $this->dbentity->createQuery($cql);
            $query->addEntityMapping('workperiod', WorkPeriod::class);
            $query->setParameter('eid', $employeeId);
        }
        
        $res = $query->getResult();
        if(isset($res[0])){
            $current->setLastPeriod($res[0]);
        }
        return $current;       
    }
    public function getEmployeeWorkPeriods($companyId, $employeeId) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        $company = $this->companies->findOneBy(['uuid' => $companyId]);
        if (NULL === $company) {
            return;
        }
        
        $cql = "match (e:Employee{uuid:{eid}})-[:WORKS_IN]->(p:WorkPeriod)
            with e, p
            match(p)-[:WORKS_IN]->(c:Company{uuid:{cId}})
            return e as employee, collect(p) as periods";
 
        $query = $this->dbentity->createQuery($cql);
        $query->addEntityMapping('employee', Employee::class);
        $query->addEntityMapping('periods', WorkPeriod::class, Query::HYDRATE_COLLECTION);
        $query->setParameter('eid', $employeeId);
        $query->setParameter('cId', $companyId);
     
        return isset($query->getResult()[0]) ? $query->getResult()[0] : null;        
    }
    public function getLastWorkPeriods($employeeId) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $cql = "match (company:Company)<-[:WORKS_IN]-(p:WorkPeriod)
        with company, p
        match (e:Employee{uuid:{eid}})-[:WORKS_IN]->(p) with e, company, p
        order by p.start desc
        with e, company, collect(p)[..1] as periods unwind periods as workperiod
        return e as employee, company, workperiod";
 
        $query = $this->dbentity->createQuery($cql);
        $query->addEntityMapping('employee', Employee::class);
        $query->addEntityMapping('workperiod', WorkPeriod::class);
        $query->addEntityMapping('company', Company::class);
        $query->setParameter('eid', $employeeId);
     
        return $query->getResult();
    }    
    public function fireEmployee(Request $request,  $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        $jsonData = json_decode($request->getBody());
        if (json_last_error() !== JSON_ERROR_NONE) {throw ApiException::json_decodeError();}
        if (NULL === $jsonData) {throw ApiException::serverError('No data povided');}
        $firmid = NULL;
        if(array_key_exists('firmid', $args)){$firmid = $args['firmid'];}
        if (NULL === $firmid) {throw ApiException::serverError('No Company provided');}
            
        $e = new EmployeeFire();
        $this->mapper->map($jsonData, $e);

        $e->setEmployeeId($jsonData->{"uuid"});
        $e->getFireemployer()->setCompanyId($jsonData->{"fireemployer"}->{"uuid"});

        $current = $this->periods->findOneBy(['uuid' => $request->getAttribute('currentperiod')]);
        
        $current->setEnd($e->getFiredate());
        $current->setactive(0);
            
        $this->endEmployeePeriod($current);
        return $this->getEmployeeByUuid($e->getUuid());
    }
    public function updateEmployee($jsonData){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $employee = $this->mapper->map($jsonData, new Employee());

        $employee->setEmployeeId($jsonData->{'uuid'});
        
        $existing = $this->getEmployeeByUuid($employee->getUuid());
        if (NULL == $existing) {
            throw ApiException::serverError('Employee not found');
        }
        $existing->import($employee);
        $existing->setmfdate('');

        $this->dbentity->persist($existing);
        $this->dbentity->flush();
        return $existing;
    }
    public function deleteEmployee($jsonData){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $e = $this->employees->findOneBy(["uuid" => $jsonData->{"uuid"}]);
        $e->setDeleted(1);
        $this->dbentity->flush();
        return $e;
    }     
    public function addEmployeeDocument($jsonData){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $employee = $this->getEmployeeByUuid($jsonData->{'parentuuid'});
        if (NULL === $employee) {
            throw ApiException::serverError('Employee not found');
        }

        $doc = $this->documents->findOneBy(['uuid' => $jsonData->{'document'}->{'uuid'}]);
        //$doc->gettype();
        $this->addDocument($employee, $doc);
        //$this->dbentity->flush();

        return $this->getEmployeeByUuid($jsonData->{'parentuuid'});
    }
    public function addEmployeeAddress($jsonData){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        $employee = $this->getEmployeeByUuid($jsonData->{'parentuuid'});

        if (NULL === $employee) {
            throw ApiException::serverError('Employee not found');
        }
        
        $addressJsonData = $jsonData->{'address'};
        $address = $this->mapper->map($addressJsonData, new Address());
        $this->addAddress($employee, $address);
        
        return $this->getEmployeeByUuid($jsonData->{'parentuuid'});
    }
    public function addEmployeeContact($jsonData){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $employee = $this->getEmployeeByUuid($jsonData->{'parentuuid'});
        if (NULL === $employee) {
            throw ApiException::serverError('Employee not found');
        }
        $contactJsonData = $jsonData->{'contact'};
        $contact = $this->mapper->map($contactJsonData, new Contact());

        $existsing = $this->contacts->findOneBy(['uuid'=>$contact->getUuid()]);
        if(NULL !== $existsing)
        {
            $existsing->import($contact);
            $this->dbentity->persist($existsing);
        }
        else{
            $this->addContact($employee, $contact);
        }
        return $employee;
    }
    public function getEmployeeByEmso($emso) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        return $this->getEmployee(['emso' => $emso]);
    }
    public function getEmployeeByUuid($uuid) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        return $this->getEmployee(['uuid' => $uuid]);
    }
    public function getEmployeeByPersonalIdNumber($personalidnumber) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        return $this->getEmployee(['personalidnumber' => $personalidnumber]);
    }
    public function getAddresses($uuid)   {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        $query = $this->dbentity->createQuery("match(e:Employee{uuid:{uuid}})-[r:LIVES{deleted:0}]-(a:Address{deleted:0}) 
            return e, collect(a) as addresses");
        $query->addEntityMapping("e", Employee::class);
        $query->addEntityMapping("addresses", Address::class, Query::HYDRATE_COLLECTION);
        $query->setParameter("uuid", $uuid);
        $emp = $query->getResult();
        return isset($emp[0]) ? $emp[0]: null;
        
    }
    public function getContacts($uuid)   {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $query = $this->dbentity->createQuery("match(e:Employee{uuid:{uuid}})-[r:CONTACT{deleted:0}]-(c:Contact{deleted:0})
                return e, collect(c) as contacts");
        $query->addEntityMapping("e", Employee::class);
        $query->addEntityMapping("contacts", Contact::class, Query::HYDRATE_COLLECTION);
        $query->setParameter("uuid", $uuid);
        $emp = $query->getResult();
        return isset($emp[0])? $emp[0]: null;        
       
    }
    public function getContactsEx($uuid)   {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $query = $this->dbentity->createQuery("match(e:Employee{uuid:{uuid}})-[r:CONTACT{deleted:0}]-(c:Contact{deleted:0})
                return c as contact");
        $query->addEntityMapping("contact", Contact::class);
        $query->setParameter("uuid", $uuid);
        
        return $query->getResult();        
       
    }    
    public function getDocuments($company, $uuid)   {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        if(NULL === $company){
            $q = "match (dt:DocumentType{deleted:0})
                with dt
                match (d:Document)-[rt:IS_OF_TYPE]->(dt)
                with d
                /*where not((d)-[:NEXT]->(:Document))*/
                match (e:Employee{uuid:{uuid}})-[r:ASSOCIATED_WITH{deleted:0}]->(d) return e, collect(d) as documents";
        }
        else{
            $q = "match(c:Company{uuid:{company}})
                with c
                match (dt:DocumentType{deleted:0})
                with c, dt
                match (c)-[:ASSOCIATED_WITH]-(d:Document)-[rt:IS_OF_TYPE]->(dt)
                with d
                /*where not((d)-[:NEXT]->(:Document))*/
                match (e:Employee{uuid:{uuid}})-[r:ASSOCIATED_WITH]->(d)
                return e, collect(d) as documents";
        }

        $query = $this->dbentity->createQuery($q);
        $query->addEntityMapping("e", Employee::class);
        $query->addEntityMapping("documents", Document::class, Query::HYDRATE_COLLECTION);
        $query->setParameter("uuid", $uuid);
        $query->setParameter("company", $company);
        
        $res = $query->getResult();
        if(isset($res[0])){
            $e = $res[0];       
            foreach ($e["documents"] as $doc) {
                $doc->getType();           
                foreach ($doc->getFiles() as $file) {
                    $file->getLanguage();
                }
            }  
            return $e;
        }
        return null;

    }
    public function getDocumentsOfType($uuid, $type){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $q = "match (dt:DocumentType{deleted:0, name:{type}})
            with dt
            match (d:Document{deleted:0})-[rt:IS_OF_TYPE]->(dt)
            with d
            /*where not((d)-[:NEXT]->(:Document))*/
            match (e:Employee{uuid:{uuid}})-[r:ASSOCIATED_WITH{deleted:0}]->(d) return e, collect(d) as documents";

        $query = $this->dbentity->createQuery($q);
        $query->addEntityMapping("e", Employee::class);
        $query->addEntityMapping("documents", Document::class, Query::HYDRATE_COLLECTION);
        $query->setParameter("uuid", $uuid);
        $query->setParameter("type", $type);
        
        $e =  isset($query->getResult()[0]) ? $query->getResult()[0]: null;
        if(NULL !== $e) 
        {
            foreach ($e["documents"] as $doc) {
                $doc->getType();          
                foreach ($doc->getFiles() as $file) {
                    $file->getLanguage();
                }
            }
        }
        return $e;
    }
    public function updateEmployeeAddress($jsonData){
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
    public function updateEmployeeContact($jsonData){
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
    public function deleteEmployeeAddress($jsonData){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $existing = $this->addresses->findOneBy(['uuid' => $jsonData->{'uuid'}]);
        if (NULL == $existing) {
            throw ApiException::serverError('Address not found');
        }
        $existing->setDeleted(1);
        $this->dbentity->flush();
        return $existing;
    } 
    public function deleteEmployeeContact($jsonData){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $existing = $this->contacts->findOneBy(['uuid' => $jsonData->{'uuid'}]);
        if (NULL == $existing) {
            throw ApiException::serverError('Contact not found');
        }
        $existing->setDeleted(1);
        $this->dbentity->flush();
        return $existing;
    }
    public function deleteEmployeeDocument($jsonData){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $existing = $this->documents->findOneBy(['uuid' => $jsonData->{'uuid'}]);
        if (NULL == $existing) {
            throw ApiException::serverError('Document not found');
        }
        $existing->setDeleted(1);
        $this->dbentity->flush();
        return $existing;
    } 
    public function getSpokenLanguage($employeeId){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $q = "match (e:Employee{uuid:{id}})-[r:SPEAKS]-(l:Language)
            return l";

        $query = $this->dbentity->createQuery($q);
        $query->addEntityMapping("l", Language::class);
        $query->setParameter("id", $employeeId);
        
        return $query->getResult();
    }
    public function getHireHistory($employeeId){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $cql = "match(e:Employee{uuid:{id}})-[:WORKS_IN]->(w:WorkPeriod)-[:WORKS_IN]->(c:Company)
            return c.shortname as company,
            apoc.date.format(apoc.date.parse(w.start, 'ms', 'yyyyMMdd'), 'ms', 'yyyy-MM-dd') + 'T00:00:00+00:00' as from,
            coalesce(apoc.date.format(apoc.date.parse(w.end, 'ms', 'yyyyMMdd'), 'ms', 'yyyy-MM-dd'), apoc.date.format(datetime().epochMillis , 'ms', 'yyyy-MM-dd')) + 'T00:00:00+00:00' as to
            order by w.start desc";
        
        $query = $this->dbentity->createQuery($cql);
        $query->setParameter("id", $employeeId);
        return $query->getResult();
    }
    public function getWorkHistory($employeeId){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $cql = "match(e:Employee{uuid:{id}})-[:OCCUPIES]->(o:Occupancy)-[:OCCUPIES]->(n) with e, o, n
            match(o)-[:START]->(ds:Day) with e, o, n, ds
            optional match(o)-[:END]->(de:Day) with e, o, n, ds, de
            optional match(n)<-[:HAS]-(cs:CSite)-[:IS_FOR]->(b:BusinessPartner) with e, o, n, ds, de, cs, b
            return trim(coalesce(b.name, '') + ' ' + coalesce(cs.name, '') + ' ' + coalesce(n.name, '')) as destination, 
            apoc.date.format(apoc.date.parse(ds.strrep, 'ms', 'yyyyMMdd'), 'ms', 'yyyy-MM-dd') + 'T00:00:00+00:00' as from,
            coalesce(apoc.date.format(apoc.date.parse(de.strrep, 'ms', 'yyyyMMdd'), 'ms', 'yyyy-MM-dd'), apoc.date.format(datetime().epochMillis , 'ms', 'yyyy-MM-dd')) + 'T00:00:00+00:00' as to
            order by ds.strrep desc";
        
        $query = $this->dbentity->createQuery($cql);
        $query->setParameter("id", $employeeId);
        return $query->getResult();
    }
    public function addSpokenLanguage($employeeId, $json) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();

        $query = "match(e:Employee{uuid:{employee}}),(l:Language{uuid:{language}})
                create unique (e)-[r:SPEAKS]->(l)
                return l";

        $client->run($query,['employee' => $employeeId, 'language' => $json->{'uuid'}]);
        $client->transaction()->commit(); 
    }    
    public function deleteSpokenLanguage($employeeId, $json) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();

        $query = "match(e:Employee{uuid:{employee}})-[r:SPEAKS]->(l:Language{uuid:{language}})
            delete r";

        $client->run($query,['employee' => $employeeId, 'language' => $json->{'uuid'}]);
        $client->transaction()->commit(); 
    }        
    public function getByUser($username) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        return $this->getEmployee(["username" => $username]);
    }        
    
    public  function startOccupancy(Request $request, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $occupany = new Occupancy();
        $this->mapper->map(json_decode($request->getBody()), $occupany);

        switch ($occupany->getType()) {
            case Occupancy::ON_SITE:
                break;
            case Occupancy::HOME:
                break;
            case Occupancy::SICK:
                $this->addEmployeeSick($args["id"], $occupancy);
                break;
            case Occupancy::VACATION:
                break;

            default:
                break;
        }
    }

    public  function endOccupancy(Request $request, $args){
        
    }
    
    public function getCoutryDuration(Request $request, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $q = "match(e:Employee)-[:OCCUPIES]->(o:Occupancy)-[:OCCUPIES]->(p:Project)-[:HAS]-(c:CSite)
            with e, o, p, c
            match(o)-[:START]-(d:Day)
            optional match(p)-[]-(a:Address)
            optional match(o)-[:END]->(de:Day)
            return e.uuid as uuid, e.lastname as lastname, e.name as name, ltrim(a.country) as country, sum(duration.inDays(date(d.strrep), date(de.strrep)).days) as days
            order by lastname asc, name asc, country asc";

        $query = $this->dbentity->createQuery($q);
        
        return $query->getResult();
    }
    
    public function getAbsentEmployees(Request $request, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $q = "match(e:Employee)--(:WorkPeriod{active:1}) with e
            match(e)-[:ABSENT]->(ab:Absence) where (ab.from <= {date} and ab.to >= {date}) or ab.from >= {date} 
            with e, ab";
        
        $query = $this->dbentity->createQuery($q, ["date" => time() * 1000]);
        
        return $query->getResult();
    }    
    
    
    public function getCoutrySiteDuration(Request $request, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        $datefrom = (new \DateTime(date("Y")."-01-01"))->format("YYYYmmdd");
        $dateto = (new \DateTime(date()))->format("YYYYmmdd");
        
        $q = "match(e:Employee)-[:OCCUPIES]->(o:Occupancy)-[:OCCUPIES]->(p:Project)-[:HAS]-(c:CSite)
            with e, o, p, c
            match(o)-[:START]-(d:Day) 
            optional match(p)-[]-(a:Address)
            optional match(o)-[:END]->(de:Day) 
            return e.uuid as uuid, e.lastname as lastname, e.name as name, c.name as site, ltrim(a.country) as country, sum(duration.inDays(date(d.strrep), date(de.strrep)).days) as days
            order by lastname asc, name asc, country asc";

        /*
        $q = "match(e:Employee)-[:OCCUPIES]->(o:Occupancy)-[:OCCUPIES]->(p:Project)-[:HAS]-(c:CSite)
            with e, o, p, c
            match(o)-[:START]-(d:Day) with e, o, p, c, d
            optional match(o)-[:END]->(de:Day) with e, o, p, c, d, de
            optional match(p)-[]-(a:Address)  with e, o, p, c, d, de, a
            where d.strrep >= {datefrom} and (de.strrep <= {dateto} or de.strrep is null)
            return e.uuid as uuid, e.lastname as lastname, e.name as name, c.name as site, 
            ltrim(a.country) as country, sum(duration.inDays(date(d.strrep), date(case when de.strrep is null then  {dateto} else de.strrep end)).days) as days,
            {datefrom} as datefrom, {dateto} as dateto
            order by lastname asc, name asc, country asc";
        */
        
        $query = $this->dbentity->createQuery($q);
        
        if(array_key_exists('date', $args)) { $datefrom = $args['date'];}
        if(array_key_exists('dateto', $args)) {$dateto = $args['dateto'];}
        
        $query->setParameter('datefrom', $datefrom);
        $query->setParameter('dateto', $dateto);

        return $query->getResult();
    }
    
    public function addAbsence(Request $request, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $absence = new Absence();
        $this->mapper->map(json_decode($request->getBody()), $absence);
        $employee =  $this->getEmployeeModel($args["id"]);
        IF(NULL === $employee || NULL === $absence){return;}

        $this->dbentity->persist($absence);
        $employee->AddAbsence($absence);
        $this->dbentity->flush();
    }
    
    public function updateAbsence(Request $request, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $absence = new Absence();
        $this->mapper->map(json_decode($request->getBody()), $absence);
        $existsing = $this->getAbsenceByUuid($absence->getUuid());
        IF(NULL === $existsing || NULL === $absence){return;}
        $existsing->setFrom($absence->getFrom());
        $existsing->setTo($absence->getTo());
        $existsing->setDuration($absence->getDuration());
        $existsing->setDescription($absence->getDescription());
        $existsing->setType($absence->getType());
        
        $this->dbentity->flush();

    }
    
    public function deleteAbsence(Request $request, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $absence = new Absence();
        $this->mapper->map(json_decode($request->getBody()), $absence);
        $cql = "match(e:Employee{uuid:{employee}})-[r:ABSENT]->(a:Absence{uuid:{absence}}) delete r delete a";
        $query = $this->dbentity->createQuery($cql);
        $query->setParameter("employee", $args["id"]);
        $query->setParameter("absence", $absence->getUuid());
        $query->execute();
    }
    
    public function listAbsence(Request $request, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $cql = "match(e:Employee{uuid:{employee}})-[:ABSENT]->(a:Absence) return a";
        $query = $this->dbentity->createQuery($cql);
        $query->setParameter("employee", $args["id"]);
        $query->addEntityMapping('a', Absence::class);
        return $query->getResult();
    }
    
    /**
     * 
     * @param string $employeeid
     * @return Document[]
     */
    public function getPromissoryNotes($employeeid) {

        $cql = "match(z:Employee{uuid:{employee}})-[]->(d:Document{active:1})-[]->(t:DocumentType{promissorynote:true}) return d";
        $query = $this->dbentity->createQuery($cql);
        $query->setParameter("employee", $employeeid);
        $query->addEntityMapping('d', Document::class);

        return $query->getResult();
    }
    // </editor-fold>


    // <editor-fold defaultstate="collapsed" desc="private functions">

    private function addDocument(Employee $employee, Document $document){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        if (NULL === $employee) {
            throw ApiException::serverError('Employee not foud');
        }
        if (NULL === $document) {
            throw ApiException::serverError('Document not foud');
        }    
        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
        $query = 'match(e:Employee{uuid:{employeeId}})
            match(d:Document{uuid:{documentId}})
            create (e)-[r:ASSOCIATED_WITH{active:1, deleted:0}]->(d)';

        $client->run($query,['documentId' => $document->getUuid(),
            'employeeId' => $employee->getUuid()]);
        $client->transaction()->commit(); 
        
        return $employee;
    }
    private function addAddress(Employee $employee, Address $address) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        if (NULL === $employee) {
            throw ApiException::serverError('Employee not foud');
        }
        if (NULL === $address) {
            throw ApiException::serverError('Address not foud');
        }
        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
        $query = 'match(e:Employee{uuid:{employeeId}})
                create (a:Address{'.$address->getQueryFields(). '})
                create UNIQUE (e)-[r:LIVES{active:1, deleted:0}]->(a)';

        $client->run($query, array_merge($address->getQueryFieldsParams(), ['employeeId'=>$employee->getUuid()]));
        $client->transaction()->commit();   
        return $employee;
    }  
    private function addContact(Employee $employee, Contact $contact) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        if (NULL === $employee) {
            throw ApiException::serverError('Employee not foud');
        }
        if (NULL === $contact) {
            throw ApiException::serverError('Contact not foud');
        }
        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
        $existing = $this->contacts->findOneBy(['name' => $contact->getname(), 'lastname' => $contact->getlastname(), 'phone' => $contact->getphone()]);

        if ($existing !== NULL) { return $this->bindContact($employee, $existing); }

        $query = 'match(e:Employee{uuid:{employeeId}})
            create (c:Contact{'. $contact->getQueryFields() .'})
            create (e)-[r:CONTACT{active:1, deleted:0}]->(c)';

        $client->run($query,array_merge(['employeeId' => $employee->getUuid()], $contact->getQueryFieldsParams()));
        $client->transaction()->commit(); 
            
        return $employee;
    }
    private function bindContact(Employee $employee, Contact $contact) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        if (NULL === $employee) {
            throw ApiException::serverError('Employee not foud');
        }
        if (NULL === $contact) {
            throw ApiException::serverError('Contact not foud');
        }
        $query = 'match(e:Employee{uuid:{employeeId}}) match(c:Contact{uuid:{contactId}}) create (e)-[r:CONTACT{active:1, deleted:0}]->(c)';
        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
        $client->run($query,array_merge(['contactId' => $contact->getUuid(), 'employeeId' => $employee->getUuid()], $contact->getQueryFieldsParams()));
        $client->transaction()->commit(); 
        return $employee;
    }  
    private function getEmployee($criteria) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $employee = $this->employees->findOneBy($criteria);
        return $employee;
    }    
    private function getEmployees($criteria, $order = null){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $employees = $this->dbentity->getRepository(Employee::class);
        $emps  = $employees->findBy($criteria, $order);
        return $emps;
    }
    private function addEmployeeWorkPeriod(EmployeeHire $employee){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $period = new WorkPeriod();
        $period->setStart($employee->getHiredate());
        
        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
        
        $stack = $client->stack();
        $stack->push("create(w:WorkPeriod{". $period->getQueryFields() ."})", $period->getQueryFieldsParams());
        $stack->push("match(e:Employee{uuid:{eid}}) "
                . "match(w:WorkPeriod{uuid:{wid}}) "
                . "create(e)-[:WORKS_IN]->(w)", ["eid" => $employee->getUuid(), "wid" => $period->getUuid()]);
        $stack->push("match(c:Company{uuid:{cid}}) "
                . "match(w:WorkPeriod{uuid:{wid}}) "
                . "create(w)-[:WORKS_IN]->(c)", ["cid" => $employee->getHireemployer()->getUuid(), "wid" => $period->getUuid()]);
        
        $stack->push("match(e:Employee{uuid:{eid}})-[:OCCUPIES]->(o:Occupancy{active:1})-[:OCCUPIES]->(:Company) with e, o "
                . "match(c:Company{uuid:{cid}}) with e, o, c "
                . "match (year:Year{value:{y}})-[:CONTAINS]->(month:Month {value:{m}})-[:CONTAINS]->(today:Day{value:{d}}) with e, o, c, today "
                . "set o.active = 0 "
                . " create (oc:Occupancy{active:1, deleted:0}) "
                . " create (oc)-[:START]->(today) "
                . " create (o)-[:END]->(today) "
                . " create (e)-[:OCCUPIES]->(oc)-[:OCCUPIES]->(c) "
                . " create (o)-[:NEXT]-> (oc)" , ["cid" => $employee->getHireemployer()->getUuid(), "eid" => $employee->getUuid(), 'y' => idate("Y"),'m' => idate("m"),'d' => idate("d"),]);
        
        $client->runStack($stack);
    }
    private function endEmployeePeriod($period){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        if(null === $period){return;}
        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
        $query = "match(p:WorkPeriod{uuid:{id}}) set p.end = {end}, p.active = 0";
        $client->run($query, ["id" => $period->getUuid(), "end" => $period->getEnd()]);
        $client->transaction()->commit();
    }
    private function updateEmployeeWorkPeriod($period){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        if(null === $period){return;}       
        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
        $query = "match(p:WorkPeriod{uuid:{id}}) set p.start = {start}, p.end = {end}, p.active = {a}";
        $client->run($query, ["id" => $period->getUuid(), "start" => $period->getStart(), "end" => $period->getEnd(), "a" => $period->getActive()]);
        $client->transaction()->commit();
        
    }    

    /**
     * 
     * @param string $id
     * @param Occupancy $occupancy
     * @return Occupancy
     */
    private function addEmployeeSick($id, $occupancy){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        if (null === $occupancy) {
            return;
        }

        if (null !== $occupancy->getCompany()) {
            return $this->addEmployeeSickHome($id, $occupancy);
        }
        if (null !== $occupancy->getProject()) {
            return $this->addEmployeeSickOnSite($id, $occupancy);
        }
    }
    
    /**
     * 
     * @param string $id
     * @param Occupancy $occupancy
     * @return Occupancy
     */
    private function addEmployeeSickHome($id, $occupancy){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        if (null === $occupancy) {
            return;
        }
        if (null === $occupancy->getCompany()) {
            return;
        }
        $d = \DateTime::createFromFormat('Ymd', $occupancy->getStart()->getDate());
        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
        $stack = $client->stack();
        $query = "match(y:Year{value:{y}})-[:CONTAINS]->(m:Month{value:{m}})-[:CONTAINS]->(d:Day{value:{d}}) with d
            match(e:Employee{uuid:{id}})-[:WORKS_IN]->(:WorkPeriod{active:1})-[:WORKS_IN]->(c:Company) with d, e, c
            match(e)-[:OCCUPIES]->(ao:Occupancy {active:1}) where not((ao)-[:END]->(:Day))  with d, e, ao
            set ao.end = d.strrep, ao.active = 0
            create (ao)-[:END]->(d)  
            create (o:Occupancy{active:1, start:d.strrep})-[:START]->(d) 
            create (e)-[:OCCUPIES]->(o)-[:OCCUPIES]->(c)
            create (ao)-[:NEXT]->(o)
            return o ";
        $stack->push($query, ["id" => $id, "y" => (int)$d->format('Y'), "m" => (int)$d->format('m'), "d" => (int)$d->format('d')]);
        return $client->runStack($stack);    
    }
    
    /**
     * 
     * @param string $id
     * @param Occupancy $occupancy
     * @return Occupancy
     */
    private function addEmployeeSickOnSite($id, $occupancy){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        if (null === $occupancy) {
            return;
        }
        
    }


    /**
     * 
     * @param string $employeeId
     * @return EmployeeModel
     */
    private function getEmployeeModel($employeeId) {
        return $this->dbentity->getRepository(EmployeeModel::class)->findOneBy(["uuid" => $employeeId]);
        
    }

    
    /**
     * 
     * @param string $id
     * @return Absence
     */
    private function getAbsenceByUuid($id) {
        return $this->dbentity->getRepository(Absence::class)->findOneBy(["uuid" => $id]);
        
    }


    // </editor-fold>
    
}

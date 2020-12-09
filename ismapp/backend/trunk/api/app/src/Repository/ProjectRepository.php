<?php
namespace App\Repository;

use Psr\Log\LoggerInterface;
use GraphAware\Neo4j\Client\Client;
use GraphAware\Neo4j\Client\ClientBuilder;
use GraphAware\Neo4j\OGM\EntityManager;
use GraphAware\Neo4j\OGM\Query;
use Slim\Http\Request;
use Slim\Http\Response;
use App\Common\ApiException;

use App\Models\Employee;
use App\Models\Document;
use App\Models\Address;
use App\Models\Contact;
use App\Models\CSite;
use App\Models\Company;
use App\Models\Project;
use App\Models\Shift;
use App\Models\Day;
use App\Models\ShiftDay;
use App\Models\ProjectMonthSummary;
use App\Models\Car;
use App\Models\Ewr;
use App\Models\ProjectSiteData;
use App\Models\EwrSiteData;
use App\Models\EmployeeList;
use App\Models\ProjectWorkPeriod;
use App\Models\WorkPlacePlan;

class ProjectRepository {
    
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
    private $projects;
    private $connection;
    private $connalias;
    private $ewrs;

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
        $this->projects = $this->dbentity->getRepository(Project::class);
        $this->ewrs = $this->dbentity->getRepository(Ewr::class);
        $this->dbentity->clear();
    }
    public function __destruct() {
        $this->dbentity->clear();
    }
    
    // <editor-fold defaultstate="collapsed" desc="public functions Projects related">
    public function getActiveProject(){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        return $this->getProjects([], ['lastname' => 'asc', 'name' => 'asc']);
    }
    public function getAllProjects()  {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        return $this->getProjects([], ['crdate' => 'desc']);
    }
    public function getProjectsByStatus(Request $request, $args)  {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        return $this->getProjects(['status' => (int)$args['status']], ['crdate' => 'desc']);
    }
    public function getEmployeeProjectsByStatus(Request $request, $args)  {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $cql = "match (p:Project{status:{status}})-[:OCCUPIES]-(o:Occupancy)-[:OCCUPIES]-(e:Employee{uuid:{employee}}) return p";        
        $query = $this->dbentity->createQuery($cql);
        $query->addEntityMapping('p', Project::class);
        $query->setParameter("status", $args["status"]);
        $query->setParameter("employee", $args["employee"]);
        return $query->execute();
    }    
    public function getUserProjects(Request $request, $args)  {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $cql = "match (p:Project{status:1})-[:OCCUPIES]-(o:Occupancy)-[:OCCUPIES]-(e:Employee{username:{user}}) return p";        
        $query = $this->dbentity->createQuery($cql);
        $query->addEntityMapping('p', Project::class);
        $query->setParameter("user", $args["user"]);
        return $query->execute();
    }    
    
    public function getUserSites(Request $request, $args)  {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $cql = "match (s:CSite)-[:HAS]->(p:Project{status:1})where exists((p)<-[:OCCUPIES]-(:Occupancy)<-[:OCCUPIES]-(:Employee{username:{user}})) return s";        
        $query = $this->dbentity->createQuery($cql);
        $query->addEntityMapping('s', CSite::class);
        $query->setParameter("user", $args["user"]);
        return $query->execute();
    }    
    
    public function getCsiteProjects($companyId, $csite){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        $company = $this->companies->findOneBy(['uuid' => $companyId]);
        if (NULL === $company) {
            return;
        }
        $cql = 'match(p:Project)<-[r:WORKS_ON]-(c:Company{uuid:{cId}}) with p match(p)-[:HAS]-(:CSite{uuid:{site}}) return p';        
        //$cql = 'match(p:Project)-[:HAS]-(:CSite{uuid:{site}}) return p';        
        $query = $this->dbentity->createQuery($cql);
        $query->addEntityMapping('p', Project::class);
        $query->setParameter('cId', $companyId);
        $query->setParameter('site', $csite);
        return $query->execute();
    }
    public function addProject($companyId, $constructionSiteId, $jsonData){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $this->mapper->bStrictNullTypes = FALSE;
        
        $project = new Project();
        $this->mapper->map($jsonData, $project);
        
        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
        $stack = $client->stack();
        
        $query = "match(company:Company{uuid:{cId}}) 
            with company
            match (site:CSite{uuid:{siteId}})
            with company, site
            create (p:Project{".$project->getQueryFields()."})
            create (company)-[r:WORKS_ON]->(p)<-[r1:HAS]-(site)";
        
        $stack->push($query, array_merge($project->getQueryFieldsParams(), ['cId' => $companyId, 'siteId' => $constructionSiteId]));

        $query= "match(p:Project{uuid:{project}})
            with p
            match(y:Year{value:{y}})-[:CONTAINS]-(m:Month{value:{m}})-[:CONTAINS]-(day:Day{value:{day}})
            create (p)-[:START]->(day)";

        $stack->push($query ,
                [
                    'project' => $project->getUuid(),
                    'y' => $project->getStart()->getMonth()->getYear()->getValue(), 
                    'm' => $project->getStart()->getMonth()->getValue(), 
                    'day' => $project->getStart()->getValue()
                ]); 
        
        $query= "match(p:Project{uuid:{project}})
            with p
            match(y:Year{value:{y}})-[:CONTAINS]-(m:Month{value:{m}})-[:CONTAINS]-(day:Day{value:{day}})
            create (p)-[:END]->(day)";

        $stack->push($query ,
                [
                    'project' => $project->getUuid(),
                    'y' => $project->getEnd()->getMonth()->getYear()->getValue(), 
                    'm' => $project->getEnd()->getMonth()->getValue(), 
                    'day' => $project->getEnd()->getValue()
                ]); 

        $client->runStack($stack);
        $client->transaction()->commit();
        return $this->getProjectByUuid($project->getUuid());
    }
    public function updateProject($jsonData){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $this->mapper->bStrictNullTypes = FALSE;
        
        $project = new Project();
        $this->mapper->map($jsonData, $project);
 
        $project->setProjectId($jsonData->{'uuid'});
        
        $existing = $this->getProjectByUuid($project->getUuid());
        if (NULL == $existing) {
            throw ApiException::serverError('Project not found');
        }

        $existing->import($project);
        $existing->setmfdate('');

        $this->dbentity->persist($existing);
        $this->dbentity->flush();
        
        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
        
        $stack = $client->stack();
        
        if(NULL !== $project->getStart()){            
            $query = "match(p:Project{uuid:{project}}) with p optional match (p)-[r:START]-() delete r";
            $stack->push($query ,['project' => $project->getUuid()]);
            
            $query= "match(p:Project{uuid:{project}})
                with p
                match(y:Year{value:{y}})-[:CONTAINS]-(m:Month{value:{m}})-[:CONTAINS]-(day:Day{value:{day}})
                create (p)-[:START]->(day)";

            $stack->push($query ,
                    [
                        'project' => $project->getUuid(),
                        'y' => $project->getStart()->getMonth()->getYear()->getValue(), 
                        'm' => $project->getStart()->getMonth()->getValue(), 
                        'day' => $project->getStart()->getValue()
                    ]); 
        }

        if(NULL !== $project->getEnd()){  
            $query = "match(p:Project{uuid:{project}}) with p optional match (p)-[r:END]-() delete r";
            $stack->push($query ,['project' => $project->getUuid()]);
            
            $query= "match(p:Project{uuid:{project}})
                with p
                match(y:Year{value:{y}})-[:CONTAINS]-(m:Month{value:{m}})-[:CONTAINS]-(day:Day{value:{day}})
                create (p)-[:END]->(day)";

            $stack->push($query ,
                    [
                        'project' => $project->getUuid(),
                        'y' => $project->getEnd()->getMonth()->getYear()->getValue(), 
                        'm' => $project->getEnd()->getMonth()->getValue(), 
                        'day' => $project->getEnd()->getValue()
                    ]); 
        }
        $client->runStack($stack);
        $client->transaction()->commit();
        
        
        return $this->getProjectByUuid($project->getUuid());
    } 
    public function deleteProject(Request $request, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();

        $query = "match(p:Project{uuid:{project}})-[r]-() delete r delete p";
           
        $client->run($query, ["project" => $args["id"]]);
        $client->transaction()->commit();
        
        return null;
    }
    public function addShift($project, $employee, $jsonData){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $this->mapper->bStrictNullTypes = FALSE;
        
        $shift = new Shift();
        $this->mapper->map($jsonData, $shift);

        
        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
        $stack = $client->stack();
        
        $query = "match(project:Project{uuid:{project}})
            match(employee:Employee{uuid:{employee}})
            match(y:Year{value:{y}})-[:CONTAINS]-(m:Month{value:{m}})-[:CONTAINS]-(day:Day{value:{day}})
            match(employee)-[wp:WORKS]->(shift:Shift{shifttype:{type}})-[wd:WORKDAY]->(day)
            match(shift)-[we:WORKS]->(project)
            delete we delete wp delete wd delete shift";
        
        $stack->push($query, ["project" => $project,
            "employee" => $employee,
            "type" => $shift->getShifttype(),
            "y" => $shift->getWorkday()->getYearValue(), 
            "m" => $shift->getWorkday()->getMonthValue(), 
            "day" => $shift->getWorkday()->getValue()]);


        if($shift->getHours() > 0){
            $query = "match(project:Project{uuid:{project}})
                match(employee:Employee{uuid:{employee}})
                match(y:Year{value:{y}})-[:CONTAINS]-(m:Month{value:{m}})-[:CONTAINS]-(day:Day{value:{day}})
                create (shift:Shift{".$shift->getQueryFields()."})
                create (employee)-[:WORKS]->(shift)
                create (shift)-[:WORKS]->(project)
                create (shift)-[:WORKDAY]->(day)
                return shift";
        
            $stack->push($query, array_merge($shift->getQueryFieldsParams(),
                    ["project" => $project,
                "employee" => $employee,
                "y" => $shift->getWorkday()->getYearValue(), 
                "m" => $shift->getWorkday()->getMonthValue(), 
                "day" => $shift->getWorkday()->getValue()]));   
        }
     

        $client->runStack($stack);
        $client->transaction()->commit();
        return null;
    }
    public function getWorkShiftsByDay($project, $jsonData) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $this->mapper->bStrictNullTypes = FALSE;

        $day = new Day();
        $this->mapper->map($jsonData, $day);

        $cql = "match(y:Year{value:{y}})-[:CONTAINS]-(m:Month{value:{m}})-[:CONTAINS]-(d:Day{value:{d}})
                with d
                match(e:Employee)-[:OCCUPIES]-(o:Occupancy)-[:OCCUPIES]-(p:Project{uuid:{project}})
                with e,d, o, p
                match(o)-[:START]-(sd:Day)
                with e,d, o, p, sd
                optional match(o)-[:END]-(ed:Day)
                with e,d, o, p , sd, ed
                where sd.strrep<=d.strrep and d.strrep<= case when ed is null then d.strrep else ed.strrep end
                with p, d,  collect(e) as employees unwind  employees as employee
                with p, d, employee
                optional match (employee)-[:WORKS]-(s:Shift)-[:WORKS]-(p), (s)-[:WORKDAY]-(d)
                return employee, collect(s) as shifts ";

        $query = $this->dbentity->createQuery($cql);
        $query->addEntityMapping('employee', Employee::class );
        $query->addEntityMapping('shifts', Shift::class, Query::HYDRATE_COLLECTION );
        $query->setParameter('project', $project);
        $query->setParameter('y', $day->getYearValue());
        $query->setParameter('m', $day->getMonthValue());
        $query->setParameter('d', $day->getValue());

        return $query->getResult();
        
    }
    public function getWorkShifts($project) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
       
        $cql = "match (day:Day)<-[:WORKDAY]-(shift:Shift)-[:WORKS]->(project:Project{uuid:{project}})"
                . " return day.strrep, shift.shifttype, sum(shift.hours) as hours ";
   
        $result = $this->dbclient->run($cql, ['project' => $project]);
        
        foreach ($result->records() as $record) {
            $dd= new ShiftDay();
            $dd->setStrrep($record->get("day.strrep"));
            $dd->setShifttype($record->get("shift.shifttype"));
            $dd->setHours($record->get("hours"));
            $rec[] = $dd;
        }
 
        return $rec;
    }      
    public function getMonthSummary($project, int $year, int $month){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        $query = "match(:Year{value:{y}})-[:CONTAINS]->(:Month {value:{m}})-[:CONTAINS]->(d:Day)
            return  null as lastname, null as name, d.strrep, 0 as hours
            union
            match(y:Year{value:{y}})-[:CONTAINS]-(m:Month{value:{m}})-[:CONTAINS]-(d:Day)
                with d
                match(e:Employee)-[:OCCUPIES]-(o:Occupancy)-[:OCCUPIES]-(p:Project{uuid:{project}})
                with e,d, o, p
                match(o)-[:START]-(sd:Day)
                with e,d, o, p, sd
                optional match(o)-[:END]-(ed:Day)
                with e,d, o, p , sd, ed
                where sd.strrep<=d.strrep and d.strrep<= case when ed is null then d.strrep else ed.strrep end
                with p, d,  collect(e) as employees unwind  employees as employee
                with p, d, employee
                optional match (employee)-[:WORKS]-(s:Shift)-[:WORKS]-(p), (s)-[:WORKDAY]-(d)
                return employee.lastname as lastname, employee.name as name, d.strrep, sum(s.hours) as hours
                order by lastname";
        
        $result =  $this->dbclient->run($query, ['project'=> $project, 'y' => (int)$year, 'm' => (int)$month]);

        foreach ($result->records() as $record) {
            $dd= new ProjectMonthSummary();
            $dd->setName($record->get("name"));
            $dd->setLastname($record->get("lastname"));
            $dd->setDate($record->get("d.strrep"));
            $dd->setHours($record->get("hours"));
            $rec[] = $dd;
        }
 
        return $rec;
    }
    public function getProjectSiteData($project){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        $cql = "match(p:Project{uuid:{project}})<-[:HAS]-(s:CSite) with p, s
                match (s)-[:IS_FOR]->(bp:BusinessPartner) 
                with p, s, bp
                with p, s, bp
                optional match (bp)-[:ASSOCIATED_WITH]-(d:Document{active:1, deleted:0})-[:IS_OF_TYPE]-(t:DocumentType{name:'LOGO'})
                with p, s, bp, d
                optional match (d)-[:ATTACHED]-(f:File{active:1, deleted:0})
                with p, s, bp, d, f
                optional match (s)-[:ASSOCIATED_WITH]-(sd:Document{active:1, deleted:0})-[:IS_OF_TYPE]-(t:DocumentType{name:'LOGO'})
                with p, s, bp, d, f, sd
                optional match (sd)-[:ATTACHED]-(sf:File{active:1, deleted:0})
                with p, s, bp, d, f, sd, sf
                return s.name as sitename, s.description as sitedescription, s.status as sitestatus
                    ,p.name as projectname, p.description as projectdescription, p.projectnumber as projectnumber, p.externalnumber as externalnumber, p.state as projectstate, p.status as projectstatus
                    ,f.uuid as partnerlogo, sf.uuid as sitelogo";
        
       
        $result =  $this->dbclient->run($cql, ['project'=> $project]);

        $record = $result->firstRecord();
       
            $dd= new ProjectSiteData();
            $dd->setSitename($record->get("sitename"));

            $dd->setSitename($record->get("sitename"));
            $dd->setSitedescription($record->get("sitedescription"));
            $dd->setSitestatus($record->get("sitestatus"));
            $dd->setProjectname($record->get("projectname"));
            $dd->setProjectdescription($record->get("projectdescription"));
            $dd->setProjectnumber($record->get("projectnumber"));
            $dd->setExternalnumber($record->get("externalnumber"));
            $dd->setProjectstate($record->get("projectstate"));
            $dd->setProjectstatus($record->get("projectstatus"));
            $dd->setPartnerlogo($record->get("partnerlogo"));
            $dd->setSitelogo($record->get("sitelogo"));

  
        return $dd;
        
    }
    public function getEwrData(Request $request, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $cql = "match(ewr:Ewr{uuid:{ewr}})-[:EWR]->(p:Project)<-[:HAS]-(s:CSite) with ewr, p, s
                match (s)-[:IS_FOR]->(bp:BusinessPartner) 
                with ewr,p, s, bp
                with ewr,p, s, bp
                optional match (bp)-[:ASSOCIATED_WITH]-(d:Document{active:1, deleted:0})-[:IS_OF_TYPE]-(t:DocumentType{name:'LOGO'})
                with ewr,p, s, bp, d
                optional match (d)-[:ATTACHED]-(f:File{active:1, deleted:0})
                with ewr,p, s, bp, d, f
                optional match (s)-[:ASSOCIATED_WITH]-(sd:Document{active:1, deleted:0})-[:IS_OF_TYPE]-(t:DocumentType{name:'LOGO'})
                with ewr,p, s, bp, d, f, sd
                optional match (sd)-[:ATTACHED]-(sf:File{active:1, deleted:0})
                with ewr,p, s, bp, d, f, sd, sf
                return s.name as sitename, s.description as sitedescription, s.status as sitestatus
                    ,p.name as projectname, p.description as projectdescription, p.projectnumber as projectnumber, p.externalnumber as externalnumber, p.state as projectstate, p.status as projectstatus
                    ,ewr.number as ewrnumber, ewr.externalnumber as ewrexternalnumber, ewr.hours as ewrhours, ewr.materialcosts as ewrmaterialcosts, ewr.description as ewrdescription
                    ,f.uuid as partnerlogo, sf.uuid as sitelogo";
        $result =  $this->dbclient->run($cql, ['ewr'=> $args["eid"]]);

        $record = $result->firstRecord();
       
            $dd= new EwrSiteData();
            $dd->setSitename($record->get("sitename"));

            $dd->setSitename($record->get("sitename"));
            $dd->setSitedescription($record->get("sitedescription"));
            $dd->setSitestatus($record->get("sitestatus"));
            $dd->setProjectname($record->get("projectname"));
            $dd->setProjectdescription($record->get("projectdescription"));
            $dd->setProjectnumber($record->get("projectnumber"));
            $dd->setExternalnumber($record->get("externalnumber"));
            $dd->setProjectstate($record->get("projectstate"));
            $dd->setProjectstatus($record->get("projectstatus"));
            $dd->setPartnerlogo($record->get("partnerlogo"));
            $dd->setSitelogo($record->get("sitelogo"));
            
            $dd->setEwrnumber($record->get("ewrnumber"));
            $dd->setEwrexternalnumber($record->get("ewrexternalnumber"));
            $dd->setEwrdescription($record->get("ewrdescription"));
            $dd->setEwrhours($record->get("ewrhours"));
            $dd->setEwrmaterialcosts($record->get("ewrmaterialcosts"));

  
        return $dd;
        
    }
    public function getActiveEmployees($project){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

//        $cql = "match(e:Employee)-[:OCCUPIES]->(o:Occupancy{active:1})-[:OCCUPIES]->(project:Project{uuid:{project}})
//            with e
//            optional match (e)-[:LIVES]->(a:Address) with e, a
//            order by id(a) desc
//            with e, collect(a)[..1] as address unwind (CASE address WHEN [] then [null] else address end )as addr
//            optional match (e)-[:CONTACT]->(c:Contact) with e, addr, c
//            order by id(c) desc
//            with e, addr, collect(c)[..1] as contact unwind (CASE contact WHEN [] then [null] else contact end )as cont
//            with e, addr, cont
//            order by e.lastname asc
//            return e as employee, addr as address, cont as contact";
//        $query = $this->dbentity->createQuery($cql);
//        $query->setParameter("project", $project);
//        //$query->addEntityMapping('company', Company::class);
//        $query->addEntityMapping('employee', Employee::class);
//        //$query->addEntityMapping('workperiod', WorkPeriod::class);
//        $query->addEntityMapping('address', Address::class);
//        $query->addEntityMapping('contact', Contact::class);
//        $res =  $query->execute();
        $cql = "match(employee:Employee)-[:OCCUPIES]->(o:Occupancy{active:1})-[:OCCUPIES]->(project:Project{uuid:{project}}) return employee";
        $query = $this->dbentity->createQuery($cql);
        $query->setParameter("project", $project);
        $query->addEntityMapping('employee', EmployeeList::class);
        $res =  $query->execute();        
        return $res;
    }    
    public function getActiveCars($project){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $cql = "match(car:Car)-[:OCCUPIES]->(o:Occupancy{active:1})-[:OCCUPIES]->(project:Project{uuid:{project}})
            with car
            match (car)-[:BELONGS_TO]->(c:Company)
            return c as company, car";
        $query = $this->dbentity->createQuery($cql);
        $query->setParameter("project", $project);
        $query->addEntityMapping('car', Car::class);
        $query->addEntityMapping('company', Company::class);
        return $query->execute();
    }    
    public function addExpense($project, $jsonData){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
    }    
    public function addIncome($project, $jsonData){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
    }    
    public function updateExpense($project, $jsonData){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
    }    
    public function updateIncome($project, $jsonData){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
    }    
    public function removeExpense($project, $jsonData){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
    }

    public function addRental(Request $request, $args)  {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
    }
    public function endRental(Request $request, $args)  {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
    }
    public function removeRental(Request $request, $args)  {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
    }
    public function updateRental(Request $request, $args)  {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
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
            match (p:Project{uuid:{uuid}})-[r:ASSOCIATED_WITH{deleted:0}]->(d) return p, collect(d) as documents";
        
        $query = $this->dbentity->createQuery($q);
        $query->addEntityMapping("p", Project::class);
        $query->addEntityMapping("documents", Document::class, Query::HYDRATE_COLLECTION);
        $query->setParameter("uuid", $uuid);

        return $query->getResult()[0];
    }
    public function addDocument($jsonData){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $project = $this->getProjectByUuid($jsonData->{'parentuuid'});
        if (NULL === $project) {
            throw ApiException::serverError('Project not found');
        }
        $document = $this->documents->findOneBy(['uuid' => $jsonData->{'document'}->{'uuid'}]);
        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
        
        $query = 'match(p:Project{uuid:{project}})
            match(d:Document{uuid:{documentId}})
            create (p)-[r:ASSOCIATED_WITH{active:1, deleted:0}]->(d)';

        $client->run($query,['documentId' => $document->getUuid(), 'project' => $jsonData->{'parentuuid'}]);
        $client->transaction()->commit(); 
        return $this->getProjectByUuid($jsonData->{'parentuuid'});
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
    
    // <editor-fold defaultstate="collapsed" desc="public functions Contacts related">
    public function getContacts($uuid)   {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $query = $this->dbentity->createQuery("match(p:Project{uuid:{project}})-[r:CONTACT{deleted:0}]-(c:Contact{deleted:0})
                return p, collect(c) as contacts");
        $query->addEntityMapping("p", Project::class);
        $query->addEntityMapping("contacts", Contact::class, Query::HYDRATE_COLLECTION);
        $query->setParameter("project", $uuid);
        return $query->getResult()[0];        
       
    }
    public function addContact($jsonData){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $project = $this->getProjectByUuid($jsonData->{'parentuuid'});
        if (NULL === $project) {
            throw ApiException::serverError('Project not found');
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
            $this->persistContact($project, $contact);
        }
        return $this->getProjectByUuid($project->getUuid());
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
        $query = $this->dbentity->createQuery("match(p:Project{uuid:{project}})-[r:LIVES{deleted:0}]-(a:Address{deleted:0}) 
            return p, collect(a) as addresses");
        $query->addEntityMapping("p", Project::class);
        $query->addEntityMapping("addresses", Address::class, Query::HYDRATE_COLLECTION);
        $query->setParameter("project", $uuid);
        return $query->getResult()[0];
    }
    public function addAddress($jsonData){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $project = $this->getProjectByUuid($jsonData->{'parentuuid'});
        if (NULL === $project) {
            throw ApiException::serverError('Project not found');
        }
        $addressJsonData = $jsonData->{'address'};
        $address = $this->mapper->map($addressJsonData, new Address());
        

        $this->persistAddress($project, $address);
        return $this->getProjectByUuid($jsonData->{'parentuuid'});
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
    public function bindAddress($jsonData){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        return $this->addAddress($jsonData);
        
        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
        $stack = $client->stack();
        
        $query = "match(p:Project{uuid:{project}})-[r:LIVES]-(:Address) delete r";
        $stack->push($query, ["project" => $project->getUuid()]);

        $query = 'match(p:Project{uuid:{project}}), (a:Address{uuid:{address}})
        create UNIQUE (p)-[r:LIVES{active:1, deleted:0}]->(a)';

        $stack->push($query, ["project" => $jsonData->{'parentuuid'}, "address" => $jsonData->{'address'}->{"uuid"}]);
        
        $client->runStack($stack);
        
        $client->transaction()->commit();

        return null;
    }    

    // </editor-fold>

    // <editor-fold defaultstate="collapsed" desc="private functions Contact related">
    private function persistContact(Project $project, Contact $contact) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        if (NULL === $project) {
            throw ApiException::serverError('Project not foud');
        }
        if (NULL === $contact) {
            throw ApiException::serverError('Contact not foud');
        }
        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
        //$existing = $this->contacts->findOneBy(['name' => $contact->getname(), 'lastname' => $contact->getlastname(), 'phone' => $contact->getphone()]);
        //if ($existing !== NULL) { return $this->bindContact($project, $existing); }

        $query = 'match(p:Project{uuid:{project}})
            create (c:Contact{'. $contact->getQueryFields() .'})
            create (p)-[r:CONTACT{active:1, deleted:0}]->(c)';

   
        $client->run($query,array_merge(['project' => $project->getUuid()], $contact->getQueryFieldsParams()));
        $client->transaction()->commit(); 
            
        return $project;
    }
    private function bindContact(Project $project, Contact $contact) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        if (NULL === $project) {
            throw ApiException::serverError('Project not foud');
        }
        if (NULL === $contact) {
            throw ApiException::serverError('Contact not foud');
        }
        $query = 'match(p:Project{uuid:{project}}) match(c:Contact{uuid:{contactId}}) create (p)-[r:CONTACT{active:1, deleted:0}]->(c)';
        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
        $client->run($query,array_merge(['contactId' => $contact->getUuid(), 'project' => $project->getUuid()], $contact->getQueryFieldsParams()));
        $client->transaction()->commit(); 
        return $project;
    }  
    // </editor-fold>

    // <editor-fold defaultstate="collapsed" desc="private functions Address related">
    private function persistAddress(Project $project, Address $address) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        if (NULL === $project) {
            throw ApiException::serverError('Constriction site not foud');
        }
        if (NULL === $address) {
            throw ApiException::serverError('Address not foud');
        }
        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
        $query = 'match(p:Project{uuid:{project}})
                create (a:Address{'.$address->getQueryFields(). '})
                create UNIQUE (p)-[r:LIVES{active:1, deleted:0}]->(a)';

        $client->run($query, array_merge($address->getQueryFieldsParams(), ['project'=>$project->getUuid()]));
        $client->transaction()->commit();   
        return $project;
    }      
    // </editor-fold>
    
    // <editor-fold defaultstate="collapsed" desc="private functions Project related">
    /**
     * 
     * @param string $uuid
     * @return Project
     */
    private function getProjectByUuid($uuid) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        return $this->getProject(['uuid' => $uuid]);
    }
    
    private function getProject($criteria) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        return $this->projects->findOneBy($criteria);
    } 
    
    private function getProjects($criteria, $order = null){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $projects = $this->dbentity->getRepository(Project::class);
        $proj  = $projects->findBy($criteria, $order);
        return $proj;
    }
    
    
    // </editor-fold>

    
    // <editor-fold defaultstate="collapsed" desc="EWR related">
    public function getAllEwrs(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->getAllEwrs($request, $args));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    public function getProjectEwrs(Request $request, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $cql = "MATCH (p:Project{uuid:{project}})-[:EWR]-(e:Ewr) return e";   
        $query = $this->dbentity->createQuery($cql);
        $query->addEntityMapping("e", Ewr::class);
        $query->setParameter('project', $args["id"]);
        $res =  $query->execute();
        return $res;
    }
    public function addEwr(Request $request, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        $json = json_decode($request->getBody());
        
        if (json_last_error() !== JSON_ERROR_NONE) {throw ApiException::json_decodeError();}
        if (NULL === $json) {throw ApiException::serverError('No data povided');}
        $firmid = NULL;
        if(array_key_exists('firmid', $args)) {$firmid = $args['firmid'];}
        if (NULL === $firmid) {throw ApiException::serverError('No Company provided');}        

        $ewr = new Ewr();
        $this->mapper->map($json, $ewr);
        
        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
        $stack = $client->stack();
        
        $query = "match(p:Project{uuid:{project}})                 
                create (e:Ewr{". $ewr->getQueryFields()."})
                create (e)-[:EWR]->(p) ";

        $stack->push($query, array_merge($ewr->getQueryFieldsParams(), ["project" => $ewr->getProject()->getUuid()]));

        if(null !== $ewr->getSitemanager()){
            $query = "match(w:Ewr{uuid:{ewr}})                 
                    match(e:Employee{uuid:{employee}})
                    create (e)-[:SITEMANAGER]->(w) ";
            $stack->push($query, ["ewr" => $ewr->getUuid(), "employee" => $ewr->getSitemanager()->getUuid()]);            
        }

        if(null !== $ewr->getExsitemanager()){
            $query = "match(w:Ewr{uuid:{ewr}})                 
                    match(c:Contact{uuid:{contact}})
                    create (c)-[:EXSITEMANAGER]->(w) ";
            $stack->push($query, ["ewr" => $ewr->getUuid(), "contact" => $ewr->getExsitemanager()->getUuid()]);            
        }
        
        $client->runStack($stack);
        
        $client->transaction()->commit();

        return $ewr; //$this->getEmployeeByUuid($employee->getUuid());
    }  
    public function updateEwr(Request $request, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $ewr = new Ewr();
        
        $json = json_decode($request->getBody());
        $this->mapper->map($json, $ewr);
        
        $existing = $this->ewrs->findOneBy(["uuid" => $ewr->getUuid()]);
        if (NULL == $existing) {
            throw ApiException::serverError('Ewr not found');
        }
        $existing->import($ewr);
        $existing->setmfdate('');

        $jsonm = $json->{'exsitemanager'};
        
        if(NULL !==$jsonm){
            $manager = new Contact();
            $this->mapper->map($manager, $jsonm);   
        
            $manager->setUuid($jsonm->{"uuid"});
            $this->addExtSiteManager($existing, $manager);            
        }
        
        $this->dbentity->persist($existing);
        $this->dbentity->flush();
        return $existing;
    }
    public function addEwrEmployee(Request $request, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        /** @var App\Models\ErwEmployee $employee */
        $employee = $this->dbentity->getRepository(\App\Models\ErwEmployee::class)->findOneBy(["uuid" => $args["employee"]]);

        /** @var App\Models\Ewr $ewr */
        $ewr = $this->ewrs->findOneBy(["uuid" => $args["ewr"]]);
        
        $ewr->addWorker($employee, $args["workplace"]);
        $this->dbentity->flush();
 
        return $ewr;
    }  
    public function deleteEwrEmployee(Request $request,  $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        /** @var App\Models\ErwEmployee $employee */
        $employee = $this->dbentity->getRepository(\App\Models\ErwEmployee::class)->findOneBy(["uuid" => $args["employee"]]);

        /** @var App\Models\Ewr $ewr */
        $ewr = $this->ewrs->findOneBy(["uuid" => $args["ewr"]]);
        
        $workerToRemove = null;

        foreach ($ewr->getWorkers() as $worker) {
            if ($worker->getEmployee() === $employee) {
                $workerToRemove = $worker;
            }
        }

        if (null === $workerToRemove) {
            return $ewr;
        }

        $ewr->getWorkers()->removeElement($workerToRemove);
        //$employee->getEwrs()->removeElement($workerToRemove);
        $this->dbentity->remove($workerToRemove);
        $this->dbentity->flush();
//        
//        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
//        $query = 'match(ewr:Ewr{uuid:{ewr}})<-[r:WORKS]-(e:Employee{uuid:{employee}}) delete r';
//
//        $client->run($query,['ewr' => $args["ewr"],'employee' => $args["employee"]]);
//        $client->transaction()->commit(); 
        return $ewr;

    }  
    public function addEwrDocument(Request $request,  $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $json = json_decode($request->getBody());
        
        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
        $query = 'match(e:Ewr{uuid:{ewr}})
            match(d:Document{uuid:{document}})
            create (e)-[r:ASSOCIATED_WITH{active:1, deleted:0}]->(d)';

        $client->run($query,['document' => $json->{'document'}->{'uuid'},
            'ewr' => $json->{'parentuuid'}]);
        $client->transaction()->commit(); 
        
        return null;
    }  
    public function getEwrDocuments(Request $request,  $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}


        $q = "match (dt:DocumentType{deleted:0})
            with dt
            match (d:Document{deleted:0})-[rt:IS_OF_TYPE]->(dt)
            with d
            match (e:Ewr{uuid:{uuid}})-[r:ASSOCIATED_WITH{deleted:0}]->(d) return e, collect(d) as documents";
       

        $query = $this->dbentity->createQuery($q);
        $query->addEntityMapping("e", Ewr::class);
        $query->addEntityMapping("documents", Document::class, Query::HYDRATE_COLLECTION);
        $query->setParameter("uuid", $args["id"]);

        return $query->getResult()[0];
    }     

    private function addExtSiteManager(Ewr $ewr, Contact $manager) {
        
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        if (NULL === $ewr) {
            throw ApiException::serverError('Ewr not foud');
        }
        if (NULL === $manager) {
            throw ApiException::serverError('Site manager not foud');
        }
        
        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();

        $stack = $client->stack();
        
        $cql = 'match(e:Ewr{uuid:{ewr}})-[r:EXSITEMANAGER]-(:Contact) delete r';
        $stack->push($cql, ["ewr" => $ewr->getUuid()]);
        
        $cql = "match(w:Ewr{uuid:{ewr}})                 
                    match(c:Contact{uuid:{contact}})
                    create (c)-[:EXSITEMANAGER]->(w) ";
        $stack->push($cql, ["ewr" => $ewr->getUuid(), "contact" => $manager->getUuid()]); 
  
        $client->runStack($stack);
        $client->transaction()->commit();
    }     
    
    // </editor-fold>

    // <editor-fold defaultstate="collapsed" desc="WORK PERIOD RELATED">

    public function addWp(Request $request, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        $json = json_decode($request->getBody());
        
        if (json_last_error() !== JSON_ERROR_NONE) {throw ApiException::json_decodeError();}
        if (NULL === $json) {throw ApiException::serverError('No data povided');}
        $firmid = NULL;
        if(array_key_exists('firmid', $args)) {$firmid = $args['firmid'];}
        if (NULL === $firmid) {throw ApiException::serverError('No Company provided');}        

        $wp = new ProjectWorkPeriod();
        $this->mapper->map($json->{"childpayload"}, $wp);

        $project = $this->getProjectByUuid($json->{'parentuuid'});
        
        $project->addWorkPeriod($wp);
        $this->dbentity->flush();
        $wp->getWorkPlans();
        return  $wp;
    }  

    public function deleteWp(Request $request, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
        $query = "match(p:ProjectWorkPeriod{uuid:{wp}})-[r]-() delete r delete p";

        $client->run($query, ["wp" => $args["wp"]]);
        $client->transaction()->commit();

        return null; //$this->getEmployeeByUuid($employee->getUuid());
    }  
    
    public function addWpPlan(Request $request, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        $json = json_decode($request->getBody());
        
        if (json_last_error() !== JSON_ERROR_NONE) {throw ApiException::json_decodeError();}
        if (NULL === $json) {throw ApiException::serverError('No data povided');}
        $firmid = NULL;
        if(array_key_exists('firmid', $args)) {$firmid = $args['firmid'];}
        if (NULL === $firmid) {throw ApiException::serverError('No Company provided');}
        
        $period = $this->getPeriod($json->{'parentuuid'});
        $workplace = $this->dbentity->getRepository(WorkPlacePlan::class)->findOneBy(["uuid" =>$json->{"childpayload"}->{"workplace"}->{"uuid"}]);
 
        if(null !== $workplace){
        
            $period->addPlan($workplace, $json->{"childpayload"}->{"plan"});
        
        }
        $this->dbentity->flush();
        return $period;
    }  
    
    // </editor-fold>

    /**
     * 
     * @param string $param
     * @return ProjectWorkPeriod
     */
    private function getPeriod($param) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        return $this->dbentity->getRepository(ProjectWorkPeriod::class)->findOneBy(["uuid" =>$param]);
    }
}

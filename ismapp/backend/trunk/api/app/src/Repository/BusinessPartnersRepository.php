<?php

namespace App\Repository;

use Psr\Log\LoggerInterface;
use GraphAware\Neo4j\Client\Client;
use GraphAware\Neo4j\Client\ClientBuilder;
use GraphAware\Neo4j\OGM\EntityManager;
use GraphAware\Neo4j\OGM\Query;
use App\Common\ApiException;
use Slim\Http\Request;
use Slim\Http\Response;


use App\Models\Document;
use App\Models\Address;
use App\Models\Contact;
use App\Models\CSite;
use App\Models\ModelType;
use App\Models\BusinessPartnerAddresses;
use App\Models\BusinessPartnerContacts;
use App\Models\BusinessPartnerDocuments;
use App\Models\BusinessPartner;
use App\Models\Company;

class BusinessPartnersRepository {
    
    protected $logger;
    protected $dbclient;
    protected $dbentity;
    protected $mapper;
    protected $settings;    

    private $partners;
    private $contacts;
    private $addresses;
    private $documents;
    private $csites;
    private $companies;

    private $connection;
    private $connalias;
    
    public function __construct(LoggerInterface $logger, \Slim\Collection $settings){
        $this->logger = $logger;
        $this->mapper = new \JsonMapper();
        //$this->mapper->setLogger($this->logger);
        $this->settings = $settings;
        
        $this->connection = $this->settings['dbclient']['type'].'://'.$this->settings['dbclient']['username'].':'.$this->settings['dbclient']['password'].'@'. $this->settings['dbclient']['host'].':'.$this->settings['dbclient']['port'];
        $this->connalias = $this->settings['dbclient']['name'];
        
        $this->dbclient = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
        $this->dbentity  = EntityManager::create($this->connection);

        
        $this->employees = $this->dbentity->getRepository(BusinessPartner::class);
        $this->contacts = $this->dbentity->getRepository(Contact::class);
        $this->addresses = $this->dbentity->getRepository(Address::class);
        $this->documents = $this->dbentity->getRepository(Document::class);
        $this->csites = $this->dbentity->getRepository(CSite::class);
        $this->companies = $this->dbentity->getRepository(Company::class);
        $this->partners = $this->dbentity->getRepository(BusinessPartner::class);
        
    }
    
    public function getAll($companyId)  {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        /*
        $company = $this->companies->findOneBy(['uuid' => $companyId]);
        if (NULL === $company) {
            return;
        }*/
        
        $cql = 'match(p:BusinessPartner) return p';
        
        $query = $this->dbentity->createQuery($cql);
        $query->addEntityMapping('p', BusinessPartner::class);

        return $query->execute();
    }
    
    public function addPartner($jsonData){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $this->mapper->bStrictNullTypes = FALSE;
        $partner = $this->mapper->map($jsonData, new BusinessPartner());
        $existing = NULL; //$this->getEmployeeByEmso($partner->getemso());
        if (NULL != $existing) {
            throw ApiException::serverError('BusinessPartner allredy exists');
        }
        /*
        foreach ($jsonData->{'addresses'} as $addressJsonData) {
            $address = $this->mapper->map($addressJsonData, new Address());
            $partner = $this->addAddress($partner, $address);
        }
        foreach ($jsonData->{'contacts'} as $contactJsonData) {
            $contact = $this->mapper->map($contactJsonData, new Contact());
            $partner = $this->addContact($partner, $contact);
        }
        */

        $this->dbentity->persist($partner);
        $this->dbentity->flush();
        return $partner;
    }
    public function updatePartner($jsonData){
        
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $this->mapper->bStrictNullTypes = FALSE;
        $partner = $this->mapper->map($jsonData, new BusinessPartner());

        $partner->setPartnerId($jsonData->{'uuid'});
        
        $existing = $this->getPartnerByUuid($partner->getUuid());
        if (NULL == $existing) {
            throw ApiException::serverError('BusinessPartner not found');
        }

        $existing->import($partner);
        $existing->setmfdate('');
        $this->dbentity->persist($existing);
        $this->dbentity->flush();
        return $existing;
    }    
    
    public function addPartnerDocument($jsonData){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $partner = $this->getPartnerByUuid($jsonData->{'parentuuid'});
        if (NULL === $partner) {
            throw ApiException::serverError('BusinessPartner not found');
        }

        return $partner;
        
        $doc = $this->documents->findOneBy(['uuid' => $jsonData->{'document'}->{'uuid'}]);
        $doc->gettype();
        $this->addDocument($partner, $doc);
        $this->dbentity->flush();

        return $this->getPartnerByUuid($jsonData->{'parentuuid'});
    }
    public function addPartnerAddress($jsonData){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $partner = $this->getPartnerByUuid($jsonData->{'parentuuid'});
        
        if (NULL === $partner) {
            throw ApiException::serverError('BusinessPartner not found');
        }        
        $addressJsonData = $jsonData->{'address'};
        
        $this->mapper->bStrictNullTypes = FALSE;
        $address = $this->mapper->map($addressJsonData, new Address());

        $this->addAddress($partner, $address);
        $this->dbentity->flush();   
        
        return $this->getPartnerByUuid($jsonData->{'parentuuid'});
    }
    public function addPartnerContact($jsonData){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $partner = $this->getPartnerByUuid($jsonData->{'parentuuid'});
        if (NULL === $partner) {
            throw ApiException::serverError('BusinessPartner not found');
        }
        $contactJsonData = $jsonData->{'contact'};
        $this->mapper->bStrictNullTypes = FALSE;
        $contact = $this->mapper->map($contactJsonData, new Contact());
        $this->addContact($partner, $contact);
        $this->dbentity->flush();
        return $partner;
    }

    public function getPartnerByUuid($uuid) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        return $this->getPartner(['uuid' => $uuid]);
    }
    
    public function getAddresses($uuid)   {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        try {
            //echo $uuid;
            $rep = $this->dbentity->getRepository(BusinessPartnerAddresses::class);
            $emp = $rep->findOneBy(['uuid' => $uuid]);
            return $emp;
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    public function getContacts($uuid)   {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        try {
            $rep = $this->dbentity->getRepository(BusinessPartnerContacts::class);
            $emp = $rep->findOneBy(['uuid' => $uuid]);
            return $emp;
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }        
    }
    public function getDocuments($uuid)   {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        $rep = $this->dbentity->getRepository(BusinessPartnerDocuments::class);

        /*
        $cql = 'match(e:BusinessPartner)-[r:ASSOCIATED_WITH]->(d:Document)-[rd:ATTACHED]-(f:File) where e.uuid={id} return e, collect(d)[0..2] as docs';
        $query = $this->dbentity->createQuery($cql);
        $query->addEntityMapping('e', EmployeeDocuments::class);
        $query->addEntityMapping('docs', Document::class, \GraphAware\Neo4j\OGM\Query::HYDRATE_COLLECTION);
        $query->setParameter('id', $uuid);
        $result = $query->execute();
        return $result;
        */
        $emp = $rep->findOneBy(['uuid' => $uuid]);

        return $emp;
      
    }
    public function getDocumentsOfType(Request $request, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $q = "match (dt:DocumentType{deleted:0, name:{type}})
            with dt
            match (d:Document{deleted:0})-[rt:IS_OF_TYPE]->(dt)
            with d
            /*where not((d)-[:NEXT]->(:Document))*/
            match (partner:BusinessPartner{uuid:{uuid}})-[r:ASSOCIATED_WITH]->(d) return partner, collect(d) as documents";

        $query = $this->dbentity->createQuery($q);
        $query->addEntityMapping("partner", BusinessPartner::class);
        $query->addEntityMapping("documents", Document::class, Query::HYDRATE_COLLECTION);
        $query->setParameter("uuid", $args["id"]);
        $query->setParameter("type", $args["type"]);
        
        $e = $query->getResult()[0];       
        foreach ($e["documents"] as $doc) {
            $doc->getType();          
            foreach ($doc->getFiles() as $file) {
                $file->getLanguage();
            }
        }  
        return $e;
    }
    private function addDocument(BusinessPartner $partner, Document $document){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        if (NULL === $partner) {
            throw ApiException::serverError('BusinessPartner not foud');
        }
        if (NULL === $document) {
            throw ApiException::serverError('Document not foud');
        }    
        
        return $partner;
        ModelType::InstaceOfBaseModel($partner);
        ModelType::InstaceOfBaseModel($document);

        $partner->getdocumentlist()->current();
        $partner->getdocumentlist()->add($document);
        
        
        return $partner;
    }
    private function addAddress(BusinessPartner $partner, Address $address) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        if (NULL === $partner) {
            throw ApiException::serverError('BusinessPartner not foud');
        }
        if (NULL === $address) {
            throw ApiException::serverError('Address not foud');
        }
        ModelType::InstaceOfBaseModel($partner);
        ModelType::InstaceOfBaseModel($address);
        $partner->getaddresslist()->current();
        $existing = $this->addresses->findOneBy(['line1' => $address->getline1(), 'line2' => $address->getline2(), 'zip' => $address->getzip(), 'country' => $address->getcountry()]);
        
        if ($existing !== NULL){
            $partner->getaddresslist()->add($existing);
        }
        else
        {
            $partner->getaddresslist()->add($address);
        }
        return $partner;
    }  
    private function addContact(BusinessPartner $partner, Contact $contact) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        if (NULL === $partner) {
            throw ApiException::serverError('Partner not foud');
        }
        if (NULL === $contact) {
            throw ApiException::serverError('Contact not foud');
        }
        ModelType::InstaceOfBaseModel($partner);
        ModelType::InstaceOfBaseModel($contact);        
        $partner->getcontactlist()->current();
        
        $existing = $this->contacts->findOneBy(['name' => $contact->getname(), 'lastname' => $contact->getlastname(), 'phone' => $contact->getphone()]);
        if ($existing !== NULL){
            $partner->getcontactlist()->add($existing);
        }
        else{
            $partner->getcontactlist()->add($contact);
        }
        return $partner;
    }
    
    private function getPartner($criteria) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        return $this->partners->findOneBy($criteria);
    }    
    private function getParnters($criteria, $order = null){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        return $this->partners->findBy($criteria, $order);
    }
}
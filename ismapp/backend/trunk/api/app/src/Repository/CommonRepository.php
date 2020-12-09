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

use App\Models\Language;
use App\Models\Country;
use App\Models\Address;
use App\Models\WorkPlace;

class CommonRepository {
    protected $logger;
    protected $dbclient;
    protected $dbentity;
    protected $mapper;
    protected $settings;
    
    private $languages;
    private $countries;
    private $connection;
    private $connalias;
    private $addresses;

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
        
        $this->languages = $this->dbentity->getRepository(Language::class); 
        $this->countries = $this->dbentity->getRepository(Country::class); 
        $this->addresses = $this->dbentity->getRepository(Address::class); 
    }  
    
    public function getAllLanguages(){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        return $this->languages->findAll();
    }
    
    public function getAllCountries(){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        return $this->countries->findAll();
    }    
    
    public function getAllAddresses(){
//        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
//        return $this->addresses->findAll();
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $cql = "match(a:Address) return distinct a.line1, a.line2, a.zip, a.city, a.country, a.state";
        $result =  $this->dbclient->run($cql);

        foreach ($result->records() as $record) {
            $dd= new Address();
            $dd->setline1($record->get("a.line1"));
            $dd->setline2($record->get("a.line2"));
            $dd->setzip($record->get("a.zip"));
            $dd->setstate($record->get("a.state"));
            $dd->setcity($record->get("a.city"));
            $dd->setcountry($record->get("a.country"));
            $rec[] = $dd;
        }
        return $rec;        
    }       
    
    
    public function addLanguage($jsonData){
        
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $this->mapper->bStrictNullTypes = FALSE;
        $language = $this->mapper->map($jsonData, new Language());
        
       
        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
            
        $query = $query. 'create (l:Language{
                language:{language},
                code2:{code2},
                code3:{code3},
                uuid:{uuid}})';

        $client->run($query,[
            'language' => $language->getLanguage(), 
            'code2' => $language->getCode2(), 
            'code3' => $language->getCode3(), 
            'uuid' => $language->getUuid()]);
        
        $client->transaction()->commit();

        return $language;        
    }
    

    // <editor-fold defaultstate="collapsed" desc="WorkPlace related">

    public function addWorkplace(Request $request, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}        
        
        $wp = new WorkPlace();
        $this->mapper->map(json_decode($request->getBody()), $wp);
        
        $this->dbentity->persist($wp);
        $this->dbentity->flush();
        return $wp;
    }
    
    public function deleteWorkplace(Request $request, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
    }
    public function getAllWorkplaces(Request $request, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}        
        return $this->dbentity->getRepository(WorkPlace::class)->findAll();
    }
    public function getWorkplace(Request $request, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
    }
    
    // </editor-fold>

}

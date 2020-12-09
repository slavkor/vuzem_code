<?php

namespace App\Repository;

use Psr\Log\LoggerInterface;
use GraphAware\Neo4j\Client\Client;
use GraphAware\Neo4j\Client\ClientBuilder;
use GraphAware\Neo4j\OGM\EntityManager;


use App\Common\ApiException;
use App\Models\Audit;
use App\Repository\UsersRepository;

class AuditRepository {
    
    protected $logger;
    protected $dbclient;
    protected $dbentity;
    protected $mapper;
    protected $settings;


    private $connection;
    private $connalias;
    
    private $audits;

    public function __construct(LoggerInterface $logger, \Slim\Collection $settings){
        $this->logger = $logger;
        $this->mapper = new \JsonMapper();
        //$this->mapper->setLogger($this->logger);
        $this->settings = $settings;
        
        $this->connection = $this->settings['dbclient']['type'].'://'.$this->settings['dbclient']['username'].':'.$this->settings['dbclient']['password'].'@'. $this->settings['dbclient']['host'].':'.$this->settings['dbclient']['port'];
        $this->connalias = $this->settings['dbclient']['name'];
        
        $this->dbclient = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
        $this->dbentity  = EntityManager::create($this->connection);
        //$this->audits = $this->dbentity->getRepository(Audit::class);
    }
    
    public function AuditAction($type, $source, $uuid, string $request, string $response, $token_id) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        try {
            
            if(NULL === $token_id) return;
            if(empty($token_id)) return;


            $audit = new Audit();
            $audit->setType($type);
            $audit->setAudituuid($uuid);
            $audit->setSource($source);
            $audit->setRequestdata($request);
            $audit->setResponsedata($response);

            $userRep = $this->dbentity->getRepository(\App\Models\User::class);
            $user = $userRep->findOneBy(['username' => $token_id]);
            $audit->setUser($user);
            $this->dbentity->persist($audit);
            $this->dbentity->flush();
            return $audit;
        } catch (\Exception $exc) {
            //var_dump($exc);die;
            throw $exc;
        }
    }   
}

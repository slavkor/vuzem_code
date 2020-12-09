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


use App\Models\Employee;
use App\Models\User;
use App\Models\UserExtendet;

/**
 * Description of UsersRepository
 *
 * @author slavko
 */
class UsersRepository {
    protected $logger;
    protected $dbclient;
    protected $dbentity;
    protected $mapper;
    protected $settings;

    private $connection;
    private $connalias;
    
    private $users;

    public function __construct(LoggerInterface $logger, \Slim\Collection $settings){
        $this->logger = $logger;
        $this->mapper = new \JsonMapper();
        //$this->mapper->setLogger($this->logger);
        $this->settings = $settings;
        
        $this->connection = $this->settings['dbclient']['type'].'://'.$this->settings['dbclient']['username'].':'.$this->settings['dbclient']['password'].'@'. $this->settings['dbclient']['host'].':'.$this->settings['dbclient']['port'];
        $this->connalias = $this->settings['dbclient']['name'];
        
        $this->dbclient = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
        $this->dbentity  = EntityManager::create($this->connection);

        $this->users = $this->dbentity->getRepository(User::class);
    }    
    public function getAllUsers(){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $userRepository = $this->dbentity->getRepository(UserExtendet::class);
        $users = $userRepository->findAll();
        return $users;
    }
    
    public function createUser($jsonData, $username){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        if (NULL === $jsonData) {
            return;
        }
        $user = $this->mapper->map($jsonData, new User());

        $userRepository = $this->dbentity->getRepository(User::class);
        $existing = $userRepository->findOneBy(['username' => $user->getusername()]);
        if (NULL !== $existing) {
            return;
        }

        if(NULL != $user->getemployeeid()){
            $repository = $this->dbentity->getRepository(Employee::class);
            $employee = $repository->findOneById($user->getemployeeid());
            
            if (NULL != $employee) {
                $user->setemployee($employee);
            }
        }

        $this->dbentity->persist($user);
        $this->dbentity->flush();

        return $user;
    }
    
    public function getUserByTokenExtendet($token) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        if (NULL === $token) {
            return;
        }
        $cql = 'match(e:Employee)<-[er:ASSIGNED_TO]-(u:User)<-[r:ASSIGNED_TO]-(t:Token) where t.identifier={identifier} return u, e';

        $query = $this->dbentity->createQuery($cql);
        $query->addEntityMapping('u', User::class);
        $query->addEntityMapping('e', Employee::class);
        $query->setParameter('identifier', $token);

        $result = $query->getOneOrNullResult();
        if (NULL === $result) {
            return;
        }

        $user = $result[0]['u'];
        $user->setemployee($result[0]['e']);

        if ($user === null) {
            return;
        }
        return $user;    
      
    }
    
    public function getUserByToken($token) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        if (NULL === $token) {
            return;
        }
        
        $cql = 'match(u:User)<-[r:ASSIGNED_TO]-(t:Token) where t.identifier={identifier} return u';

        $query = $this->dbentity->createQuery($cql);
        $query->addEntityMapping('u', User::class);
        $query->setParameter('identifier', $token);

        $result = $query->getOneOrNullResult();
        if (NULL === $result) {
            return;
        }
        $this->dbentity->flush();
        return $result;
    }    
}

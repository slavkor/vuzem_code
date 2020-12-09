<?php

namespace App\Repository;

use League\OAuth2\Server\Repositories\UserRepositoryInterface;
use League\OAuth2\Server\Entities\ClientEntityInterface;


use Psr\Log\LoggerInterface;
use GraphAware\Neo4j\Client\Client;
use GraphAware\Neo4j\Client\ClientBuilder;
use GraphAware\Neo4j\OGM\EntityManager;

use App\Models\UserEntity;
use App\Models\Token;
use App\Models\Employee;
use App\Common\ApiException;
use App\Model\ScopeEntity;

//include __DIR__.'/../Models/UserEntity.php';


class UserRepository implements UserRepositoryInterface{

    private $logger;
    private $dbclient;
    private $dbentity;
    private $mapper;
    private $settings;

    private $connection;
    private $connalias;
    
    private $scopes;
    private $users;
    public function __construct(LoggerInterface $logger, \Slim\Collection $settings){
        $this->logger = $logger;
        $this->settings = $settings;
        
        $this->connection = $this->settings['dbclient']['type']. '://'.$this->settings['dbclient']['username'].':'.$this->settings['dbclient']['password'].'@'. $this->settings['dbclient']['host'].':'.$this->settings['dbclient']['port'];
        $this->connalias = $this->settings['dbclient']['name'];
        
        $this->dbclient = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
        $this->dbentity  = EntityManager::create($this->connection);
        $this->mapper = new \JsonMapper();
        $this->mapper->setLogger($this->logger);
        $this->mapper->bStrictNullTypes = FALSE;

        $this->scopes = $this->dbentity->getRepository(\App\Models\ScopeEntity::class);
        $this->users = $this->dbentity->getRepository(UserEntity::class);
        
    } //put your code here
    
    public function getUserEntityByUserCredentials($username, $password, $grantType, ClientEntityInterface $clientEntity) {
        try 
        {

            $userRepository = $this->dbentity->getRepository(UserEntity::class);

            $user = $userRepository->findOneBy(['username'=> $username]);


            if ($user === null) {
                return;
            }
            if (!password_verify($password, $user->getpassword())) {
                return;
            }
            return $user;
        } catch (Exception $exc) {
            return;
        }
    }
    
    public function getUserEntityByToken($identifier) {
        try {
            $cql = 'match(token:Token{identifier:{identifier}})-[r:ASSIGNED_TO]->(user:User) return user';
            
            $query = $this->dbentity->createQuery($cql);
            $query->addEntityMapping('user', UserEntity::class);
            $query->setParameter('identifier', $identifier);
 
            return $query->getResult()[0]; 

        } catch (\Exception $exc) {
            throw ApiException::serverError($exc->getMessage());
        }
    }
    
    public function getAllUsers(){
        $this->logger->info(__CLASS__.':'.__FUNCTION__);
        $userRepository = $this->dbentity->getRepository(UserEntity::class);
        $users = $userRepository->findAll();
        return $users;
    }
    
    public function createUser($jsonData){
        if (NULL === $jsonData) {
            return;
        }
        $user = new UserEntity();
        $this->mapper->map($jsonData, $user);
        $existing = $this->users->findOneBy(['username' => $user->getusername()]);
        if (NULL !== $existing) {
            return;
        }
        $scope = $this->scopes->findOneBy(["identifier" => "user"]);
        if(NULL === $scope){
            $userScope = new \App\Models\ScopeEntity();
            $userScope->setIdentifier("user");
            $this->dbentity->persist($userScope);
            $this->dbentity->flush();
        }
        $scope = $this->scopes->findOneBy(["identifier" => "user"]);
        $user->getScopes()->add($scope);
        $this->dbentity->persist($user);
        $this->dbentity->flush();

        return $user;
    }
    public function updateUser($jsonData){
        if (NULL === $jsonData) {
            return;
        }
        $user = new UserEntity();
        $this->mapper->map($jsonData, $user);

        $existing = $this->users->findOneBy(['uuid' => $user->getuuid()]);
        if(NULL === $existing){
            return;
        }
        
        $existing->setUsername($user->getUsername());
        $existing->setPassword($user->getPassword());
        $this->dbentity->persist($existing);
        $this->dbentity->flush();

        return $existing;
    }
    
    public function addScope($jsonData, $userId){
        if (NULL === $jsonData) {
            return;
        }

        $user = $this->users->findOneBy(['uuid' => $userId]);
        if(NULL === $user){
            return;
        }

        $scope = $this->scopes->findOneBy(['uuid' => $jsonData->{"uuid"}]);
        if(NULL === $scope){
            return;
        }
        
        $user->getScopes()->add($scope);
        $this->dbentity->persist($user);
        $this->dbentity->flush();

        return $user;
    }  
    
    public function removeScope($jsonData, $userId){
       if (NULL === $jsonData) {
            return;
        }

        $user = $this->users->findOneBy(['uuid' => $userId]);
        if(NULL === $user){
            return;
        }

        $scope = $this->scopes->findOneBy(['uuid' => $jsonData->{"uuid"}]);
        if(NULL === $scope){
            return;
        }
        
        $user->getScopes()->removeElement($scope);
        $this->dbentity->persist($user);
        $this->dbentity->flush();

        return $user;
    }      
}


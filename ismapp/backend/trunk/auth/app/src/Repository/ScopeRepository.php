<?php


namespace App\Repository;

use League\OAuth2\Server\Entities\ClientEntityInterface;
use League\OAuth2\Server\Repositories\ScopeRepositoryInterface;

use Psr\Log\LoggerInterface;
use GraphAware\Neo4j\Client\Client;
use GraphAware\Neo4j\Client\ClientBuilder;
use GraphAware\Neo4j\OGM\EntityManager;

use App\Models\ScopeEntity;
use App\Models\Scope;


class ScopeRepository implements ScopeRepositoryInterface
{
    private $logger;
    private $dbclient;
    private $dbentity;
    private $mapper;
    private $settings;

    private $connection;
    private $connalias;
    
    private $scopes;
    
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
        
        $this->scopes = $this->dbentity->getRepository(ScopeEntity::class);
    } 
    
    
    /**
     * {@inheritdoc}
     */
    public function getScopeEntityByIdentifier($scopeIdentifier)
    {
        return $this->scopes->findOneBy(['identifier' => $scopeIdentifier]);
    }

    /**
     * {@inheritdoc}
     */
    public function finalizeScopes(array $scopes, $grantType, ClientEntityInterface $clientEntity, $userIdentifier = null) {
        
        $query = 'MATCH(u:User {username:{username}})-[r:IS_IN]->(s:Scope) RETURN s';
        
        $result = $this->dbclient->run($query, ['username' => $userIdentifier]);
        
        foreach ($result->records() as $sc) {
            $scope = new Scope();
            $scope->setIdentifier($sc->get('s')->value('identifier'));
            $scopes[] = $scope;
        }
        
        return $scopes;
    }
    
    public function AddScope($jsonData){
        $this->logger->info(__CLASS__.':'.__FUNCTION__);
        
        if (NULL === $jsonData) {
            return;
        }

        $scope = $this->mapper->map($jsonData, new ScopeEntity());

        if (NULL === $scope) {
            return;
        }

        $existing = $this->getScopeEntityByIdentifier($scope->getIdentifier());
        
        if (NULL != $existing) {
            return;
        }

        $this->dbentity->persist($scope);
        $this->dbentity->flush();
        return $scope;        
    }
    
    public function GetScopeByToken($identifier) {
        try {
            //$cql = 'match(e:Employee)<-[er:ASSIGNED_TO]-(u:User)<-[r:ASSIGNED_TO]-(t:Token) where t.identifier={identifier} return u, e';
            $cql = 'match(s:Scope) <-[c:IS_IN]-(u:User)<-[r:ASSIGNED_TO]-(t:Token) where t.identifier={identifier} return s';
            
            $query = $this->dbentity->createQuery($cql);
            $query->addEntityMapping('s', ScopeEntity::class);
            $query->setParameter('identifier', $identifier);
            
            $result = $query->getResult();
            if (NULL === $result || empty($result)) {
                return;
            }
            return $result;    
        } catch (\Exception $exc) {
            throw ApiException::serverError($exc->getMessage());
        }        
    }
    
    public function GetAllScopes() {
        try {
            return $this->scopes->findAll();
        } catch (\Exception $exc) {
            throw ApiException::serverError($exc->getMessage());
        }        
    }    
}

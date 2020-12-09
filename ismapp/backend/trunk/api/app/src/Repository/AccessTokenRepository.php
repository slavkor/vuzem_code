<?php
namespace App\Repository;
/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
use Psr\Log\LoggerInterface;
use League\OAuth2\Server\Entities\AccessTokenEntityInterface;
use League\OAuth2\Server\Entities\ClientEntityInterface;
use League\OAuth2\Server\Repositories\AccessTokenRepositoryInterface;
use GraphAware\Neo4j\Client\Client;
use GraphAware\Neo4j\Client\ClientBuilder;
use GraphAware\Neo4j\OGM\EntityManager;
use League\OAuth2\Server\Exception\OAuthServerException;

/**
 * Description of AccessTokenRepository
 *
 * @author slavko
 */
class AccessTokenRepository implements AccessTokenRepositoryInterface {
    protected $logger;
    protected $dbclient;
    protected $dbentity;
    protected $mapper;
    protected $settings;    

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

    }
    /**
     * {@inheritdoc}
     */
    public function persistNewAccessToken(AccessTokenEntityInterface $accessTokenEntity)
    {
        throw  new \BadMethodCallException();        // Some logic here to save the access token to a database
    }
    
    
    /**
     * {@inheritdoc}
     */
    public function revokeAccessToken($tokenId)
    {
        throw  new \BadMethodCallException();
    }

    /**
     * {@inheritdoc}
     */
    public function isAccessTokenRevoked($tokenId)
    {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $query = 'MATCH (t:Token) WHERE t.identifier = {identifier} RETURN t.expiryDateTime as expires';
        $result = $this->dbclient->run($query, ['identifier' => $tokenId]);
        $records = $result->records();

        if (sizeof($records) === 0) {
            return true;
        }
        $e = strtotime($result->records()[0]->get("expires"));
        return $e<=time();
    }

    /**
     * {@inheritdoc}
     */
    public function getNewToken(ClientEntityInterface $clientEntity, array $scopes, $userIdentifier = null)
    {
       throw  new \BadMethodCallException();
    }    

}

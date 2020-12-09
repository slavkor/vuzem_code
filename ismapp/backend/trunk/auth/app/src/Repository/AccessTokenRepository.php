<?php
namespace App\Repository;

use League\OAuth2\Server\Entities\AccessTokenEntityInterface;
use League\OAuth2\Server\Entities\ClientEntityInterface;
use League\OAuth2\Server\Repositories\AccessTokenRepositoryInterface;

use Psr\Log\LoggerInterface;
use GraphAware\Neo4j\Client\Client;
use GraphAware\Neo4j\Client\ClientBuilder;
use GraphAware\Neo4j\OGM\EntityManager;

use Ramsey\Uuid\Uuid;
use Ramsey\Uuid\Exception\UnsatisfiedDependencyException;

use App\Models\AccessTokenEntity;

include __DIR__.'/../Models/AccessTokenEntity.php';

class AccessTokenRepository implements AccessTokenRepositoryInterface
{
    private $logger;
    private $dbclient;
    private $dbentity;
    private $mapper;
    private $settings;

    private $connection;
    private $connalias;
    

    
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
    } //put your code here
    
    
    /**
     * {@inheritdoc}
     */
    public function persistNewAccessToken(AccessTokenEntityInterface $accessTokenEntity)
    {
        $timeZone = new \DateTimeZone('Europe/Ljubljana');
          

        $userName = $accessTokenEntity->getUserIdentifier();
        $identifier = $accessTokenEntity->getIdentifier();
        $expiryDateTime = $accessTokenEntity->getExpiryDateTime();
        $expiryDateTime->setTimezone($timeZone);
        $userIdentifier = $accessTokenEntity->getUserIdentifier();
        $clientName = $accessTokenEntity->getClient()->getName();
        $tokenType = "accessToken";
        
        $t = microtime(true);
        $micro = sprintf("%06d",($t - floor($t)) * 1000000);
        $createDate = new \DateTime( date('Y-m-d H:i:s.'.$micro, $t) );
        $createDate->setTimezone($timeZone);    

        $query = 'MATCH(u:User {username:{username}})
                    CREATE(t:Token {
                                    identifier:{identifier},
                                    expiryDateTime:{expiryDateTime},
                                    userIdentifier:{userIdentifier},
                                    client:{client},
                                    createDate:{createDate}, 
                                    uuid:{uuid}})
                    CREATE(t)-[a:ASSIGNED_TO{tokenType:{tokenType}}]->(u)';

        //var_dump($query);
        $this->dbclient->run($query,
                [
                    'username' => $userName, 
                    'identifier' => $identifier,
                    'expiryDateTime' => $expiryDateTime->format('YmdHis'),
                    'userIdentifier' => $userIdentifier,
                    'client' => $clientName,
                    'createDate' => $createDate->format('YmdHis.u'),
                    'tokenType' => $tokenType, 
                    'uuid' => Uuid::uuid4()->serialize()
                ]);
    }
    

    /**
     * {@inheritdoc}
     */
    public function revokeAccessToken($tokenId)
    {
        // Some logic here to revoke the access token
    }

    /**
     * {@inheritdoc}
     */
    public function isAccessTokenRevoked($tokenId)
    {
        return false; // Access token hasn't been revoked
    }

    /**
     * {@inheritdoc}
     */
    public function getNewToken(ClientEntityInterface $clientEntity, array $scopes, $userIdentifier = null)
    {
        $accessToken = new AccessTokenEntity();
        $accessToken->setClient($clientEntity);
        foreach ($scopes as $scope) {
            $accessToken->addScope($scope);
        }
        $accessToken->setUserIdentifier($userIdentifier);

        return $accessToken;
    }
}

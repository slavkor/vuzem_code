<?php
namespace App\Repository;

use League\OAuth2\Server\Repositories\ClientRepositoryInterface;

use Psr\Log\LoggerInterface;
use GraphAware\Neo4j\Client\Client;
use GraphAware\Neo4j\Client\ClientBuilder;
use GraphAware\Neo4j\OGM\EntityManager;

use App\Models\ClientEntity;
include __DIR__.'/../Models/ClientEntity.php';

class ClientRepository implements ClientRepositoryInterface
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
    public function getClientEntity($clientIdentifier, $grantType, $clientSecret = null, $mustValidateSecret = true)
    {
        /*
        $clients = [
            'myawesomeapp' => [
                'secret'          => password_hash('abc123', PASSWORD_BCRYPT),
                'name'            => 'My Awesome App',
                'redirect_uri'    => 'http://foo/bar',
                'is_confidential' => true,
            ],
        ];
        
        // Check if client is registered
        if (array_key_exists($clientIdentifier, $clients) === false) {
            return;
        }

        if (
            $mustValidateSecret === true
            && $clients[$clientIdentifier]['is_confidential'] === true
            && password_verify($clientSecret, $clients[$clientIdentifier]['secret']) === false
        ) {
            return;
        }
        */

        $client = new ClientEntity();
      
        $client->setIdentifier($clientIdentifier);
        $client->setName($clientIdentifier);
        //$client->setRedirectUri($clients[$clientIdentifier]['redirect_uri']);
        
        return $client;
    }



}

<?php
namespace App\Models;
use League\OAuth2\Server\Entities\UserEntityInterface;
use GraphAware\Neo4j\OGM\Annotations as OGM;
use GraphAware\Neo4j\OGM\Common\Collection;

/**
 *
 * @OGM\Node(label="User")
 */
class UserEntity extends BaseModel implements UserEntityInterface,\JsonSerializable {
    
    public function __construct() {
        parent::__construct();
        $this->scopes = new Collection();    

    }
    
    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $username;
    
    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $password;
    
    /**
     * @var ScopeEntity[]|Collection
     * 
     * @OGM\Relationship(type="IS_IN", direction="OUTGOING", collection=true, mappedBy="users", targetEntity="ScopeEntity")
     */
    protected $scopes;
    
    function getUsername() {
        return $this->username;
    }

    function getPassword() {
        return $this->password;
    }

    function getScopes() {
        return $this->scopes;
    }

    function setUsername($username) {
        $this->username = $username;
    }

    function setPassword($password) {
        $this->password = $password;
    }
        
    public function getIdentifier() {
        return $this->username;
    }
    

    public function jsonSerialize() {

        $ret = array_merge(parent::jsonSerialize(),
                [
                    'username' => $this->username,
                    'scopes' => $this->scopes->toArray()
                ]);
        
        return  $ret;
    }
    
    //<editor-fold desc="BaseModel implementation">    
    public function getid() {
        return $this->id;
    }
    public function getuuid() {
        return $this->uuid;
    }
    public function setuuid($uuid) {
        $this->uuid = $uuid;
    }
    //</editor-fold>   

}

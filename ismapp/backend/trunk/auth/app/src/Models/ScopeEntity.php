<?php

namespace App\Models;


use League\OAuth2\Server\Entities\ScopeEntityInterface;
use League\OAuth2\Server\Entities\Traits\EntityTrait;
use GraphAware\Neo4j\OGM\Annotations as OGM;
use GraphAware\Neo4j\OGM\Common\Collection;


/**
 *
 * @OGM\Node(label="Scope")
 */
class ScopeEntity extends BaseModel implements ScopeEntityInterface 
{
    public function __construct() {
        parent::__construct();
        $this->users = new Collection();
    }
    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $identifier;   
    
    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $description;    

    /**
     * @var UserEntity[]|Collection
     * 
     * @OGM\Relationship(type="IS_IN", direction="INCOMING", collection=true, mappedBy="scopes", targetEntity="UserEntity")
     */
    protected $users;
    
    //<editor-fold desc="setter and getter methods">   
    
    function getDescription(): string {
        return $this->description;
    }

    function setDescription($description) {
        $this->description = $description;
    }

    public function getIdentifier(): string {
        return $this->identifier;
    }
    public function setIdentifier($identifier){
        $this->identifier = $identifier;
    }    
    //</editor-fold> 
    
    
    //<editor-fold desc="JsonSerializable implementation">    
    public function jsonSerialize() {
        return array_merge(parent::jsonSerialize(), ["identifier" => $this->identifier, "description" => $this->description]);
    }
    //</editor-fold>    

    //<editor-fold desc="BaseModel implementation">    
    public function getid() {
        return $this->id;
    }
    public function getuuid() {
        return $this->uuid;
    }
    protected function setuuid($uuid) {
        $this->uuid = $uuid;
    }
    //</editor-fold>   

}

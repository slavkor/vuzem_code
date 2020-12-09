<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;
use League\OAuth2\Server\Entities\UserEntityInterface;
use GraphAware\Neo4j\OGM\Annotations as OGM;
use GraphAware\Neo4j\OGM\Common\Collection;

/**
 *
 * @OGM\Node(label="Token")
 */
class Token extends BaseModel implements \JsonSerializable {
    public function __construct() {
        parent::__construct();
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
    protected $userIdentifier;
    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $client;
    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $expiryDateTime;
    
    /**
     * @var UserEntity
     * 
     * @OGM\Relationship(type="ASSIGNED_TO", direction="OUTGOING", collection=false, mappedBy="tokens", targetEntity="UserEntity")
     */    
    protected $user;
    
    public function getidentifier() {
        return $this->identifier;
    }
    public function getuserIdentifier() {
        return $this->userIdentifier;
    }
    public function getclient() {
        return $this->client;
    }
    public function getexpiryDateTime() {
        return $this->expiryDateTime;
    }
    public function getuser() {
        return $this->user;
    }
    
    
    public function setidentifier($identifier) {
        $this->identifier = $identifier;
    }
    public function setuserIdentifier($userIdentifier) {
        $this->userIdentifier = $userIdentifier;
    }
    public function setclient($client) {
        $this->client = $client;
    }
    public function setexpiryDateTime($expiryDateTime) {
        $this->expiryDateTime = $expiryDateTime;
    }
    public function setuser($user) {
        $this->user = $user;
    }
    
    //<editor-fold desc="JsonSerializable">
    public function jsonSerialize() {
        return array_merge(parent::jsonSerialize(), [
            'identifier' => $this->identifier,
            'userIdentifier' => $this->userIdentifier,
            'client' => $this->client,
            'expiryDateTime' => $this->expiryDateTime,
        ]); 
    }
    //</editor-fold>        

   //<editor-fold desc="BaseModel implementation">    
    public function getid() {
        return $this->id;
    }
    public function getuuid() {
        return $this->uuid;
    }
    /**
     * 
     * @param string
     */
    protected function setuuid($uuid) {
        $this->uuid = $uuid;
    }
    //</editor-fold>       
}
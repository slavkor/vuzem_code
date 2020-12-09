<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;

use GraphAware\Neo4j\OGM\Annotations as OGM;

/**
 *
 * @OGM\Node(label="Audit")
 */
class Audit extends BaseModel implements \JsonSerializable {

    //<editor-fold desc="properties">   
    
    /**
     * @OGM\Property(type="string")
     *
     * @var string
     */
    protected $type;

    /**
     * @OGM\Property(type="string")
     *
     * @var string|null
     */
    protected  $audituuid;

    /**
     * @OGM\Property(type="string")
     *
     * @var string
     */
    protected $source;

    /**
     * @OGM\Property(type="string")
     *
     * @var string
     */
    protected $requestdata;

    /**
     * @OGM\Property(type="string")
     *
     * @var string
     */
    protected $responsedata;

    /**
     * @var User
     * 
     * @OGM\Relationship(type="AUDIT", direction="INCOMING", collection=false, mappedBy="", targetEntity="User")
     */    
    protected  $user;
    //</editor-fold>  
 
    //<editor-fold desc="getter/setter methods">   
    public function getType() {
        return $this->type;
    }

    public function getAudituuid() {
        return $this->audituuid;
    }

    public function getSource() {
        return $this->source;
    }

    public function getRequestdata() {
        return $this->requestdata;
    }

    public function getResponsedata() {
        return $this->responsedata;
    }

    public function getUser(): User {
        return $this->user;
    }

    public function setType($type) {

        $this->type = $type;
    }

    public function setAudituuid($audituuid) {
        $this->audituuid = $audituuid;
    }

    public function setSource($source) {
        $this->source = $source;
    }

    public function setRequestdata($requestdata) {
        $this->requestdata = $requestdata;
    }

    public function setResponsedata($responsedata) {
        $this->responsedata = $responsedata;
    }

    public function setUser($user) {
        $this->user = $user;
    }
    //</editor-fold>  
 
    //<editor-fold desc="JsonSerializable implementation">   
    public function jsonSerialize() {
        
        $ret = array_merge(
            parent::jsonSerialize(),
            [
                'type' => $this->type,
                'audituuid' => $this->audituuid
            ]
        ); 
        return $ret;
    }
    //</editor-fold>    
    
    //<editor-fold desc="BaseModel implementation">    
    public function getid() {
        return $this->id;
    }
    public function getUuid() {
        return $this->uuid;
    }
    public function setUuid($uuid) {
        $this->uuid = $uuid;
    }
    public function import($object) {
        $vars=is_object($object)?get_object_vars($object):$object;

        if (!is_array($vars)) {
            throw \Exception('no props to import into the object!');
        }
        /*
        foreach ($vars as $key => $value) {
            switch ($key) {
                case 'lastname':
                    $this->$key = $value;
                    break;

                default:
                    break;
            }
        }
         * 
         */  
    }

    public function tag(): string {
        $reflect = new \ReflectionClass($this);
        return $reflect->getShortName()."_".$this->tagId();
    }

    public function tagId(): string {
        return $this->tagId;
    }

    //</editor-fold>    
}

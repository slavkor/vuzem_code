<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;

use GraphAware\Neo4j\OGM\Annotations as OGM;
use GraphAware\Neo4j\OGM\Common\Collection;

/**
 *
 * @OGM\Node(label="User")
 */
class BaseUser extends BaseModel implements \JsonSerializable {
    //<editor-fold desc="protected fields">
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
    //</editor-fold>

    //<editor-fold desc="Getter funcitons">
    public function getusername(){
        return $this->username;
    }
    
    public function getpassword(){
        return $this->password;
    }
    
    public function getIdentifier() {
        return $this->username;
    }
    //</editor-fold>
    
    //<editor-fold desc="Setter funcitons">
    public function setusername($username) {
        $this->username = $username;
    }
    
    public function setpassword($password) {
        $this->password = $password;
    }
    //</editor-fold>
    
    //<editor-fold desc="JsonSerializable implementation">
    public function jsonSerialize() {
        return array_merge(
                parent::jsonSerialize(),
                [
                    'id' => $this->id,
                    'username' => $this->username
                ]);
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
        
        foreach ($vars as $key => $value) {
            switch ($key) {
                case 'username':
                case 'password':
                    $this->$key = $value;
                    break;

                default:
                    break;
            }
        }

    }    
    public function tag(): string {
        $reflect = new \ReflectionClass($this);
        return $reflect->getShortName()."_".$this->tagId();
    }

    public function tagId(): string {
        return $this->tagId;
    }    //</editor-fold>
    
}

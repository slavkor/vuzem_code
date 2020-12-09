<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;

use GraphAware\Neo4j\OGM\Annotations as OGM;

class BasePerson extends BaseModel implements \JsonSerializable {
    public function __construct() {
        parent::__construct();
        $this->tagId = uniqid();
    }
    //<editor-fold desc="protected fields">    
    /**
     * name
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    public $name;
    /**
     * lastname
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    public $lastname;   
    //</editor-fold>    

    //<editor-fold desc="getters">    
    public function getname(){
        return $this->name;
    }
    public function getlastname(){
        return $this->lastname;
    }
    //</editor-fold>    

    //<editor-fold desc="setters">    
    public function setname($name){
        $this->name = $name;
    }
    public function setlastname($lastname) {
        $this->lastname = $lastname;
    }
    //</editor-fold>    

    //<editor-fold desc="JsonSerializable implementation">    
    public function jsonSerialize() {
        $ret = parent::jsonSerialize();
        $ret['name'] = $this->name;
        $ret['lastname'] = $this->lastname;
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
        foreach ($vars as $key => $value) {
            switch ($key) {
                case 'name':
                case 'lastname':
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
        return $this->tagId ;
    }    //</editor-fold>   


}

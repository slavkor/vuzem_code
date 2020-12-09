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
 * @OGM\Node(label="BusinessPartner")
 */
class BaseBusinessPartner extends BasePerson implements \JsonSerializable{
    
    /**
     * shortname
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    public $shortname;
    
    function getShortname() {
        return $this->shortname;
    }

    function setShortname($shortname) {
        $this->shortname = $shortname;
    }

        
    //<editor-fold desc="JsonSerializable">
    public function jsonSerialize() {
        return array_merge(
                parent::jsonSerialize(),
                [
                    'shortname' => $this->shortname
                ]);
    }
    //</editor-fold>

    public function setPartnerId($id) {
        $this->setUuid($id);
    }
    
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
    
    public function setEmployeeId($id){
        $this->setUuid($id);
    }
    
    public function import($object) {
        
        parent::import($object);
        
        $vars=is_object($object)?get_object_vars($object):$object;

        if (!is_array($vars)) {
            throw \Exception('no props to import into the object!');
        }
        foreach ($vars as $key => $value) {
            switch ($key) {
                case 'name':
                case 'shortname':
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
        return $this->tagId;
    }
    //</editor-fold>
}

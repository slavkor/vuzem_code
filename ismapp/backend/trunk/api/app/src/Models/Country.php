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
 * @OGM\Node(label="Country")
 */
class Country extends BaseModel implements \JsonSerializable {
 
    // <editor-fold defaultstate="collapsed" desc="properties">
    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $iso;

    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $name;
    
    // </editor-fold>
    
    // <editor-fold defaultstate="collapsed" desc="getters / setters">

    public function getIso() {
        return $this->iso;
    }

    public function getName() {
        return $this->name;
    }

    public function setIso($iso) {
        $this->iso = $iso;
    }

    public function setName($name) {
        $this->name = $name;
    }


// </editor-fold>
        
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
                case 'iso':
                case 'name':
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

    //<editor-fold desc="JsonSerializable implementation">
    public function jsonSerialize() {
        return array_merge(
                parent::jsonSerialize(),
                [
                    'iso' => $this->iso,
                    'name' => $this->name
                ]);
    }
    //</editor-fold>
}

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
 * @OGM\Node(label="Shift")
 */
class BaseShift extends BaseModel implements \JsonSerializable {

    //<editor-fold desc="protected fields">
    /**
     * @var int
     * 
     * @OGM\Property(type="int")
     */
    protected $hours;
    
    /**
     * @var int
     * 
     * @OGM\Property(type="int")
     */
    protected $shifttype;    


    //</editor-fold>

// <editor-fold defaultstate="collapsed" desc="getters /setters">

    function getHours() {
        return $this->hours;
    }

    function setHours($hours) {
        $this->hours = $hours;
    }
    function getShifttype() {
        return $this->shifttype;
    }

    function setShifttype($shifttype) {
        $this->shifttype = $shifttype;
    }



// </editor-fold>
 
    //<editor-fold desc="JsonSerializable implementation">   
    public function jsonSerialize() {
        
        $ret = array_merge(
            parent::jsonSerialize(),
            [
                'hours' => $this->hours, 
                'shifttype' => $this->shifttype
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
        
        foreach ($vars as $key => $value) {
            switch ($key) {
                case 'hours':
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

    public function getQueryFields() {
        $tag = $this->tag();
        $vars = get_object_vars($this);
       
        foreach ($vars as $key => $value) {
            switch ($key) {
                case 'id':
                case 'tagId':
                case 'workday':
                    break;
                default:
                    $fields .= $key.":{".$tag."_".$key."},";                    
                    break;
            }            
        }
        return substr($fields, 0, strlen($fields)-1);
    }
    
    public function getQueryFieldsParams() :array {
        $tag = $this->tag();
        $vars = get_object_vars($this);
        $params = array();
        foreach ($vars as $key => $value) {
            switch ($key) {
                case 'id':
                case 'tagId':
                case 'workday':
                    break;
                default:
                    $params[$tag."_".$key] = $value;
                    break;
            }            
        }
        return $params;
    }    
    //</editor-fold>       
}

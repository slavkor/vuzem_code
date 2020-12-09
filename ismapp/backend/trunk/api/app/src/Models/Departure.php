<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;

use GraphAware\Neo4j\OGM\Annotations as OGM;
use GraphAware\Neo4j\OGM\Common\Collection;

use App\Models\Origin;
use App\Models\Destination;

/**
 *
 * @OGM\Node(label="Departure")
 */
class Departure extends BaseModel implements \JsonSerializable {

    /**
     *
     * @var string 
     * 
     * @OGM\Property(type="string")
     */    
    public $departtime;

    /**
     *
     * @var int 
     * 
     * @OGM\Property(type="int")
     */    
    public $estimatedworkers;

    /**
     *
     * @var boolean 
     * 
     * @OGM\Property(type="boolean")
     */    
    public $internal;
    
    
    public $employeesremove;
    public $employeesadd;

    /**
     *
     * @var Origin 
     * 
     */    
    public $origin;
    /**
     *
     * @var Destination 
     * 
     */
    public $destination;
    
    // <editor-fold defaultstate="collapsed" desc="getters /setters ">


    public function getDeparttime() {
        return $this->departtime;
    }

    public function setDeparttime($departtime) {
        $this->departtime = $departtime;
    }

    function getEmployeesremove() {
        return $this->employeesremove;
    }

    function getEmployeesadd()  {
        return $this->employeesadd;
    }

    function setEmployeesremove($employeesremove) {
        $this->employeesremove = $employeesremove;
    }

    function setEmployeesadd(   $employeesadd) {
        $this->employeesadd = $employeesadd;
    }
    
    function getOrigin(): Origin {
        return $this->origin;
    }

    function getDestination(): Destination {
        return $this->destination;
    }

    function setOrigin(Origin $origin) {
        $this->origin = $origin;
    }

    function setDestination(Destination $destination) {
        $this->destination = $destination;
    }

    function getEstimatedworkers() {
        return $this->estimatedworkers;
    }

    function setEstimatedworkers($estimatedworkers) {
        $this->estimatedworkers = $estimatedworkers;
    }

            // </editor-fold>

    // <editor-fold defaultstate="collapsed" desc="BaseModel ">
    public function setUuid($uuid) {
        $this->uuid = $uuid;
    }

    public function getid() {
        return $this->id;
    }

    public function getUuid() {
        return $this->uuid;
    }

    public function import($object) {
        
        $vars=is_object($object)?get_object_vars($object):$object;

        if (!is_array($vars)) {
            throw \Exception('no props to import into the object!');
        }
        foreach ($vars as $key => $value) {
            switch ($key) {
                case 'departtime':
                case "internal":
                case 'estimatedworkers':
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
        $fields ="";
        foreach ($vars as $key => $value) {
            switch ($key) {
                case 'id':
                case 'tagId':
                case 'departdate':
                case 'employeesremove':
                case 'employeesadd':
                case 'origin':
                case 'destination':
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
                case 'departdate':
                case 'employeesremove':
                case 'employeesadd':
                case 'origin':
                case 'destination':
                    break;
                default:
                    $params[$tag."_".$key] = $value;
                    break;
            }            
        }
        return $params;
    }    
    // </editor-fold>

    // <editor-fold defaultstate="collapsed" desc="JsonSerializable">
    public function jsonSerialize() {

        $ret = array_merge(
                parent::jsonSerialize(),
                [
                    'departtime' => $this->departtime, 
                    "estimatedworkers" => $this->estimatedworkers, 
                    "internal" => $this->internal
                    ]);
  
        return $ret;
    }
// </editor-fold>
}

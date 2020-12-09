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
 * @OGM\Node(label="Car")
 */
class Car extends BaseModel implements \JsonSerializable {
    
    //<editor-fold desc="protected fields">
    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $make;

    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $model;
    
    
    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $registration;    

    
    /**
     * @var int
     * 
     * @OGM\Property(type="int")
     */
    protected $seats;

    /**
     * @var boolean
     * 
     * @OGM\Property(type="boolean")
     */
    protected $towhitch;

    /**
     * @var Company
     * 
     * @OGM\Relationship(type="BELONGS_TO", direction="OUTGOING", collection=false, mappedBy="", targetEntity="Company")
     */   
    protected $company;
    //</editor-fold>

    /**
     * @var int
     * 
     * @OGM\Property(type="int")
     */
    protected $milage;
    
// <editor-fold defaultstate="collapsed" desc="getters /setters">
    function getMake() {
        return $this->make;
    }

    function getModel() {
        return $this->model;
    }

    function getRegistration() {
        return $this->registration;
    }

    function getSeats() {
        return $this->seats;
    }

    function getTowhitch() {
        return $this->towhitch;
    }

    function setMake($make) {
        $this->make = $make;
    }

    function setModel($model) {
        $this->model = $model;
    }

    function setRegistration($registration) {
        $this->registration = $registration;
    }

    function setSeats($seats) {
        $this->seats = $seats;
    }

    function setTowhitch($towhitch) {
        $this->towhitch = $towhitch;
    }

    function getMilage() {
        return $this->milage;
    }

    function setMilage($milage) {
        $this->milage = $milage;
    }

        /**
     * 
     * @return \App\Models\Company
     */
    function getCompany() {
        return $this->company;
    }

    /**
     * 
     * @param \App\Models\Company $company
     */
    function setCompany($company) {
        $this->company = $company;
    }


// </editor-fold>
 
    //<editor-fold desc="JsonSerializable implementation">   
    public function jsonSerialize() {
        if(null === $this->company){
            $this->getCompany();
        }
        $ret = array_merge(
            parent::jsonSerialize(),
            [
                'make' => $this->make,
                'model' => $this->model,
                'registration' => $this->registration,
                'seats' => $this->seats,
                'towhitch' => $this->towhitch,
                'milage' => $this->milage,
                'company' => $this->company
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
                case 'make':
                case 'model':
                case 'registration':
                case 'seats':
                case 'towhitch':
                case 'milage':
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
                case 'company':
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
                case 'company':
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

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
 * @OGM\Node(label="Departure")
 */
class DepartureReport extends Departure implements \JsonSerializable {
    
    public function __construct() {
        parent::__construct();
        $this->employees = new Collection();
        
    }
    
    /**
     * @var Collection
     * 
     * @OGM\Relationship(type="DEPARTS_IN", direction="INCOMING", collection=true, mappedBy="", targetEntity="EmployeeEx")
     */      
    protected $employees;
    
    /**
     * @var Project|NULL
     * 
     * @OGM\Relationship(type="TO", direction="OUTGOING", collection=false, mappedBy="", targetEntity="Project")
     */      
    protected $to;    

    /**
     * 
     * @return Collection
     */
    function getEmployees() {
        return $this->employees;
    }

    /**
     * 
     * @return Project
     */
    function getTo() {
        return $this->to;
    }

    /**
     * 
     * @param Collection
     */
    function setEmployees($employees) {
        $this->employees = $employees;
    }
    
    /**
     * 
     * @param Project
    */
    function setTo($to) {
        $this->to = $to;
    }

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
        
                
    }

    public function tag(): string {
        $reflect = new \ReflectionClass($this);
        return $reflect->getShortName()."_".$this->tagId();        
    }
    
    public function tagId(): string {
        return $this->tagId;
    }   
    
   public function getQueryFields() {
       
    }
    
    public function getQueryFieldsParams() :array {
        return NULL;
    } 

        
    public function jsonSerialize() {

        
        
        if(null === $this->employees){
            $this->getEmployees();
        }
        
        if(null === $this->to){
            $this->getTo();
        }
       
//        $iterator = $this->employees->getIterator();
//        $iterator->uasort(function ($first, $second) {
//            return  $first->getLastname() > (int) $second->getPosition() ? 1 : -1;
//        });
//        iterator_to_array($iterator);
        
        $ret = array_merge(parent::jsonSerialize(), ["employees" => $this->employees->toArray(), "to" => $this->to]);;
  
        return  ["departure" => $ret] ;
    }
}

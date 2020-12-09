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
 * @OGM\Node(label="Month")
 */
class Month extends BaseModel implements \JsonSerializable{

    public function __construct() {
        $this->getYear();
    }

    /**
     * @OGM\GraphId()
     *
     * @var int
     */
    protected $id;
    
    /**
     * value
     * @var int
     * 
     * @OGM\Property(type="int")
     */
    protected $value;
    
    /**
     * value
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $strrep;    
    
    /**
     * @var Year
     * 
     * @OGM\Relationship(type="CONTAINS", direction="INCOMING", collection=False, mappedBy="", targetEntity="Year")
     */      
    protected $year;
    
    public function getStrrep() {
        if(null == $this->strrep){
            
            $this->strrep = $this->getYear()->getStrrep().$this->getValue();
        }        
        return $this->strrep;
    }

    public function setStrrep($strrep) {
        $this->strrep = $strrep;
    }

    public function getValue() {
        return $this->value;
    }

    public function getYear() {
        return $this->year;
    }

    public function setValue($value) {
        $this->value = $value;
    }

    public function setYear(Year $year) {
        $this->year = $year;
    }

    public function jsonSerialize() {
        if(null == $this->year)
            $this->getYear();
        
        $ret = array_merge(
                parent::jsonSerialize(),
                [
                    'value' => $this->value,
                    'strrep' => $this->strrep,
                    'year' => $this->year
                    ]);
  
        return $ret;
        
        return $this->strrep;
    }

    public function setUuid($uuid) {
        
    }

    public function getid() {
        
    }

    public function getUuid() {
        
    }

    public function import($object) {
        
    }

    public function tag(): string {
        
    }

}

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
 * @OGM\Node(label="Day")
 */
class Day extends BaseModel implements  \JsonSerializable{

    public function __construct() {
        $this->getMonth();
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
     * date
     * @var \DateTime
     *
     * @OGM\Property()
     * @OGM\Convert(type="datetime", options={"format":"long_timestamp"})
     */
     protected $datevalue;

     /**
     * @var Month
     * 
     * @OGM\Relationship(type="CONTAINS", direction="INCOMING", collection=False, mappedBy="", targetEntity="Month")
     */      
    protected $month;
    
    
    public function getStrrep() {
        if(null == $this->strrep){
            
            $this->strrep = $this->getMonth()->getStrrep().$this->getValue();
        }
        return $this->strrep;
    }

    public function setStrrep($strrep) {
        $this->strrep = $strrep;
    }

    public function getValue() {
        return $this->value;
    }

    public function getMonth() {
        return $this->month;
    }

    public function setValue($value) {
        $this->value = $value;
    }

    public function setMonth(Month $month) {
        $this->month = $month;
    }

    /**
     * 
     * @return \DateTime
     */
    public function getDatevalue() {
        return $this->datevalue;
    }

    /**
     * 
     * @param \DateTime $datevalue
     */
    public function setDatevalue($datevalue) {
        $this->datevalue = $datevalue;
    }

    
    
    public function getYearValue() :int {
        return $this->getMonth()->getYear()->getValue();
    }
    public function getMonthValue() {
        return $this->getMonth()->getValue();
    }
    
    public function getDate() : \DateTime{
        $date = new \DateTime();
        $date->setDate ($this->month->getYear()->getValue(), $this->month->getValue(), $this->value);
        return $date;
    }
    
    
    
    public function jsonSerialize() {
        if(null == $this->month)
            $this->getMonth();
        
        $ret = array_merge(
                parent::jsonSerialize(),
                [
                    'value' => $this->value,
                    'strrep' => $this->strrep,
                    'date' => $this->datevalue,
                    'month' => $this->month
                    ]);
  
        return $ret;
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

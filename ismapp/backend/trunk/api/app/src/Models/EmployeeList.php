<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;
use GraphAware\Neo4j\OGM\Annotations as OGM;
use \GraphAware\Neo4j\OGM\Common\Collection;
use App\Models\OccupancyRelation;

/**
 *
 * @OGM\Node(label="Employee")
 */
class EmployeeList extends BaseEmployee implements \JsonSerializable {
    public function __construct() {
        parent::__construct();
        $this->address = new Collection();
        $this->periods = new Collection();
    }

    /**
     * @var BusinessPartner 
     * 
     * @OGM\Relationship(type="LOANS", direction="INCOMING", collection=false, mappedBy="", targetEntity="BusinessPartner")
     */    
    protected  $loaner;
    
    /**
     * @var OccupancyRelation[]
     * 
     * @OGM\Relationship(relationshipEntity="OccupancyRelation", type="OCCUPIES", direction="OUTGOING", collection=true, mappedBy="employee")
     */
    protected $occupancies;    
    
    
    /**
     * @var DepartureRelation[]
     * 
     * @OGM\Relationship(relationshipEntity="DepartureRelation", type="DEPARTS_IN", direction="OUTGOING", collection=true, mappedBy="employee")
     */
    protected $departures;        
    
    /**
     * @var Collection
     * 
     * @OGM\Relationship(type="LIVES", direction="OUTGOING", collection=true, mappedBy="", targetEntity="Address")
     */    
    protected $address;

    /**
     * @var Collection
     * 
     * @OGM\Relationship(type="WORKS_IN", direction="OUTGOING", collection=true, mappedBy="", targetEntity="WorkPeriod")
     */    
    protected $periods;
        
    /**
     * 
     * @return OccupancyRelation[]
     */
    function getOccupancies() {
        return $this->occupancies;
    }

    function setOccupancies(Collection $occupancies) {
        $this->occupancies = $occupancies;
    }

    /**
     * 
     * @return DepartureRelation[]
     */
    function getDepartures() {
        return $this->departures;
    }

    function setDepartures(Collection $departures) {
        $this->departures = $departures;
    }

    /**
     * 
     * @return Collection
     */
    public function getAddress() {
        return $this->address;
    }

    /**
     * 
     * @param Collection $address
     */
    public function setAddress($address) {
        $this->address = $address;
    }

    /**
     * 
     * @return Collection
     */    
    function getPeriods() {
        return $this->periods;
    }

    /**
     * 
     * @param Collection $periods
     */
    function setPeriods($periods) {
        $this->periods = $periods;
    }

    /**
     * 
     * @return \App\Models\BusinessPartner
     */
    public function getLoaner() {
        return $this->loaner;
    }
    /**
     * 
     * @param \App\Models\BusinessPartner $loaner
     */
    public function setLoaner($loaner) {
        $this->loaner = $loaner;
    }   
    
    /**
     * 
     * @return OccupancyRelation
     */
    public function getLastOccupancy(){
        if(NULL !== $this->occupancies){
            return  $this->occupancies->filter(function($occ){ return $occ->getOccupancy()->getActive() === 1; })->first();
        } else {
            return NULL;            
        }      
    }
    public function jsonSerialize() {
        $ret = parent::jsonSerialize();
        if (null == $this->loaner) {
            $this->getLoaner();
        }
        if (null == $this->address) {
            $this->getAddress();
        }        
        if (null == $this->occupancies) {
            $this->getOccupancies();
        }

        if (null == $this->departures) {
            $this->getDepartures();
        }

        $stalni = $this->address->filter(function($address){ return $address->gettype() === "STALNI"; })->filter(function($address){ return $address->getDeleted() === 0; })->first();
        $zac = $this->address->filter(function($address){ return $address->gettype() === "ZAČASNI"; })->filter(function($address){ return $address->getDeleted() === 0; })->first();

        $naslov = $zac;
        if($stalni !== FALSE)
            $naslov = $stalni;
 
        $ret["address"] = $naslov;
 
        $period = $this->periods->filter(function($period){ return $period->getActive(); })->first();
        $ret["workperiod"] = $period;
        
        /*
        if(NULL !== $this->occupancies){
            $ret['occupancies'] = $this->occupancies->toArray();
        } else {
            $ret['occupancies'] = NULL;            
        }
        
        if(NULL !== $this->departures){
            $ret['departures'] = $this->departures->toArray();
        } else {
            $ret['departures'] = NULL;            
        }
        */
        
        /*
        if(NULL !== $this->departures){
            $ret['lastdeparture'] = $this->departures->filter(function($dep){ return $dep->getDeparture()->getStatus() === 1; })->first();
        } else {
            $ret['lastdeparture'] = NULL;            
        }
         */
        
        if(NULL !== $this->occupancies){
            $ret['lastoccupancy'] = $this->occupancies->filter(function($occ){ return $occ->getOccupancy()->getActive() === 1; })->first();
        } else {
            $ret['lastoccupancy'] = NULL;            
        } 
        
        if(NULL !== $this->departures){
            $ret['planeddepartures'] = $this->departures->filter(function($dep){ return $dep->getDeparture()->getStatus() === 0; })->toArray();
        } else {
            $ret['planeddepartures'] = NULL;            
        } 
        
        if(NULL !== $this->loaner){
            $ret['loaner'] = $this->loaner;
        } else {
            $ret['loaner'] = NULL;            
        }        
        return $ret;//["employee" =>  $ret];
        
        //return $ret;
    }
}

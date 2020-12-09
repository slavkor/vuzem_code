<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;

use GraphAware\Neo4j\OGM\Annotations as OGM;
use \GraphAware\Neo4j\OGM\Common\Collection;


/**
 *
 * @OGM\Node(label="Employee")
 */
class EmployeeEx extends BaseEmployee implements \JsonSerializable {
    
    
    public function __construct() {
        parent::__construct();
        $this->address = new Collection();
        $this->departures = new Collection();
    }
    /**
     * @var BusinessPartner 
     * 
     * @OGM\Relationship(type="LOANS", direction="INCOMING", collection=false, mappedBy="", targetEntity="BusinessPartner")
     */    
    protected  $loaner;

    /**
     * @var WorkPlace 
     * 
     * @OGM\Relationship(type="WORK", direction="OUTGOING", collection=false, mappedBy="", targetEntity="WorkPlace")
     */    
    protected  $workplace;    
    
    
     /**
     * @var Collection
     * 
     * @OGM\Relationship(relationshipEntity="DepartureRelationEx", type="DEPARTS_IN", direction="OUTGOING", collection=true, mappedBy="employee")
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
     * @OGM\Relationship(type="ASSOCIATED_WITH", direction="OUTGOING", collection=true, mappedBy="", targetEntity="Document")
     */    
    protected $document;

//    /**
//     * @var Collection
//     * 
//     * @OGM\Relationship(type="WORKS_IN", direction="OUTGOING", collection=true, mappedBy="", targetEntity="WorkPeriod")
//     */    
//    protected $periods;
//    
 /**
     * 
     * @return \App\Models\WorkPlace
     */
    function getWorkplace(){
        return $this->workplace;
    }

    /**
     * 
     * @param \App\Models\WorkPlace $workplace
     */
    function setWorkplace($workplace) {
        $this->workplace = $workplace;
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
    public function getDocument() {
        return $this->address;
    }

    /**
     * 
     * @param Collection $address
     */
    public function setDocument($address) {
        $this->address = $address;
    }    

    /**
     * 
     * @return Collection
     */    
//    function getPeriods() {
//        return $this->periods;
//    }

    /**
     * 
     * @param Collection $periods
     */
//    function setPeriods($periods) {
//        $this->periods = $periods;
//    }

    /**
     * 
     * @return Collection
     */
    function getDepartures() {
        return $this->departures;
    }

    /**
     * 
     * @param Collection $departures
     */
    function setDepartures($departures) {
        $this->departures = $departures;
    }

    public function addDeparture(DepartureEx $departure){
        $relation = new DepartureRelationEx($this, $departure);
        $this->getDepartures()->add($relation);
    }
    
    /**
     *
     * @var DepartureEx $departure
     */
    protected $departure;
    public function setDeparture(DepartureEx $departure){
        $this->departure = $departure;
    }

    public function jsonSerialize() {
        $ret = parent::jsonSerialize();
        if (null == $this->loaner) {
            $this->getLoaner();
        }
        if (null == $this->address) {
            $this->getAddress();
        }
        
        if (null == $this->document) {
            $this->getDocument();
        }

//        if (null == $this->periods) {
//            $this->getPeriods();
//        }
        
//        $period = $this->periods->filter(function($period){ return $period->getActive(); })->first();
//        $ret["workperiod"] = $period;
        
        if(NULL !== $this->loaner){
            $ret['loaner'] = $this->loaner;
        } else {
            $ret['loaner'] = NULL;            
        }
        
        
        
        $stalni = $this->address->filter(function($address){ return $address->gettype() === "STALNI"; })->filter(function($address){ return $address->getDeleted() === 0; })->first();
        $zac = $this->address->filter(function($address){ return $address->gettype() === "ZAČASNI"; })->filter(function($address){ return $address->getDeleted() === 0; })->first();

        $naslov = $zac;
        if($stalni !== FALSE)
            $naslov = $stalni;
 
        $ret["address"] = $naslov;
 
        
        $osebna = $this->document->filter(function($doc){return $doc->getType()->getName() === "OSEBNA_IZKAZNICA";})->first();
        $potni = $this->document->filter(function($doc){return $doc->getType()->getName() === "POTNI_LIST";})->first();
        
        $doc = $osebna;
        if($potni !== FALSE)
            $doc = $potni;
        
        $ret['document'] = $doc;
                
        if(NULL !== $this->departure){
            $ret['departure'] = $this->departures->filter(function($relation){return $relation->getDeparture() === $this->departure;})->first();
        }
        
        return $ret;
    }
}

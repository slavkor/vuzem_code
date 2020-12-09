<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;

use GraphAware\Neo4j\OGM\Annotations as OGM;
use \GraphAware\Neo4j\OGM\Common\Collection;
use App\Models\BusinessPartner;
use App\Models\EmployeeEx;


/**
 *
 * @OGM\Node(label="CSite")
 */
class CSiteOverview extends BaseCSite {
    
    public function __construct() {
        parent::__construct();
        $this->projects = new Collection();
    }
    /**
     * @var BusinessPartner
     * 
     * @OGM\Relationship(type="IS_FOR", direction="OUTGOING", collection=false, mappedBy="", targetEntity="BusinessPartner")
     */    
    public $customer; 
    
    /**
     *
     * @var EmployeeEx[]|Collection
     */
    protected $employees;

    /**
     * 
     * @return EmployeeEx[]|Collection
     */
    public function getEmployees() {
        return $this->employees;
    }

    /**
     * 
     * @param EmployeeEx[]|Collection $employees
     */
    public function setEmployees($employees) {
        $this->employees = $employees;
    }

    /**
     * 
     * @return \App\Models\BusinessPartner|NULL
     */
    public function getCustomer()   {
        return $this->customer;
    }

    /**
     * 
     * @param \App\Models\BusinessPartner|NULL
     */
    public function setCustomer($customer) {
        $this->customer = $customer;
    }
    
            
    //<editor-fold desc="JsonSerializable implementation">    
    public function jsonSerialize() {
        $ret = parent::jsonSerialize();
        if($this->customer === NULL){
            $this->getCustomer();
        }    
       if($this->customer !== NULL){
            $ret['customer'] = $this->customer;
        }            
        return array_merge($ret, ['employees' => $this->employees]);
        
    }
    //</editor-fold>    
}

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

/**
 *
 * @OGM\Node(label="CSite")
 */
class CSite extends BaseCSite {
    
    public function __construct() {
        parent::__construct();
        //$this->addresses = new Collection();
        //$this->contacts = new Collection();
        //$this->documents = new Collection();
        //$this->projects = new Collection();
        //$this->customer = new BusinessPartner();
    }
    
    // <editor-fold defaultstate="collapsed" desc="properties">
    
//    /**
//     * @var Address[]|Collection
//     * 
//     * @OGM\Relationship(type="LIVES", direction="OUTGOING", collection=true, mappedBy="", targetEntity="Address")
//     */    
//    protected $addresses;
//
//    /**
//     * @var Contact[]|Collection
//     * 
//     * @OGM\Relationship(type="CONTACT", direction="OUTGOING", collection=true, mappedBy="", targetEntity="Contact")
//     */    
//    protected  $contacts;
//
//    /**
//     * @var Document[]|Collection
//     * 
//     * @OGM\Relationship(type="ASSOCIATED_WITH", direction="OUTGOING", collection=true, mappedBy="", targetEntity="Document")
//     */    
//    protected $documents;    
//
//    /**
//     * @var Project[]|Collection
//     * 
//     * @OGM\Relationship(type="HAS", direction="OUTGOING", collection=true, mappedBy="", targetEntity="Project")
//     */    
//    protected $projects;    

    /**
     * @var BusinessPartner
     * 
     * @OGM\Relationship(type="IS_FOR", direction="OUTGOING", collection=false, mappedBy="", targetEntity="BusinessPartner")
     */    
    public $customer; 
    
    /**
     * @var Company
     * 
     * @OGM\Relationship(type="IS_FOR", direction="OUTGOING", collection=false, mappedBy="", targetEntity="Company")
     */    
    public $company;     

    // </editor-fold>
    
    //<editor-fold desc="getters and setters"> 
    
    public function getAddresses(){
        return $this->addresses;
    }

    public function getContacts(){
        return $this->contacts;
    }

    public function getDocuments() {
        return $this->documents;
    }

    public function getProjects() {
        return $this->projects;
    }

    public function setAddresses($addresses) {
        $this->addresses = $addresses;
    }

    public function setContacts($contacts) {
        $this->contacts = $contacts;
    }

    public function setDocuments($documents) {
        $this->documents = $documents;
    }

    public function setProjects($projects) {
        $this->projects = $projects;
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
    
    function getCompany() {
        return $this->company;
    }

    function setCompany( $company) {
        $this->company = $company;
    }

        //</editor-fold>

    //<editor-fold desc="JsonSerializable implementation">    
    public function jsonSerialize() {
        $ret = parent::jsonSerialize();
        if($this->customer === NULL){
            $this->getCustomer();
        }
        if($this->company === NULL){
            $this->getCompany();
        }        
        if($this->customer !== NULL){
            $ret['customer'] = $this->customer;
        }        
        if($this->company !== NULL){
            $ret['company'] = $this->company;
        }
        return $ret;//['constructionsite' =>$ret];
    }
    //</editor-fold>    
}

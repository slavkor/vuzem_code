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
 * @OGM\Node(label="BusinessPartner")
 */
class BusinessPartner extends BaseBusinessPartner {

    public function __construct() {
        parent::__construct();
        $this->addresslist = new Collection();
        $this->contactlist = new Collection();
        $this->documentlist = new Collection();
    }
    
    /**
     * @var Address[]|Collection
     * 
     * @OGM\Relationship(type="LIVES", direction="OUTGOING", collection=true, mappedBy="", targetEntity="Address")
     */    
    protected $addresslist;

    /**
     * @var Document[]|Collection
     * 
     * @OGM\Relationship(type="ASSOCIATED_WITH", direction="OUTGOING", collection=true, mappedBy="", targetEntity="Document")
     */    
    protected $documentlist;

    /**
     * @var Contact[]|Collection
     * 
     * @OGM\Relationship(type="CONTACT", direction="OUTGOING", collection=true, mappedBy="", targetEntity="Contact")
     */    
    protected  $contactlist;

    public function getAddresslist() {
        return $this->addresslist;
    }

    public function getDocumentlist() {
        return $this->documentlist;
    }

    public function getContactlist() {
        return $this->contactlist;
    }

    public function setAddresslist($addresslist) {
        $this->addresslist = $addresslist;
    }

    public function setDocumentlist($documentlist) {
        $this->documentlist = $documentlist;
    }

    public function setContactlist($contactlist) {
        $this->contactlist = $contactlist;
    }

            
    public function jsonSerialize() {
        $ret = array_merge(parent::jsonSerialize(),[
            'addresses' => $this->addresslist->toArray(),
            'contacts' => $this->contactlist->toArray(),
            'documents' => $this->documentlist->toArray()
                
        ]);
        
        return $ret;
    }
}

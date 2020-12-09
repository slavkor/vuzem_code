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
 * @OGM\Node(label="Company")
 */
class CompanyDocuments extends BaseCompany implements \JsonSerializable {

    public function __construct() {
        parent::__construct();
        $this->documents = new Collection();
    }
    
    /**
     * @var Document[]|Collection
     * 
     * @OGM\Relationship(type="ASSOCIATED_WITH", direction="OUTGOING", collection=true, mappedBy="", targetEntity="Document")
     */  
    protected $documents;  


    //<editor-fold desc="JsonSerializable implementation">   
    public function getDocuments() {
        return $this->documents;
    }

    public function setDocuments($documents) {
        $this->documents = $documents;
    }

    //</editor-fold>

    //<editor-fold desc="JsonSerializable implementation">   
    public function jsonSerialize() {
        
        $ret = array_merge(
            parent::jsonSerialize(),
            [
                'name' => $this->name,
                'shortname' => $this->shortname,
                'taxnumber' => $this->taxnumber,
                'regnumber' => $this->regnumber
            ]
        ); 
        if(null !== $this->documents){
            $ret['documents'] = $this->documents;
        }
        return $ret;
    }
    //</editor-fold>       
}

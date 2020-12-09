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
class ErwEmployee extends BaseEmployee implements \JsonSerializable {
    
    public function __construct() {
        parent::__construct();
        $this->ewrs = new Collection();
    }
    
    /**
     * @var Collection
     *
     * @OGM\Relationship(relationshipEntity="EwrWorker", type="WORKS", direction="OUTGOING", collection=true, mappedBy="employee")
     */
    protected $ewrs;
    
    /**
     * 
     * @return Collection
     */
    function getEwrs() {
        return $this->ewrs;
    }

    function setEwrs(Collection $ewrs) {
        $this->ewrs = $ewrs;
    }

        
    public function jsonSerialize() {
        $ret = parent::jsonSerialize();
        if (null == $this->ewrs) {
            $this->getEwrs();
    
        if(NULL !== $this->ewrs){
            $ret['ewrs'] = $this->ewrs;
        } else {
            $ret['ewrs'] = NULL;            
        }
        //return["employee" => $ret];
        return $ret;
        }
    }
}

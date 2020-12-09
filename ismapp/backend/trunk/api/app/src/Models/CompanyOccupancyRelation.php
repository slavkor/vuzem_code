<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;

use GraphAware\Neo4j\OGM\Annotations as OGM;
use App\Models\Company;
use App\Models\Occupancy;

/**
 *
 * @OGM\RelationshipEntity(type="OCCUPIES")
 */
class CompanyOccupancyRelation implements \JsonSerializable{


    /**
     *
     * @var string 
     */
    protected $type;

    /**
     * @var int
     *
     * @OGM\GraphId()
     */
    protected $id;

    /**
     * @var Occupancy
     *
     * @OGM\StartNode(targetEntity="Occupancy")
     */
    protected $occupancy;
    
    /**
     * @var Company
     *
     * @OGM\EndNode(targetEntity="Company")
     */
    protected $company;    

    public function __construct()
    {
        $this->type = "COMPANY";
    }

    /**
     * 
     * @return Occupancy
     */
    function getOccupancy(){
        return $this->occupancy;
    }
    
    /**
     * 
     * @return Company
     */
    function getCompany() {
        return $this->company;
    }
    
    /**
     * 
     * @param Company $company
     */
    function setCompany($company) {
        $this->company = $company;
    }
    
    function getType() {
        return "COMPANY";
    }

    public function jsonSerialize() {
        
        if (strpos(get_class ($this->company), 'Company') === false) {
            return $this->company;
        }
        
    }
}

<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;

use GraphAware\Neo4j\OGM\Annotations as OGM;
use App\Models\Occupancy;
use App\Models\EmployeeList;
/**
 *
 * @OGM\RelationshipEntity(type="OCCUPIES")
 */
class OccupancyRelation implements \JsonSerializable{
    /**
     * @var int
     *
     * @OGM\GraphId()
     */
    protected $id;

    /**
     * @var EmployeeList
     *
     * @OGM\StartNode(targetEntity="EmployeeList")
     */
    protected $employee;

    /**
     * @var Occupancy
     *
     * @OGM\EndNode(targetEntity="Occupancy")
     */
    protected $occupancy;
    

    public function __construct(EmployeeList $employee, Occupancy $occupancy)
    {
        $this->employee = $employee;
        $this->occupancy = $occupancy;
    }


    /**
     * 
     * @return EmployeeList
     */
    function getEmployee(){
        return $this->employee;
    }

    
    /**
     * 
     * @return Occupancy
     */
    function getOccupancy() {
        return $this->occupancy;
    }

    public function jsonSerialize() {
    
        //$this->getOccupancy();
        
        return $this->occupancy;
    }
}

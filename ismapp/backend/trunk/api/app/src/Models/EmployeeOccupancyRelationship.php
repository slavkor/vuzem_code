<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;

use GraphAware\Neo4j\OGM\Annotations as OGM;
use App\Models\Occupancy;
use App\Models\EmployeeAdd;

/**
 *
 * @OGM\RelationshipEntity(type="OCCUPIES")
 */
class EmployeeOccupancyRelationship {
  /**
     * @OGM\GraphId()
     * @var int
     */
    protected $id;

    /**
     * @OGM\StartNode(targetEntity="EmployeeAdd")
     * @var EmployeeAdd
     */
    protected $employee;

    /**
     * @OGM\EndNode(targetEntity="Occupancy")
     * @var Occupancy
     */
    protected $occupancy;

    /**
     * @OGM\Property(type="int")
     * @var int
     */
    protected $type;

    /**
     * Rating constructor.
     * @param EmployeeAdd $employee
     * @param Occupancy $occupancy
     * @param int $type
     */
    
    public function __construct(EmployeeAdd $employee, Occupancy $occupancy, $type)
    {
        $this->employee = $employee;
        $this->occupancy = $occupancy;
        $this->type = $type;
    }
    
    /**
     * @return int
     */
    function getId() {
        return $this->id;
    }
    
    /**
     * @return EmployeeAdd
     */
    function getEmployee(): EmployeeAdd {
        return $this->employee;
    }
    
    /**
     * @return Occupancy
     */
    function getOccupancy(): Occupancy {
        return $this->occupancy;
    }
    
    /**
     * @return int
     */
    function getType() {
        return $this->type;
    }


}

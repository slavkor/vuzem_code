<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;
use GraphAware\Neo4j\OGM\Annotations as OGM;
use GraphAware\Neo4j\OGM\Common\Collection;
use App\Models\Employee;

/**
 *
 * @OGM\Node(label="Occupancy")
 */
class OccupancyOverview extends BaseModel implements  \JsonSerializable {
    
    /**
     * @var Employee
     * 
     * @OGM\Relationship(type="OCCUPIES", direction="INCOMING", collection=false, targetEntity="Employee")
     */
    protected $employee; 


    /**
     * 
     * @return Employee
     */
    public function getEmployee() {
        return $this->employee;
    }

    /**
     * 
     * @param Employee $employee
     */
    public function setEmployee($employee) {
        
        $this->employee = $employee;
    }

    public function jsonSerialize() {
        $this->getEmployee();
        return $this->employee;
    }

    public function getUuid() {
        
    }

    public function getid() {
        
    }

    public function import($object) {
        
    }

    public function setUuid($uuid) {
        
    }

    public function tag(): string {
        
    }

}

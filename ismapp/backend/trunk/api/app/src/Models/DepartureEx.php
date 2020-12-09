<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;
use GraphAware\Neo4j\OGM\Annotations as OGM;
use GraphAware\Neo4j\OGM\Common\Collection;
use App\Models\DepartureCarRelation;

/**
 *
 * @OGM\Node(label="Departure")
 */
class DepartureEx extends Departure implements \JsonSerializable{

    /**
     * @var Collection
     * 
     * @OGM\Relationship(relationshipEntity="DepartureCarRelation", type="TRANSPORT", direction="INCOMING", collection=true, mappedBy="departure")
     */
    protected $cars;      

    
    /**
     * @var Collection
     * 
     * @OGM\Relationship(relationshipEntity="DepartureRelationEx", type="DEPARTS_IN", direction="INCOMING", collection=true, mappedBy="departure")
     */
    protected $employees;  
    
    
    /**
     * 
     * @return Collection
     */
    function getEmployees() {
        return $this->employees;
    }

    /**
     * 
     * @param Collection $employees
     */
    function setEmployees($employees) {
        $this->employees = $employees;
    }
    
    /**
     * 
     * @return Collection
     */
    function getCars() {
        return $this->cars;
    }

    /**
     * 
     * @param Collection $cars
     */
    function setCars($cars) {
        $this->cars = $cars;
    }

        
    public function addEmployee(EmployeeEx $employee, int $state, string $note) {
        $relation = new DepartureRelationEx($employee, $this, $state, $note);
        $this->getEmployees()->add($relation);
    }

    public function addCar(Car $car) {
        $relation = new DepartureCarRelation($car, $this);
        $this->getCars()->add($relation);
    }
    
    
    //<editor-fold desc="JsonSerializable implementation">    
    public function jsonSerialize() {
        $this->getEmployees();
        $ret = parent::jsonSerialize();
        $ret["employees"] = $this->employees->toArray();
        $ret["cars"] = $this->cars->toArray();
        return $ret;
        
    }
    //</editor-fold>     
        
}

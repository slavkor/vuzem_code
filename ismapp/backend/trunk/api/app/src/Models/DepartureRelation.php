<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;


use GraphAware\Neo4j\OGM\Annotations as OGM;
use App\Models\DepartureList;
use App\Models\EmployeeList;
/**
 *
 * @OGM\RelationshipEntity(type="DEPARTS_IN")
 */
class DepartureRelation implements \JsonSerializable{
    
    public function __construct(EmployeeList $employee, DepartureList $departure) {
        $this->employee = $employee;
        $this->departure = $departure;
    }
    
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
     * @var DepartureList
     *
     * @OGM\EndNode(targetEntity="DepartureList")
     */
    protected $departure;

    /**
     * @OGM\Property(type="string")
     *
     * @var string
     */
    protected $note;
    
    /**
     * @OGM\Property(type="int")
     *
     * @var int
     */
    protected $state;
    /**
     * 
     * @return EmployeeList
     */
    function getEmployee(){
        return $this->employee;
    }
    
    /**
     * 
     * @return DepartureList
     */
    function getDeparture() {
        return $this->departure;
    }

    function getNote() {
        return $this->note;
    }

    function getState() {
        return $this->state;
    }

    function setNote($note) {
        $this->note = $note;
    }

    function setState($state) {
        $this->state = $state;
    }

    public function jsonSerialize() {

        return ["departure" =>$this->departure, "note" => $this->note, "state" => $this->state];
    }
}

<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;


use GraphAware\Neo4j\OGM\Annotations as OGM;
use App\Models\DepartureEx;
use App\Models\EmployeeEx;
/**
 *
 * @OGM\RelationshipEntity(type="DEPARTS_IN")
 */
class DepartureRelationEx implements \JsonSerializable{
    
    public function __construct(EmployeeEx $employee, DepartureEx $departure, int $state, string $note) {
        $this->employee = $employee;
        $this->departure = $departure;
        $this->state = $state;
        $this->note = $note;
    }
    
    /**
     * @var int
     *
     * @OGM\GraphId()
     */
    protected $id;

    /**
     * @var EmployeeEx
     *
     * @OGM\StartNode(targetEntity="EmployeeEx")
     */
    protected $employee;

    /**
     * @var DepartureEx
     *
     * @OGM\EndNode(targetEntity="DepartureEx")
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
     * @return EmployeeEx
     */
    function getEmployee(){
        return $this->employee;
    }
    
    /**
     * 
     * @return DepartureEx
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

        return ["state" => $this->state, "note" => $this->note];
    }
}

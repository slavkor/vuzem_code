<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;


use GraphAware\Neo4j\OGM\Annotations as OGM;
use App\Models\Absence;
use App\Models\EmployeeModel;

/**
 * Description of EmployeeAbsenceRelation
 *
 * @author slavko.rihtaric
 */


/**
 *
 * @OGM\RelationshipEntity(type="ABSENT")
 */

class EmployeeAbsenceRelation implements \JsonSerializable {
  public function __construct(Absence $absence, EmployeeModel $employee) {
        $this->absence = $absence;
        $this->employee = $employee;
        
    }
    
    /**
     * @var int
     *
     * @OGM\GraphId()
     */
    protected $id;

    /**
     * @var EmployeeModel
     *
     * @OGM\StartNode(targetEntity="Employee")
     */
    protected $employee;

    /**
     * @var Absence
     *
     * @OGM\EndNode(targetEntity="Absence")
     */
    protected $absence;

    /**
     * 
     * @return EmployeeModel
     */
    public function getEmployee() {
        return $this->employee;
    }

    /**
     * 
     * @return Absence
     */
    public function getAbsence() {
        return $this->absence;
    }

    /**
     * 
     * @param EmployeeModel $employee
     */
    public function setEmployee($employee) {
        $this->employee = $employee;
    }

    /**
     * 
     * @param Absence $absence
     */
    public function setAbsence($absence) {
        $this->absence = $absence;
    }

    public function jsonSerialize() {
        return  [
                'id' => $this->id, 
                'dd' => $this->absence
            ];
    }

}

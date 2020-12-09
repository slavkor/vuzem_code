<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;

use GraphAware\Neo4j\OGM\Annotations as OGM;

/**
 *
 * @OGM\Node(label="WorkPeriod")
 */
class WorkPeriodReport extends WorkPeriod implements \JsonSerializable {
    
    /**
     * @var EmployeeEx
     * 
     * @OGM\Relationship(type="WORKS_IN", direction="INCOMING", collection=false, mappedBy="", targetEntity="EmployeeEx")
     */  
    public $employee;
    
    /**
     * 
     * @return \App\Models\EmployeeEx
     */
    function getEmployee(){
        return $this->employee;
    }

    /**
     * 
     * @param \App\Models\EmployeeEx $employee
     */
    function setEmployee($employee) {
        $this->employee = $employee;
    }

        
    
    //<editor-fold desc="JsonSerializable implementation">    
    public function jsonSerialize() {
        

        $ret = parent::jsonSerialize();
        if (null === $this->employee) {
            $this->getEmployee();
        }
        $ret["employee"] = $this->employee;
    
        
        
        return $ret;
    }
    //</editor-fold>    
}

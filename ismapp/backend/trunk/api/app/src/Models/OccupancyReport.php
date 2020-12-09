<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;
use GraphAware\Neo4j\OGM\Annotations as OGM;
use GraphAware\Neo4j\OGM\Common\Collection;
use App\Models\Day;
use App\Models\CompanyOccupancyRelation;
use App\Models\ProjectOccupancyRelation;

/**
 *
 * @OGM\Node(label="Occupancy")
 */
class OccupancyReport extends Occupancy implements \JsonSerializable {

    /**
     * @var Employee
     * 
     * @OGM\Relationship(type="OCCUPIES", direction="INCOMING", collection=false, targetEntity="Employee")
     */
    protected $employee;    
    // <editor-fold defaultstate="collapsed" desc="getters /setters ">

    
    /**
     * 
     * @return \App\Models\Employee
     */
    function getEmployee(){
        return $this->employee;
    }
    /**
     * 
     * @param Employee $employee
     */
    function setEmployee($employee) {
        $this->employee = $employee;
    }

    
    // </editor-fold>


    // <editor-fold defaultstate="collapsed" desc="JsonSerializable">
    public function jsonSerialize() {
        if (NULL === $this->employee) $this->getEmployee();
        
        $start = null;
        $end = null;
        $loc = null;
        $name = null;
        $lastname = null;
        $uuid = null;
        
        if(NULL !== $this->getStart()) $start = $this->getStart()->getStrrep();
        if(NULL !== $this->getEnd()) $end = $this->getEnd()->getStrrep();

        if(NULL !== $this->getCompany()){$loc = $this->getCompany()->getName();}
        
        if(NULL !== $this->getProject()){
            $st = null;
            $ct = null;
            
            $stn = null;
            $ctn = null;
            
            if(NULL !== $this->getProject()->getSite()) {$st = $this->getProject()->getSite(); }
            if(NULL !== $st->getCustomer()) {$ct = $st->getCustomer(); }
            if(NULL !== $st) {$stn = $st->getname(); }
            if(NULL !== $ct) {$ctn = $ct->getname(); }
            
            $loc = $ctn ?? '' . ' - ' . $stn ?? '' . ' - ' . $this->getProject()->getName();
        }
        
        if(NULL !== $this->employee) {
            $name = $this->employee->getname();
            $lastname = $this->employee->getlastname();
            $uuid = $this->employee->getUuid();
        }

        return [
            'start' => $start,
            'end' => $end,
            'name' => $name,
            'lastname' => $lastname,
            'location' => $loc,
            'uuid' => $uuid];
        
        //return array_merge(parent::jsonSerialize(),["employee" => $this->employee]);
    }
// </editor-fold>


}

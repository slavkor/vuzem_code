<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;

use GraphAware\Neo4j\OGM\Annotations as OGM;
use GraphAware\Neo4j\OGM\Common\Collection;

use App\Models\Project;
use App\Models\Day;
use App\Models\Company;

/**
 *
 * @OGM\Node(label="Departure")
 */
class DepartureList extends Departure implements \JsonSerializable {

    public function __construct() {
        parent::__construct();
        $this->employees = new Collection();
    }

    /**
     * @var Day 
     * 
     * @OGM\Relationship(type="DEPARTDATE", direction="OUTGOING", collection=false, mappedBy="", targetEntity="Day")
     */      
    public $departdate;

    /**
     * @var Project 
     * 
     * @OGM\Relationship(type="FROM", direction="OUTGOING", collection=false, mappedBy="", targetEntity="Project")
     */      
    public $orgn;
    
    /**
     * @var Project 
     * 
     * @OGM\Relationship(type="TO", direction="OUTGOING", collection=false, mappedBy="", targetEntity="Project")
     */      
    public $dstn;

    
    /**
     * @var Company 
     * 
     * @OGM\Relationship(type="FROM", direction="OUTGOING", collection=false, mappedBy="", targetEntity="Company")
     */      
    public $corgn;
    
    /**
     * @var Company 
     * 
     * @OGM\Relationship(type="TO", direction="OUTGOING", collection=false, mappedBy="", targetEntity="Company")
     */      
    public $cdstn;

    // <editor-fold defaultstate="collapsed" desc="getters /setters ">

    /**
     * 
     * @return Day
     */
    function getDepartdate(){
        return $this->departdate;
    }

    /**
     * 
     * @param Day $departdate
     */
    function setDepartdate($departdate) {
        $this->departdate = $departdate;
    }

    /**
     * 
     * @return Project
     */
    function getDstn(){
        return $this->dstn;
    }

    /**
     * 
     * @param Project $dstn
     */
    function setDst($dstn) {
        $this->dstn = $dstn;
    }

    /**
     * 
     * @return Project
     */
    function getOrgn(){
        return $this->orgn;
    }

    /**
     * 
     * @param Project $orgn
     */
    function setOrgn($orgn) {
        $this->orgn = $orgn;
    }
    
    /**
     * 
     * @return Company
     */
    function getCorgn(){
        return $this->corgn;
    }

    /**
     * 
     * @return Company
     */
    function getCdstn(){
        return $this->cdstn;
    }

    /**
     * 
     * @param Company $corgn
     */
    function setCorgn($corgn) {
        $this->corgn = $corgn;
    }

    /**
     * 
     * @param Company $cdstn
     */
    function setCdstn($cdstn) {
        $this->cdstn = $cdstn;
    }

    // </editor-fold>

    // <editor-fold defaultstate="collapsed" desc="JsonSerializable">
    public function jsonSerialize() {
        if (null === $this->departdate) {
            $this->getDepartdate();
        }
        if (null === $this->dstn) {
            $this->getDstn();
        }
        if (null === $this->orgn) {
            $this->getOrgn();
        }

        if (null === $this->cdstn) {
            $this->getCdstn();
        }
        if (null === $this->corgn) {
            $this->getCorgn();
        }

        $ret = array_merge(
                parent::jsonSerialize(),
                [
                    'departdate' => $this->departdate,
                    "origincopmany" => $this->corgn,
                    "originproject" => $this->orgn,
                    "destinationcompany" => $this->cdstn,
                    "destinationproject" => $this->dstn
                    ]);
  
        return $ret;
    }
// </editor-fold>


}


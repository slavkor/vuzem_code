<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;

use GraphAware\Neo4j\OGM\Annotations as OGM;
use GraphAware\Neo4j\OGM\Common\Collection;

/**
 *
 * @OGM\Node(label="User")
 */
class User extends BaseUser implements \JsonSerializable {
    
    public function __construct() {
        parent::__construct();
        $this->documents = new Collection();
        $this->documentsdownloaded = new Collection();
    }

    /**
     * @var Document[]|Collection
     * 
     * @OGM\Relationship(type="CREATED", direction="OUTGOING", collection=true, mappedBy="", targetEntity="Document")
     */    
    protected $documents;

    /**
     * @var File[]|Collection
     * 
     * @OGM\Relationship(type="DOWNLOADED", direction="OUTGOING", collection=true, mappedBy="", targetEntity="File")
     */    
    protected $documentsdownloaded;    
    
    /**
     *
     * @var Employee
     * @OGM\Relationship(type="ASSIGNED_TO", direction="OUTGOING", collection=false, mappedBy="", targetEntity="Employee")
     */
    protected $employee;

    /**
     *
     * @var User
     * @OGM\Relationship(type="CREATED_BY", direction="INCOMING", collection=false, mappedBy="", targetEntity="User")
     */
    //protected $creator;
    
    protected $employeeid;
    
    public function getemployee() {
        return $this->employee;
    }
    public function getemployeeid() {
        return $this->employeeid;
    }
    public function getdocuments(){
        return $this->documents;
    }
    public function getdocumentsdownloaded(){
        return $this->documentsdownloaded;
    }
    /*
    public function getcreator() {
        return $this->creator;
    }
    public function setcreator($creator) {
        $this->creator = $creator;
    }
    */
    public function setemployee($employee) {
        $this->employee = $employee;
    }
    public function setemployeeid($employeeid) {
        $this->employeeid = $employeeid;
    }
}

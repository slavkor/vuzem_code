<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;

/**
 * Description of EmployeeFire
 *
 * @author slavko
 */
class EmployeeFire extends EmployeeHire {
    
    public function __construct() {
        parent::__construct();
    }

    /**
     * @var Company
     * 
     */      
    protected $fireemployer;
    
    /**
     * @var string
     * 
     */   
    protected $firedate;
    
    public function getFireemployer(): Company {
        return $this->fireemployer;
    }

    public function getFiredate() {
        return $this->firedate;
    }

    public function setFireemployer(Company $fireemployer) {
        $this->fireemployer = $fireemployer;
    }

    public function setFiredate($firedate) {
        $this->firedate = $firedate;
    }
    
    public function getDateFire() : \DateTime {
        $date = new \DateTime();
        $date->setTimestamp(strtotime($this->firedate));
        return $date;
    }


}


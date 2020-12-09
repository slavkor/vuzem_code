<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;


class EmployeeHire extends BaseEmployee implements \JsonSerializable {
    
    public function __construct() {
        parent::__construct();
    }

    /**
     * @var Company
     * 
     */      
    public $hireemployer;
    
    /**
     * @var string
     * 
     */   
    public $hiredate;



    
    public function getHireemployer() {
        return $this->hireemployer;
    }

    public function getHiredate() {
        return $this->hiredate;
    }

    public function setHireemployer(Company $hireemployer) {
        $this->hireemployer = $hireemployer;
    }

    public function setHiredate($hiredate) {
        $this->hiredate = $hiredate;
    }

    public function getDateHire() : \DateTime {
        $date = new \DateTime();
        $date->setTimestamp(strtotime($this->hiredate));
        return $date;
    }



        public function jsonSerialize() {
        $ret =  parent::jsonSerialize();
        $ret["employer"] = $this->hireemployer;
        $ret["hiredate"] = $this->hiredate;

        return $ret;
        
    }
}

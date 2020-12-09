<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;


class CSiteReport implements \JsonSerializable {
    

    /**
     *
     * @var array 
     */
    protected $employees;
    
    /**
     * 
     * @return array
     */
    function getEmployees() {
        return $this->employees;
    }

    /**
     * 
     * @param array $employees
     */
    function setEmployees($employees) {
        $this->employees = $employees;
    }

    public function jsonSerialize() {
        return ["employees" => $this->employees];
    }

}

<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;

/**
 * Description of Podlaga
 *
 * @author slavko
 */
class Podlaga implements \JsonSerializable{
    
    protected $name;
    protected $lastname;
    protected $type;
    protected $fromdate;
    protected $todate;
    function getName() {
        return $this->name;
    }

    function getType() {
        return $this->type;
    }

    function getFromdate() {
        return $this->fromdate;
    }

    function getTodate() {
        return $this->todate;
    }

    function setName($name) {
        $this->name = $name;
    }

    function setType($type) {
        $this->type = $type;
    }

    function setFromdate($fromdate) {
        $this->fromdate = $fromdate;
    }

    function setTodate($todate) {
        $this->todate = $todate;
    }
    function getLastname() {
        return $this->lastname;
    }

    function setLastname($lastname) {
        $this->lastname = $lastname;
    }

            
    //put your code here
    public function jsonSerialize() {
        return [
            "lastname" => $this->lastname,
            "name" => $this->name,
            "type" => $this->type,
            "fromdate" => $this->fromdate,
            "todate" => $this->todate,
        ];
    }

}

<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;

/**
 * Description of ProjectMonthSummary
 *
 * @author slavko
 */
class ProjectMonthSummary implements \JsonSerializable{
    /**
     *
     * @var string 
     * 
     */    
    public $date;
    
    /**
     *
     * @var string 
     * 
     */    
    public $name;
    /**
     *
     * @var string 
     * 
     */    
    public $lastname;
    /**
     *
     * @var int 
     * 
     */    
    public $hours;
    
    // <editor-fold defaultstate="collapsed" desc="getters /setters">
    function getDate() {
        return $this->date;
    }

    function getName() {
        return $this->name;
    }

    function getLastname() {
        return $this->lastname;
    }

    function getHours() {
        return $this->hours;
    }

    function setDate($date) {
        $this->date = $date;
    }

    function setName($name) {
        $this->name = $name;
    }

    function setLastname($lastname) {
        $this->lastname = $lastname;
    }

    function setHours($hours) {
        $this->hours = $hours;
    }


// </editor-fold>
        
    // <editor-fold defaultstate="collapsed" desc="JsonSerializable">
    public function jsonSerialize() {
  
        return [ "summay" =>[
            'lastname' => $this->lastname,
            'name' => $this->name,
            'date' => $this->date,
            'hours' => $this->hours]
        ];
    }
// </editor-fold>

}

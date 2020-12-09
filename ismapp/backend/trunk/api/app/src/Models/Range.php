<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;

/**
 * Description of Range
 *
 * @author slavko
 */
class Range {

    /**
     * @var Day
     * 
     * 
     */      
    protected $from;  

    /**
     * @var Day
     * 
     * 
     */          
    protected $to;    

    /**
     *
     * @var \App\Models\Origin|NULL
     * 
     */    
    public $origin;
    /**
     *
     * @var \App\Models\Destination|NULL
     * 
     */
    public $destination;    
    public function getFrom(): Day {
        return $this->from;
    }

    public function getTo() :Day {
        return $this->to;
    }

    public function setFrom(Day $from) {
        $this->from = $from;
    }

    public function setTo(Day $to) {
        $this->to = $to;
    }
    
    /**
     * 
     * @return \App\Models\Origin|NULL
     */
    function getOrigin() {
        return $this->origin;
    }

    /**
     * 
     * @return \App\Models\Destination|NULL
     */
    function getDestination() {
        return $this->destination;
    }

    /**
     * 
     * @param \App\Models\Origin|NULL $origin
     */
    function setOrigin($origin) {
        $this->origin = $origin;
    }

    /**
     * 
     * @param \App\Models\Destination|NULL $destination
     */
    function setDestination($destination) {
        $this->destination = $destination;
    }
    
    public function getDuration() : int {
        return $this->from->getDate()->diff($this->to->getDate())->days;
        
    }
    
}

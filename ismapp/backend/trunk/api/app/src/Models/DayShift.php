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
 * @OGM\Node(label="Shift")
 */
class DayShift extends BaseShift implements \JsonSerializable{

     /**
     * @var Day
     * 
     * @OGM\Relationship(type="WORKDAY", direction="OUTGOING", collection=false, mappedBy="", targetEntity="Day")
     */  
    protected $workday;
    
    // <editor-fold defaultstate="collapsed" desc="getters /setters">
    function getWorkday(){
        return $this->workday;
    }

    /**
     * 
     * @param \App\Models\Day $workday
     */
    function setWorkday(Day $workday) {
        $this->workday = $workday;
    }
    
// </editor-fold>
        
    //<editor-fold desc="JsonSerializable implementation">   
    public function jsonSerialize() {
        
        if($this->workday === NULL){
            $this->getWorkday();
        }
        $ret = array_merge(
            parent::jsonSerialize(),
            [
                'workday' => $this->workday
            ]
        ); 
        return $ret;

    }
    //</editor-fold>    
}

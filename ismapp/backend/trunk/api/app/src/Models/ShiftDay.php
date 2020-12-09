<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;

/**
 * Description of ShiftDay
 *
 * @author slavko
 */
class ShiftDay implements \JsonSerializable {
    
    protected $shifttype;
    protected $hours;
    protected $strrep;

    // <editor-fold defaultstate="collapsed" desc="getters /setters">
    function getShifttype() {
        return $this->shifttype;
    }

    function getHours() {
        return $this->hours;
    }

    function getStrrep() {
        return $this->strrep;
    }

    function setShifttype($shifttype) {
        $this->shifttype = $shifttype;
    }

    function setHours($hours) {
        $this->hours = $hours;
    }

    function setStrrep($strrep) {
        $this->strrep = $strrep;
    }


// </editor-fold>
    
    //<editor-fold desc="JsonSerializable implementation">   
    public function jsonSerialize() {
        $ret = 
            [
                'strrep' => $this->strrep,
                'shifttype' => $this->shifttype,
                'hours' => $this->hours,
            ]
        ; 
        return $ret;

    }
    //</editor-fold>       
}

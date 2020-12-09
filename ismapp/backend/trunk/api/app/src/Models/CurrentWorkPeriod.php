<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;

/**
 * Description of CurrentWorkPeriod
 *
 * @author slavko
 */
class CurrentWorkPeriod extends BaseModel implements \JsonSerializable  {
    /**
     *
     * @var WorkPeriod
     */
    protected $currentPeriod;
    
    /**
     *
     * @var WorkPeriod
     */
    protected $lastPeriod;

    // <editor-fold defaultstate="collapsed" desc="get/set">

    public function getCurrentPeriod() {
        return $this->currentPeriod;
    }

    public function getLastPeriod() {
        return $this->lastPeriod;
    }

    public function setCurrentPeriod($currentPeriod) {
        $this->currentPeriod = $currentPeriod;
    }

    public function setLastPeriod($lastPeriod) {
        $this->lastPeriod = $lastPeriod;
    }

        // </editor-fold>
        
    // <editor-fold defaultstate="collapsed" desc="BaseModel">
    public function setUuid($uuid) {
        
    }

    public function getid() {
        
    }

    public function getUuid() {
        
    }

    public function import($object) {
        
    }

    public function tag(): string {
        
    }
    // </editor-fold>

    
    public function jsonSerialize() {
        $ret =  parent::jsonSerialize();
        
        $ret['current'] = $this->currentPeriod;
        $ret['last'] = $this->lastPeriod;
        
        
        return $ret;
    }
}

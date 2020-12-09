<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;

/**
 * Description of Origin
 *
 * @author slavko
 */
class Origin  extends BaseModel {

    /**
     *
     * @var string 
     * 
     */    
    public $departtype;

    // <editor-fold defaultstate="collapsed" desc="getters /setters ">
    function getDeparttype() {
        return $this->departtype;
    }

    function setDeparttype($departtype) {
        $this->departtype = $departtype;
    }

    
    // </editor-fold>

    public function NodeName() : string {
        switch ($this->departtype){
            case "COMPANY":
                return "Company";
            case "PROJECT":
                return "Project";
        }
    }
    // <editor-fold defaultstate="collapsed" desc="BaseModel ">
    public function setUuid($uuid) {
        $this->uuid = $uuid;
    }

    public function getid() {
        return $this->id;
    }

    public function getUuid() {
        return $this->uuid;
    }

    public function import($object) {
    }

    public function tag(): string {
        $reflect = new \ReflectionClass($this);
        return $reflect->getShortName()."_".$this->tagId();        
    }
    
    public function tagId(): string {
        return $this->tagId;
    }   
    
    public function getQueryFields() {
    }
    
    public function getQueryFieldsParams() :array {
    }    
    // </editor-fold>

}

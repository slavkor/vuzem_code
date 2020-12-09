<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;

class Hdr extends BaseModel implements \JsonSerializable {
    /**
     * @var Company
     * 
     * 
     */
    protected $company;

    // <editor-fold defaultstate="collapsed" desc="getter/setters">

    public function getCompany(): Company {
        return $this->company;
    }

    public function setCompany(Company $company) {
        $this->company = $company;
    }

        // </editor-fold>
        
    
    //<editor-fold desc="JsonSerializable implementation">
    public function jsonSerialize() {
        
        $ret = array_merge(parent::jsonSerialize(), ["company" => $this->company, 
            'constructionsite' => null, 
            'project' => null]);
        return["hdr" => $ret];
        
    }
    //</editor-fold>

    //<editor-fold desc="BaseModel implementation">
    public function getid() {
        return $this->id;
    }
    public function getUuid() {
        return $this->uuid;
    }
    public function setUuid($uuid) {
        $this->uuid = $uuid;
    }
    public function import($object) {
        throw \Exception('no props to import into the object!');
    }
    public function tag(): string {
        $reflect = new \ReflectionClass($this);
        return $reflect->getShortName()."_".$this->tagId();
    }

    public function tagId(): string {
        return $this->tagId;
    }    
    //</editor-fold>   

}

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
 * @OGM\Node(label="Company")
 */
class Company extends BaseCompany implements \JsonSerializable {

    
    public function setCompanyId($id){
        parent::setUuid($id);
    }
    //<editor-fold desc="JsonSerializable implementation">   
    public function jsonSerialize() {
        
        $ret = array_merge(
            parent::jsonSerialize(),
            [
                'name' => $this->name,
                'shortname' => $this->shortname,
                'taxnumber' => $this->taxnumber,
                'regnumber' => $this->regnumber
            ]
        ); 
        return $ret;
    }
    //</editor-fold>       
}

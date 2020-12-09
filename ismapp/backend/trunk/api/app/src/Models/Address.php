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
 * @OGM\Node(label="Address")
 */
class Address extends BaseAddress{
    
    public function __construct() {
        parent::__construct();
    }
    
    public function getUuid() {
        return parent::getUuid();
    }

    public function setUuid($uuid) {
        parent::setUuid($uuid);
    }
    
    public function setDeleted($deleted) {
        parent::setdeleted($deleted);
    }
}

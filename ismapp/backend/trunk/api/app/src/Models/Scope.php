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
 * @OGM\Node(label="Scope")
 */
class Scope {

    /**
     * @OGM\GraphId()
     *
     * @var int
     */
    protected $id;
    
    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $name;
    
    public function getname() {
        return $this->name;
    }
    public function setname($name) {
        $this->name=$name;
    }
    
}

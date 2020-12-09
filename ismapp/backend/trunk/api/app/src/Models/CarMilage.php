<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;

use GraphAware\Neo4j\OGM\Annotations as OGM;
use App\Models\CarMilageRelationship;


/**
 *
 * @OGM\Node(label="Car")
 */
class CarMilage extends Car {
    
    
    
    
    /**
     * @var Collection
     * 
     * @OGM\Relationship(relationshipEntity="CarMilageRelationship", type="MILAGE", direction="OUTGOING", collection=true, mappedBy="car")
     */
    protected $milages;   
}

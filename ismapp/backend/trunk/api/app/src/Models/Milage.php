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
 * @OGM\Node(label="Milage")
 */
class Milage extends BaseModel implements \JsonSerializable {
    
    /**
     * @var int
     * 
     * @OGM\Property(type="int")
     */
    protected $milage;

    /**
     * @var \DateTime
     *
     * @OGM\Property()
     * @OGM\Convert(type="datetime", options={"format":"long_timestamp"})
     */
    protected $date;

    /**
     * @var CarMilageRelationship
     * 
     * @OGM\Relationship(relationshipEntity="CarMilageRelationship", type="MILAGE", direction="INCOMING", collection=false, mappedBy="milage")
     */
    protected $car;
    
    
    /**
     * 
     * @return int
     */
    public function getMilage() {
        return $this->milage;
    }

    /**
     * 
     * @param int $milage
     */
    public function setMilage($milage) {
        $this->milage = $milage;
    }

    
    public function getUuid() {
        
    }

    public function getid() {
        
    }

    public function import($object) {
        
    }

    public function setUuid($uuid) {
        
    }

    public function tag(): string {
        
    }

}

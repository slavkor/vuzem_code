<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;

use GraphAware\Neo4j\OGM\Annotations as OGM;
use \GraphAware\Neo4j\OGM\Common\Collection;


/**
 *
 * @OGM\Node(label="Employee")
 */
class EmployeeAdd extends BaseEmployee implements \JsonSerializable {
    //put your code here
    public function __construct() {
        parent::__construct();
        $this->occupancies = new Collection();
    }
    
    /**
     * @OGM\Relationship(relationshipEntity="EmployeeOccupancyRelationship", type="OCCUPIES", collection = true, direction="OUTGOING", mappedBy="occupancy")
     *
     * @var Collection
     */
    protected $occupancies;

    /**
     * 
     * @param Occupancy $occupancy
     * @param int $type
     */
    public function startOccupancy($occupancy, $type){
        $empOccupancy = new EmployeeOccupancyRelationship($this, $occupancy, $type);
        $this->occupancies->add($empOccupancy);
    }
    

    /**
     * 
     * @return Collection
     */
    function getOccupancies(){
        return $this->occupancies;
    }
    
    
    public function jsonSerialize() {
        $ret = parent::jsonSerialize();

        $ret['occupancies'] = $this->occupancies->toArray();

        return $ret;
    }

}

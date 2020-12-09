<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;


use GraphAware\Neo4j\OGM\Annotations as OGM;
use \GraphAware\Neo4j\OGM\Common\Collection;
use App\Models\ProjectWorkPeriod;
use App\Models\ProjectWorkPeriodPlanRelation;
use App\Models\CSite;
use App\Models\CSiteOverview;

/**
 *
 * @OGM\Node(label="Project")
 */
class ProjectOverview extends BaseProject implements \JsonSerializable{
    public function __construct() {
        parent::__construct();
        $this->occupancies = new Collection();
    }
    
    /**
     * @var OccupancyOverview[]|Collection
     * 
     * @OGM\Relationship(type="HAS", direction="OUTGOING", collection=true, mappedBy="", targetEntity="OccupancyOverview")
     */   
    protected $occupancies;


    /**
     * 
     * @return OccupancyOverview[]|Collection
     */
    public function getOccupancies() {
        return $this->occupancies;
    }

    /**
     * 
     * @param OccupancyOverview[]|Collection $occupancies
     */
    public function setOccupancies($occupancies) {
        $this->occupancies = $occupancies;
    }

        
    //<editor-fold desc="JsonSerializable implementation">    
    public function jsonSerialize() {
        $ret = parent::jsonSerialize();
        return array_merge($ret,['occupancies' => $this->getOccupancies()->toArray()]);
        
    }
    //</editor-fold>     
    
    
}

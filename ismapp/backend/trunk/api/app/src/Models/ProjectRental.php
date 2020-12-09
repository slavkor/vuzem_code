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


/**
 * Description of ProjectRental
 *
 * @author slavko.rihtaric
 */
class ProjectRental extends BaseProject implements \JsonSerializable {

    public function __construct() {
        parent::__construct();
        $this->rentals = new Collection();
    }
    
    /**
     * @var Rental[]|Collection
     * 
     * @OGM\Relationship(type="RENT", direction="OUTGOING", collection=true, mappedBy="", targetEntity="Rental")
     */    
    protected  $rentals;

    /**
     * @var CSite
     * 
     * @OGM\Relationship(type="HAS", direction="INCOMING", collection=false, mappedBy="", targetEntity="CSite")
     */        
    protected $site;  
    
  
    /**
     * 
     * @return CSite
     */
    function getSite(){
        return $this->site;
    }

    /**
     * 
     * @param CSite $site
     */
    function setSite($site) {
        $this->site = $site;
    }

    public function jsonSerialize() {
        
    }

}

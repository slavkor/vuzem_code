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
class EmployeeOverview extends BaseEmployee implements \JsonSerializable {
    public function __construct() {
        parent::__construct();
        $this->departures = new Collection();
    }

    /**
     *
     * @var Departure[]|Collection
     */
    protected $departures;        
    
    /**
     * 
     * @return Departure[]|Collection
     */
    public function getDepartures(){
        return $this->departures;
    }

    /**
     * 
     * @param Departure[]|Collection $departures
     */
    public function setDepartures($departures) {
        $this->departures = $departures;
    }

      
    
    public function jsonSerialize() {
        $ret = parent::jsonSerialize();
       
        
        return array_merge($ret,["departures" =>  $this->departures] );
        
        //return $ret;
    }
}

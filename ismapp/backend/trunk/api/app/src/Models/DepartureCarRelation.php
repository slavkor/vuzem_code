<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;


use GraphAware\Neo4j\OGM\Annotations as OGM;
use App\Models\DepartureEx;
use App\Models\Car;

/**
 *
 * @OGM\RelationshipEntity(type="TRANSPORT")
 */
class DepartureCarRelation implements \JsonSerializable{
    
    public function __construct(Car $car, DepartureEx $departure) {
        $this->car = $car;
        $this->departure = $departure;
        
    }
    
    /**
     * @var int
     *
     * @OGM\GraphId()
     */
    protected $id;

    /**
     * @var Car
     *
     * @OGM\StartNode(targetEntity="Car")
     */
    protected $car;

    /**
     * @var DepartureEx
     *
     * @OGM\EndNode(targetEntity="DepartureEx")
     */
    protected $departure;

    /**
     * 
     * @return Car
     */
    function getCar(){
        return $this->car;
    }    
    /**
     * 
     * @return DepartureEx
     */
    function getDeparture() {
        return $this->departure;
    }


    public function jsonSerialize() {

        return ["car" => $this->car];
    }
}

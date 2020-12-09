<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;

use GraphAware\Neo4j\OGM\Annotations as OGM;
use App\Models\CarMilage;
use App\Models\Milage;


/**
 *
 * @OGM\RelationshipEntity(type="RATED")
 */
class CarMilageRelationship {
    /**
     * @var int
     *
     * @OGM\GraphId()
     */
    protected $id;

    /**
     * @var CarMilage
     *
     * @OGM\StartNode(targetEntity="Car")
     */
    protected $car;

    /**
     * @var Milage
     *
     * @OGM\EndNode(targetEntity="Milage")
     */
    protected $milage;

    /**
     * @var \DateTime
     *
     * @OGM\Property()
     * @OGM\Convert(type="datetime", options={"format":"long_timestamp"})
     */
    protected $date;
    

    public function __construct(CarMilage $car, Milage $milage, $score)
    {
        $this->car = $car;
        $this->milage = $milage;
    }
    
    public function getId() {
        return $this->id;
    }

    /**
     * 
     * @return CarMilage
     */
    public function getCar(){
        return $this->car;
    }

    /**
     * 
     * @return Milage
     */
    public function getMilage() {
        return $this->milage;
    }

    /**
     * 
     * @return \DateTime
     */
    public function getDate(){
        return $this->date;
    }

    public function setId($id) {
        $this->id = $id;
    }

    public function setCar(CarMilage $car) {
        $this->car = $car;
    }

    public function setMilage(Milage $milage) {
        $this->milage = $milage;
    }

    public function setDate(\DateTime $date) {
        $this->date = $date;
    }


}

<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;

use GraphAware\Neo4j\OGM\Annotations as OGM;
use App\Models\Project;
use App\Models\Occupancy;

/**
 *
 * @OGM\RelationshipEntity(type="OCCUPIES")
 */
class ProjectOccupancyRelation implements \JsonSerializable{

    /**
     *
     * @var string 
     */
    protected $type;

    /**
     * @var int
     *
     * @OGM\GraphId()
     */
    protected $id;

    /**
     * @var Occupancy
     *
     * @OGM\StartNode(targetEntity="Occupancy")
     */
    protected $occupancy;
    
    /**
     * @var Project
     *
     * @OGM\EndNode(targetEntity="Project")
     */
    protected $project;    

    
    public function __construct()
    {
        $this->type = "PROJECT";
    }

    /**
     * 
     * @return Occupancy
     */
    function getOccupancy(){
        return $this->occupancy;
    }
    /**
     * 
     * @return Project
     */
    function getProject() {
        return $this->project;
    }
    /**
     * 
     * @param Project $project
     */
    function setProject($project) {
        $this->project = $project;
    }
    
    function getType() {
        return "PROJECT";
    }
    
    public function jsonSerialize() {
            return $this->project;
    }
}

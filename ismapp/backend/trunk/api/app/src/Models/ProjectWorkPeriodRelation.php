<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;

use GraphAware\Neo4j\OGM\Annotations as OGM;
use App\Models\ProjectWorkPeriod;
use App\Models\Project;

/**
 *
 * @OGM\RelationshipEntity(type="WORK")
 */
class ProjectWorkPeriodRelation implements \JsonSerializable{

    /**
     * @var int
     *
     * @OGM\GraphId()
     */
    protected $id;

    /**
     * @var ProjectWorkPeriod
     *
     * @OGM\StartNode(targetEntity="ProjectWorkPeriod")
     */
    protected $workperiod;
    
    /**
     * @var Project
     *
     * @OGM\EndNode(targetEntity="Project")
     */
    protected $project;    

    
    public function __construct(ProjectWorkPeriod $workperiod, Project $project)
    {
        $this->workperiod = $workperiod;
        $this->project = $project;
        
    }

  
    /**
     * 
     * @return ProjectWorkPeriod
     */
    public function getWorkperiod(){
        return $this->workperiod;
    }

    /**
     * 
     * @param ProjectWorkPeriod $workperiod
     */
    public function setWorkperiod($workperiod) {
        $this->workperiod = $workperiod;
    }

    /**
     * 
     * @return Project
     */
    public function getProject(){
        return $this->project;
    }

    /**
     * 
     * @param Project $project
     */
    public function setProject($project) {
        $this->project = $project;
    }

        
    public function jsonSerialize() {
            return $this->workperiod;
    }
}


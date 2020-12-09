<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;


use GraphAware\Neo4j\OGM\Annotations as OGM;
use App\Models\ProjectWorkPeriod;
use App\Models\WorkPlace;

/**
 *
 * @OGM\RelationshipEntity(type="PLAN")
 */
class ProjectWorkPeriodPlanRelation implements \JsonSerializable{

    /**
     * @var int
     *
     * @OGM\GraphId()
     */
    protected $id;
    
    
    /**
     *
     * @var int
     *
     * @OGM\Property(type="int")
     */
    protected $plan;


    /**
     * @var ProjectWorkPeriod
     *
     * @OGM\StartNode(targetEntity="ProjectWorkPeriod")
     */
    protected $workperiod;
    
    /**
     * @var WorkPlacePlan
     *
     * @OGM\EndNode(targetEntity="WorkPlace")
     */
    protected $workplace;    

    
    public function __construct(ProjectWorkPeriod $workperiod, WorkPlacePlan $workplace, $plan)
    {
        $this->workperiod = $workperiod;
        $this->workplace = $workplace;
        $this->plan = $plan;
    }

    function getPlan() {
        return $this->plan;
    }

    /**
     * 
     * @return ProjectWorkPeriod
     */
    function getWorkperiod(){
        return $this->workperiod;
    }

    /**
     * 
     * @return WorkPlacePlan
     */
    function getWorkplace(){
        return $this->workplace;
    }

    function setPlan($plan) {
        $this->plan = $plan;
    }

    /**
     * 
     * @param ProjectWorkPeriod $workperiod
     */
    function setWorkperiod($workperiod) {
        $this->workperiod = $workperiod;
    }
    
    /**
     * 
     * @param WorkPlacePlan $workplace
     */
    function setWorkplace($workplace) {
        $this->workplace = $workplace;
    }
    
    public function jsonSerialize() {
            //return ["workplan" =>  ["workplace" => $this->workplace, "plan" => $this->plan]];
            return ["workplace" => $this->workplace, "plan" => $this->plan];
    }
}

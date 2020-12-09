<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;
use \GraphAware\Neo4j\OGM\Common\Collection;
use GraphAware\Neo4j\OGM\Annotations as OGM;


/**
 *
 * @OGM\Node(label="WorkPlace")
 */
class WorkPlacePlan extends BaseModel {
    
    public function __construct() {
        parent::__construct();
        $this->plans = new Collection();
    }
    
    protected $plan;

    /**
     *
     * @var WorkPlace
     */
    protected $workplace;
    
    function getPlan() {
        return $this->plan;
    }
    /**
     * 
     * @return \App\Models\WorkPlace
     */
    function getWorkplace(){
        return $this->workplace;
    }

    function setPlan($plan) {
        $this->plan = $plan;
    }

    /**
     * 
     * @param \App\Models\WorkPlace $workplace
     */
    function setWorkplace($workplace) {
        $this->workplace = $workplace;
    }

    public function getUuid() {
        return $this->uuid;
    }

    public function getid() {
        return $this->id;
    }

    public function import($object) {
        
    }

    public function setUuid($uuid) {
        $this->uuid = $uuid;
    }

    public function tag(): string {
        
    }

}

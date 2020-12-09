<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
namespace App\Models;
use GraphAware\Neo4j\OGM\Annotations as OGM;
use App\Models\WorkPlacePlan;

/**
 *
 * @OGM\Node(label="ProjectWorkPeriod")
 */
class ProjectWorkPeriod extends BaseModel implements \JsonSerializable {
    
    //<editor-fold desc="properties">   
    
    /**
     * end
     * @var string
     * 
     * @OGM\Property(type="string")
     */    
    public $description;
    
    /**
     * @var Day
     * 
     * @OGM\Relationship(type="START", direction="OUTGOING", collection=false, mappedBy="", targetEntity="Day")
     */  
    protected $startat;

    /**
     * @var Day
     * 
     * @OGM\Relationship(type="END", direction="OUTGOING", collection=false, mappedBy="", targetEntity="Day")
     */  
    protected $endat;

    /**
     * start
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    public $start;

    /**
     * end
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    public $end;

    /**
     * @var WorkPlacePlan[]
     * 
     * @OGM\Relationship(relationshipEntity="ProjectWorkPeriodPlanRelation", type="PLAN", direction="OUTGOING", collection=true, mappedBy="workperiod")
     */
    protected $workplans;  
    
    //</editor-fold>    

    //<editor-fold desc="setters/getters">  
    
    /**
     * 
     * @return \App\Models\Day
     */
    public function getStartat() {
        return $this->startat;
    }

    /**
     * 
     * @return \App\Models\Day
     */
    public function getEndat(){
        return $this->endat;
    }

    public function getStart() {
        return $this->start;
    }

    public function getEnd() {
        return $this->end;
    }

    /**
     * 
     * @param \App\Models\Day $startat
     */
    public function setStartat($startat) {
        $this->startat = $startat;
    }

    /**
     * 
     * @param \App\Models\Day $endat
     */
    public function setEndat($endat) {
        $this->endat = $endat;
    }

    public function setStart($start) {
        $this->start = $start;
    }

    public function setEnd($end) {
        $this->end = $end;
    }

    function getDescription() {
        return $this->description;
    }

    function setDescription($description) {
        $this->description = $description;
    }

        /**
     * 
     * @return WorkPlacePlan[]
     */
    public function getWorkPlans(){
        return $this->workplans;
    }

    /**
     * 
     * @param WorkPlacePlan[] $plans
     */
    function setWorkPlans($plans) {
        $this->workplans = $plans;
    }

    //</editor-fold>    

    //<editor-fold desc="JsonSerializable implementation">    
    public function jsonSerialize() {
            
        $ret = parent::jsonSerialize();

        $this->getWorkPlans();       

        $ret["start"] = $this->start;
        $ret["end"] = $this->end;
        $ret["description"] = $this->description;
        if(null !==$this->workplans) $ret['workplans'] = $this->workplans->toArray();

        return $ret;
    }
    //</editor-fold>    

    //<editor-fold desc="BaseModel implementation">    
    public function getid() {
        return $this->id;
    }
    public function getUuid() {
        return $this->uuid;
    }
    public function setUuid($uuid) {
        $this->uuid = $uuid;
    }
    public function import($object) {
        $vars=is_object($object)?get_object_vars($object):$object;

        if (!is_array($vars)) {
            throw \Exception('no props to import into the object!');
        }
        foreach ($vars as $key => $value) {
            switch ($key) {
                case 'start':
                case 'end':
                    $this->$key = $value;
                    break;

                default:
                    break;
            }
        }   
    }  
    public function tag(): string {
        $reflect = new \ReflectionClass($this);
        return $reflect->getShortName()."_".$this->tagId();
    }

    public function tagId(): string {
        return $this->tagId;
    }    
    //</editor-fold>   

    /**
     * 
     * @param WorkPlacePlan|NULL $workplace
     * @param type $plan
     */
    public function addPlan($workplace, $plan){
        $relation= new ProjectWorkPeriodPlanRelation($this, $workplace, $plan);
        //$workplace->getPlans()->add($relation);
        $this->getWorkPlans()->add($relation);
    }
}

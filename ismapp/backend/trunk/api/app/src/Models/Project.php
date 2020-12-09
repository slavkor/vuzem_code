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
 *
 * @OGM\Node(label="Project")
 */
class Project extends BaseProject implements \JsonSerializable{

    /**
     * @var Day
     * 
     * @OGM\Relationship(type="START", direction="OUTGOING", collection=false, mappedBy="", targetEntity="Day")
     */        
    protected $start;

    /**
     * @var Address
     * 
     * @OGM\Relationship(type="LIVES", direction="OUTGOING", collection=false, mappedBy="", targetEntity="Address")
     */        
    protected $address;    
    
    /**
     * @var Day
     * 
     * @OGM\Relationship(type="END", direction="OUTGOING", collection=false, mappedBy="", targetEntity="Day")
     */        
    protected $end;  

    /**
     * @var CSite
     * 
     * @OGM\Relationship(type="HAS", direction="INCOMING", collection=false, mappedBy="", targetEntity="CSite")
     */        
    protected $site;  
    
    /**
     * @var ProjectWorkPeriod[]
     * 
     * @OGM\Relationship(relationshipEntity="ProjectWorkPeriodRelation", type="WORK", direction="INCOMING", collection=true, mappedBy="workperiod")
     */
    protected $workperiods;      
    
    public function getStart() {
        return $this->start;
    }

    public function getEnd() {
        return $this->end;
    }

    public function setStart(Day $start) {
        $this->start = $start;
    }

    public function setEnd(Day $end) {
        $this->end = $end;
    }
    
    function getAddress() {
        return $this->address;
    }

    function setAddress($address) {
        $this->address = $address;
    }

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

    /**
     * 
     * @return ProjectWorkPeriod[]
     */
    function getWorkperiods(){
        return $this->workperiods;
    }

    /**
     * 
     * @param ProjectWorkPeriod[] $workperiods
     */
    function setWorkperiods($workperiods) {
        $this->workperiods = $workperiods;
    }

        
    public function addWorkPeriod(ProjectWorkPeriod $workperiod){
        $relation = new ProjectWorkPeriodRelation($workperiod, $this);
        $this->getWorkperiods()->add($relation);
    }
    
    //<editor-fold desc="JsonSerializable implementation">    
    public function jsonSerialize() {
        if(null === $this->start)
            $this->getStart();
        if(null === $this->end)
            $this->getEnd ();
        if(null === $this->address)
            $this->getAddress ();
        if(null === $this->site)
            $this->getSite ();
        
        $this->getWorkperiods();
        
        $ret = parent::jsonSerialize();

        if(NULL !== $this->start){
            $ret['start'] = $this->start;
        } else {
            $ret['start'] = NULL;            
        }
        
        if(NULL !== $this->end){
            $ret['end'] = $this->end;
        } else {
            $ret['end'] = NULL;            
        }    
        
        if(NULL !== $this->address){
            $ret['address'] = $this->address;
        } else {
            $ret['address'] = NULL;            
        }       
        
        if(NULL !== $this->site){
            $ret['site'] = $this->site;
        } else {
            $ret['site'] = NULL;            
        }  
   
        $ret["workperiods"] = $this->workperiods->toArray();
        
        return $ret;
        
    }
    //</editor-fold>     
    
    public function getQueryFields() {
        $tag = $this->tag();
        $vars = get_object_vars($this);
       
        foreach ($vars as $key => $value) {
            switch ($key) {
                case 'id':
                case 'tagId':
                case 'start':
                case 'end':
                case 'site':
                case 'workperiods':
                    break;
                default:
                    $fields .= $key.":{".$tag."_".$key."},";                    
                    break;
            }            
        }
        return substr($fields, 0, strlen($fields)-1);
    }
    
    public function getQueryFieldsParams(): array{
        $tag = $this->tag();
        $vars = get_object_vars($this);
        $params = array();
        foreach ($vars as $key => $value) {
            switch ($key) {
                case 'id':
                case 'tagId':
                case 'start':
                case 'end':
                case 'site':
                case 'workperiods':
                    break;
                default:
                    $params[$tag."_".$key] = $value;
                    break;
            }            
        }
        return $params;
    }
    
    public function import($object) {
        
        parent::import($object);
        
        $vars=is_object($object)?get_object_vars($object):$object;

        if (!is_array($vars)) {
            throw \Exception('no props to import into the object!');
        }
        foreach ($vars as $key => $value) {
            switch ($key) {
                case 'name':
                case 'description':
                case 'status':
                    $this->$key = $value;
                    break;
                default:
                    break;
            }
        }   
    }    
}

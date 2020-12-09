<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;

namespace App\Models;
use GraphAware\Neo4j\OGM\Annotations as OGM;

/**
 *
 * @OGM\Node(label="WorkPeriod")
 */
class WorkPeriod extends BaseModel implements \JsonSerializable {


    //<editor-fold desc="properties">   
    
    /**
     * @var Day
     * 
     * @OGM\Relationship(type="START_AT", direction="OUTGOING", collection=false, mappedBy="", targetEntity="Day")
     */  
    protected $startat;

    /**
     * @var Day
     * 
     * @OGM\Relationship(type="END_AT", direction="OUTGOING", collection=false, mappedBy="", targetEntity="Day")
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
     * @var Company
     * 
     * @OGM\Relationship(type="WORKS_IN", direction="OUTGOING", collection=false, mappedBy="", targetEntity="Company")
     */  
    public $company;
    
    //</editor-fold>    

    //<editor-fold desc="setters/getters">  
    
    public function getStartat(): Day {
        return $this->startat;
    }

    public function getEndat(): Day {
        return $this->endat;
    }

    public function getStart() {
        return $this->start;
    }

    public function getEnd() {
        return $this->end;
    }

    public function setStartat(Day $startat) {
        $this->startat = $startat;
    }

    public function setEndat(Day $endat) {
        $this->endat = $endat;
    }

    public function setStart($start) {
        $this->start = $start;
    }

    public function setEnd($end) {
        $this->end = $end;
    }

    public function getStartDate() : \DateTime{
        $date = new \DateTime();
        $date->setTimestamp(strtotime($this->start));
        return $date;
    }
    public function getEndDate() : \DateTime{
        $date = new \DateTime();
        $date->setTimestamp(strtotime($this->end));
        return $date;
    }

    public function getCompany() {
        return $this->company;
    }

    public function setCompany(Company $company) {
        $this->company = $company;
    }

        //</editor-fold>    

    //<editor-fold desc="JsonSerializable implementation">    
    public function jsonSerialize() {
        
        if (null === $this->company) {
            $this->getCompany();
        }

        $ret = parent::jsonSerialize();

        $ret["start"] = $this->start;
        $ret["end"] = $this->end;

        if(NULL !== $this->startat){
            $ret['startat'] = $this->startat;
        } else {
            $ret['startat'] = NULL;            
        }

        if(NULL !== $this->endat){
            $ret['endat'] = $this->endat;
        } else {
            $ret['endat'] = NULL;            
        }
        
        if(NULL !== $this->company){
            $ret['company'] = $this->company;
        } else {
            $ret['company'] = NULL;            
        }
        
        
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
}

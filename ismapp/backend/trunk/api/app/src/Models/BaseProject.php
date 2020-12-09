<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;
use GraphAware\Neo4j\OGM\Annotations as OGM;

/**
 *
 * @OGM\Node(label="Project")
 */
class BaseProject extends BaseModel implements \JsonSerializable {

    //<editor-fold desc="protected fields">
    /**
     * name
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $name;

    /**
     * description
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $description;   

    /**
     * description
     * @var string
     * 
     * @OGM\Property(type="string")
     */    
    protected $state;

    /**
     * externalnumber
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $externalnumber;

    /**
     * projectnumber
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $projectnumber;    

    /**
     * estimatedhours
     * @var int
     * 
     * @OGM\Property(type="int")
     */
    protected $estimatedhours;  

    /**
     * estimatedvalue
     * @var float
     * 
     * @OGM\Property(type="float")
     */
    protected $estimatedvalue;      

    /**
     * estimatedworkers
     * @var int
     * 
     * @OGM\Property(type="int")
     */
    protected $estimatedworkers;    
    
    //</editor-fold>    

    //<editor-fold desc="getters">    
    public function getName() {
        return $this->name;
    }

    public function getDescription() {
        return $this->description;
    }

    public function getState() {
        return $this->state;
    }
    public function setName($name) {
        $this->name = $name;
    }

    public function setDescription($description) {
        $this->description = $description;
    }

    public function setState($state) {
        $this->state = $state;
    }
    public function getExternalnumber() {
        return $this->externalnumber;
    }

    public function getProjectnumber() {
        return $this->projectnumber;
    }

    public function setExternalnumber($externalnumber) {
        $this->externalnumber = $externalnumber;
    }

    public function setProjectnumber($projectnumber) {
        $this->projectnumber = $projectnumber;
    }
    
    public function setProjectId($id){
        $this->setUuid($id);
    }
    
    function getEstimatedhours() {
        return $this->estimatedhours;
    }

    function getEstimatedvalue() {
        return $this->estimatedvalue;
    }

    function getEstimatedworkers() {
        return $this->estimatedworkers;
    }

    function setEstimatedhours($estimatedhours) {
        $this->estimatedhours = $estimatedhours;
    }

    function setEstimatedvalue($estimatedvalue) {
        $this->estimatedvalue = $estimatedvalue;
    }

    function setEstimatedworkers($estimatedworkers) {
        $this->estimatedworkers = $estimatedworkers;
    }

        //</editor-fold>    

    //<editor-fold desc="JsonSerializable implementation">    
    public function jsonSerialize() {
        $ret = parent::jsonSerialize();
        $ret['name'] = $this->name;
        $ret['description'] = $this->description;
        $ret['state'] = $this->state;
        $ret['projectnumber'] = $this->projectnumber;
        $ret['externalnumber'] = $this->externalnumber;
        $ret['estimatedhours'] = $this->estimatedhours;
        $ret['estimatedvalue'] = $this->estimatedvalue;
        $ret['estimatedworkers'] = $this->estimatedworkers;
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
                case 'name':
                case 'description':
                case 'state':
                case 'projectnumber':
                case 'externalnumber':
                case 'estimatedhours':
                case 'estimatedvalue':
                case 'estimatedworkers':
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

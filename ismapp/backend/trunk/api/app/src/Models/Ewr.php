<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;
use GraphAware\Neo4j\OGM\Annotations as OGM;
use GraphAware\Neo4j\OGM\Common\Collection;


use App\Models\Employee;

/**
 *
 * @OGM\Node(label="Ewr")
 */
class Ewr extends BaseModel implements \JsonSerializable {

    public function __construct() {
        parent::__construct();
//        $this->employees = new Collection();
    }
    //<editor-fold desc="protected fields">    
    
 
    /**
     * name
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $number;

    /**
     * description
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $description;   


    /**
     * externalnumber
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $externalnumber;



    /**
     * description
     * @var string
     * 
     * @OGM\Property(type="string")
     */    
    protected $state;

    /**
     * hours
     * @var int
     * 
     * @OGM\Property(type="int")
     */    
    protected $hours;    

    /**
     * hours
     * @var float
     * 
     * @OGM\Property(type="float")
     */    
    protected $materialcosts;    
    

    /**
     * @var Project
     * 
     * @OGM\Relationship(type="EWR", direction="OUTGOING", collection=false, mappedBy="", targetEntity="Project")
     */    
    protected $project; 
    
    /**
     * @var Employee
     * 
     * @OGM\Relationship(type="SITEMANAGER", direction="INCOMING", collection=false, mappedBy="", targetEntity="Employee")
     */    
    protected $sitemanager;  
    /**
     * @var Contact
     * 
     * @OGM\Relationship(type="EXSITEMANAGER", direction="INCOMING", collection=false, mappedBy="", targetEntity="Contact")
     */    
    protected $exsitemanager;  
    
//    /**
//     *
//     * @var Employee[]
//     * 
//     * @OGM\Relationship(type="WORKS", direction="INCOMING", collection=true, mappedBy="", targetEntity="Employee")
//     */   
//    public $employees;    

    /**
     * @var EwrWorker[]
     * 
     * @OGM\Relationship(relationshipEntity="EwrWorker", type="WORKS", direction="INCOMING", collection=true, mappedBy="ewr")
     */
    protected $workers;
    
     /**
     * @var string 
     * 
     * @OGM\Property(type="string")
     */    
    public $date;    
    
    //</editor-fold>    

    //<editor-fold desc="getters">    

    function getNumber() {
        return $this->number;
    }

    function getDescription() {
        return $this->description;
    }

    function getExternalnumber() {
        return $this->externalnumber;
    }

    function getState() {
        return $this->state;
    }

    function getHours() {
        return $this->hours;
    }

    function getMaterialcosts() {
        return $this->materialcosts;
    }

    /**
     * 
     * @return Project
     */
    function getProject() {
        return $this->project;
    }

    function setNumber($number) {
        $this->number = $number;
    }

    function setDescription($description) {
        $this->description = $description;
    }

    function setExternalnumber($externalnumber) {
        $this->externalnumber = $externalnumber;
    }

    function setState($state) {
        $this->state = $state;
    }

    function setHours($hours) {
        $this->hours = $hours;
    }

    function setMaterialcosts($materialcosts) {
        $this->materialcosts = $materialcosts;
    }
    
    /**
     * @return EwrWorker[]
     */
    function getWorkers(){
        return $this->workers;
    }
    
    /**
     * 
     * @param Project $project
     */
    function setProject($project) {
        $this->project = $project;
    }

    /**
     * 
     * @return Employee
     */
    function getSitemanager() {
        return $this->sitemanager;
    }
    /**
     * 
     * @return Contact
     */
    function getExsitemanager()  {
        return $this->exsitemanager;
    }
    /**
     * 
     * @param Employee $sitemanager
     */
    function setSitemanager($sitemanager) {
        $this->sitemanager = $sitemanager;
    }

    /**
     * 
     * @param Contact $exsitemanager
     */
    function setExsitemanager($exsitemanager) {
        $this->exsitemanager = $exsitemanager;
    }
    
//    /**
//     * @return Employee[]
//     */
//    function getEmployees() {
//        return $this->employees;
//    }
    
    function getDate() {
        return $this->date;
    }

    function setDate($date) {
        $this->date = $date;
    }

    
    //</editor-fold>    
    
    public function addWorker(ErwEmployee $employee, $workplace){
        
        $worker = new EwrWorker($employee, $this, $workplace);
        $employee->getEwrs()->add($worker);
        $this->getWorkers()->add($worker);
    }
    
    
    //<editor-fold desc="JsonSerializable implementation">    
    public function jsonSerialize() {
        $ret = parent::jsonSerialize();
        
        if(null === $this->project){
            $this->getProject();
        }
        if(null === $this->sitemanager){
            $this->getSitemanager();
        }
        if(null === $this->exsitemanager){
            $this->getExsitemanager();
        }
//        if(null === $this->employees){
//            $this->getEmployees();
//        } 
        
        if(null === $this->workers){
            $this->getWorkers();
        }         
        $ret['number'] = $this->number;
        $ret['externalnumber'] = $this->externalnumber;
        $ret['description'] = $this->description;
        $ret['state'] = $this->state;
        $ret['hours'] = $this->hours;
        $ret['materialcosts'] = $this->materialcosts;
        $ret['project'] = $this->project;
        $ret['sitemanager'] = $this->sitemanager;
        $ret['exsitemanager'] = $this->exsitemanager;
        //$ret['employees'] = $this->employees->toArray();
        $ret['date'] = $this->date;
        if(null !== $this->workers){
        $ret['workers'] = $this->workers->toArray();
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
    public function getQueryFields() {
        $tag = $this->tag();
        $vars = get_object_vars($this);
       
        foreach ($vars as $key => $value) {
            switch ($key) {
                case 'id':
                case 'tagId':
                case 'project':                    
                case 'sitemanager':                    
                case 'exsitemanager':                    
                case 'workers':   
                    break;
                default:
                    $fields .= $key.":{".$tag."_".$key."},";                    
                    break;
            }            
        }
        return substr($fields, 0, strlen($fields)-1);
    }
    
    public function getQueryFieldsParams() :array {
        $tag = $this->tag();
        $vars = get_object_vars($this);
        $params = array();
        foreach ($vars as $key => $value) {
            switch ($key) {
                case 'id':
                case 'tagId':
                case 'project':
                case 'sitemanager':
                case 'exsitemanager':
                case 'work':
                    break;
                default:
                    $params[$tag."_".$key] = $value;
                    break;
            }            
        }
        return $params;
    }     
    public function import($object) {
        $vars=is_object($object)?get_object_vars($object):$object;

        if (!is_array($vars)) {
            throw \Exception('no props to import into the object!');
        }
 
        foreach ($vars as $key => $value) {
            switch ($key) {
                case 'number':
                case 'externalnumber':
                case 'description':
                case 'state':
                case 'hours':
                case 'materialcosts':
                case 'date':
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

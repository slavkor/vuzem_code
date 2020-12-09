<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;

use GraphAware\Neo4j\OGM\Annotations as OGM;
use GraphAware\Neo4j\OGM\Common\Collection;
use App\Models\Day;
/**
 *
 * @OGM\Node(label="Occupancy")
 */
class Occupancy extends BaseModel implements \JsonSerializable {

    const ON_SITE = 0;
    const HOME = 1;
    const SICK = 2;
    const VACATION = 3;
    
    
    /**
     * @OGM\Property(type="int")
     *
     * @var int
     */    
    public $type;
    
    /**
     * @var Day
     * 
     * @OGM\Relationship(type="START", direction="OUTGOING", collection=false, mappedBy="", targetEntity="Day")
     */        
    public $start;  

    /**
     * @var Day
     * 
     * @OGM\Relationship(type="END", direction="OUTGOING", collection=false, mappedBy="", targetEntity="Day")
     */        
    public $end;
    
    /**
     * @var Company
     * 
     * @OGM\Relationship(type="OCCUPIES", direction="OUTGOING", collection=false, targetEntity="Company")
     */
    protected $company;  

    /**
     * @var Project
     * 
     * @OGM\Relationship(type="OCCUPIES", direction="OUTGOING", collection=false, targetEntity="Project")
     */
    protected $project;

    /**
     * @OGM\Property(type="string")
     *
     * @var string
     */    
    public $description;
            
    /**
     * @OGM\Property(type="string")
     *
     * @var string
     */    
    public $note;    
    
    
    /**
     * @var \DateTime
     *
     * @OGM\Property()
     * @OGM\Convert(type="datetime", options={"format":"long_timestamp"})
     */
    protected $begin;

    /**
     * @var \DateTime
     *
     * @OGM\Property()
     * @OGM\Convert(type="datetime", options={"format":"long_timestamp"})
     */
    protected $finish;
    
    // <editor-fold defaultstate="collapsed" desc="getters /setters ">

    public function getType() {
        return $this->type;
    }

    /**
     * 
     * @return Day|NULL
     */
    public function getStart() {
        return $this->start;
    }

    /**
     * 
     * @return Day|NULL
     */
    public function getEnd() {
        return $this->end;
    }

    public function setType($type) {
        $this->type = $type;
    }

    /**
     * 
     * @param Day $start
     */
    public function setStart($start) {
        $this->start = $start;
    }

    /**
     * 
     * @param Day $end
     */
    public function setEnd($end) {
        $this->end = $end;
    }

    /**
     * 
     * @return Project|NULL
     */
    function getProject()  {
        return $this->project;
    }

    /**
     * 
     * @return Company|NULL
     */
    function getCompany() {
        return $this->company;
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
     * @param Company $company
     */
    function setCompany($company) {
        $this->company = $company;
    }
    
    function getDescription() {
        return $this->description;
    }

    function getNote() {
        return $this->note;
    }

    function setDescription($description) {
        $this->description = $description;
    }

    function setNote($note) {
        $this->note = $note;
    }

    /**
     * 
     * @return \DateTime
     */
    function getBegin() {
        return $this->begin;
    }
    /**
     * 
     * @return \DateTime
     */
    function getFinish(): \DateTime {
        return $this->finish;
    }
    /**
     * 
     * @param \DateTime $begin
     */
    function setBegin( $begin) {
        $this->begin = $begin;
    }
/**
     * 
     * @param \DateTime $begin
     */
    function setFinish($finish) {
        $this->finish = $finish;
    }

            // </editor-fold>

    // <editor-fold defaultstate="collapsed" desc="BaseModel ">
    public function setUuid($uuid) {
        $this->uuid = $uuid;
    }

    public function getid() {
        return $this->id;
    }

    public function getUuid() {
        return $this->uuid;
    }

    public function import($object) {
    }

    public function tag(): string {
        return "";
        $reflect = new \ReflectionClass($this);
        return $reflect->getShortName()."_".$this->tagId();        
    }
    
    public function tagId(): string {
        return $this->tagId;
    }   
    
   public function getQueryFields() {
        $tag = $this->tag();
        $vars = get_object_vars($this);
       
        foreach ($vars as $key => $value) {
            switch ($key) {
                case 'id':
                case 'tagId':
                case 'start':
                case 'stop':
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
                case 'start':
                case 'stop':
                    break;
                default:
                    $params[$tag."_".$key] = $value;
                    break;
            }            
        }
        return $params;
    }    
    // </editor-fold>

    // <editor-fold defaultstate="collapsed" desc="JsonSerializable">
    public function jsonSerialize() {
        if (NULL === $this->start) {
            $this->getStart();
        }

        if (NULL === $this->end) {
            $this->getEnd();
        }   
        if (NULL === $this->project) $this->getProject();
        if (NULL === $this->company) $this->getCompany();
        
        return array_merge(parent::jsonSerialize(),['type' => $this->type, 'start' => $this->start, 'end' => $this->end, 
            "company" => $this->company,
            "project" => $this->project, 
            "note" => $this->note,
            "description" => $this->description
            ]);
    }
// </editor-fold>
}

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
 * @OGM\Node(label="Absence")
 */
class Absence extends BaseModel implements \JsonSerializable{
    
    /**
     * @var \DateTime
     *
     * @OGM\Property()
     * @OGM\Convert(type="datetime", options={"format":"long_timestamp"})
     */
    protected $from;  

    /**
     * @var \DateTime
     *
     * @OGM\Property()
     * @OGM\Convert(type="datetime", options={"format":"long_timestamp"})
     */ 
    protected $to;    

    /**
     * @var int
     * @OGM\Property()
     */
    protected $duration;

    /**
     * @var int
     */
    protected $actualDuration;
    
    /**
     * @var string
     * @OGM\Property()
     */
    protected $type;
    
    /**
     * @var string
     * @OGM\Property()
     */
    protected $description;
    
    public function getDescription() {
        return $this->description;
    }

    public function setDescription($description) {
        $this->description = $description;
    }

            
    /**
     * 
     * @return \DateTime
     */
    public function getFrom(){
        return $this->from;
    }

    /**
     * 
     * @return \DateTime
     */
    public function getTo() {
        return $this->to;
    }

    public function getDuration() {
        return $this->duration;
    }

    public function getActualDuration() {
        return $this->actualDuration;
    }

    public function getType() {
        return $this->type;
    }

    /**
     * 
     * @param \DateTime $from
     */
    public function setFrom($from) {
        $this->from = $from;
    }

    /**
     * 
     * @param \DateTime $to
     */
    public function setTo($to) {
        $this->to = $to;
    }

    public function setDuration($duration) {
        $this->duration = $duration;
    }

    public function setActualDuration($actualDuration) {
        $this->actualDuration = $actualDuration;
    }

    public function setType($type) {
        $this->type = $type;
    }
    
    //<editor-fold desc="BaseModel implementation"> 
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
    //</editor-fold>   
    
    //<editor-fold desc="JsonSerializable implementation">   
    public function jsonSerialize() {
        
        return array_merge(
            parent::jsonSerialize(),
            [
                'description' => $this->description,
                'from' => $this->from->format("c"),
                'to' => $this->to->format("c"),
                'duration' => $this->duration,
                'type' => $this->type,
            ]
        ); 
    }
    //</editor-fold>    

}

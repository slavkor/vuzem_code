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
 * @OGM\Node(label="Year")
 */
class Year extends BaseModel implements \JsonSerializable{

    /**
     * @OGM\GraphId()
     *
     * @var int
     */
    protected $id;
    
    /**
     * value
     * @var int
     * 
     * @OGM\Property(type="int")
     */
    protected $value;

    /**
     * value
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $strrep;    

    public function getStrrep() {
        return $this->strrep;
    }

    public function setStrrep($strrep) {
        $this->strrep = $strrep;
    }
    
    public function getValue() {
        return $this->value;
    }

    public function setValue($value) {
        $this->value = $value;
    }

    public function jsonSerialize() {
        
        $ret = array_merge(
                parent::jsonSerialize(),
                [
                    'value' => $this->value,
                    'strrep' => $this->strrep
                    ]);
  
        return $ret;
    }

    public function setUuid($uuid) {
        
    }

    public function getid() {
        
    }

    public function getUuid() {
        
    }

    public function import($object) {
        
    }

    public function tag(): string {
        
    }

}

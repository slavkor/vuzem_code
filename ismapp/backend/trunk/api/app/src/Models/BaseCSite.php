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
 * @OGM\Node(label="CSite")
 */
class BaseCSite extends BaseModel implements \JsonSerializable {

    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $name;

    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $description;

    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $color;

    public function getname() {
        return $this->name;
    }
    
    public function getdescription() {
        return $this->description;
    }
    
    public function setname($name) {
        $this->name = $name;
    }
    
    public function setdescription($description) {
        $this->description = $description;
    }
    public function getColor() {
        return $this->color;
    }

    public function setColor($color) {
        $this->color = $color;
    }

        //<editor-fold desc="JsonSerializable implementation">    
    public function jsonSerialize() {
        return array_merge(
                parent::jsonSerialize(),
                [
                    'id' => $this->id,
                    'name' => $this->name,
                    'description' => $this->description,
                    'color' => $this->color
                ]);
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
                case 'color':
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
    }    //</editor-fold>
    
}

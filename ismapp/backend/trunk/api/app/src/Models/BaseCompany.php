<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;

use GraphAware\Neo4j\OGM\Annotations as OGM;
use App\Models\Address;

/**
 *
 * @OGM\Node(label="Company")
 */
class BaseCompany  extends BaseModel implements \JsonSerializable{

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
    protected $shortname;    

    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $taxnumber;    

    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $regnumber;    
    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $color;
    
    public function getName() {
        return $this->name;
    }

    public function getShortname() {
        return $this->shortname;
    }

    public function getTaxnumber() {
        return $this->taxnumber;
    }

    public function getRegnumber() {
        return $this->regnumber;
    }

    public function setName($name) {
        $this->name = $name;
    }

    public function setShortname($shortname) {
        $this->shortname = $shortname;
    }

    public function setTaxnumber($taxnumber) {
        $this->taxnumber = $taxnumber;
    }

    public function setRegnumber($regnumber) {
        $this->regnumber = $regnumber;
    }

    public function getColor() {
        return $this->color;
    }

    public function setColor($color) {
        $this->color = $color;
    }    
    //<editor-fold desc="JsonSerializable implementation">   
    public function jsonSerialize() {
        
        $ret = array_merge(
            parent::jsonSerialize(),
            [
                'name' => $this->name,
                'shortname' => $this->shortname,
                'taxnumber' => $this->taxnumber,
                'regnumber' => $this->regnumber,
                'color' => $this->color
            ]
        ); 

        return $ret;
    }
    //</editor-fold>    
    //    
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
                case 'shortname':
                case 'taxnumber':
                case 'regnumber':
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

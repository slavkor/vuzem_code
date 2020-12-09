<?php

/*
 * 
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 * 
 */

namespace App\Models;

use GraphAware\Neo4j\OGM\Annotations as OGM;
/**
 *
 * @OGM\Node(label="Address")
 */
class BaseAddress extends BaseModel implements \JsonSerializable{
    
    public function __construct() {
        parent::__construct();
    }
    
    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $line1;

    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $line2;

    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $city;

    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $state;

    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $zip;

    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $country;

    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */    
    protected $type;

    public function getline1() {
        return $this->line1;
    }
    public function getline2() {
        return $this->line2;
    }
    public function getcity() {
        return $this->city;
    }
    public function getstate() {
        return $this->state;
    }
    public function getzip() {
        return $this->zip;
    }
    public function getcountry() {
        return $this->country;
    }
    public function gettype() {
        return $this->type;
    }

    public function setline1($line1) {
        $this->line1 = $line1;
    }
    public function setline2($line2) {
        $this->line2 = $line2;
    }
    public function setcity($city) {
        $this->city = $city;
    }
    public function setstate($state) {
        $this->state = $state;
    }
    public function setzip($zip) {
        $this->zip = $zip;
    }
    public function setcountry($country) {
        $this->country = $country;
    }
    public function settype($type) {
        $this->type = $type;
    }
    
    
    public function jsonSerialize() {
        return array_merge(
            parent::jsonSerialize(),
            [
            'line1' => $this->line1,
            'line2' => $this->line2,
            'city' => $this->city,
            'state' => $this->state,
            'zip' => $this->zip,
            'country' => $this->country, 
            'type' => $this->type
        ]);
    }
    
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
                case 'line1':
                case 'line2':
                case 'city':
                case 'state':
                case 'zip':
                case 'country':
                case 'type':
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

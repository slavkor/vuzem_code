<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;

use GraphAware\Neo4j\OGM\Annotations as OGM;

class BaseContact extends BasePerson implements \JsonSerializable {
    public function __construct() {
        parent::__construct();
        $this->tagId = uniqid();
    }
    
    //<editor-fold desc="protected fields">
    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    public $description;
    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    public $phone;
    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    public $mobilephone;
    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    public $organizationphone;
    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    public $organizationmobilephone;
    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    public $email;

    //</editor-fold>

    //<editor-fold desc="getters">
    public function getDescription() {
        return $this->description;
    }

    public function getPhone() {
        return $this->phone;
    }

    public function getMobilephone() {
        return $this->mobilephone;
    }

    public function getOrganizationphone() {
        return $this->organizationphone;
    }

    public function getOrganizationmobilephone() {
        return $this->organizationmobilephone;
    }
    public function getEmail() {
        return $this->email;
    }

    public function setEmail($email) {
        $this->email = $email;
    }

    //</editor-fold>

    //<editor-fold desc="setters">
    public function setDescription($description) {
        $this->description = $description;
    }

    public function setPhone($phone) {
        $this->phone = $phone;
    }

    public function setMobilephone($mobilephone) {
        $this->mobilephone = $mobilephone;
    }

    public function setOrganizationphone($organizationphone) {
        $this->organizationphone = $organizationphone;
    }

    public function setOrganizationmobilephone($organizationmobilephone) {
        $this->organizationmobilephone = $organizationmobilephone;
    }

    
    //</editor-fold>
    
    //<editor-fold desc="JsonSerializable implementation">
    public function jsonSerialize() {
        return array_merge(
                parent::jsonSerialize(), 
                [
                    'phone' => $this->phone,
                    'mobilephone' => $this->mobilephone,
                    'organizationphone' => $this->organizationphone,
                    'organizationmobilephone' => $this->organizationmobilephone,
                    'description' => $this->description, 
                    'email' =>$this->email
                ]
        );
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
        parent::import($object);
        
        $vars=is_object($object)?get_object_vars($object):$object;

        if (!is_array($vars)) {
            throw \Exception('no props to import into the object!');
        }
        foreach ($vars as $key => $value) {
            switch ($key) {
                case 'phone':
                case 'mobilephone':
                case 'organizationphone':
                case 'organizationmobilephone':
                case 'description':
                case 'email':
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

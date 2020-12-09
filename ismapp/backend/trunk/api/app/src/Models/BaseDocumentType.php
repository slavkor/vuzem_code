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
 * @OGM\Node(label="DocumentType")
 */
class BaseDocumentType extends BaseModel implements \JsonSerializable{

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
    protected $notificationmail;

    /**
     * @var boolean
     * 
     * @OGM\Property(type="boolean")
     */    
    protected $expirable;

    /**
     * @var boolean
     * 
     * @OGM\Property(type="boolean")
     */    
    protected $numbered;
    /**
     * @var boolean
     * 
     * @OGM\Property(type="boolean")
     */    
    protected $csitebound;
    
    /**
     * @var boolean
     * 
     * @OGM\Property(type="boolean")
     */    
    protected $countrybound;
    

    /**
     * @var boolean
     * 
     * @OGM\Property(type="boolean")
     */    
    protected $csiteprint;


    /**
     * @var boolean
     * 
     * @OGM\Property(type="boolean")
     */    
    protected $hqglobal;
    
    /**
     * @var boolean
     * 
     * @OGM\Property(type="boolean")
     */    
    protected $credit;    

    /**
     * @var boolean
     * 
     * @OGM\Property(type="boolean")
     */    
    protected $debet;    

    /**
     * @var boolean
     * 
     * @OGM\Property(type="boolean")
     */    
    protected $prolongable;
    
    /**
     * @var boolean
     * 
     * @OGM\Property(type="boolean")
     */       
    protected $promissorynote;
    
    public function getName() {
        return $this->name;
    }

    public function getDescription() {
        return $this->description;
    }

    public function getNotificationmail() {
        return $this->notificationmail;
    }

    public function setName($name) {
        $this->name = $name;
    }

    public function setDescription($description) {
        $this->description = $description;
    }

    public function setNotificationmail($notificationmail) {
        $this->notificationmail = $notificationmail;
    }

    public function getExpirable() {
        return $this->expirable;
    }

    public function setExpirable($expirable) {
        $this->expirable = $expirable;
    }

    
    public function getNumbered() {
        return $this->numbered;
    }

    public function setNumbered($numbered) {
        $this->numbered = $numbered;
    }
    function getCsitebound() {
        return $this->csitebound;
    }

    function getCountrybound() {
        return $this->countrybound;
    }

    function setCsitebound($csitebound) {
        $this->csitebound = $csitebound;
    }

    function setCountrybound($countrybound) {
        $this->countrybound = $countrybound;
    }

    function getCsiteprint() {
        return $this->csiteprint;
    }

    function setCsiteprint($csiteprint) {
        $this->csiteprint = $csiteprint;
    }

    function getHqglobal() {
        return $this->hqglobal;
    }

    function setHqglobal($hqglobal) {
        $this->hqglobal = $hqglobal;
    }

    function getCredit() {
        return $this->credit;
    }

    function getDebet() {
        return $this->debet;
    }

    function setCredit($credit) {
        $this->credit = $credit;
    }

    function setDebet($debet) {
        $this->debet = $debet;
    }
    function getProlongable() {
        return $this->prolongable;
    }

    function setProlongable($prolongable) {
        $this->prolongable = $prolongable;
    }

    public function getPromissorynote() {
        return $this->promissorynote;
    }

    public function setPromissorynote($promissorynote) {
        $this->promissorynote = $promissorynote;
    }

                                    
    public function jsonSerialize() {
        return array_merge(
                parent::jsonSerialize(),
                [
                    'name' => $this->name,
                    'description' => $this->description,
                    'notificationmail' => $this->notificationmail,
                    'expirable' => $this->expirable, 
                    'numbered' => $this->numbered,
                    'csitebound' => $this->csitebound,
                    'countrybound' => $this->countrybound,
                    'csiteprint' => $this->csiteprint,
                    'hqglobal' => $this->hqglobal,
                    'credit' => $this->credit,
                    'debet' => $this->debet,
                    'prolongable' => $this->prolongable,
                    'promissorynote' => $this->promissorynote,
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
                case 'name':
                case 'description':
                case 'notificationmail':
                case 'expirable':
                case 'numbered':
                case 'csitebound':                    
                case 'countrybound':                    
                case 'csiteprint':                    
                case 'hqglobal':                    
                case 'credit':                    
                case 'debet':    
                case 'prolongable':
                case 'promissorynote':                    
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
    }//</editor-fold>

}

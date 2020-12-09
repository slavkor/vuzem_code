<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;

use GraphAware\Neo4j\OGM\Annotations as OGM;
use \GraphAware\Neo4j\OGM\Common\Collection;


/**
 *
 * @OGM\Node(label="Contact")
 */
class Contact extends BaseContact {
    public function __construct() {
        parent::__construct();
        $this->addresses = new Collection();
        $this->tagId = uniqid();
    }

    /**
     * @var Address[]|Collection
     * 
     * @OGM\Relationship(type="AVAILABLE_ON", direction="OUTGOING", collection=true, mappedBy="", targetEntity="Address")
     */      
    protected $addresses;
    
    public function getAddresses(): array {
        return $this->addresses;
    }
    
    public function getUuid() {
       return $this->uuid;
    }
    
    public function setUuid($uuid) {
       $this->uuid = $uuid;
    }
    
    
    public function setdeleted($deleted) {
        parent::setdeleted($deleted);
    }
    public function getQueryFields() {
        $tag = $this->tag();
        $vars = get_object_vars($this);
       
        foreach ($vars as $key => $value) {
            switch ($key) {
                case 'id':
                case 'tagId':
                case 'addresses':
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
                case 'addresses':
                    break;
                default:
                    $params[$tag."_".$key] = $value;
                    break;
            }            
        }
        return $params;
    }
    public function tag(): string {
        $reflect = new \ReflectionClass($this);
        
        return $reflect->getShortName()."_".$this->tagId();
    }

    public function tagId(): string {
        if($this->tagId === null)
            $this->tagId = uniqid();
        return $this->tagId;
    }    //</editor-fold>    

}

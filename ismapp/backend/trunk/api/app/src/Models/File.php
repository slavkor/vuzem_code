<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;

use GraphAware\Neo4j\OGM\Annotations as OGM;
use GraphAware\Neo4j\OGM\Common\Collection;


/**
 *
 * @OGM\Node(label="File")
 */
class File extends BaseFile implements \JsonSerializable {
    public function __construct() {
        parent::__construct();
        $this->uniquename =  uniqid();
    }

    /**
     * @var Language
     * 
     * @OGM\Relationship(type="IS_LANG", direction="OUTGOING", collection=false, mappedBy="", targetEntity="Language")
     */      
    protected $language;

    // <editor-fold defaultstate="collapsed" desc="setters geters">
    public function getLanguage() {
        return $this->language;
    }

    public function setLanguage($language) {
        $this->language = $language;
    }

        // </editor-fold>
        
    // <editor-fold defaultstate="collapsed" desc="JsonSerializable">
    public function jsonSerialize() {
        $ret = parent::jsonSerialize();
        
        if (NULL !== $this->language) {
            $ret["language"] = $this->language;
        }
        
        return $ret;
    }
    // </editor-fold>

    
  public function getQueryFields() {
        $tag = $this->tag();
        $vars = get_object_vars($this);
       
        foreach ($vars as $key => $value) {
            switch ($key) {
                case 'id':
                case 'tagId':
                case 'language':
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
                case 'language':
                    break;
                default:
                    $params[$tag."_".$key] = $value;
                    break;
            }            
        }
        return $params;
    }    
}

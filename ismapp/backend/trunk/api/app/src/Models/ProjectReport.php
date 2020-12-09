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
 * @OGM\Node(label="Project")
 */
class ProjectReport extends BaseProject implements \JsonSerializable{

    /**
     * @var CSite
     * 
     * @OGM\Relationship(type="HAS", direction="INCOMING", collection=false, mappedBy="", targetEntity="CSite")
     */        
    protected $site;
        
    // <editor-fold defaultstate="collapsed" desc="getters /setters">

    function getSite() {
        return $this->site;
    }

    function setSite($site) {
        $this->site = $site;
    }

        // </editor-fold>
        
    //<editor-fold desc="JsonSerializable implementation">    
    public function jsonSerialize() {
        $ret = parent::jsonSerialize();

        if (null == $this->site) {
            $this->getSite();
        }

        if(NULL !== $this->site){
            $ret['site'] = $this->site;
        } else {
            $ret['site'] = NULL;            
        }
        

        return [ "project" => $ret];
        
    }
    //</editor-fold>     
    
    public function getQueryFields() {
      
    }
    
    public function getQueryFieldsParams(): array{
      
    }
    
    public function import($object) {
        
       
    }    
}

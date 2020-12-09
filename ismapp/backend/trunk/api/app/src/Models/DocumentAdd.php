<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;

/**
 * Description of DocumentAdd
 *
 * @author slavko.rihtaric
 */
class DocumentAdd {
    
     /**
     * @var int
     * 
    */
    
    protected $active;

    /**
     * @var string
     * 
    */
    
    protected $parentuuid;

    /**
     * @var Document
     * 
    */
    
    protected $document;

    
    function getActive() {
        return $this->active;
    }

    function getParentuuid() {
        return $this->parentuuid;
    }

    /**
     * 
     * @return \App\Models\Document
     */
    function getDocument() {
        return $this->document;
    }

    function setActive($active) {
        $this->active = $active;
    }

    function setParentuuid($parentuuid) {
        $this->parentuuid = $parentuuid;
    }

    /**
     * 
     * @param \App\Models\Document $document
     */
    function setDocument($document) {
        $this->document = $document;
    }


}

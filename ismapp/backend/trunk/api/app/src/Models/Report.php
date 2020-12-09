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
 * @OGM\Node(label="Report")
 */
class Report extends BaseModel implements \JsonSerializable{
    
    // <editor-fold defaultstate="collapsed" desc="fields">

    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $reportid;

    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $module;
    
    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $friendlyname;
    
    
    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $reportpath;
    
    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $metadata;    

    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $isoid;    
    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $isodate;    
    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $isorev;    


    
    // </editor-fold>
    
    // <editor-fold defaultstate="collapsed" desc="getters / setters">
    
    function getReportid() {
        return $this->reportid;
    }

    function getModule() {
        return $this->module;
    }

    function getFriendlyname() {
        return $this->friendlyname;
    }

    function getReportpath() {
        return $this->reportpath;
    }

    function getMetadata() {
        return $this->metadata;
    }

    function setReportid($reportid) {
        $this->reportid = $reportid;
    }

    function setModule($module) {
        $this->module = $module;
    }

    function setFriendlyname($friendlyname) {
        $this->friendlyname = $friendlyname;
    }

    function setReportpath($reportpath) {
        $this->reportpath = $reportpath;
    }

    function setMetadata($metadata) {
        $this->metadata = $metadata;
    }

    function getIsoid() {
        return $this->isoid;
    }

    function getIsodate() {
        return $this->isodate;
    }

    function getIsorev() {
        return $this->isorev;
    }

    function setIsoid($isoid) {
        $this->isoid = $isoid;
    }

    function setIsodate($isodate) {
        $this->isodate = $isodate;
    }

    function setIsorev($isorev) {
        $this->isorev = $isorev;
    }

    
    // </editor-fold>
    
    
    
    //<editor-fold desc="JsonSerializable implementation">   
    public function jsonSerialize() {
        
        $ret = array_merge(
            parent::jsonSerialize(),
            [
                'reportid' => $this->reportid, 
                'module' => $this->module, 
                'friendlyname' => $this->friendlyname,
                'reportpath' => $this->reportpath,
                'metadata' => $this->metadata,
                'isoid' => $this->isoid,
                'isodate' => $this->isodate,
                'isorev' => $this->isorev
            ]
        ); 
        return $ret;
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
                case 'reportid':
                case 'module':
                case 'friendlyname':
                case 'metadata':
                case 'reportpath':
                case 'isoid':
                case 'isodate':
                case 'isorev':
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

    public function getQueryFields() {
        $tag = $this->tag();
        $vars = get_object_vars($this);
       
        foreach ($vars as $key => $value) {
            switch ($key) {
                case 'id':
                case 'tagId':
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
                    break;
                default:
                    $params[$tag."_".$key] = $value;
                    break;
            }            
        }
        return $params;
    }    
    //</editor-fold>  

}

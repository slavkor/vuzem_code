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
 * @OGM\Node(label="WorkPlace")
 */
class WorkPlace  extends BaseModel implements \JsonSerializable {
    // <editor-fold defaultstate="collapsed" desc="properties">
    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $code;

    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $workplace;
    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $deworkplace;
        /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $enworkplace;
    
    // </editor-fold>
    
    // <editor-fold defaultstate="collapsed" desc="getters / setters">

    public function getCode() {
        return $this->code;
    }

    public function getWorkplace() {
        return $this->workplace;
    }

    public function setCode($code) {
        $this->code = $code;
    }

    public function setWorkplace($workplace) {
        $this->workplace = $workplace;
    }
    
    function getDeworkplace() {
        return $this->deworkplace;
    }

    function getEnworkplace() {
        return $this->enworkplace;
    }

    function setDeworkplace($deworkplace) {
        $this->deworkplace = $deworkplace;
    }

    function setEnworkplace($enworkplace) {
        $this->enworkplace = $enworkplace;
    }

        // </editor-fold>
        
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
                case 'code':
                case 'wokrplace':
                case 'dewokrplace':
                case 'enwokrplace':
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

    //<editor-fold desc="JsonSerializable implementation">
    public function jsonSerialize() {
        return array_merge(
                parent::jsonSerialize(),
                [
                    'code' => $this->code,
                    'workplace' => $this->workplace,
                    'deworkplace' => $this->deworkplace,
                    'enworkplace' => $this->enworkplace
                ]);
    }
    //</editor-fold>
}

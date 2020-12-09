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
 * @OGM\Node(label="User")
 */
class UserExtendet  extends BaseUser implements \JsonSerializable{
    public function __construct() {
        parent::__construct();
        $this->documents = new Collection();
        $this->documentsdownloaded = new Collection();
    }
    
    /**
     * @var UserDocument[]|Collection
     * 
     * @OGM\Relationship(type="CREATED", direction="OUTGOING", collection=true, mappedBy="", targetEntity="UserDocument")
     */    
    protected $documents;

    public function getdocuments(){
        return $this->documents;
    }
    
    /**
     * @var UserDocument[]|Collection
     * 
     * @OGM\Relationship(type="DOWNLOADED", direction="OUTGOING", collection=true, mappedBy="", targetEntity="UserDocument")
     */    
    protected $documentsdownloaded;

    public function getdocumentsdownloaded(){
        return $this->documentsdownloaded;
    }

    public function jsonSerialize() {
        $ret = parent::jsonSerialize();
        
        if ($this->documents->count() > 0) {
            $ret['created'] = $this->documents->toArray();
        }
        if ($this->documentsdownloaded->count() > 0) {
            $ret['downloads'] = $this->documentsdownloaded->toArray();
        }
        
        return $ret;
    }
}

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
 * @OGM\Node(label="Document")
 */
class DocumentExtendet extends BaseDocument implements \JsonSerializable {
    public function __construct() {
        parent::__construct();
        $this->employees = new Collection();
        $this->users = new Collection();
    }

    /**
     * @var DocumentEmployee[]|Collection
     * 
     * @OGM\Relationship(type="HAS", direction="INCOMING", collection=true, mappedBy="", targetEntity="DocumentEmployee")
     */    
    protected $employees;

    public function getemployees() {
        return $this->employees;
    }
    
    /**
     * @var DocumentUser[]|Collection
     * 
     * @OGM\Relationship(type="CREATED", direction="INCOMING", collection=false, mappedBy="", targetEntity="DocumentUser")
     */    
    protected $creator;

    /**
     * @var DocumentUser[]|Collection
     * 
     * @OGM\Relationship(type="DOWNLOADED", direction="INCOMING", collection=true, mappedBy="", targetEntity="DocumentUser")
     */    
    protected $users;    

    public function getusers() {
        return $this->users;
    }
    
    public function jsonSerialize() {
        $ret = parent::jsonSerialize();
        
        if ($this->employees->count() > 0) {
            $ret['employees'] = $this->employees;
        }
        if (NULL != $this->creator) {
            $ret['creator'] = $this->creator;
        }
        if ($this->users->count() > 0) {
            $ret['users'] = $this->users;
        }

        return $ret;
    }
}

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
 * @OGM\Node(label="Address")
 */
class AddressExtendet extends BaseAddress implements \JsonSerializable {
   public function __construct() {
        parent::__construct();
        $this->residents = new Collection();
        $this->mailrecipients = new Collection();
    }

    /**
     * @var AddressEmployee[]|Collection
     * 
     * @OGM\Relationship(type="LIVES_ON", direction="INCOMING", collection=true, mappedBy="", targetEntity="AddressEmployee")
     */   
    private $residents;

    /**
     * @var AddressEmployee[]|Collection
     * 
     * @OGM\Relationship(type="RECEIVES_POST", direction="INCOMING", collection=true, mappedBy="", targetEntity="AddressEmployee")
     */   
    private $mailrecipients;
    
    public function getmailrecipients() {
        return $this->mailrecipients;
    }

    public function jsonSerialize() {
        $ret = parent::jsonSerialize();
        if ($this->residents->count() > 0) {
            $ret['residents'] = $this->residents->toArray();
        }
        if ($this->mailrecipients->count() > 0) {
            $ret['recipients'] = $this->mailrecipients->toArray();
        }

        return ret;
    }
}

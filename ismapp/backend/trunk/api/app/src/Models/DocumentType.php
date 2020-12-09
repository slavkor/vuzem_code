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
class DocumentType extends BaseDocumentType implements \JsonSerializable {
    
     /**
     * @OGM\Property(type="int")
     *
     * @var int
     */
    public $notifybeforedays;
    
    public function jsonSerialize() {
        $ret = parent::jsonSerialize();
        $ret['notifybeforedays'] = $this->notifybeforedays;
        return $ret;
        //return parent::jsonSerialize();
    }
}

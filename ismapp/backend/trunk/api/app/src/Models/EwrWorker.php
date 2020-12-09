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
 * @OGM\RelationshipEntity(type="WORKS")
 */
class EwrWorker  implements \JsonSerializable{
    /**
     * @var int
     *
     * @OGM\GraphId()
     */
    protected $id;

    /**
     * @var ErwEmployee
     *
     * @OGM\StartNode(targetEntity="Employee")
     */
    protected $employee;

    /**
     * @var Ewr
     *
     * @OGM\EndNode(targetEntity="Ewr")
     */
    protected $ewr;

    /**
     * @var string
     *
     * @OGM\Property(type="string")
     */
    protected $workplace;

    public function __construct(ErwEmployee $employee, Ewr $ewr, $workplace)
    {
        $this->employee = $employee;
        $this->ewr = $ewr;
        $this->workplace = $workplace;
    }


    /**
     * 
     * @return ErwEmployee
     */
    function getEmployee(){
        return $this->employee;
    }

    function getEwr(): Ewr {
        return $this->ewr;
    }

    function getWorkplace() {
        return $this->workplace;
    }

        public function jsonSerialize() {
        return ["workplace" => $this->workplace,  "worker" => $this->employee];
    }
}

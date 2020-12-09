<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;

use GraphAware\Neo4j\OGM\Annotations as OGM;
use \GraphAware\Neo4j\OGM\Common\Collection;

use App\Models\Absence;

/**
 * Description of EmployeeModel
 *
 * @author slavko.rihtaric
 */

/**
 *
 * @OGM\Node(label="Employee")
 */
class EmployeeModel extends Employee  implements \JsonSerializable{
    public function __construct() {
        parent::__construct();
        $this->absences = new Collection();    
    }
    
    /**
     * @var Collection
     * 
     * @OGM\Relationship(relationshipEntity="EmployeeAbsenceRelation", type="ABSENT", direction="OUTGOING", collection=true, mappedBy="employee")
     */
    protected $absences;      
    
    /**
     * 
     * @return Collection
     */
    public function getAbsences() {
        return $this->absences;
    }

    /**
     * 
     * @param Collection $absences
     */
    public function setAbsences($absences) {
        $this->absences = $absences;
    }
    public function AddAbsence(Absence $absence) {
        $relation = new EmployeeAbsenceRelation($absence, $this);
        $this->getAbsences()->add($relation);
    }

    public function RemoveAbsense(Absence $absence) {
        $this->getAbsences()->removeElement($absence);
    }
    
        //<editor-fold desc="JsonSerializable implementation">   
    public function jsonSerialize() {
        $this->getAbsences();
        return array_merge(
            parent::jsonSerialize(),
            [
                'absences' => $this->getAbsences()->toArray()
            ]
        ); 
    }
    //</editor-fold>    
}

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
 *
 * @OGM\Node(label="Employee")
 */
class EmployeeHome extends BaseEmployee implements \JsonSerializable {
    
    public function __construct() {
        parent::__construct();
    }

    /**
     * @var Collection 
     * 
     * @OGM\Relationship(type="ABSENT", direction="OUTGOING", collection=true, mappedBy="", targetEntity="Absence")
     */    
    protected  $absence;

    /**
     * 
     * @return Collection
     */
    public function getAbsence() {
        return $this->absence;
    }

    /**
     * 
     * @param Collection $absence
     */
    public function setAbsence($absence) {
        $this->absence = $absence;
    }

        
    public function jsonSerialize() {
        $ret = parent::jsonSerialize();
        if (null == $this->absence) {
            $this->getAbsence();
        }
  
        if(NULL !== $this->absenceabsence){
            $ret['absence'] = $this->loaner;
        } else {
            $ret['absence'] = NULL;            
        }
      
        return $ret;
    }
    
    public function getcrdate() {
        return parent::getcrdate();
    }
    
    public function getmfdate() {
        return parent::getmfdate();
    }
    
    public function setdeleted($deleted) {
        parent::setdeleted($deleted);
    }

   public function getQueryFields(){
        $tag = $this->tag();
        $vars = get_object_vars($this);
        $fields = "";
        foreach ($vars as $key => $value) {
            switch ($key) {
                case 'id':
                case 'tagId':
                case 'loaner':
                case 'workplace':
                    break;
                default:
                    $fields .= $key.":{".$tag."_".$key."},";                    
                    break;
            }            
        }
        return substr($fields, 0, strlen($fields)-1);
    }
    
  

    public function getQueryFieldsParams(): array{
        $tag = $this->tag();
        $vars = get_object_vars($this);
        $params = array();
        foreach ($vars as $key => $value) {
            switch ($key) {
                case 'id':
                case 'tagId':
                case 'loaner':
                case 'workplace':
                    break;
                default:
                    $params[$tag."_".$key] = $value;
                    break;
            }            
        }
        return $params;
    }    
}

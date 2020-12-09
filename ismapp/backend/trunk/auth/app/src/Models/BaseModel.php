<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;

use GraphAware\Neo4j\OGM\Annotations as OGM;
use Ramsey\Uuid\Uuid;
use Ramsey\Uuid\Exception\UnsatisfiedDependencyException;


abstract class BaseModel implements \JsonSerializable {
    public function __construct() {
        try {
            $this->uuid = Uuid::uuid4()->serialize();
            $this->active = 1;
            /*
            $timeZone = new \DateTimeZone('Europe/Ljubljana');
            $t = microtime(true);
            $micro = sprintf("%06d",($t - floor($t)) * 1000000);
            $createDate = new \DateTime( date('Y-m-d H:i:s.'.$micro, $t) );
            $createDate->setTimezone($timeZone);    
        
            $this->crdate = $createDate;
            $this->mfdate = $createDate;
            */
            
        } catch (UnsatisfiedDependencyException $exc) {
            echo $exc->getMessage();
        }
    }
    
    /**
     * @OGM\GraphId()
     *
     * @var int
     */
    protected $id;
    
    /**
     * @OGM\Property(type="string")
     *
     * @var string
     */
    protected $uuid;

    /**
     * @OGM\Property(type="int")
     *
     * @var int
     */
    protected $active;    

    /**
     * @var \DateTime
     *
     * @OGM\Property()
     * @OGM\Convert(type="datetime", options={"db_format"="string", "format":"'YmdHis.u'"})
     */
    protected $crdate;
    
    /**
     * @var \DateTime
     *
     * @OGM\Property()
     * @OGM\Convert(type="datetime", options={"db_format"="string", "format":"'YmdHis.u'"})
     */
    protected $mfdate;    
    
    abstract public function getid();
    abstract public function getuuid();
    abstract protected function setuuid($uuid);
    
    protected function getactive(){
        return $this->active;
    }
    protected function setactive($active){
        $this->active = $active;
    }
    
    protected function getcrdate(){
        return $this->crdate;
    }

    protected function getmfdate(){
        return $this->time;
    }
    
    protected function setmfdate($mfdate){
        $this->mfdate = $mfdate;
    }    

    //<editor-fold desc="JsonSerializable">
    public function jsonSerialize() {
        return [
            'id' => $this->id,
            'uuid' => $this->uuid, 
            'active' => $this->active,
            'crdate' => $this->crdate,
            'mfdate' => $this->mfdate
        ];
    }
    //</editor-fold>    

    public function import($object){
        $vars=is_object($object)?get_object_vars($object):$object;
        
        if (!is_array($vars)) {
            throw \Exception('no props to import into the object!');
        }
        foreach ($vars as $key => $value) {
            $this->$key = $value;
        }        
    }
}

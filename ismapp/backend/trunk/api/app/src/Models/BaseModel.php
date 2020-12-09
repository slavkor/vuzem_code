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
    /**
     * 
     *
     * @var string
     */    
    public $tagId;

    public function __construct() {
    
        $this->uuid = Uuid::uuid4()->toString();
        $this->active = 1;
        $this->deleted = 0;
        $this->tagId = uniqid();
        $this->iid =  idate("y").idate("m").idate("d").mt_rand(100000,999999);
        
//        $this->created = new \DateTime("NOW");
//        $this->modified = new \DateTime("NOW");
        
        /*
        try {
            $t = microtime(true);
            $micro = sprintf("%06d",($t - floor($t)) * 1000000);
            $createDate = new \DateTime( date('Y-m-d H:i:s.'.$micro, $t) );

            $this->crdate = $createDate->format('YmdHis.u');
            $this->setmfdate($createDate->format('YmdHis.u'));
            
        } catch (\Exception $exc) {
            try {
                $t = microtime(true);
                $micro = sprintf("%06d",($t - floor($t)) * 1000000);
                $createDate = new \DateTime( date('Y-m-d H:i:s.'.$micro, $t) );

                $this->crdate = $createDate->format('YmdHis.u');
                $this->setmfdate($createDate->format('YmdHis.u'));
                
            } catch (\Exception $exc) {}
        }*/
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
    public $uuid;
    
    
    /**
     * @OGM\Property(type="int")
     *
     * @var int
     */
    protected $iid;   
    

    /**
     * @OGM\Property(type="int")
     *
     * @var int
     */
    protected $active;    
    
    /**
     * @var string
     *
     * @OGM\Property(type="string")
     * 
     */
    protected $crdate;    

//    /**
//     * @var \DateTime
//     *
//     * @OGM\Property()
//     * @OGM\Convert(type="datetime", options={"format":"long_timestamp"})
//     */
//    protected $created;
//
//    
//    /**
//     * 
//     * @var \DateTime
//     * 
//     * @OGM\Property()
//     * @OGM\Convert(type="datetime", options={"format":"long_timestamp"})
//     */
//    private $modified;    
    
    
    /**
     * @var int
     *
     * @OGM\Property(type="int")
     */
    protected $mfdate;   
    
    /**
     * @OGM\Property(type="int")
     *
     * @var int
     */
    protected $deleted;     

    /**
     * @var string
     *
     * @OGM\Property(type="string")
     */    
    protected $status;
    
    /**
     * @var string
     *
     * @OGM\Property(type="string")
     */    
    protected $user;    
    
    public function getIid() {
        return $this->iid;
    }

    abstract public function getid();
    abstract public function getUuid();
    abstract public function setUuid($uuid);
    abstract function import($object);
    abstract public function tag():string;
    
    public function getactive(){
        return $this->active;
    }
    public function getStatus() {
        return $this->status;
    }

    public function setStatus($status) {
        $this->status = $status;
    }

    public function getUser() {
        return $this->status;
    }

    public function setUser($user) {
        $this->user = $user;
    }

    public function setactive($active){
        $this->active = $active;
    }
    
    public function getdeleted(){
        return $this->deleted;
    }
    
    public function setdeleted($deleted){
        if ($deleted === 0) {
            return;
        }
        $this->deleted = $deleted;
    }
    
    public function getcrdate(){
        return $this->crdate;
    }
    
    public function getmfdate(){
        return $this->time;
    }

    public function setmfdate($mfdate){
        /*
         * try{
            if ($mfdate !== '' && $mfdate !== '00010101000000') {
                $this->mfdate = $mfdate;
            } else {
                $t = microtime(true);
                $micro = sprintf("%06d", ($t - floor($t)) * 1000000);
                $dt = new \DateTime(date('Y-m-d H:i:s.' . $micro, $t));
                $this->mfdate = $dt->format('YmdHis.u');
            }
        } catch (\Exception $ex) {
            $this->mfdate = $mfdate;
        }
        */
    }
    
//    /**
//     * @return \DateTime
//     */
//    function getCreated() {
//        return $this->created;
//    }
//
//    /**
//     * @param \DateTime $created
//     */
//    function setCreated($created) {
//        $this->created = $created;
//    }
//    
//    /**
//     * @return \DateTime
//     */  
//    function getModified() {
//        return $this->modified;
//    }
//    
//    /**
//     * @param \DateTime $modified
//     */
//    function setModified( $modified) {
//        $this->modified = $modified;
//    }

    public function getQueryFields(){
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
    
    public function getFieldsAsArray(): array {
        $vars = get_object_vars($this);
        $f = array();
        foreach ($vars as $key => $value) {
            $f[$key] = $key;
        }
        return $f;
    }    

    public function getQueryFieldsParams(): array{
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
    
    //<editor-fold desc="JsonSerializable">
    public function jsonSerialize() {
        $ret =  [
            'id' => $this->id,
            'iid' => $this->iid,
            'uuid' => $this->uuid, 
            'active' => $this->active,
            'crdate' => $this->crdate,
            'mfdate' => $this->mfdate,
            'deleted' =>$this->deleted, 
            'status'=> $this->status, 
//            'created' => $this->created,
//            'modified' => $this->modified
        ];
        
        if ($ret['iid'] === NULL) {
            $ret['iid'] = 0;
        }
        
        return $ret;
    }
    //</editor-fold>    
    
}

<?php

namespace App\Models;

use GraphAware\Neo4j\OGM\Annotations as OGM;
use GraphAware\Neo4j\OGM\Common\Collection;

/**
 *
 * @OGM\Node(label="Document")
 */
class BaseDocument extends BaseModel implements \JsonSerializable{
    
    public function __construct() {
        parent::__construct();
    }

    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $name;
    
 
    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $description;    

    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $documentnumber;  
    
    /**
     * @var float
     * 
     * @OGM\Property(type="float")
     */
    protected $amount;  
    /**
     * @var float
     * 
     * @OGM\Property(type="float")
     */
    protected $amountwot;  
    /**
     * @var float
     * 
     * @OGM\Property(type="float")
     */
    protected $amountwt;  

    /**
     * @var \DateTime
     *
     * @OGM\Property()
     * @OGM\Convert(type="datetime", options={"format":"long_timestamp"})
     */
    protected $expiredate;
    
    function getAmount() {
        return $this->amount;
    }

    function getAmountwot() {
        return $this->amountwot;
    }

    function getAmountwt() {
        return $this->amountwt;
    }

    function setAmount($amount) {
        $this->amount = $amount;
    }

    function setAmountwot($amountwot) {
        $this->amountwot = $amountwot;
    }

    function setAmountwt($amountwt) {
        $this->amountwt = $amountwt;
    }

    
    
    public function getName() {
        return $this->name;
    }

    public function getDescription() {
        return $this->description;
    }

    public function getDocumentnumber() {
        return $this->documentnumber;
    }

    public function setName($name) {
        $this->name = $name;
    }

    public function setDescription($description) {
        $this->description = $description;
    }

    public function setDocumentnumber($documentnumber) {
        $this->documentnumber = $documentnumber;
    }
    /**
     * 
     * @return \DateTime
     */
    function getExpiredate() {
        return $this->expiredate;
    }

    /**
     * 
     * @param \DateTime $expiredate
     */
    function setExpiredate($expiredate) {
        $this->expiredate = $expiredate;
    }
        
    public function jsonSerialize() {
        $r[] = null;
        if(null != $this->expiredate){
            $r = [ 'expiredate' => $this->expiredate->format("c")];
        }
        return array_merge(
                parent::jsonSerialize(),
                [
                    'name' => $this->name,
                    'description' => $this->description,
                    'documentnumber' => $this->documentnumber,
                    'amount' => $this->amount,
                    'amountwot' => $this->amountwot,
                    'amountwt' => $this->amountwt
                ], $r);
    }

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
                case 'name':
                case 'description':
                case 'documentnumber':
                case 'amount':
                case 'amountwot':
                case 'amountwt':
                    $this->$key = $value;
                    break;
//                case 'expiredate':
//                    var_dump($this->$key);
//                               var_dump($value);
//                    die;
//                    $this->$key = $value;
//                    break;

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
    }//</editor-fold>
}

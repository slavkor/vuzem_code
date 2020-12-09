<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;
use GraphAware\Neo4j\OGM\Annotations as OGM;
use GraphAware\Neo4j\OGM\Common\Collection;
use App\Models\Employee;

/**
 *
 * @OGM\Node(label="Rental")
 */
class Rental extends BaseModel implements \JsonSerializable {

    public function __construct() {
        parent::__construct();
        $this->inusedays = new Collection();
        $this->outagedays = new Collection();
        $this->contacts = new Collection();
    }
    //<editor-fold desc="protected fields">    
    
    /**
     * contract
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $contract;    
    
    /**
     * model
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $model;

    /**
     * specification
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $specification;   

    /**
     * state
     * @var string
     * 
     * @OGM\Property(type="string")
     */    
    protected $state;

    /**
     * start
     * @var \DateTime
     *
     * @OGM\Property()
     * @OGM\Convert(type="datetime", options={"format":"long_timestamp"})
     */
    protected $start;
    
     /**
     * end
     * @var \DateTime
     *
     * @OGM\Property()
     * @OGM\Convert(type="datetime", options={"format":"long_timestamp"})
     */
    protected $end;
    
    /**
     * priceday
     * @var float
     * 
     * @OGM\Property(type="float")
     */    
    protected $priceday;     
    /**
     * priceweek
     * @var float
     * 
     * @OGM\Property(type="float")
     */    
    protected $priceweek;     
    
    /**
     * pricemonth
     * @var float
     * 
     * @OGM\Property(type="float")
     */    
    protected $pricemonth;     

    /**
     * pricetransport
     * @var float
     * 
     * @OGM\Property(type="float")
     */    
    protected $pricetransport;    
    
    /**
     * priceensurance
     * @var float
     * 
     * @OGM\Property(type="float")
     */    
    protected $priceensurance;    
    

    /**
     * pricetotal
     * @var float
     * 
     * @OGM\Property(type="float")
     */    
    protected $pricetotal;        
    
    //</editor-fold>    
    
    //<editor-fold desc="relationships">

    /**
     * @var Project
     * 
     * @OGM\Relationship(type="RENT", direction="INCOMING", collection=false, mappedBy="", targetEntity="Project")
     */  
    protected $project;

    /**
     * @var CSite
     * 
     * @OGM\Relationship(type="RENT", direction="INCOMING", collection=false, mappedBy="", targetEntity="CSite")
     */  
    protected $constructionsite;
    
    /**
     * @var Company
     * 
     * @OGM\Relationship(type="RENTAL", direction="INCOMING", collection=false, mappedBy="", targetEntity="Company")
     */  
    protected $rentalcompany;    
    
     /**
     * @var Day
     * 
     * @OGM\Relationship(type="INUSE", direction="OUTGOING", collection=true, mappedBy="", targetEntity="Day")
     */  
    protected $inusedays;
    
     /**
     * @var Day
     * 
     * @OGM\Relationship(type="NOTINUSE", direction="OUTGOING", collection=true, mappedBy="", targetEntity="Day")
     */  
    protected $outagedays;    

    /**
     * @var Contact[]|Collection
     * 
     * @OGM\Relationship(type="CONTACT", direction="OUTGOING", collection=true, mappedBy="", targetEntity="Contact")
     */    
    protected  $contacts;

    //</editor-fold>   

    //<editor-fold desc="JsonSerializable implementation">    
    public function jsonSerialize() {
        $ret = parent::jsonSerialize();
        
        return $ret;
    }


    //</editor-fold>    
    
    //<editor-fold desc="BaseModel implementation">    
    
    public function getUuid() {
        
    }

    public function getid() {
        
    }

    public function import($object) {
        
    }

    public function setUuid($uuid) {
        
    }

    public function tag(): string {
        
    }

    //</editor-fold>   
}


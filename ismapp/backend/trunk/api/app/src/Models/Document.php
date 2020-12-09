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
class Document extends BaseDocument implements \JsonSerializable {
    public function __construct() {
        parent::__construct();
    }

    /**
     * @var DocumentType
     * 
     * @OGM\Relationship(type="IS_OF_TYPE", direction="OUTGOING", collection=false, mappedBy="", targetEntity="DocumentType")
     */    
    protected $type;

    /**
     * @var object
     * 
     * @OGM\Relationship(type="ATTACHED", direction="OUTGOING", collection=true, mappedBy="", targetEntity="File")
     */        
    protected $files;
    
    /**
     * @var Day
     * 
     * @OGM\Relationship(type="VALID_FROM", direction="OUTGOING", collection=false, mappedBy="", targetEntity="Day")
     */        
    protected $validfrom;
    
    /**
     * @var Day
     * 
     * @OGM\Relationship(type="VALID_TO", direction="OUTGOING", collection=false, mappedBy="", targetEntity="Day")
     */        
    protected $validto;  

    /**
     * @var Day
     * 
     * @OGM\Relationship(type="DOCUMENTDATE", direction="OUTGOING", collection=false, mappedBy="", targetEntity="Day")
     */        
    protected $docdate;  
    
    

    /**
     * @var Document
     * 
     * @OGM\Relationship(type="NEXT", direction="OUTGOING", collection=false, mappedBy="", targetEntity="Document")
     */        
    protected $next;
    
    /**
     * @var User
     * 
     * @OGM\Relationship(type="CREATED", direction="INCOMING", collection=false, mappedBy="", targetEntity="User")
     */        
    protected $user;    
    
    /**
     * @var Company
     * 
     * @OGM\Relationship(type="ASSOCIATED_WITH", direction="OUTGOING", collection=false, mappedBy="", targetEntity="Company")
     */        
    protected $company;    
    
    /**
     * 
     * @return \App\Models\Company
     */
    function getCompany() {
        return $this->company;
    }

    /**
     * 
     * @param \App\Models\Company $company
     */
    function setCompany($company) {
        $this->company = $company;
    }

    /**
     * 
     * @return \App\Models\User
     */
    function getUser() {
        return $this->user;
    }

    /**
     * 
     * @param \App\Models\User $user
     */
    function setUser($user) {
        $this->user = $user;
    }

        /**
     * 
     * @return Document
     */
    function getNext()  {
        return $this->next;
    }

    /**
     * 
     * @param Document|NULL $next
     */
    function setNext($next) {
        $this->next = $next;
    }

        
    public function getValidFrom() {
        return $this->validfrom;
    }

    public function getValidTo() {
        return  $this->validto;
    }

    /**
     * 
     * @param Day|NULL $day
     */
    public function setValidFrom($day) {
        $this->validfrom= $day;
    }

    /**
     * 
     * @param Day|NULL $day
     */

    public function setValidTo($day) {
        $this->validto = $day;
    }

    /**
     * 
     * @return \App\Models\DocumentType
     */
    public function getType(){
        return $this->type;
    }

    public function getFiles() {
        return $this->files;
    }

    /**
     * 
     * @param \App\Models\DocumentType $type
     */
    public function setType($type) {
        $this->type = $type;
    }

    public function setFiles($files) {
        $this->files = $files;
    }
    
    function getDocdate() {
        return $this->docdate;
    }

    /**
     * 
     * @param Day|NULL $day
     */
    function setDocdate($day) {
        $this->docdate = $day;
    }

//    /**
//     * 
//     * @return \DateTime
//     */
//    function getExpiredate() {
//        return $this->expiredate;
//    }
//
//    /**
//     * 
//     * @param \DateTime $expiredate
//     */
//    function setExpiredate($expiredate) {
//        $this->expiredate = $expiredate;
//    }
    
    public function jsonSerialize() {
        $ret = parent::jsonSerialize();
        
        if(null === $this->type){
            $this->getType();
        }
        
        if(null === $this->next){
            $this->getNext();
        }
        
        if(NULL !== $this->type){
         
            $ret['type'] = $this->type;
        } else {
            $ret['type'] = NULL;            
        }
        
        if(NULL !== $this->files){
            $ret['files'] = $this->files->toArray();
        } else {
            $ret['files'] = NULL;            
        }      
        
        if(null == $this->validfrom)
            $this->getValidFrom ();
        
        if(NULL !== $this->validfrom){
            $ret['validfrom'] = $this->validfrom;
        } else {
            $ret['validfrom'] = NULL;            
        }

        if(null == $this->validto)
            $this->getValidTo();
        
        if(NULL !== $this->validto){
            $ret['validto'] = $this->validto;
        } else {
            $ret['validto'] = NULL;            
        }      

        if(null == $this->docdate)
            $this->getDocdate();
        
        if(NULL !== $this->docdate){
            $ret['docdate'] = $this->docdate;
        } else {
            $ret['docdate'] = NULL;            
        }      
        
        if(NULL !== $this->next){
            if ($this->next->getdeleted() === 0) {
                $ret['next'] = $this->next;
            } else {
                $ret['next'] = NULL;
            }
        } else {
            $ret['next'] = NULL;            
        }              
        
        
        
        return $ret;
    } 
    
    
    public function getQueryFields() {
        $tag = $this->tag();
        $vars = get_object_vars($this);
       
        foreach ($vars as $key => $value) {
            switch ($key) {
                case 'id':
                case 'tagId':
                case 'files':
                case 'type':
                case 'validfrom': 
                case 'validto':
                case 'docdate':
                    break;
                default:
                    $fields .= $key.":{".$tag."_".$key."},";                    
                    break;
            }            
        }
        return substr($fields, 0, strlen($fields)-1);
    }
    
    public function getQueryFieldsParams() :array {
        $tag = $this->tag();
        $vars = get_object_vars($this);
        $params = array();
        foreach ($vars as $key => $value) {
            switch ($key) {
                case 'id':
                case 'tagId':
                case 'files':
                case 'type':
                case 'validfrom': 
                case 'validto':
                case 'docdate':
                    break;
                default:
                    $params[$tag."_".$key] = $value;
                    break;
            }            
        }
        return $params;
    }
    
    public function setdeleted($deleted) {
        parent::setdeleted($deleted);
    }
    
    public function setDocumentId($id){
        $this->setUuid($id);
    }
}

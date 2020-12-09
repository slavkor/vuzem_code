<?php
namespace App\Models;

use GraphAware\Neo4j\OGM\Annotations as OGM;
use \GraphAware\Neo4j\OGM\Common\Collection;

/**
 *
 * @OGM\Node(label="Employee")
 */
class Employee extends BaseEmployee implements \JsonSerializable {
    
    public function __construct() {
        parent::__construct();
        $this->periods = new Collection();
    }

    /**
     * @var BusinessPartner 
     * 
     * @OGM\Relationship(type="LOANS", direction="INCOMING", collection=false, mappedBy="", targetEntity="BusinessPartner")
     */    
    protected  $loaner;

    /**
     * @var WorkPlace 
     * 
     * @OGM\Relationship(type="WORK", direction="OUTGOING", collection=false, mappedBy="", targetEntity="WorkPlace")
     */    
    protected  $workplace;    

    /**
     * @var Collection 
     * 
     * @OGM\Relationship(type="WORKS_IN", direction="OUTGOING", collection=true, mappedBy="", targetEntity="WorkPeriod")
     */      
    protected $periods;


    /**
     * 
     * @return \App\Models\WorkPlace
     */
    function getWorkplace(){
        return $this->workplace;
    }

    /**
     * 
     * @param \App\Models\WorkPlace $workplace
     */
    function setWorkplace($workplace) {
        $this->workplace = $workplace;
    }
        
    /**
     * 
     * @return \App\Models\BusinessPartner
     */
    public function getLoaner() {
        return $this->loaner;
    }
    /**
     * 
     * @param \App\Models\BusinessPartner $loaner
     */
    public function setLoaner($loaner) {
        $this->loaner = $loaner;
    }
    
    /**
     * 
     * @return Collection
     */
    public function getPeriods(){
        return $this->periods;
    }

    /**
     * 
     * @param Collection $periods
     */
    public function setPeriods($periods) {
        $this->periods = $periods;
    }

        public function jsonSerialize() {
        $ret = parent::jsonSerialize();
        if (null == $this->loaner) {
            $this->getLoaner();
        }
        if (null == $this->workplace) {
            $this->getWorkplace();
        }
        
        if (null == $this->periods) {
            $this->getPeriods();
        }
        
        if(NULL !== $this->loaner){
            $ret['loaner'] = $this->loaner;
        } else {
            $ret['loaner'] = NULL;            
        }
        if(NULL !== $this->workplace){
            $ret['workplace'] = $this->workplace;
        } else {
            $ret['workplace'] = NULL;            
        }    
        
        $ret['workperiods'] = $this->periods->toArray(); 
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
                case 'periods':
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
                case 'periods':
                    break;
                default:
                    $params[$tag."_".$key] = $value;
                    break;
            }            
        }
        return $params;
    }    
}

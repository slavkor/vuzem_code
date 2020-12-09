<?php
namespace App\Models;

use GraphAware\Neo4j\OGM\Annotations as OGM;
use \GraphAware\Neo4j\OGM\Common\Collection;

/**
 *
 * @OGM\Node(label="Employee")
 */
class EmployeeDocumentsCertVar extends BaseEmployee implements \JsonSerializable {
    
    public function __construct() {
        parent::__construct();
        $this->documentlist = new Collection();        
    }
    
    /**
     * @var Document[]|Collection
     * 
     * @OGM\Relationship(type="ASSOCIATED_WITH", direction="OUTGOING", collection=true, mappedBy="", targetEntity="Document")
     */    
    protected $documentlist;
    
    public function getDocumentlist() {
        return $this->documentlist;
    }

    public function setDocumentlist($documentlist) {
        $this->documentlist = $documentlist;
    }

    public function jsonSerialize() {
        $ret = parent::jsonSerialize();

        if ($this->documentlist->count() > 0) {
            $ret['documents'] = $this->documentlist->filter(function($document){
                return substr($document->getType()->getName(), 0, strlen("CERT_VAR")) === "CERT_VAR"; })->toArray();
        } else {
            $ret['documents'] = NULL;
        }
        return $ret;
    }
}

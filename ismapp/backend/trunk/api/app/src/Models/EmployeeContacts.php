<?php
namespace App\Models;

use GraphAware\Neo4j\OGM\Annotations as OGM;
use \GraphAware\Neo4j\OGM\Common\Collection;

/**
 *
 * @OGM\Node(label="Employee")
 */
class EmployeeContacts extends BaseEmployee implements \JsonSerializable {
    
    public function __construct() {
        parent::__construct();
        $this->contactlist = new Collection();
    }

    /**
     * @var Contact[]|Collection
     * 
     * @OGM\Relationship(type="CONTACT", direction="OUTGOING", collection=true, mappedBy="", targetEntity="Contact")
     */    
    protected  $contactlist;
    
    public function getContactlist() : array{
        return $this->contactlist;
    }

    public function setContactlist(array $contactlist) {
        $this->contactlist = $contactlist;
    }

    public function jsonSerialize() {
        $ret = parent::jsonSerialize();
        
        if ($this->contactlist->count() > 0) {
            $ret['contacts'] = $this->contactlist->toArray();
        } else {
            $ret['contacts'] = NULL;
        }
        
        return $ret;
    }
}

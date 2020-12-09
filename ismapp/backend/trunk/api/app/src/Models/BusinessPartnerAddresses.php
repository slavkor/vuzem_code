<?php
namespace App\Models;

use GraphAware\Neo4j\OGM\Annotations as OGM;
use \GraphAware\Neo4j\OGM\Common\Collection;


/**
 *
 * @OGM\Node(label="BusinessPartner")
 */
class BusinessPartnerAddresses extends BaseBusinessPartner implements \JsonSerializable {
    
    public function __construct() {
        parent::__construct();
        $this->addresslist = new Collection();
    }

    /**
     * @var Address[]|Collection
     * 
     * @OGM\Relationship(type="LIVES", direction="OUTGOING", collection=true, mappedBy="", targetEntity="Address")
     */    
    protected $addresslist;

    public function getAddresslist(): array {
        return $this->addresslist;
    }

    public function setAddresslist(array $addresslist) {
        $this->addresslist = $addresslist;
    }

    public function jsonSerialize() {
        $ret = parent::jsonSerialize();
        
        if ($this->addresslist->count() > 0) {
            $ret['addresses'] = $this->addresslist->toArray();
        } else {
            $ret['addresses'] = NULL;
        }
        return $ret;
    }
}

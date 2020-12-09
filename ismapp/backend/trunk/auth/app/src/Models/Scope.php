<?php
namespace App\Models;

use League\OAuth2\Server\Entities\ScopeEntityInterface;



class Scope implements ScopeEntityInterface {

    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $identifier;       

    /**
     * 
     * @return string
     */
    public function getIdentifier(){
        return $this->identifier;
    }
    /**
     * 
     * @param string $identifier
     */
    public function setIdentifier($identifier){
        $this->identifier = $identifier;
    }    

    public function jsonSerialize() {
        return $this->identifier;
    }

}

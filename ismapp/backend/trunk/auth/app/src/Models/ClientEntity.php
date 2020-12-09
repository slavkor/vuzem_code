<?php
namespace App\Models;


use League\OAuth2\Server\Entities\ClientEntityInterface;
use League\OAuth2\Server\Entities\Traits\ClientTrait;
use League\OAuth2\Server\Entities\Traits\EntityTrait;



/**
 * Description of ClientEntity
 *
 * @author slavko
 */
class ClientEntity implements ClientEntityInterface {
    use EntityTrait;
    use ClientTrait;
    
    public function setName($name)
    {
        $this->name = $name;
    }

    public function setRedirectUri($uri)
    {
        $this->redirectUri = $uri;
    }
}

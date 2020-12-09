<?php
namespace App\Models;

use GraphAware\Neo4j\OGM\Annotations as OGM;
use \GraphAware\Neo4j\OGM\Common\Collection;

/**
 *
 * @OGM\Node(label="Employee")
 */
class Employee extends BaseEmployee {
    
    public function __construct() {
        parent::__construct();
    }
}

<?php


namespace App\Models;

use GraphAware\Neo4j\OGM\Annotations as OGM;
use GraphAware\Neo4j\OGM\Common\Collection;


/**
 *
 * @OGM\Node(label="File")
 */
class BaseFile extends BaseModel implements \JsonSerializable {
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
    protected $uniquename;
    
    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $path;

    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $ext;
    

    public function getName() {
        return $this->name;
    }

    public function getUniquename() {
        return $this->uniquename;
    }

    public function getPath() {
        return $this->path;
    }

    public function getExt() {
        return $this->ext;
    }

    public function setName($name) {
        $this->name = $name;
    }

    public function setUniquename($uniquename) {
        $this->uniquename = $uniquename;
    }

    public function setPath($path) {
        $this->path = $path;
    }

    public function setExt($ext) {
        $this->ext = $ext;
    }

    public function getServerFileName() {
        return $this->path.$this->uniquename;
    }


    //<editor-fold desc="JsonSerializable implementation">
    public function jsonSerialize() {
        return array_merge(
                parent::jsonSerialize(), 
                [
                    'name' => $this->name,
                    'uniquename' => $this->uniquename,
                    'path' => $this->path,
                    'ext' => $this->ext
                ]
        );
    }
    //</editor-fold>

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
        throw \Exception('no props to import into the object!');
    }
    public function tag(): string {
        $reflect = new \ReflectionClass($this);
        return $reflect->getShortName()."_".$this->tagId();
    }

    public function tagId(): string {
        return $this->tagId;
    }    //</editor-fold>   
    
}

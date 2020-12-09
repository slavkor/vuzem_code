<?php



namespace App\Models;

use GraphAware\Neo4j\OGM\Annotations as OGM;

/**
 *
 * @OGM\Node(label="Language")
 */
class Language extends BaseModel implements \JsonSerializable {
    // <editor-fold defaultstate="collapsed" desc="properties">
    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $language;

    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $code2;    

    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $code3;
    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $locale;
    
    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $language_code;  

    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $lcid_str;  

    /**
     * @var string
     * 
     * @OGM\Property(type="int")
     */
    protected $lcid_dec;  

    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $lcid_hex;

    /**
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    protected $codepage;
    
    // </editor-fold>
    
    // <editor-fold defaultstate="collapsed" desc="getters and setters">
    public function getLanguage() {
        return $this->language;
    }

    public function getCode2() {
        return $this->code2;
    }

    public function getCode3() {
        return $this->code3;
    }

    public function getLocale() {
        return $this->locale;
    }

    public function getLanguage_code() {
        return $this->language_code;
    }

    public function getLcid_str() {
        return $this->lcid_str;
    }

    public function getLcid_dec() {
        return $this->lcid_dec;
    }

    public function getLcid_hex() {
        return $this->lcid_hex;
    }

    public function getCodepage() {
        return $this->codepage;
    }

    public function setLanguage($language) {
        $this->language = $language;
    }

    public function setCode2($code2) {
        $this->code2 = $code2;
    }

    public function setCode3($code3) {
        $this->code3 = $code3;
    }

    public function setLocale($locale) {
        $this->locale = $locale;
    }

    public function setLanguage_code($language_code) {
        $this->language_code = $language_code;
    }

    public function setLcid_str($lcid_str) {
        $this->lcid_str = $lcid_str;
    }

    public function setLcid_dec($lcid_dec) {
        $this->lcid_dec = $lcid_dec;
    }

    public function setLcid_hex($lcid_hex) {
        $this->lcid_hex = $lcid_hex;
    }

    public function setCodepage($codepage) {
        $this->codepage = $codepage;
    }    
    // </editor-fold>
        
    //<editor-fold desc="BaseModel implementation">  
    public function setUuid($uuid) {
        $this->uuid = $uuid;
    }

    public function getid() {
        return $this->id;
    }

    public function getUuid() {
        return $this->uuid;
    }

    public function import($object) {
        parent::import($object);
        
        $vars=is_object($object)?get_object_vars($object):$object;

        if (!is_array($vars)) {
            throw \Exception('no props to import into the object!');
        }
        foreach ($vars as $key => $value) {
            switch ($key) {
                case 'lang':
                case 'locale':
                    $this->$key = $value;
                    break;
                default:
                    break;
            }
        }           
    }
    public function tag(): string {
        $reflect = new \ReflectionClass($this);
        return $reflect->getShortName()."_".$this->tagId();
    }

    public function tagId(): string {
        return $this->tagId;
    }
    //</editor-fold>

    // <editor-fold defaultstate="collapsed" desc="JsonSerializable">
    public function jsonSerialize() {
        $ret = array_merge(
                parent::jsonSerialize(),
                [
                    'language' => $this->language,
                    'code2' => $this->code2,
                    'code3' => $this->code3,
                    'code' => $this->language_code,
                    'lcid_dec' => $this->lcid_dec,
                    'lcid_hex' => $this->lcid_hex,
                    'lcid_str' => $this->lcid_str,
                    'locale' => $this->locale,
                    'codepage' => $this->codepage
                ]);
        
        //return["language" => $ret];
        return $ret;
        
    }
    // </editor-fold>
}

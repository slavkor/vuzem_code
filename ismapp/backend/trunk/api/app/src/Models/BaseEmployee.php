<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;


use GraphAware\Neo4j\OGM\Annotations as OGM;

class BaseEmployee extends BasePerson implements \JsonSerializable{
    
    //<editor-fold desc="public fields">
    /**
     * birthday
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    public $birthday;
    /**
     * birthday
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    public $birthplace;
    /**
     * emso
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    public $emso;  
    /**
     * taxnumber
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    public $taxnumber;
    /**
     * personalidnumber
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    public $personalidnumber;
    /**
     * iban
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    public $iban;
    /**
     * nickname
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    public $nickname;
    /**
     * position
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    public $position;

    /**
     * insunumber
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    public $insunumber;

    /**
     * email
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    public $email;

    /**
     * martialstatus
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    public $martialstatus;
    /**
     * nationality
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    public $nationality;

    /**
     * position
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    public $username;    
    //</editor-fold>

    //<editor-fold desc="setters / getters">
    public function getBirthday() {
        return $this->birthday;
    }

    public function getBirthplace() {
        return $this->birthplace;
    }

    public function getEmso() {
        return $this->emso;
    }

    public function getTaxnumber() {
        return $this->taxnumber;
    }

    public function getPersonalidnumber() {
        return $this->personalidnumber;
    }

    public function getIban() {
        return $this->iban;
    }

    public function getNickname() {
        return $this->nickname;
    }

    public function getPosition() {
        return $this->position;
    }

    public function getInsunumber() {
        return $this->insunumber;
    }

    public function setBirthday($birthday) {
        $this->birthday = $birthday;
    }

    public function setBirthplace($birthplace) {
        $this->birthplace = $birthplace;
    }

    public function setEmso($emso) {
        $this->emso = $emso;
    }

    public function setTaxnumber($taxnumber) {
        $this->taxnumber = $taxnumber;
    }

    public function setPersonalidnumber($personalidnumber) {
        $this->personalidnumber = $personalidnumber;
    }

    public function setIban($iban) {
        $this->iban = $iban;
    }

    public function setNickname($nickname) {
        $this->nickname = $nickname;
    }

    public function setPosition($position) {
        $this->position = $position;
    }

    public function setInsunumber($insunumber) {
        $this->insunumber = $insunumber;
    }
    public function getEmail() {
        return $this->email;
    }

    public function getMartialstatus() {
        return $this->martialstatus;
    }

    public function setEmail($email) {
        $this->email = $email;
    }
    public function getNationality() {
        return $this->nationality;
    }

    public function setNationality($nationality) {
        $this->nationality = $nationality;
    }

    public function setMartialstatus($martialstatus) {
        $this->martialstatus = $martialstatus;
    }

    function getUsername() {
        return $this->username;
    }

    function setUsername($username) {
        $this->username = $username;
    }

        //</editor-fold>
    
    //<editor-fold desc="JsonSerializable">
    public function jsonSerialize() {
        return array_merge(
                parent::jsonSerialize(),
                [
                    'nickname' => $this->nickname,
                    'birthday' => $this->birthday,
                    'birthplace' => $this->birthplace,
                    'emso' => $this->emso,
                    'taxnumber' => $this->taxnumber,
                    'personalidnumber' => $this->personalidnumber,
                    'iban' => $this->iban, 
                    'position' => $this->position,
                    'insunumber' => $this->insunumber,
                    'email' => $this->email,
                    'martialstatus' => $this->martialstatus,
                    'nationality' => $this->nationality,
                    'username' => $this->username
                ]);
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
    
    public function setEmployeeId($id){
        $this->setUuid($id);
    }
    
    public function import($object) {
        
        parent::import($object);
        
        $vars=is_object($object)?get_object_vars($object):$object;

        if (!is_array($vars)) {
            throw \Exception('no props to import into the object!');
        }
        foreach ($vars as $key => $value) {
            switch ($key) {
                case 'nickname':
                case 'birthday':
                case 'birthplace':
                case 'emso':
                case 'taxnumber':
                case 'personalidnumber':
                case 'iban':
                case 'position':
                case 'insunumber':
                case 'email':
                case 'martialstatus':
                case 'nationality':
                case 'username':
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
}

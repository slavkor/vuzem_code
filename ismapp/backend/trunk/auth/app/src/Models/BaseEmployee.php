<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;


use GraphAware\Neo4j\OGM\Annotations as OGM;

/**
 *
 * @OGM\Node(label="Employee")
 */
class BaseEmployee extends BaseModel implements \JsonSerializable{
    
    //<editor-fold desc="properties">
    /**
     * Id
     * @var int
     * @OGM\GraphId()
     */
    protected $id;
    /**
     * name
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    public $name;
    /**
     * lastname
     * @var string
     * 
     * @OGM\Property(type="string")
     */
    public $lastname;
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
    //</editor-fold>

    
    //<editor-fold desc="getters">
    public function getid() {
        return $this->id;
    }
    public function getname(){
        return $this->name;
    }
    public function getlastname() {
        return $this->lastname;
    }
    public function getbirthday() {
        return $this->birthday;
    }
    public function getbirthplace() {
        return $this->birthplace;
    }
    public function getemso() {
        return $this->emso;
    }
    public function gettaxnumber() {
        return $this->taxnumber;
    }
    public function getpersonalidnumber() {
        return $this->personalidnumber;
    }
    public function getiban() {
        return $this->iban;
    }
    public function getnickname() {
        return $this->nickname;
    }
    //</editor-fold>    
    //
    //<editor-fold desc="setters">    
    public function setname($name){
        $this->name = $name;
    }
    public function setlastname($lastname){
        $this->lastname = $lastname;
    }
    public function setbirthday($birthday) {
        $this->birthday = $birthday;
    }
    public function setbirthplace($birhtplace) {
        $this->birthplace = $birhtplace;
    }
    public function setemso($emso) {
        $this->emso = $emso;
    }
    public function settaxnumber($taxnumber) {
        $this->taxnumber = $taxnumber;
    }
    public function setpersonalidnumber($personalidnumber){
        $this->personalidnumber = $personalidnumber;
    }
    public function setiban($iban) {
        $this->iban = $iban;
    }
    public function setnickname($nickname) {
        $this->nickname = $nickname;
    }
    //</editor-fold>
    
    //<editor-fold desc="JsonSerializable">
    public function jsonSerialize() {
        return array_merge(
                parent::jsonSerialize(),
                [
                    'id' => $this->id,
                    'name' => $this->name,
                    'lastName' => $this->lastname,
                    'nickname' => $this->nickname,
                    'birthday' => $this->birthday,
                    'birthplace' => $this->birthplace,
                    'emso' => $this->emso,
                    'taxnumber' => $this->taxnumber,
                    'personalidnumber' => $this->personalidnumber,
                    'iban' => $this->iban
                ]);
    }
    //</editor-fold>
    
    //<editor-fold desc="BaseModel implementation">
    public function getuuid() {
        return $this->uuid;
    }
    protected function setuuid($uuid) {
        $this->uuid = $uuid;
    }
    //</editor-fold>
}

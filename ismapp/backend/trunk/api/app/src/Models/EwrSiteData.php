<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;

/**
 * Description of EwrSiteData
 *
 * @author slavko
 */
class EwrSiteData extends ProjectSiteData  implements \JsonSerializable {
    
    private $ewrnumber;
    private $ewrexternalnumber;
    private $ewrhours;
    private $ewrmaterialcosts;
    private $ewrdescription;
    
    function getEwrnumber() {
        return $this->ewrnumber;
    }

    function getEwrexternalnumber() {
        return $this->ewrexternalnumber;
    }

    function getEwrhours() {
        return $this->ewrhours;
    }

    function getEwrmaterialcosts() {
        return $this->ewrmaterialcosts;
    }

    function getEwrdescription() {
        return $this->ewrdescription;
    }

    function setEwrnumber($ewrnumber) {
        $this->ewrnumber = $ewrnumber;
    }

    function setEwrexternalnumber($ewrexternalnumber) {
        $this->ewrexternalnumber = $ewrexternalnumber;
    }

    function setEwrhours($ewrhours) {
        $this->ewrhours = $ewrhours;
    }

    function setEwrmaterialcosts($ewrmaterialcosts) {
        $this->ewrmaterialcosts = $ewrmaterialcosts;
    }

    function setEwrdescription($ewrdescription) {
        $this->ewrdescription = $ewrdescription;
    }

    public function jsonSerialize() {
        
        
        return array_merge(
                parent::jsonSerialize(),
                [
                    "ewrnumber" => $this->ewrnumber,
                    "ewrexternalnumber" => $this->ewrexternalnumber,
                    "ewrdescription" => $this->ewrdescription,
                    "ewrhours" => $this->ewrhours,
                    "ewrmaterialcosts" => $this->ewrmaterialcosts
                ]);
    }
}

<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;

/**
 * Description of ProjectSiteData
 *
 * @author slavko
 */
class ProjectSiteData implements \JsonSerializable{
    
    protected $sitename;
    protected $sitedescription;
    protected $sitestatus;
    protected $projectname;
    protected $projectdescription;
    protected $projectnumber;
    protected $externalnumber;
    protected $projectstate;
    protected $projectstatus;
    protected $partnerlogo;
    protected $sitelogo;
           
    function getSitename() {
        return $this->sitename;
    }

    function getSitedescription() {
        return $this->sitedescription;
    }

    function getSitestatus() {
        return $this->sitestatus;
    }

    function getProjectname() {
        return $this->projectname;
    }

    function getProjectdescription() {
        return $this->projectdescription;
    }

    function getProjectnumber() {
        return $this->projectnumber;
    }

    function getExternalnumber() {
        return $this->externalnumber;
    }

    function getProjectstate() {
        return $this->projectstate;
    }

    function getProjectstatus() {
        return $this->projectstatus;
    }

    function getPartnerlogo() {
        return $this->partnerlogo;
    }

    function getSitelogo() {
        return $this->sitelogo;
    }

    function setSitename($sitename) {
        $this->sitename = $sitename;
    }

    function setSitedescription($sitedescription) {
        $this->sitedescription = $sitedescription;
    }

    function setSitestatus($sitestatus) {
        $this->sitestatus = $sitestatus;
    }

    function setProjectname($projectname) {
        $this->projectname = $projectname;
    }

    function setProjectdescription($projectdescription) {
        $this->projectdescription = $projectdescription;
    }

    function setProjectnumber($projectnumber) {
        $this->projectnumber = $projectnumber;
    }

    function setExternalnumber($externalnumber) {
        $this->externalnumber = $externalnumber;
    }

    function setProjectstate($projectstate) {
        $this->projectstate = $projectstate;
    }

    function setProjectstatus($projectstatus) {
        $this->projectstatus = $projectstatus;
    }

    function setPartnerlogo($partnerlogo) {
        $this->partnerlogo = $partnerlogo;
    }

    function setSitelogo($sitelogo) {
        $this->sitelogo = $sitelogo;
    }

        
    public function jsonSerialize() {
        return [
            "sitename" => $this->sitename,
            "sitedescription" => $this->sitedescription,
            "sitestatus" => $this->sitestatus,
            "projectname" => $this->projectname,
            "projectdescription" => $this->projectdescription,
            "projectnumber" => $this->projectnumber,
            "externalnumber" => $this->externalnumber,
            "projectstate" => $this->projectstate,
            "projectstatus" => $this->projectstatus,
            "partnerlogo" => $this->partnerlogo,
            "sitelogo" => $this->sitelogo,
        ];
    }

}

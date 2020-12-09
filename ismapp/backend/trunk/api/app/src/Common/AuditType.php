<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Common;

/**
 * Description of AuditType
 *
 * @author slavko
 */
class AuditType extends Enum {
    const NONE = 0;
    const GET = 1;
    const CREATE = 2;
    const UPDATE = 3;
    const DELETE = 4;
    const ACTIVATE = 5;
    const DEACTIVATE = 6;
}

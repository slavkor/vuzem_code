<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Models;

use App\Models\BaseModel;
use App\Common\ApiException;

class ModelType {
    //put your code here
    public static function InstaceOfBaseModel($object){
        if (!($object instanceof BaseModel)) {
            throw ApiException::serverError('Expecting a type of BaseModel');
        }
    }
}

<?php
error_reporting(E_ALL & ~E_NOTICE);

use Slim\App;

define('FILEPATH', '/mnt/app/app_files/');
define("DEV", FALSE);

//define('FILEPATH',dirname(__FILE__, 3)."\\files");
//define("DEV", TRUE);

// To help the built-in PHP dev server, check if the request was actually for
// something which should probably be served as a static file
if (PHP_SAPI === 'cli-server' && $_SERVER['SCRIPT_FILENAME'] !== __FILE__) {
    return false; 
}

date_default_timezone_set('Europe/Ljubljana');

require __DIR__.'/../vendor/autoload.php';

// Instantiate the app
$settings = require __DIR__.'/settings.php';

//var_dump($settings);

$app = new App($settings);

// Register middleware
require __DIR__.'/middleware.php';
// Set up dependencies
require __DIR__.'/dependencies.php';

// Register routes
require __DIR__.'/routes.php';
require __DIR__.'/routesEmployee.php';
require __DIR__.'/routesReports.php';
require __DIR__.'/routesProject.php';
require __DIR__.'/routesMobile.php';


// Run!
$app->run();
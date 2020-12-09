<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Middleware;

namespace App\Middleware;
use Slim\Http\Request;
use Slim\Http\Response;
use Slim\App;
use App\Common\ApiException;
use Psr\Log\LoggerInterface;
use App\Repository\DepartureArrivalRepository;
use PHPMailer\PHPMailer\PHPMailer;
use App\Models\Departure;
use JsonMapper;

//require_once __DIR__.'/../../../vendor/autoload.php';
class DeparturePlanMailer {
    /**
     * @var Slim\App
     */
    private $app;
    private $settings;
    protected $logger;
    protected $repository;
    protected $mapper;

    public function __construct($settings, LoggerInterface $logger, DepartureArrivalRepository $repository) {
        //$this->app = $app;
        $this->settings = $settings;
        $this->logger = $logger;
        $this->repository = $repository;
        $this->mapper = new JsonMapper();
        $this->mapper->setLogger($logger);
        $this->mapper->bStrictNullTypes = FALSE;
    }
    
    public function __invoke(Request $request, Response $response, callable $next)
    {
        return $next($request, $response); 
        //$this->logger->info(_       $this->connection = $this->settings['dbclient']['type'].'://'.$this->settings['dbclient']['username'].':'.$this->settings['dbclient']['password'].'@'. $this->settings['dbclient']['host'].':'.$this->settings['dbclient']['port'];
        $mail = new \PHPMailer\PHPMailer\PHPMailer(true);
       
        
        try {
             //Server settings
            //$mail->SMTPDebug = 2;                                 // Enable verbose debug output
            $mail->isSMTP(); // Set mailer to use SMTP
            $mail->Hostname  = $this->settings['mailer']['host'];
            $mail->Host = $this->settings['mailer']['host'];  // Specify main and backup SMTP servers
            $mail->SMTPAuth = true;                               // Enable SMTP authentication
            $mail->SMTPAutoTLS = FALSE;
            $mail->Username = $this->settings['mailer']['username'];                 // SMTP username
            $mail->Password = $this->settings['mailer']['password'];                           // SMTP password
            $mail->Port = $this->settings['mailer']['port'];                                    // TCP port to connect to

            //Recipients
            $mail->setFrom($this->settings['mailer']['from'], 'Admin');
            $mail->addAddress('slavko@ismvuzem.si');     // Add a recipient
            $mail->addAddress('slavko@rihtas.si');     // Add a recipient

            $recipients = $this->repository->planNotifyMailAddresses($request, $args);

            
            //Content
            $mail->isHTML(true);                                  // Set email format to HTML
            $mail->Subject = 'Here is the subject';
            $mail->Body    = 'This is the HTML message body <b>in bold!</b>';
            $mail->AltBody = 'This is the body in plain text for non-HTML mail clients';

            $mail->send();


            return $next($request, $response); 
        } 
        catch (ApiException $exc) {
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(json_encode(array(
                'error' => array(
                    'msg' => $exc->getMessage(),
                    'code' => $exc->getCode(),
                ),
            )), 500);
        }
        
        return $next($request, $response); 
    }
}

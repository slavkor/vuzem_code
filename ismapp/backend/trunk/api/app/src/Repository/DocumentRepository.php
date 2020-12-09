<?php

namespace App\Repository;

use Slim\Http\Request;
use Slim\Http\Response;
use Psr\Log\LoggerInterface;
use GraphAware\Neo4j\Client\Client;
use GraphAware\Neo4j\Client\ClientBuilder;
use GraphAware\Neo4j\OGM\EntityManager;

use App\Models\Document;
use App\Models\User;
use App\Models\DocumentType;
use App\Models\Employee;
use App\Models\File;
use App\Models\Language;
use App\Common\ApiException;
use App\Models\DocumentAdd;
use App\Models\Company;


use App\Models\EmployeeDocumentsCertVar;
use PHPMailer\PHPMailer\PHPMailer;

/**
 * Description of DocumentRepository
 *
 * @author slavko
 */
class DocumentRepository    {
    protected $logger;
    protected $dbclient;
    protected $dbentity;
    protected $mapper;
    protected $settings;

    protected $employeeRepository;
    protected $files;
    protected $doctypes;
    private $connection;
    private $connalias;
    private $languages;
    
    public function __construct(LoggerInterface $logger, \Slim\Collection $settings){
        $this->logger = $logger;
        $this->mapper = new \JsonMapper();
        $this->mapper->bStrictNullTypes = FALSE;
        //$this->mapper->setLogger($this->logger);
        $this->settings = $settings;
        
        $this->connection = $this->settings['dbclient']['type'].'://'.$this->settings['dbclient']['username'].':'.$this->settings['dbclient']['password'].'@'. $this->settings['dbclient']['host'].':'.$this->settings['dbclient']['port'];
        $this->connalias = $this->settings['dbclient']['name'];
        
        $this->dbclient = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
        $this->dbentity  = EntityManager::create($this->connection);

        $this->employeeRepository = $this->dbentity->getRepository(Employee::class);
        $this->files = $this->dbentity->getRepository(File::class);
        $this->doctypes = $this->dbentity->getRepository(DocumentType::class);
        $this->languages = $this->dbentity->getRepository(Language::class);
    }  
    
    public function getAllDocuments($nodeId){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        if(NULL !== $nodeId)
        {
            $c = "match(d:Document)-[ATTACHED]-(f:File) return d";
        }
        else
        {
            $c = "match(n{uuid:{nodeId}})-[r:ASSOCIATED_WITH]->(d:Document)-[ATTACHED]->(f:File) return d";
        }
        
        $query = $this->dbentity->createQuery($c);
        $query->addEntityMapping("d", Document::class);
        $query->setParameter("nodeId", $nodeId);
        
        $documents = $query->getResult();

        return $documents;
    }
    
    
    public function notifyExpireableOnDate(Request $request, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $jsonData = json_decode($request->getBody());  
        
        $c = "match (t:DocumentType{expirable:true}) with t return distinct id(t) as id, t.name as tip, t.notificationmail as notificationmail, coalesce(t.notifybeforedays, 0) as notifybeforedays";        
        $query = $this->dbentity->createQuery($c);      
        foreach ($query->getResult() as $result) {
            $sendmail = false;
            
            $date = (new \DateTime($jsonData->{'date'}))->add(new \DateInterval('P'.$result["notifybeforedays"].'D'));
            
            $mail = new \PHPMailer\PHPMailer\PHPMailer(true);
            $mail->CharSet = 'UTF-8';
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
            $recipients = explode(";", $result["notificationmail"] );
            foreach($recipients as $recipient)
            {
               $mail->addAddress($recipient);
            }
            
            $mail->addCC('slavko@ismvuzem.si');
            
            //Content
            $mail->isHTML(true);                                  // Set email format to HTML
            $mail->Subject = 'Potečeni dokumenti tipa ' . $result["tip"] . ' na dan ' . $date->format('d.m.Y');
            $mail->Body    = '<!DOCTYPE html>'
                    . '<html>'
                    . '<head>'
                    . '<style>'
                    . 'table '
                    . '{'
                    . 'border-collapse: collapse;'
                    . 'border: 1px solid black;'
                    . '}'
                    . '</style>'
                    . '</head>'
                    . '<body>'
                    . 'Čez <b> '.$result["notifybeforedays"].' </b> dni, na dan <b>' . $date->format('d.m.Y') .  '</b> potečejo dokumenti tipa <b>' . $result["tip"] . '</b> naslednjim zaposlenim'
                    . '<br/><br/>'
                    . '<table>'
                    . '<tbody>'
                    . '<tr>'
                    . '<td>&nbsp;Zaposleni</td>'
                    . '<td>&nbsp;Dokument</td>'
                    . '<td>&nbsp;Tip</td>'
                    . '<td>&nbsp;Podjetje</td>'
                    . '<td>&nbsp;Dokument ustvaril</td>'
                    . '</tr>';

            $mail->AltBody = 'Na dan ' . $date->format('d.m.Y') .  ' potečejo dokumenti tipa ' . $result["tip"] .  ' naslednjim zaposlenim \n\n';

            $c = "match (d:Document{active:1, deleted:0})-[:IS_OF_TYPE]->(t:DocumentType{expirable:true}) where id(t) = {type} and not ((d)-[:NEXT]-()) with d, t
            match (:Year{value:{year}})-[:CONTAINS]->(:Month{value:{month}})-[:CONTAINS]->(dt:Day{value:{day}})<-[:VALID_TO]-(d) with d, t, dt
            match (c:Company)<-[:ASSOCIATED_WITH]-(d)<-[:ASSOCIATED_WITH]-(e:Employee)-[:WORKS_IN]->(:WorkPeriod{active:1}) with d, t, dt, c, e
            match (d)<-[:CREATED]-(u:User) with d, t, dt, c, e, u
            return t.name as tip, d.name as dokument, c.shortname as podjetje, e.lastname + ' ' + e.name as zaposleni, u.username as user";
            
            $querydocs = $this->dbentity->createQuery($c);
            $querydocs->setParameter("year", (int)$date->format('Y'));
            $querydocs->setParameter("month", (int)$date->format('m'));
            $querydocs->setParameter("day", (int)$date->format('d'));
            $querydocs->setParameter("type", (int)$result["id"]);
            
            foreach ($querydocs->getResult() as $doc) {
                
                 $mail->Body    .= '<tr>'
                         . '<td>&nbsp;' . $doc['zaposleni'] . '</td>'
                         . '<td>&nbsp;' . $doc['dokument'] . '</td>'
                         . '<td>&nbsp;' . $doc['tip'] . '</td>'
                         . '<td>&nbsp;' . $doc['podjetje'] . '</td>'
                         . '<td>&nbsp;' . $doc['user'] . '</td>'
                         . '</tr>';
                 
                 $mail->AltBody .= '' . $doc['zaposleni'] .  ' \t\t ' . $result["tip"] .  ' \t\t ' . $doc['podjetje'] . ' \t\t ' . $doc['user'] . '\n';
                 $sendmail = true;
            }
            
            $mail->Body .= '</tbody>'
                    . '</table>'
                    . '<p>&nbsp;</p> '
                    . '<br/><br/>'. date("d.m.yy H:i:s")
                    . '</body>'
                    . '</html>';
            $mail->AltBody .= '\n\n'. date("d.mm.yyyy H:i:s");
            
            
            if($sendmail === true) {$mail->send();}
        }
    }    
    
    
    
    public function getByTypeForEmployee(Request $request, Response $response, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $c="match (:WorkPeriod{active:1, deleted:0})--(e:Employee)-[:ASSOCIATED_WITH]->(d:Document{deleted:0})-[:IS_OF_TYPE]->(t:DocumentType)
            where t.name contains {type} and not ((d)-[:NEXT]->(:Document))
            with e, d
            optional match (d)-[:VALID_FROM]->(f:Day)
            with e, d, f
            optional match (d)-[:VALID_TO]->(td:Day)
            with e, d, f, td
            optional match (d)-[:DOCUMENTDATE]->(dd:Day)
            with e, d, f, td, dd            
            return e.uuid as uniqueid, e.lastname as lastname, e.name as name, 
            d.name as document, d.description as description, d.documentnumber as documentnumber, 
            dd.strrep as docdate, f.strrep as from, td.strrep as to, d.expiredate as expiredate
            order by e.lastname, e.name, to desc";
        
        $query = $this->dbentity->createQuery($c);
        $query->setParameter("type", $args["type"]);

        return $query->getResult();
    }
   public function activate(Request $request, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $c="match(d:Document{uuid:{docid}}) set d.deleted=0, d.active=1 return d ";
        
        $query = $this->dbentity->createQuery($c);
        $query->setParameter("docid", $args["id"]);

        $query->execute();
    }

    public function getAllDocumentsOfType($nodeId, $typeId){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $documentRepository = $this->dbentity->getRepository(Document::class);
        $documents = $documentRepository->findAll();

        foreach ($documents as $doc) {

            $doc->getFiles();
//            var_dump(json_encode($doc));
//            PHP_EOL;
//            PHP_EOL;
//            PHP_EOL;
//            die;
        }
        return $documents;
    }
    public function initDocument($firmId, $jsonData, $username, $typeId) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $this->mapper->bStrictNullTypes = FALSE;

        $usersRepository = $this->dbentity->getRepository(User::class);
        $user = $usersRepository->findOneBy(['username'=>$username ]);
        
        if (NULL === $user) { return; }

        $document = new Document();
        $parent = $jsonData->{'parentuuid'};
        $this->mapper->map($jsonData->{'document'}, $document);

        if(null == $document){
            throw ApiException::serverError('No dokument provided!');
        }

        $company = $this->dbentity->getRepository(Company::class)->findOneBy(["uuid" => $firmId]);
        $type = $this->doctypes->findOneBy(["uuid" => $document->getType()->getUuid()]);       

        $from = null;
        if(null !==$document->getValidFrom()  ) $from = $this->dbentity->getRepository(\App\Models\Day::class)->findOneBy(["strrep" => $document->getValidFrom()->getDate()->format("Ymd")]);
        $to = null ;
        if(null !==$document->getValidTo()  ) $to = $this->dbentity->getRepository(\App\Models\Day::class)->findOneBy(["strrep" => $document->getValidTo()->getDate()->format("Ymd")]);
        $dd = null ;
        if(null !==$document->getDocdate()  ) $dd =  $this->dbentity->getRepository(\App\Models\Day::class)->findOneBy(["strrep" => $document->getDocdate()->getDate()->format("Ymd")]);
       
        $pdoc = null;
        if($parent !== $document->getUuid()) $pdoc = $this->dbentity->getRepository(Document::class)->findOneBy(["uuid" => $parent]);

        $document->setCompany($company);
        $document->setType($type);
        $document->setUser($user);

        $document->setDocdate($dd);

        $document->setValidFrom(NULL);
        $document->setValidTo(NULL);
        if($type->getExpirable()){
            $document->setValidFrom($from);
            $document->setValidTo($to);
        }
        
        $this->dbentity->persist($document);
        if(null !== $pdoc){
            $pdoc->setNext($document);
            $this->dbentity->persist($pdoc);
            
        }
        $this->dbentity->flush();
        return $document;
    }
    
    public function addDocumentFile($uuid, File $file, Language $language, $username){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        $documentRepository = $this->dbentity->getRepository(Document::class);
        $document = $documentRepository->findOneBy(['uuid' => $uuid]);
        
        if (empty($document) || $document === NULL) {
            throw new \Exception('Document '.$uuid.' not found');
        }

        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
        $stack = $client->stack();
        
        $query = 'match(d:Document{uuid:{docId}})
            create (f:File{'. $file->getQueryFields() .'})
            create (d)-[r:ATTACHED]->(f)';

        $stack->push($query, array_merge(['docId' => $document->getUuid()], $file->getQueryFieldsParams()));

        if(NULL != $language)
        {
            
            $query = 'match(l:Language{uuid:{langId}})
                match(f:File{uuid:{fileId}})
                create (f)-[r:IS_LANG]->(l)';

            $stack->push($query, [
            'langId' => $language->getUuid(),
            'fileId' => $file->getUuid()]);
            
        }
        
        $usersRepository = $this->dbentity->getRepository(User::class);
        $user = $usersRepository->findOneBy(['username'=> $username]);
        
        if($user !== NULL){
            $query = 'match(u:User{uuid:{userId}})
                match(f:File{uuid:{fileId}})
                create (u)-[r:UPLOAD]->(f)';

            $stack->push($query, [
            'userId' => $user->getUuid(),
            'fileId' => $file->getUuid()]);            
        }
        
        $client->runStack($stack);

        $client->transaction()->commit();
        
        return $this->getFile($file->getUuid());
    }
    
    public function finishDocument($jsonData) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        $documentRepository = $this->dbentity->getRepository(Document::class);
        $document = $documentRepository->findOneBy(['uuid' => $jsonData->{'uuid'}]);
        
        if (empty($document) || $document === NULL) {
            throw new \Exception('Document '.$uuid.' not found');
        }

        //$document->setactive(1);
        //$this->dbentity->flush();

        return $document;        
    }   
    public function updateDocument($jsonData, $username) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $this->mapper->bStrictNullTypes = FALSE;

        $document = new Document();
        $this->mapper->map($jsonData, $document);

        $usersRepository = $this->dbentity->getRepository(User::class);
        $user = $usersRepository->findOneBy(['username'=> $username]);

        if (NULL === $user) {
            return;
        }

        $document->setDocumentId($jsonData->{'uuid'});    

        $existing = $this->getDocument($document->getUuid());
        

        
        if (NULL == $existing) {
            throw ApiException::serverError('Document not found');
        }

        $existing->getValidFrom();
        $existing->getValidTo();
        $existing->getDocdate();

        $from = null;
        if(null !==$document->getValidFrom()  ) $from = $this->dbentity->getRepository(\App\Models\Day::class)->findOneBy(["strrep" => $document->getValidFrom()->getDate()->format("Ymd")]);
        $to = null ;
        if(null !==$document->getValidTo()  ) $to = $this->dbentity->getRepository(\App\Models\Day::class)->findOneBy(["strrep" => $document->getValidTo()->getDate()->format("Ymd")]);
        $dd = null ;
        if(null !==$document->getDocdate()  ) $dd =  $this->dbentity->getRepository(\App\Models\Day::class)->findOneBy(["strrep" => $document->getDocdate()->getDate()->format("Ymd")]);
  
        
        $ex  = NULL;
        $ex = $document->getExpiredate();
        
        $existing->setValidFrom($from);
        $existing->setValidTo($to);
        $existing->setDocdate($dd);
        $existing->setExpiredate($ex);

        $existing->setAmount($document->getAmount());
        $existing->setAmountwot($document->getAmountwot());
        $existing->setAmountwt($document->getAmountwt());
        $existing->setDocumentnumber($document->getDocumentnumber());
        $existing->setDescription($document->getDescription());
        $existing->setName($document->getName());
        
                
        $this->dbentity->persist($existing);
        $this->dbentity->flush();
                
        return $existing;
    }
    
    public function getDocument($id){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $documentRepository = $this->dbentity->getRepository(Document::class);
        $document = $documentRepository->findOneBy(['uuid' => $id]);

        if (empty($document) || $document === NULL) {
            return;
        }
        
        if(null !== $document->getFiles()){
            foreach ($document->getFiles() as $file) {
                $file->getLanguage();
            }
        }

        return $document;
    }
    
    public function getFile($id){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $files = $this->dbentity->getRepository(File::class);
        
        $file = $files->findOneBy(['uuid' => $id]);
        if (empty($file) || $file === NULL) {
            return;
        }
        return $file;
    }    
    
    public function deleteFile($id, $fileid, $username){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        $client = ClientBuilder::create()->addConnection($this->connalias, $this->connection)->build();
        
        $stack = $client->stack();
        
        $query = "match(d:Document{uuid:{d}})-[r:ATTACHED]-(f:File{uuid:{f}})
            delete r
            create(d)-[r1:WAS_ATTACHED]->(f)";

        $stack->push($query ,['d' => $id,'f' => $fileid, 'mf' => $validFrom['month'], 'df' => $validFrom['day']]);

        $client->runStack($stack);
        $client->transaction()->commit();
        return $this->getDocument($id);
    }    
    
    public function getDocumentFile($id, $fileid, $username){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

        $documentRepository = $this->dbentity->getRepository(Document::class);
        $document = $documentRepository->findOneBy(['uuid' => $id]);
        if (empty($document) || $document === NULL) {
            return;
        }
       
        $fileRepository = $this->dbentity->getRepository(File::class);
        $file = $fileRepository->findOneBy(['uuid' => $fileid]);
        if (empty($document) || $document === NULL) {
            return;
        }
        
        /*
        $usersRepository = $this->dbentity->getRepository(User::class);
        $user = $usersRepository->findOneBy(['username'=> $username]);
        $user->getdocumentsdownloaded()->add($document);
        $this->dbentity->flush();
        */
         
        return $file;
    }  
    
    

    public function addDocumentType($jsonData){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        if (NULL === $jsonData) {
            return;
        }

        $documentType = $this->mapper->map($jsonData, new DocumentType());

        if (NULL === $documentType) {
            return;
        }

        $documentTypeRepository = $this->dbentity->getRepository(DocumentType::class);
        $existing = $documentTypeRepository->findOneBy(['name' => $documentType->getname()]);
        
        if (NULL != $existing) {
            return;
        }

        $this->dbentity->persist($documentType);
        $this->dbentity->flush();
        return $documentType;
        
    }
    public function updateDocumentType($jsonData){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        
        if (NULL === $jsonData) {
            return;
        }

        $documentType = $this->mapper->map($jsonData, new DocumentType());

        if (NULL === $documentType) {
            return;
        }

        $documentTypeRepository = $this->dbentity->getRepository(DocumentType::class);
        $existing = $documentTypeRepository->findOneBy(['name' => $documentType->getname()]);
        
        if (NULL == $existing) {
            return;
        }

        $existing->import($documentType);
        $this->dbentity->persist($existing);
        $this->dbentity->flush();
        return $existing;
        
    }    
    
    public function getDocumentType($name) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        return $this->doctypes->findOneBy(['name' => $name]);
    }
    
    public function getDocumentTypeByUuid($uuid) {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        return $this->doctypes->findOneBy(['uuid' => $uuid]);
    }
    
    public function getAllTypes() {
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        return $this->doctypes->findAll();
    }    
    
    public function getLanguage($uuid){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        return $this->languages->findOneBy(['uuid' => $uuid]);
    }
    
    
}

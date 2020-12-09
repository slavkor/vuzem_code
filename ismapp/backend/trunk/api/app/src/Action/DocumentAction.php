<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Action;

use Psr\Log\LoggerInterface;
use Slim\Http\Request;
use Slim\Http\Response;
use Slim\Http\Stream;
use Slim\App;

use App\Models\File;
use App\Repository\DocumentRepository;

/**
 * Description of FileAction
 *
 * @author slavko
 */
final class DocumentAction {
    /**
     * @var Slim\App
     */
    private $app;

    /**
     * @var \Psr\Log\LoggerInterface
     */
    private $logger;
    
    /**
     *
     * @var \App\Repository\DocumentRepository
     */
    private $repository;
    
    //public function __construct(App $app, LoggerInterface $logger, DocumentRepository $repository) {
    public function __construct(LoggerInterface $logger, DocumentRepository $repository) {
        //$this->app  = $app;
        $this->logger = $logger;
        $this->repository = $repository;
    }

    public function initUpload(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            
            if (empty($request->getAttribute('oauth_user_id')) || $request->getAttribute('oauth_user_id') === NULL) { return $response->withStatus(401);}
            
            $jsonData = json_decode($request->getBody());        
            if (json_last_error() !== JSON_ERROR_NONE) {
                throw ApiException::json_decodeError();
            }
            
            $jsonData->{'path'} = FILEPATH;
            
            $firmid = NULL;
            if(array_key_exists('firmid', $args))
            {
                $firmid = $args['firmid'];
            }
            
            $document = $this->repository->initDocument($firmid, $jsonData, $request->getAttribute('oauth_user_id'), $jsonData->{'document'}->{'type'}->{'uuid'});

            return $response->withJson($document);
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }

    public function uploadFile(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            if (empty($request->getAttribute('oauth_user_id')) || $request->getAttribute('oauth_user_id') === NULL) { return $response->withStatus(401);}

            $uniqueName = $request->getParam('uniquename');
            if (empty($uniqueName)) { throw new \Exception('Expected a uniqueName'); }
            $uuid = $request->getParam('uuid');
            if (empty($uuid)) { throw new \Exception('Expected a uuid'); }

            $lang = $request->getParam('language');
            
            $files = $request->getUploadedFiles();
            if (empty($files[$uniqueName])) { throw new \Exception('Expected a '. $uniqueName); }

            $newfile = $files[$uniqueName];
            $uploadErr = $newfile->getError();

            if ($uploadErr === UPLOAD_ERR_OK) {
                $path = FILEPATH.$request->getAttribute('oauth_user_id')."/". idate('Y').'/';
                $this->createFolder($path);
                $file = new File();
                $file->setName($newfile->getClientFilename());
                $file->setPath($path);
                $newfile->moveTo($path.$file->getUniquename());
            }
            else{
                throw new \Exception('Upload Error '. $uploadErr);
            }
            $file = $this->repository->addDocumentFile($uuid, $file, $this->repository->getLanguage($lang), 
                    $request->getAttribute('oauth_user_id'));
            
            return $response->withJson($file);
            
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    
    public function finishUpload(Request $request, Response $response, $args){
        try {
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            if (empty($request->getAttribute('oauth_user_id')) || $request->getAttribute('oauth_user_id') === NULL) { return $response->withStatus(401);}

            $jsonData = json_decode($request->getBody());        
            if (json_last_error() !== JSON_ERROR_NONE) {
                throw ApiException::json_decodeError();
            }
            
            $doc = $this->repository->finishDocument($jsonData);
            $response = $response->withJson($doc);
            return $response;
            
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }

    public function updateDocument(Request $request, Response $response, $args){
        try{
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            if (empty($request->getAttribute('oauth_user_id')) || $request->getAttribute('oauth_user_id') === NULL) { return $response->withStatus(401);}

            $jsonData = json_decode($request->getBody());
            if (json_last_error() !== JSON_ERROR_NONE) {
                throw ApiException::json_decodeError();
            }
            if (NULL === $jsonData) {
                throw ApiException::serverError('No data povided');
            }
            
            return $response->withJson($this->repository->updateDocument($jsonData, $request->getAttribute('oauth_user_id')));
            
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }    
    }
    
    public function getAll(Request $request, Response $response, $args){
        try{
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            $docs = $this->repository->getAllDocuments();
            $response = $response->withJson($docs);
        return $response;
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }        
    }
    public function notifyExpireableOnDate(Request $request, Response $response, $args){
        try{
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            $response = $response->withJson($this->repository->notifyExpireableOnDate($request, $args));
        return $response;
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }        
    }
    
    public function getByTypeForEmployee(Request $request, Response $response, $args){
        try{
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            return $response->withJson($this->repository->getByTypeForEmployee($request, $response, $args));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }        
    }
    
    
    public function getDocument(Request $request, Response $response, $args){
        try{
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            if (empty($request->getAttribute('oauth_user_id')) || $request->getAttribute('oauth_user_id') === NULL) { return $response->withJson(array('message' => 'Not authorized'), 403);}

            return $response->withJson($this->repository->getDocument($args['id']), 200);
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }        
    }    
    public function activate(Request $request, Response $response, $args) {  
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        try{
            return $response->withJson($this->repository->activate($request, $args));
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }   
    }
    
    
    public function deleteFile(Request $request, Response $response, $args) {
        try{
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            if (empty($request->getAttribute('oauth_user_id')) || $request->getAttribute('oauth_user_id') === NULL) { return $response->withJson(array('message' => 'Not authorized'), 403);}

            $file = $this->repository->deleteFile($args['id'], $args['fid'], $request->getAttribute('oauth_user_id'));

            if(empty($file) || $file === NULL){
                return $response->withJson(array('message' => 'File not found'), 404);
            }
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    
    
    public function download(Request $request, Response $response, $args) {
        try{
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            if (empty($request->getAttribute('oauth_user_id')) || $request->getAttribute('oauth_user_id') === NULL) { return $response->withJson(array('message' => 'Not authorized'), 403);}

            //$file = $this->repository->getDocumentFile($args['id'], $args['fid'], $request->getAttribute('oauth_user_id'));
            $file = $this->repository->getDocumentFile($args['id'], $args['fid'], $request->getAttribute('oauth_user_id'));

            if(empty($file) || $file === NULL){
                return $response->withJson(array('message' => 'File not found'), 404);
            }
            

            $fh = fopen($file->getServerFileName(), 'rb');

            if(!$fh){
                throw \App\Common\ApiException::serverError("file not found");
            }

            $stream = new Stream($fh); // create a stream instance for the response body

            return $response->withHeader('Content-Type', 'application/force-download')
                            ->withHeader('Content-Type', 'application/octet-stream')
                            ->withHeader('Content-Type', 'application/download')
                            ->withHeader('Content-Description', 'File Transfer')
                            ->withHeader('Content-Transfer-Encoding', 'binary')
                            ->withHeader('Content-Disposition', 'attachment; filename="' . $file->getName() . '"')
                            ->withHeader('Expires', '0')
                            ->withHeader('Cache-Control', 'must-revalidate, post-check=0, pre-check=0')
                            ->withHeader('Pragma', 'public')
                            ->withBody($stream); // all stream contents will be sent to the response        
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }
    public function downloadFile(Request $request, Response $response, $args) {
        try{
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
            $file = $this->repository->getFile($args['fid']);

            if(empty($file) || $file === NULL){
                return $response->withJson(array('message' => 'File not found'), 404);
            }

            $fh = fopen($file->getServerFileName(), 'rb');
            if(!$fh){
                throw \App\Common\ApiException::serverError("file not found");
            }

            $stream = new Stream($fh); // create a stream instance for the response body

            return $response->withHeader('Content-Type', 'application/force-download')
                            ->withHeader('Content-Type', 'application/octet-stream')
                            ->withHeader('Content-Type', 'application/download')
                            ->withHeader('Content-Description', 'File Transfer')
                            ->withHeader('Content-Transfer-Encoding', 'binary')
                            ->withHeader('Content-Disposition', 'attachment; filename="' . $file->getName() . '"')
                            ->withHeader('Expires', '0')
                            ->withHeader('Cache-Control', 'must-revalidate, post-check=0, pre-check=0')
                            ->withHeader('Pragma', 'public')
                            ->withBody($stream); // all stream contents will be sent to the response        
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }
    }    
    
    public function addDocumentType(Request $request, Response $response) {
        try{
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

            if (empty($request->getAttribute('oauth_user_id')) || $request->getAttribute('oauth_user_id') === NULL) {
                return $response->withJson(array('message' => 'Not authorized'), 403);
            }
            $jsonData = json_decode($request->getBody());
            if (json_last_error() !== JSON_ERROR_NONE) {
                throw ApiException::json_decodeError();
            }

            if (NULL === $jsonData) {
                return;
            }

            $documentType = $this->repository->addDocumentType($jsonData);
            return $response->withJson($documentType);
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }        
    }

    public function updateDocumentType(Request $request, Response $response) {
        try{
            if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}

            if (empty($request->getAttribute('oauth_user_id')) || $request->getAttribute('oauth_user_id') === NULL) {
                return $response->withJson(array('message' => 'Not authorized'), 403);
            }
            $jsonData = json_decode($request->getBody());
            if (json_last_error() !== JSON_ERROR_NONE) {
                throw ApiException::json_decodeError();
            }

            if (NULL === $jsonData) {
                return;
            }

            $documentType = $this->repository->updateDocumentType($jsonData);
            return $response->withJson($documentType);
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }        
    }    

    
    public function getAllTypes(Request $request, Response $response, $args) {  
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        try{
            return $response->withJson($this->repository->getAllTypes());
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }   
    }
    
    private function getType($jsonData){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        try{
            if (NULL === $jsonData) {
                return;
            }

            if(NULL === $jsonData['type']){
                $type = $this->repository->getDocumentType('UNDEFINED');
            }
            else {
                $type = $this->repository->getDocumentType($jsonData['type']);
            }
            return $type;
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }        
    }
    private function createFolder($path){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        if (!\file_exists($path)) {
            mkdir($path, 0777, true);
        }
    }
    
    public function juhu(Request $request, Response $response, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
        try{
            
        return $response;
        } catch (ApiException $exc){
            return $exc->generateHttpResponse($response);
        }
        catch (\Exception $exc) {
            return $response->withJson(['error' => 0,'message' => $exc->getMessage()], 500);
        }        
    }
}

<?php

namespace App\Action;

use Psr\Log\LoggerInterface;
use Slim\Http\Request;
use Slim\Http\Response;

use App\Repository\FinanceRepository;

/**
 * Description of UsersAction
 *
 * @author slavko
 */
final class FinanceAction {
    /**
     * @var \Psr\Log\LoggerInterface
     */
    private $logger;

    /**
     * @var \App\Repository\FinanceRepository
     */
    private $repository;

    /**
     * @param \Psr\Log\LoggerInterface       $logger
     * @param \App\Repository\FinanceRepository $repository
     */
    public function __construct(LoggerInterface $logger,  FinanceRepository $repository){
        $this->logger = $logger;
        $this->repository = $repository;
    }

    public function addInvoice(Request $request, Response $response, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
    }
    public function updateInvoice(Request $request, Response $response, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
    }
    public function deleteInvoice(Request $request, Response $response, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
    }
    public function bookInvoice(Request $request, Response $response, $args){
        if(DEV === TRUE) {$this->logger->info(__CLASS__.':'.__FUNCTION__);}
    }  
}

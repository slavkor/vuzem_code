<?php

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

namespace App\Common;

use Psr\Http\Message\ResponseInterface;

class ApiException extends \Exception{
    /**
     * @var int
     */
    private $httpStatusCode;

    /**
     * @var string
     */
    private $errorType;

    /**
     * @var null|string
     */
    private $hint;

    /**
     * @var null|string
     */
    private $redirectUri;

    /**
     * Throw a new exception.
     *
     * @param string      $message        Error message
     * @param int         $code           Error code
     * @param string      $errorType      Error type
     * @param int         $httpStatusCode HTTP status code to send (default = 400)
     * @param null|string $hint           A helper hint
     * @param null|string $redirectUri    A HTTP URI to redirect the user back to
     */
    public function __construct($message, $code, $errorType, $httpStatusCode = 400, $hint = null, $redirectUri = null)
    {
        parent::__construct($message, $code);
        $this->httpStatusCode = $httpStatusCode;
        $this->errorType = $errorType;
        $this->hint = $hint;
        $this->redirectUri = $redirectUri;
    }

    public static function serverError($hint)
    {
        return new static(
            'The server encountered an unexpected condition which prevented it from fulfilling'
            . ' the request: ' . $hint,
            1,
            'server_error',
            500, 
            $hint
        );
    }


    /**
     * @return string
     */
    public function getErrorType()
    {
        return $this->errorType;
    }

    /**
     * Generate a HTTP response.
     *
     * @param ResponseInterface $response
     * @param bool              $useFragment True if errors should be in the URI fragment instead of query string
     *
     * @return ResponseInterface
     */
    public function generateHttpResponse(ResponseInterface $response, $useFragment = false)
    {
        $headers = $this->getHttpHeaders();

        $payload = [
            'error'   => $this->getErrorType(),
            'message' => $this->getMessage(),
        ];

        if ($this->hint !== null) {
            $payload['hint'] = $this->hint;
        }

        /*
        if ($this->redirectUri !== null) {
            if ($useFragment === true) {
                $this->redirectUri .= (strstr($this->redirectUri, '#') === false) ? '#' : '&';
            } else {
                $this->redirectUri .= (strstr($this->redirectUri, '?') === false) ? '?' : '&';
            }

            return $response->withStatus(302)->withHeader('Location', $this->redirectUri . http_build_query($payload));
        }
         * 
         */

        foreach ($headers as $header => $content) {
            $response = $response->withHeader($header, $content);
        }

        $response->getBody()->write(json_encode($payload));

        return $response->withStatus($this->getHttpStatusCode());
    }

    /**
     * Get all headers that have to be send with the error response.
     *
     * @return array Array with header values
     */
    public function getHttpHeaders()
    {
        $headers = [
            'Content-type' => 'application/json',
        ];

        /*
        // Add "WWW-Authenticate" header
        //
        // RFC 6749, section 5.2.:
        // "If the client attempted to authenticate via the 'Authorization'
        // request header field, the authorization server MUST
        // respond with an HTTP 401 (Unauthorized) status code and
        // include the "WWW-Authenticate" response header field
        // matching the authentication scheme used by the client.
        // @codeCoverageIgnoreStart
        if ($this->errorType === 'invalid_client') {
            $authScheme = 'Basic';
            if (array_key_exists('HTTP_AUTHORIZATION', $_SERVER) !== false
                && strpos($_SERVER['HTTP_AUTHORIZATION'], 'Bearer') === 0
            ) {
                $authScheme = 'Bearer';
            }
            $headers['WWW-Authenticate'] = $authScheme . ' realm="OAuth"';
        }
         * 
         */
        
        // @codeCoverageIgnoreEnd
        return $headers;
    }

    /**
     * Returns the HTTP status code to send when the exceptions is output.
     *
     * @return int
     */
    public function getHttpStatusCode()
    {
        return $this->httpStatusCode;
    }

    /**
     * @return null|string
     */
    public function getHint()
    {
        return $this->hint;
    }
}

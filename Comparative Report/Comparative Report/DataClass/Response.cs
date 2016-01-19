using System;
using System.ComponentModel;

class Response
{

    [DefaultValue(ResponseCode.Error)]
    public ResponseCode responseCode { get; set; }
    public String responseMessage { get; set; }
    public dynamic responseData { get; set; }

    public Response() { }
    public Response(ResponseCode responseCode, String responseMessage, Object responseData)
    {
        this.responseCode = responseCode;
        this.responseMessage = responseMessage;
        this.responseData = responseData;
    }
    public Response(ResponseCode responseCode, String responseMessage)
    {
        this.responseCode = responseCode;
        this.responseMessage = responseMessage;
        //   this.responseData = responseData;
    }
}
enum ResponseCode
{
    NotFound = 404,
    InvalidDate = 2,
    OK = 1,
    Error = 0

}

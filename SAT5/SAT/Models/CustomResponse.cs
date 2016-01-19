using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAT.Enum;

namespace SAT.Models
{
    public class CustomResponse
    {
        public ResponseCode responseCode { get; set; }
        public string responseMessage { get; set; }
        public Object responseData { get; set; }

        public CustomResponse() { }
        public CustomResponse(ResponseCode responseCode, string responseMessage, Object responseData)
        {
            setResponseCode(responseCode);
            setResponseMessage(responseMessage);
            setResponseData(responseData);
        }

        //setters
        public void setResponseCode(ResponseCode responseCode)
        { this.responseCode = responseCode; }
        public void setResponseMessage(string responseMessage)
        { this.responseMessage = responseMessage; }
        public void setResponseData(Object responseData)
        { this.responseData = responseData; }

        //getters
        public ResponseCode getResponseCode()
        { return this.responseCode; }

        public string getResponseMessage()
        { return this.responseMessage; }
        public Object getResponseData()
        { return this.responseData; }
    }
}
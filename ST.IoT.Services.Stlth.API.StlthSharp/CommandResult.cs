using System;
using System.Net;
using System.Net.Configuration;
using Newtonsoft.Json.Linq;
using RestSharp;
using ST.IoT.Data.Stlth.Social.Model;

namespace ST.IoT.Services.Stlth.API.StlthSharp
{
    public abstract class CommandResult
    {
        protected HttpStatusCode _statusCode;

        public HttpStatusCode StatusCode
        {
            get { return _statusCode; }
        }

        protected string _resultJson;
        public string ResultJson
        {
            get {  return _resultJson; }
        }

        public virtual bool Success
        {
            get { return _statusCode == HttpStatusCode.OK; }
        }
    }
}
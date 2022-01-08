using Commons.ExceptionHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MVC卓越项目.Controller.ExceptionRedirect.Rest
{
    [RoutePrefix("Exception")]
    
    public class ExceptionController : ApiController
    {
        [Route("MethodNotFound")]
        [HttpGet]
        public void MethodNotFound()
        {
            throw new MethodNotFoundException();
        }
        [HttpGet]
        [Route("Forbidden")]
        public void Forbidden()
        {
            throw new ForbiddenException();
        }
        [HttpGet]
        [HttpPost]
        [Route("MethodNotSupport")]
        public void MethodNotSupport()
        {
            
            throw new MethodNotSupportException(Request.Method.Method);
        }
    }
}
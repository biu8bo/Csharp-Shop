using MVC卓越项目.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC卓越项目.Commons.BaseModels
{
    public class ApiExceptionModel : ApiBaseModel
    {
        public string StackTraceMessage { get; set; }
    }
}
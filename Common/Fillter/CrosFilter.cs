using MVC卓越项目.Commons.BaseModels;
using MVC卓越项目.Commons.ExceptionHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace MVC卓越项目.Commons.Fillter
{
    /// <summary>
    /// 跨域拦截器
    /// </summary
    public class CrosFilter : ActionFilterAttribute
    {

        /// <summary>
        /// 跨域拦截器
        /// </summary>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            string origin = actionExecutedContext.Request.Headers.GetValues("Origin").FirstOrDefault();
            actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Origin", origin);
            actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Headers", "Origin, x-requested-with, Content-Type, Accept, Authorization");
            actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, OPTIONS, DELETE");
            actionExecutedContext.Response.Headers.Add("Access-Control-Expose-Headers", "Cache-Control, Content-Language, Content-Type, Expires, Last-Modified, Pragma");
            actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            actionExecutedContext.Response.Headers.Add("Access-Control-Max-Age", "60");
            //传递给下一个过滤器
            base.OnActionExecuted(actionExecutedContext);

        }
    }
}
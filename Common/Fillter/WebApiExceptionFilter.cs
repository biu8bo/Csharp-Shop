using MVC卓越项目.Commons.BaseModels;
using MVC卓越项目.Commons.ExceptionHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;

namespace MVC卓越项目.Commons.Fillter
{
    /// <summary>
    /// 全局异常捕获
    /// </summary
    public class WebApiExceptionFilter : ExceptionFilterAttribute
    {
        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            Exception ex = actionExecutedContext.Exception;
            ApiException customerException = ex as ApiException;
            //获取状态代码
            if (customerException == null)
            {
                customerException = new ApiException();
               
                try
                {
                    customerException.Code = (int)actionExecutedContext.ActionContext.Response.StatusCode;
                }
                catch (Exception)
                {
                    customerException.Code = (int)HttpStatusCode.InternalServerError;
                }

            }
            ApiExceptionModel result = new ApiExceptionModel(); ;
            result.Code = (HttpStatusCode)customerException.Code;
            result.Msg = ex.Message;
            result.IsSuccess = false;
            //只在debug环境打印堆栈信息
            #if DEBUG
            result.StackTraceMessage = ex.StackTrace;
            #else
                result.StackTraceMessage = "堆栈信息仅能在DEBUG环境下获取";
            #endif
            //结果转为JSON消息格式
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage()
            {
                Content = new ObjectContent<ApiExceptionModel>(
                   result,
                   new JsonMediaTypeFormatter(),
                   "application/json"
                 )
            };

            // 回传
            actionExecutedContext.Response = httpResponseMessage;
            //解决报错产生的跨域问题
            if (actionExecutedContext.Request.Headers.Contains("Origin"))
            {
                //尝试获取orgin头
                IEnumerable<string> strings = actionExecutedContext.Request.Headers.GetValues("Origin");
                actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Origin", strings.FirstOrDefault());
                actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type,Content-Length,Authorization,Accept,X-Requested-With");
                actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Methods", "Get,Post,Put,Options,Delete");
                actionExecutedContext.Response.Headers.Add("Access-Control-Expose-Headers", "Cache-Control,Content-Language,Content-Type,Expires,Last-Modified,Pragma");
                actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
                actionExecutedContext.Response.Headers.Add("Access-Control-Max-Age", "60");
            }
            base.OnException(actionExecutedContext);
        }

    }
}
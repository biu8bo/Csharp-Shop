using MVC卓越项目.Models;
using MVC卓越项目.Utils;
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

namespace MVC卓越项目.App_Start
{
    /// <summary>
    /// 全局异常过滤器 
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
            ApiExceptionModel result = new ApiExceptionModel();
            result.IsSuccess = false;
            //500
            result.Code =HttpStatusCode.InternalServerError;
            result.Msg = ex.Message;
            //只在debug环境打印堆栈信息
            #if DEBUG
            result.StackTraceMessage = ex.StackTrace;
            #else
            result.StackTraceMessage = "堆栈信息只能在DEBUG环境下获取";
            #endif
            //结果转为JSON消息格式
            HttpResponseMessage httpResponseMessage =  new HttpResponseMessage()
            {
                Content = new ObjectContent<ApiExceptionModel>(
                   result,
                   new JsonMediaTypeFormatter(),
                   "application/json"
                 )
            }; ;
            // 回传
            actionExecutedContext.Response = httpResponseMessage;
            base.OnException(actionExecutedContext);
        }

    }
}
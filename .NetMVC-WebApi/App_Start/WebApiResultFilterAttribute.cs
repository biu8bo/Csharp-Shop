using MVC卓越项目.Models;
using MVC卓越项目.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http.Filters;

namespace MVC卓越项目.App_Start
{
    /// <summary>
    /// 定义统一格式过滤器
    /// </summary>
    public class WebApiResultFilterAttribute :ActionFilterAttribute
    {
        //action执行后,统一执行的结果
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
           
            //如果有异常就抛出异常
            if (actionExecutedContext.Exception != null)
            {
                //抛出
                throw actionExecutedContext.Exception;
            }
            else
            {
                // 包裹返回值
                ApiModelsBase result = new ApiModelsBase();
                // 取得由 API 返回的状态代码
                result.Code = actionExecutedContext.ActionContext.Response.StatusCode;
                //是否有返回结果
                var a = actionExecutedContext.ActionContext.Response.Content.ReadAsAsync<object>();
                if (!a.IsFaulted)
                {
                    // 把返回结果塞进去
                    result.Data = actionExecutedContext.ActionContext.Response.Content.ReadAsAsync<object>().Result;
                }
                //请求是否成功
                result.IsSuccess = actionExecutedContext.ActionContext.Response.IsSuccessStatusCode;
                //结果转为JSON
                HttpResponseMessage httpResponseMessage = new HttpResponseMessage()
                {
                    Content = new ObjectContent<ApiModelsBase>(
                       result,
                       new JsonMediaTypeFormatter(),
                       "application/json"
                     )
                };
                // 重新封装回传格式 
                actionExecutedContext.Response = httpResponseMessage;
            }
            
           
        }
    }
}
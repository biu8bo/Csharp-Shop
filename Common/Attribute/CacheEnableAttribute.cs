
using MVC卓越项目.Commons.Utils;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;


namespace MVC卓越项目.Commons.Attribute
{
    /// <summary>
    /// 缓存方法的结果到redis中去
    /// </summary>
    public class CacheEnableAttribute : ActionFilterAttribute
    {

        /// <summary>
        /// 要设置到redis 中的键名称
        /// 如果不设置key的话 默认使用方法名称作为key
        /// </summary>
        public string key;

        /// <summary>
        /// 如果redis中存在这个了,直接返回结果不执行这个action
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            //得到方法参数
            Dictionary<string, object> args = actionContext.ActionArguments;
            //如果没有设置key的话,默认使用方法的名称
            if (key==null)
            {
                key = actionContext.ActionDescriptor.ActionName;
            }
            //生成redis Key
            string templateKey =key +":"+ JsonConvert.SerializeObject(args).GetHashCode().ToString();
            //通过key得到该方法结果的json
            string result = RedisHelper.GetStringValue(templateKey);
            //如果不为空就直接返回而不去执行action
            if (result != null)
            {
                //反序列化json对象
                Dictionary<string, object> dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(result);
                //设置响应内容
                HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
                httpResponseMessage.Content = new ObjectContent<Dictionary<string, object>>(dict, new JsonMediaTypeFormatter(), "application/json");
                //  设置响应体
                actionContext.Response = httpResponseMessage;
                return;
            }
        }

        /// <summary>
        /// 得到结果写进redis
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            //如果没有设置key的话,默认使用方法的名称
            if (key == null)
            {
                key = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;
            }
            //得到方法返回的结果
            object result = actionExecutedContext.ActionContext.Response.Content.ReadAsAsync<object>().Result;
            //得到方法参数
            Dictionary<string, object> args = actionExecutedContext.ActionContext.ActionArguments;
            //使用key和方法参数的hashcode拼接生成redisKey 保证传参一致时能正确读取
            string templateKey = key + ":" + JsonConvert.SerializeObject(args).GetHashCode().ToString();
            //序列化成json
            string json = JsonConvert.SerializeObject(result);
            //写入redis
            RedisHelper.SetStringValue(templateKey, json);
            base.OnActionExecuted(actionExecutedContext);
           
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
   
            IEnumerable<string> strings = Enumerable.Empty<string>();
            string origin = "*";
            //尝试获取orgin头
            bool issuccess = actionExecutedContext.Request.Headers.TryGetValues("Origin", out strings);
            //如果是跨域请求
            if (issuccess)
            {
                //如果响应体为空
                if (actionExecutedContext.Response is null)
                {
                    actionExecutedContext.Response = new HttpResponseMessage();
                }
                origin = strings.FirstOrDefault();
                actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Origin", origin);
                actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type,Content-Length,Authorization,Accept,X-Requested-With");
                actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Methods", "Get,Post,Put,Options,Delete");
                actionExecutedContext.Response.Headers.Add("Access-Control-Expose-Headers", "Cache-Control,Content-Language,Content-Type,Expires,Last-Modified,Pragma");
                actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
                actionExecutedContext.Response.Headers.Add("Access-Control-Max-Age", "60");
            }

            ////有异常就抛出
            if (!(actionExecutedContext.Exception is null))
            {
                throw actionExecutedContext.Exception;
            }
            //传递给下一个过滤器
            base.OnActionExecuted(actionExecutedContext);

        }
    }
}

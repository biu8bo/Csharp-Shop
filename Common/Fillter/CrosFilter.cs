using System.Collections.Generic;
using System.Linq;
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
            ////有异常就抛出
            if (!(actionExecutedContext.Exception is null))
            {
                throw actionExecutedContext.Exception;
            }
            IEnumerable<string> strings = Enumerable.Empty<string>();
            string origin = "*";
            //尝试获取orgin头
            bool issuccess = actionExecutedContext.Request.Headers.TryGetValues("origin", out strings);
            //如果是跨域请求
            if (issuccess)
            {
                origin = strings.FirstOrDefault();
                actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Origin", origin);
                actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type,Content-Length,Authorization,Accept,X-Requested-With");
                actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Methods", "Get,Post,Put,Options,Delete");
                actionExecutedContext.Response.Headers.Add("Access-Control-Expose-Headers", "Cache-Control,Content-Language,Content-Type,Expires,Last-Modified,Pragma");
                actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
                actionExecutedContext.Response.Headers.Add("Access-Control-Max-Age", "60");
            }

            //传递给下一个过滤器
            base.OnActionExecuted(actionExecutedContext);

        }
    }
}

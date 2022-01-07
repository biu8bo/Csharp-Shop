using MVC卓越项目.Models;
using MVC卓越项目.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace MVC卓越项目.App_Start
{
    /// <summary>
    /// 权限拦截器
    /// </summary>
    public class AuthCheckAttribute : AuthorizationFilterAttribute
    {

        public override void OnAuthorization(HttpActionContext actionContext)
        {

            try
            {
                //获取token
                IEnumerable<string> tokens = actionContext.Request.Headers.GetValues("token");
                string token = tokens.First();
                //尝试获取用户对象
                UserInfo user = JwtHelper.getUserByToken(token);
                if ("666".Equals(user.UserName) && "666".Equals(user.Password))
                {
                    //验证通过！
                    return;
                }
                else
                {
                    //验证失败
                    new Exception("登录失效！");
                }
            }
            catch
            {
                ApiModelsBase apiModelsBase = new ApiModelsBase();
                apiModelsBase.Code = HttpStatusCode.Unauthorized;
                apiModelsBase.Msg = "登录失效！";
                apiModelsBase.IsSuccess = false;
                actionContext.Response =  new HttpResponseMessage()
                {
                    Content = new ObjectContent<ApiModelsBase>(
                   apiModelsBase,
                   new JsonMediaTypeFormatter(),
                   "application/json"
                 )
                };
            }
            base.OnAuthorization(actionContext);
        }
    }
}
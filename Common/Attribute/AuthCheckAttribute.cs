
using Mapper;
using MVC卓越项目.Commons.ExceptionHandler;
using MVC卓越项目.Commons.Utils;
using MVC卓越项目.Models;
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

namespace MVC卓越项目.Commons.Attribute
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
                IEnumerable<string> tokens = actionContext.Request.Headers.GetValues("Authorization");
                string token = tokens.First();
                //尝试获取用户对象
                eshop_user user = JwtHelper.getUserByToken(token);
                //从缓存获取
                eshop_user redisUser = RedisHelper.GetStringKey<eshop_user>(token);
                if (redisUser.username.Equals(user.username) && redisUser.password.Equals(user.password))
                {
                    //验证通过！
                    //存储用户对象
                    LocalUser.threadLocalTable.Value.Add("USER", user);
                    base.OnAuthorization(actionContext);
                }
                else
                {
                    //验证失败
                   throw new AuthException();
                }
            }
            catch
            {
                throw new AuthException();
            }
           
        }
    }
}
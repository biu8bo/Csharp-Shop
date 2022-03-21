
using Commons.Utils;
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
    /// 后台权限拦截器
    /// </summary>
    public class BackAuthCheckAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 是否必须登录
        /// </summary>
        public bool required = true;
        /// <summary>
        /// 方法执行前的权限检查
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            try
            {
                //获取token
                string token = actionContext.Request.Headers.GetValues("Authorization").FirstOrDefault();
                //尝试获取用户对象
                user user = JwtHelper<user>.getUserByToken(token);
                //从缓存获取用户
                user redisUser = RedisHelper.GetStringKey<user>("BackUser:" + user.username + ":" + token);
                if (ObjectUtils<eshop_user>.isNull(redisUser))
                {
                    throw new AuthException("登录状态过期,请重新登陆!");
                }
                if (redisUser.username.Equals(user.username) && redisUser.password.Equals(user.password))
                {
                    //验证通过！
                    //存储用户对象
                    LocalUser.threadLocalTable.Value.Add("USER", user);
                    //给下一个过滤器
                    base.OnActionExecuting(actionContext);
                }
                else
                {
                    if (!this.required)
                    {
                        //给下一个过滤器
                        base.OnActionExecuting(actionContext);
                        return;
                    }
                    //验证失败
                    throw new AuthException();
                 
                }
            }
            catch(Exception e)
            {
                if (!this.required)
                {
                    //给下一个过滤器
                    base.OnActionExecuting(actionContext);
                    return;
                }

                throw new ApiException(401,"暂未登录！"); ;
               
            }
        }
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            //清空
            LocalUser.Clear();
            base.OnActionExecuted(actionExecutedContext);
        }
    }
}
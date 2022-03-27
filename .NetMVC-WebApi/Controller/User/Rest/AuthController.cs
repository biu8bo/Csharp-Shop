using Commons.BaseModels;
using Commons.Utils;
using Mapper;
using MVC卓越项目.Commons.Attribute;
using MVC卓越项目.Commons.ExceptionHandler;
using MVC卓越项目.Commons.Utils;
using MVC卓越项目.Controller.Auth.Param;
using Service.Service;
using Service.UserService.Param;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace MVC卓越项目.Controller.Auth
{
    /// <summary>
    /// 权限模块
    /// </summary>
    [RoutePrefix("api")]
    public class LoginController : ApiController
    {
        private readonly IAuthService iAuthService = Bootstrapper.Resolve<IAuthService>();
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginParam"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public ApiResult<Hashtable> Login([FromBody] LoginParam loginParam)
        {
            return ApiResult<Hashtable>.ok(iAuthService.Login(loginParam, getIP()), "登陆成功");
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [AuthCheck]
        [HttpPost]
        [Route("logout")]


        public ApiResult<bool> Logout()
        {
            return ApiResult<bool>.ok(iAuthService.Logout(Request.Headers.GetValues("Authorization").First()));
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="registerParam"></param>
        /// <returns></returns>

        [HttpPost]
        [Route("register")]
        public ApiResult<Hashtable> Register([FromBody] RegisterParam registerParam)
        {
            registerParam.password = Md5Utils.Md5(registerParam.password);
            return ApiResult<Hashtable>.ok(iAuthService.Register(registerParam, getIP()), "注册成功");
        }
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("register/vertify")]
        public ApiResult<int> vertify(string phone)
        {
            if (String.IsNullOrEmpty(phone))
            {
                throw new ApiException(500, "请输入手机号码");
            }
            int vertifyCode = iAuthService.verify(phone);
            return ApiResult<int>.ok(vertifyCode);

        }
        /// <summary>
        /// 获取ip
        /// </summary>
        /// <returns></returns>
        public string getIP()
        {
            string userIP = "";
            try
            {
                if (System.Web.HttpContext.Current == null
            || System.Web.HttpContext.Current.Request == null
            || System.Web.HttpContext.Current.Request.ServerVariables == null)
                    return "";
                string CustomerIP = "";
                //CDN加速后取到的IP simone 090805
                CustomerIP = System.Web.HttpContext.Current.Request.Headers["Cdn-Src-Ip"];
                if (!string.IsNullOrEmpty(CustomerIP))
                {
                    return CustomerIP;
                }
                CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!String.IsNullOrEmpty(CustomerIP))
                {
                    return CustomerIP;
                }
                if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                {
                    CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    if (CustomerIP == null)
                        CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                else
                {
                    CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                if (string.Compare(CustomerIP, "unknown", true) == 0)
                    return System.Web.HttpContext.Current.Request.UserHostAddress;
                return CustomerIP;
            }
            catch { }
            return userIP;
        }
    }
}
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
    [RoutePrefix("api")]
    public class LoginController : ApiController
    {
        private readonly IAuthService iAuthService = Bootstrapper.Resolve<IAuthService>();
        [HttpPost]
        [Route("login")]
      public  ApiResult<eshop_user> Login([FromBody] LoginParam loginParam)
        {
            return ApiResult<eshop_user>.ok(iAuthService.Login(loginParam, getIP()),"登陆成功");
        }
        [AuthCheck]
        [HttpPost]
        [Route("logout")]

        ///退出登录
        public ApiResult<eshop_user> Logout()
        {
            return ApiResult<eshop_user>.ok(iAuthService.Logout(Request.Headers.GetValues("Authorization").First()));
        }
        //注册

        [HttpPost]
        [Route("register")]
        public ApiResult<Hashtable> Register([FromBody] RegisterParam registerParam)
        {
            registerParam.password = Md5Utils.Md5(registerParam.password); 
            return ApiResult<Hashtable>.ok(iAuthService.Register(registerParam, getIP()), "注册成功");
        }
        [HttpGet]
        [Route("register/vertify")]
        public ApiResult<int> vertify(string phone)
        {
            if (String.IsNullOrEmpty(phone))
            {
                throw new ApiException(500,"请输入手机号码");
            }
            int vertifyCode =  iAuthService.verify(phone);
            return ApiResult<int>.ok(vertifyCode);

        }
        /// <summary>
        /// 获取ip
        /// </summary>
        /// <returns></returns>
        public string getIP()
        {
            string ip = Convert.ToString(System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]);
            // 利用 Dns.GetHostEntry 方法，由获取的 IPv6 位址反查 DNS 纪录，<br> // 再逐一判断何者为 IPv4 协议，即可转为 IPv4 位址。
            if (string.IsNullOrEmpty(ip))
            {
                ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return ip;

        }
    }
}
using Commons.BaseModels;
using Mapper;
using MVC卓越项目.Commons.Attribute;
using MVC卓越项目.Commons.Utils;
using MVC卓越项目.Controller.Auth.Param;
using Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
           string ip = Convert.ToString(System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]);
           string ipv4 = string.Empty;
            // 利用 Dns.GetHostEntry 方法，由获取的 IPv6 位址反查 DNS 纪录，<br> // 再逐一判断何者为 IPv4 协议，即可转为 IPv4 位址。
            if (string.IsNullOrEmpty(ip))
            {
                ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            
            foreach (IPAddress ipAddr in Dns.GetHostEntry(ip).AddressList)
            {
                if (ipAddr.AddressFamily.ToString() == "InterNetwork")
                {
                   ipv4 = ipAddr.ToString();
                }
            }
            return ApiResult<eshop_user>.ok(iAuthService.Login(loginParam, ipv4),"登陆成功");
        }
        [AuthCheck]
        [HttpPost]
        [Route("logout")]
        public ApiResult<eshop_user> Logout()
        {
            return ApiResult<eshop_user>.ok(iAuthService.Logout(Request.Headers.GetValues("Authorization").First()));
        }

    }
}
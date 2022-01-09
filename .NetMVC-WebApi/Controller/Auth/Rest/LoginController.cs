using Commons.BaseModels;
using Mapper;
using MVC卓越项目.Commons.Utils;
using MVC卓越项目.Controller.Auth.Param;
using Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MVC卓越项目.Controller.Auth.Rest
{
    public class LoginController : ApiController
    {
        private readonly IAuthService iAuthService = Bootstrapper.Resolve<IAuthService>();
        private readonly static Log4NetHelper logger = Log4NetHelper.Default;
        [Route("login")]
      public  ApiResult<eshop_user> Login([FromBody] LoginParam loginParam)
        {
            IPAddress[] ips = Dns.GetHostAddresses(Dns.GetHostName());
            string ip = "";
            foreach (IPAddress e in ips)
            {
                ip += e.ToString() + "|";
            }
            logger.WriteInfo($"IP为:{ip}的用户尝试登录 用户名:{loginParam.Username} 密码:{loginParam.Password}");
            return ApiResult<eshop_user>.ok(iAuthService.login(loginParam),"登陆成功");
        }
    }
}
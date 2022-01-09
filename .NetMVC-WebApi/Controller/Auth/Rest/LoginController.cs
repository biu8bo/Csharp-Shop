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
            return ApiResult<eshop_user>.ok(iAuthService.Login(loginParam,Dns.GetHostAddresses(Dns.GetHostName())),"登陆成功");
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
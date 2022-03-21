using Commons.BaseModels;
using Commons.Utils;
using Mapper;
using MVC卓越项目.Commons.Attribute;
using MVC卓越项目.Commons.Utils;
using MVC卓越项目.Controller.Auth.Param;
using Service.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace MVC卓越项目.Controller.User.BackRest
{
    /// <summary>
    /// 后台权限控制器
    /// </summary>
    [RoutePrefix("api/auth")]
    public class AuthController:ApiController
    {
        private readonly IAuthService iAuthService = Bootstrapper.Resolve<IAuthService>();
        /// <summary>
        /// 过期时间
        /// </summary>
        private readonly TimeSpan expire = TimeSpan.FromMilliseconds(1000 * 100 * 3);
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("code")]
        public ApiResult<Hashtable> getCode()
        {
            Hashtable hashtable = new Hashtable();
            ArithmeticUtils arithmetic = new ArithmeticUtils();
            string img =   arithmetic.SetValidateLenth(4).CreateValidateGraphic();
            hashtable.Add("img", img);
            string uuid = Guid.NewGuid().ToString();
            RedisHelper.SetStringKey("code/key:"+ uuid, arithmetic.validateNumberStr, expire);
            hashtable.Add("uuid", "code/key:" + uuid);
            return ApiResult<Hashtable>.ok(hashtable);
        }

        /// <summary>
        /// 后台登录
        /// </summary>
        /// <param name="loginParam"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public ApiResult<Hashtable> Login([FromBody]LoginParam loginParam)
        {
            return ApiResult<Hashtable>.ok(iAuthService.BackEndLogin(loginParam));
        }

        /// <summary>
        /// 后台退出登录
        /// </summary>
        /// <param name="loginParam"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("logout")]
        [BackAuthCheck]
        public ApiResult<Hashtable> Logout()
        {
            IEnumerable<string> token;
            Request.Headers.TryGetValues("Authorization", out token);
            iAuthService.LogoutBackEnd(token.FirstOrDefault());
            return ApiResult<Hashtable>.ok();
        }

        [HttpGet]
        [Route("info")]
        [BackAuthCheck]
        public ApiResult<user> info()
        {
            using (var db =new eshoppingEntities())
            {
                long uid = LocalUser.getUidByUserBackEnd();
               
                return ApiResult<user>.ok(db.users.Where(e => e.id == uid).FirstOrDefault());
            }
           
        }
    }
}
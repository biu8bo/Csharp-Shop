using Mapper;
using MVC卓越项目.Commons.ExceptionHandler;
using MVC卓越项目.Commons.Utils;
using MVC卓越项目.Controller.Auth.Param;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class AuthServiceImpl : IAuthService

    {
        /// <summary>
        /// 登录返回 token和 user对象
        /// </summary>
        /// <param name="loginParam"></param>
        /// <returns></returns>
        public Hashtable login(LoginParam loginParam)
        {
            using (var db = new eshoppingEntities())
            {
              eshop_user result =  db.eshop_user.Where(e => e.username == loginParam.Username && e.password == loginParam.Password).FirstOrDefault();
                if (result==null)
                {
                    throw new AuthException("登录失败！");
                }
                else
                {
                //获取设置登录的过期时间
                string  exTime  = ConfigurationManager.AppSettings["tokenExpired"];
                    //获取token
                 string token =  JwtHelper.getJwtEncode(result);
                //将用户名设为键 写入缓存
                RedisHelper.SetStringKey("USER:"+result.username+":"+ token, result, TimeSpan.FromMilliseconds(Convert.ToDouble(exTime)));
                Hashtable hashtable = new Hashtable();
                hashtable.Add("token", token);
                hashtable.Add("USER", result);
                return hashtable;
                }
            }
        }
    }
}

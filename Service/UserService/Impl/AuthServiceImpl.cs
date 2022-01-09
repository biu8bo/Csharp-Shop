using Mapper;
using MVC卓越项目.Commons.ExceptionHandler;
using MVC卓越项目.Commons.Utils;
using MVC卓越项目.Controller.Auth.Param;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class AuthServiceImpl : IAuthService

    {

        private readonly static Log4NetHelper logger = Log4NetHelper.Default;
        /// <summary>
        /// 登录返回 token和 user对象
        /// </summary>
        /// <param name="loginParam"></param>
        /// <returns></returns>
        public Hashtable Login(LoginParam loginParam, IPAddress[] ips)
        {
            string ip = "";
            foreach (IPAddress e in ips)
            {
                ip += e.ToString() + "|";
            }
            using (var db = new eshoppingEntities())
            {
                eshop_user result = db.eshop_user.Where(e => e.username == loginParam.Username && e.password == loginParam.Password).FirstOrDefault();
                if (result == null)
                {
                    logger.WriteInfo($"IP为:{ip}的用户尝试登录 用户名:{loginParam.Username}  登录失败！");
                    throw new AuthException("登录失败！");
                }
                else
                {
                    //获取设置登录的过期时间
                    string exTime = ConfigurationManager.AppSettings["tokenExpired"];
                    //获取token
                    string token = JwtHelper.getJwtEncode(result);
                    //将用户名设为键 写入缓存
                    RedisHelper.SetStringKey("USER:" + result.username + ":" + token, result, TimeSpan.FromMilliseconds(Convert.ToDouble(exTime)));

                    logger.WriteInfo($"IP为:{ip}的用户尝试登录 用户名:{loginParam.Username} 登陆成功！" );
                    Hashtable hashtable = new Hashtable();
                    hashtable.Add("token", token);
                    hashtable.Add("USER", result);
                    return hashtable;
                }
            }
        }

        public bool Logout(string token)
        {

            //获取当前接口用户
            eshop_user user = LocalUser.getUser();
            logger.WriteInfo($"用户：{user.username} 退出登录");
            return RedisHelper.DeleteStringKey("USER:" + user.username + ":" + token);
        }
    }
}

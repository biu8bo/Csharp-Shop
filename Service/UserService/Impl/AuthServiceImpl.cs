using Commons.Utils;
using Mapper;
using MVC卓越项目.Commons.ExceptionHandler;
using MVC卓越项目.Commons.Utils;
using MVC卓越项目.Controller.Auth.Param;
using Service.UserService.Param;
using StackExchange.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
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
        /// 通过手机号码生成验证码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public int verify(string phone)
        {

            RedisValueWithExpiry flag = RedisHelper.GetStringWithExpiry("verify:" + phone);
            if (!flag.Value.IsNullOrEmpty)
            {
                throw new ApiException(501, $"请等待{(int)flag.Expiry.Value.TotalSeconds}秒");
            }
            else
            {
                Random r = new Random();
                int verify = r.Next(1000, 10000);
                RedisHelper.SetStringKey("verify:" + phone, verify, TimeSpan.FromMilliseconds(60 * 1000));
                return verify;
            }

        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="registerParam"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public Hashtable Register(RegisterParam registerParam, string ip)
        {


            string code = registerParam.vertity;

            if (String.IsNullOrEmpty(code))
            {
                throw new ApiException(500, "请输入验证码！");
            }

            string redisCode = RedisHelper.GetStringValue("verify:" + registerParam.phone);
            if (string.IsNullOrEmpty(redisCode))
            {
                throw new ApiException(500, "请先获取验证码！");
            }

            if (redisCode.Equals(code))
            {
           
                    using (var db = new eshoppingEntities())
                    {
                        var tran = db.Database.BeginTransaction();
                          eshop_user result1 = db.eshop_user.FirstOrDefault(e => e.phone == registerParam.phone);
                    if (result1 is null)
                    {
                        throw new ApiException(501, "该手机号已被注册！");
                    }
                        eshop_user user = new eshop_user();
                        user.add_ip = ip;
                        user.user_type = "h5";
                        user.addres = "";
                        user.login_type = "";
                        user.username = registerParam.username;
                        user.password = registerParam.password;
                        user.now_money = 0;
                        user.create_time = DateTime.Now;
                        user.update_time = DateTime.Now;
                        user.last_ip = ip;
                        user.phone = registerParam.phone;
                        db.eshop_user.Add(user);
                        db.SaveChanges();
                        tran.Commit();
                    }
            }
            else
            {
                throw new ApiException(500, "验证码错误！");
            }
            return new Hashtable()
            {
                {"user", registerParam.username},
            { "phone",registerParam.phone},
                { "msg","注册成功"}
            };

        }
        /// <summary>
        /// 登录返回 token和 user对象
        /// </summary>
        /// <param name="loginParam"></param>
        /// <returns></returns>
        public Hashtable Login(LoginParam loginParam, string ip)
        {
            using (var db = new eshoppingEntities())
            {
                var tran = db.Database.BeginTransaction();
                eshop_user result = db.eshop_user.Where(e => e.username == loginParam.Username && e.password == loginParam.Password && e.is_del == false).FirstOrDefault();
                if (result == null)
                {
                    logger.WriteInfo($"IP[{ip}]:用户尝试登录 用户名:{loginParam.Username}  登录失败！");
                    throw new AuthException("用户名或密码错误！");
                }
                else
                {
                    if (result.status == false)
                    {
                        throw new AuthException("该用户已被封禁！");
                    }
                    //修改登录时间
                    result.update_time = DateTime.Now;
                    //修改IP
                    result.last_ip = ip;
                    db.SaveChanges();
                    tran.Commit();
                    //获取设置登录的过期时间
                    string exTime = ConfigurationManager.AppSettings["tokenExpired"];
                    //获取token
                    string token = JwtHelper<eshop_user>.getJwtEncode(result);
                    //踢出原来的用户
                    kictOutUser(result.username);
                    //将用户名设为键 写入缓存
                    RedisHelper.SetStringKey("USER:" + result.username + ":" + token, result, TimeSpan.FromMilliseconds(Convert.ToDouble(exTime)));

                    logger.WriteInfo($"IP为:{ip}的用户尝试登录 用户名:{loginParam.Username} 登陆成功！");
                    //不返回密码
                    result.password = null;
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
            //踢出用户
            return RedisHelper.DeleteStringKey("USER:" + user.username + ":" + token);
        }

        /// <summary>
        /// 踢出用户
        /// </summary>
        public void kictOutUser(string username)
        {
            RedisHelper.DeleteKeyByLike($"USER:{username}*");
        }

        public Hashtable BackEndLogin(LoginParam loginParam)
        {
            using (var db = new eshoppingEntities())
            {
                //判断验证码是否正确
                string ocode = RedisHelper.GetStringKey<string>(loginParam.Uuid);
                if (ocode is null)
                {
                    throw new ApiException(500,"验证码已过期!,请重启获取");
                }
                //删除验证码
                RedisHelper.DeleteStringKey(loginParam.Uuid);
                if (!ocode.Equals(loginParam.Code))
                {
                  
                    throw new ApiException(500, "验证码输入错误!");
                }
                var tran = db.Database.BeginTransaction();
                user result = db.users.Where(e => e.username == loginParam.Username && e.password == loginParam.Password && e.is_del == false).FirstOrDefault();
                if (result == null)
                {

                    throw new ApiException(500, "用户名或密码错误！");
                }
                else
                {
                    if (result.enabled == 0)
                    {
                        throw new AuthException("该账户已被封禁！");
                    }
                    //修改登录时间
                    result.update_time = DateTime.Now;
                    //修改IP

                    db.SaveChanges();
                    tran.Commit();
                    //获取设置登录的过期时间
                    string exTime = ConfigurationManager.AppSettings["tokenExpired"];
                    //获取token
                    string token = JwtHelper<user>.getJwtEncode(result);
                    //踢出原来的用户
                    RedisHelper.DeleteKeyByLike($"BackUser:{result.username}*");
                    //将用户名设为键 写入缓存
                    RedisHelper.SetStringKey("BackUser:" + result.username + ":" + token, result, TimeSpan.FromMilliseconds(Convert.ToDouble(exTime)));


                    //不返回密码
                    result.password = null;
                    Hashtable hashtable = new Hashtable();
                    hashtable.Add("token", token);
                    hashtable.Add("user", result);
                    return hashtable;
                }
            }
        }

        public bool LogoutBackEnd(string token)
        {
            return RedisHelper.DeleteStringKey("BackUser:" + LocalUser.getBackEndUser().username + ":" + token);
        }
    }
}

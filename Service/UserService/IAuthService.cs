using Mapper;
using MVC卓越项目.Controller.Auth.Param;
using Service.UserService.Param;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
  public  interface IAuthService
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginParam"></param>
        /// <returns></returns>
        Hashtable Login(LoginParam loginParam, string ip);
        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
         bool Logout(string token);
        /// <summary>
        /// 退出后台用户登录
        /// </summary>
        /// <returns></returns>
        bool LogoutBackEnd(string token);
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="registerParam"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        Hashtable Register(RegisterParam registerParam, string ip);

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        int verify(string phone);

        /// <summary>
        /// 踢出用户
        /// </summary>
        void kictOutUser(string username);

        /// <summary>
        /// 后台登录
        /// </summary>
        /// <param name="loginParam"></param>
        /// <returns></returns>
        Hashtable BackEndLogin(LoginParam loginParam);
    }
}

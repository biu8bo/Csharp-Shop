﻿using Commons.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC卓越项目.Controller.Auth.Param
{
    public class LoginParam
    {
        string username;
        string password;
        string code;
        string uuid;
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get => username; set => username = value; }
        /// <summary>
        /// 密码经过md5加密
        /// </summary>
        public string Password { get => password; set => password = Md5Utils.Md5(value); }
        public string Code { get => code; set => code = value; }
        public string Uuid { get => uuid; set => uuid = value; }
    }
}
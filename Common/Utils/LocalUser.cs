﻿
using Mapper;
using MVC卓越项目.Commons.ExceptionHandler;
using MVC卓越项目.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace MVC卓越项目.Commons.Utils
{
    /// <summary>
    /// LocalUser工具类
    /// </summary>
    public static class LocalUser
    {
        //线程隔离
        public static ThreadLocal<Hashtable> threadLocalTable = new ThreadLocal<Hashtable>(()=>new Hashtable());

        //获取user实例
        public static eshop_user getUser()
        {
            eshop_user userInfo = null;
            try
            {
                userInfo= threadLocalTable.Value["USER"] as eshop_user;
            }
            catch (Exception)
            {

                throw new AuthException();
            }
            return userInfo;
        } 

        //获取UID
        public static  decimal getUidByUser()
        {
            eshop_user userInfo = getUser();
            if (userInfo==null)
            {
                throw new AuthException();
            }
           return userInfo.uid;
        }
    }
}
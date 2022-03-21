
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
    public class LocalUser
    {
        //线程资源隔离
        public static ThreadLocal<Hashtable> threadLocalTable = new ThreadLocal<Hashtable>(()=>new Hashtable());

        /// <summary>
        /// 获取前端user实例
        /// </summary>
        /// <returns></returns>
        public static eshop_user getUser()
        {
            eshop_user userInfo = null;
      
            userInfo= threadLocalTable.Value["USER"] as eshop_user;
           
            return userInfo;
        }
        /// <summary>
        /// 获取后台user实例
        /// </summary>
        /// <returns></returns>
        public static user getBackEndUser()
        {
            user userInfo = null;

            userInfo = threadLocalTable.Value["USER"] as user;

            return userInfo;
        }

        /// <summary>
        /// 获取前端UID
        /// </summary>
        /// <returns></returns>
        public static long getUidByUser()
        {
            eshop_user userInfo = getUser();
            if (userInfo==null)
            {
                return 0;
            }
           return userInfo.uid;
        }
        //获取UID
        public static long getUidByUserBackEnd()
        {
            user userInfo = getBackEndUser();
            if (userInfo == null)
            {
                return 0;
            }
            return userInfo.id;
        }
        /// <summary>
        /// 清空ThreadLocalTable
        /// </summary>
        public static void Clear()
        {
            threadLocalTable.Value.Clear();
        }
    }
}
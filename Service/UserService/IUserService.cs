using Commons.BaseModels;
using Mapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
  public  interface IUserService
    {
        /// <summary>
        /// 获取用户消费信息
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        Hashtable GetBalance(long uid);
        /// <summary>
        /// 获取用户数据
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        eshop_user getUserInfo(long uid);
        /// <summary>
        /// 所有用户
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        PageModel getUsers(QueryData queryData);

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="eshop_User"></param>
        void UpdateUserInfo(Dictionary<string, object> eshop_User);
    }
}

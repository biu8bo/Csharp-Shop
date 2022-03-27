using Commons.BaseModels;
using Mapper;
using MVC卓越项目.Commons.Attribute;
using MVC卓越项目.Commons.Utils;
using Service.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace MVC卓越项目.Controller.User.Rest
{
    /// <summary>
    /// 用户充值模块
    /// </summary>
    [RoutePrefix("api/recharge")]
    public class RechargeController:ApiController
    {
        private readonly IRechargeService rechargeService = Bootstrapper.Resolve<IRechargeService>();
        private readonly IUserService userService = Bootstrapper.Resolve<IUserService>();
        /// <summary>
        /// 获取充值计划
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getRecharge")]
        public ApiResult<List<system_group_data>> GetRecharge()
        {
             return ApiResult<List<system_group_data>>.ok(rechargeService.GetRecharge());
        }

        /// <summary>
        /// 获取用户余额信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AuthCheck]
        [Route("getBalance")]
        public ApiResult<Hashtable> GetBalance()
        {
            return ApiResult<Hashtable>.ok(userService.GetBalance(LocalUser.getUidByUser()));
        }
        /// <summary>
        /// 充值
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AuthCheck]
        [Route("recharge")]
        public ApiResult<Hashtable> Recharge(int id)
        {
            rechargeService.Recharge(LocalUser.getUidByUser(), id);
            return ApiResult<Hashtable>.ok();
        }
    }
}
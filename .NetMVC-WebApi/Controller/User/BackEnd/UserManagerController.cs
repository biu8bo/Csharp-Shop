using Commons.BaseModels;
using Mapper;
using MVC卓越项目.Commons.Attribute;
using MVC卓越项目.Commons.Utils;
using Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MVC卓越项目.Controller.User.BackEnd
{
    [RoutePrefix("api/user")]
    public class UserManagerController : ApiController
    {
        private readonly IUserService userService = Bootstrapper.Resolve<IUserService>();
        /// <summary>
        /// 获取全部用户信息
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        [Route("get")]
        [BackAuthCheck]
        public ApiResult<PageModel> getUsers([FromUri]QueryData queryData)
        {
            return ApiResult<PageModel>.ok(userService.getUsers(queryData));
        }
        /// <summary>
        /// 冻结/解冻用户状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("status/{id}")]
        [BackAuthCheck]
        public ApiResult<PageModel> OnStatus(long id,[FromBody]Dictionary<string,Object> dic)
        {
            using (var db  = new eshoppingEntities())
            {
                eshop_user user = db.eshop_user.Find(id);
                user.status = Convert.ToBoolean(dic["status"]);
                RedisHelper.DeleteKeyByLike($"USER:{user.username}*");
                db.SaveChanges();
            }
            return ApiResult<PageModel>.ok();
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        [Route("update")]
        [BackAuthCheck]
        public ApiResult<PageModel> UpdateUserInfo([FromBody] Dictionary<string,object> eshop_User)
        {
            userService.UpdateUserInfo(eshop_User);
            return ApiResult<PageModel>.ok();
        }


        /// <summary>
        /// 修改用户余额
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        [Route("money")]
        [BackAuthCheck]
        public ApiResult<PageModel> UpdateUserMoney([FromBody] Dictionary<string, object> eshop_User)
        {
            using (var  db = new eshoppingEntities())
            {
                eshop_user user = db.eshop_user.Find(Convert.ToInt64(eshop_User["uid"]));
                if (eshop_User["ptype"].ToString()=="1")
                {
                    user.now_money += Convert.ToDecimal(eshop_User["money"]);
                    
                }
                else
                {
                    user.now_money -= Convert.ToDecimal(eshop_User["money"]);
                
                }
                db.SaveChanges();
            }
            return ApiResult<PageModel>.ok();
        }
    }
}
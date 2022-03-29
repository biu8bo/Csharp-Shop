using Commons.BaseModels;
using Mapper;
using MVC卓越项目.Commons.Attribute;
using MVC卓越项目.Commons.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace MVC卓越项目.Controller.Express.Rest
{
    /// <summary>
    /// 快递模块
    /// </summary>
    [RoutePrefix("api")]
    public class ExpressController:ApiController
    {
        /// <summary>
        /// 获取所有快递公司
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        [BackAuthCheck]
        [HttpGet]
        [Route("express")]
        public ApiResult<PageModel> GetExpresses([FromUri]QueryData queryData)
        {
            using (var db = new eshoppingEntities())
            {
                var result =  new PageUtils<express>(queryData.Page, queryData.Limit).StartPage(db.expresses.Where(e => e.is_del == 0).OrderByDescending(e=>e.sort));
                return ApiResult<PageModel>.ok(result);
            }
        }

        /// <summary>
        /// 添加快递公司
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        [BackAuthCheck]
        [HttpPost]
        [Route("express")]
        public ApiResult<PageModel> AddExpresses([FromBody]express express)
        {
            using (var db = new eshoppingEntities())
            {
                express.is_del = 0;
                express.is_show = true;
                express.create_time = DateTime.Now;
                express.update_time = DateTime.Now;
                db.expresses.Add(express);
                db.SaveChanges();
                return ApiResult<PageModel>.ok();
            }
        }


        /// <summary>
        /// 删除快递公司
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        [BackAuthCheck]
        [HttpDelete]
        [Route("express/{id}")]
        public ApiResult<PageModel>  DeleteExpresses(int id)
        {
            using (var db = new eshoppingEntities())
            {
               db.expresses.Where(e=>e.id==id).First().is_del=1;
                db.SaveChanges();
                return ApiResult<PageModel>.ok();
            }
        }


        /// <summary>
        /// 编辑快递公司
        /// </summary>
        /// <returns></returns>
        [BackAuthCheck]
        [HttpPut]
        [Route("express")]
        public ApiResult<PageModel> EditExpresses([FromBody] express express)
        {
            using (var db = new eshoppingEntities())
            {
                express.is_del = 0;
                express.update_time = DateTime.Now;
                db.Entry(express).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return ApiResult<PageModel>.ok();
            }
        }
    }
}
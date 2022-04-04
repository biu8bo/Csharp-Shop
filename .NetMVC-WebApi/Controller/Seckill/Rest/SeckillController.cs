using Commons.BaseModels;
using Mapper;
using MVC卓越项目.Commons.Attribute;
using MVC卓越项目.Commons.ExceptionHandler;
using MVC卓越项目.Commons.Utils;
using MVC卓越项目.Controller.Order.Param;
using MVC卓越项目.Controller.Seckill.Param;
using Newtonsoft.Json;
using Service.CartService.Param;
using Service.OrderService.VO;
using Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace MVC卓越项目.Controller.Seckill.Rest
{
    /// <summary>
    /// 秒杀模块
    /// </summary>
    [RoutePrefix("api")]
    public class SeckillController:ApiController
    {
        /// <summary>
        /// 注入秒杀Service
        /// </summary>
        private readonly ISeckillService seckillService = Bootstrapper.Resolve<ISeckillService>();

        /// <summary>
        ///编辑秒杀的时间模板
        /// </summary>
        /// <param name="seckillTimeConfigParam"></param>
        [HttpPut]
        [Route("editSeckillTime")]
        [BackAuthCheck]
        public ApiResult<int> UpdateSeckillTime([FromBody] SeckillTimeConfigParam seckillTimeConfigParam)
        {
            using (var db = new eshoppingEntities())
            {
               system_group_data Group_Data = db.system_group_data.Find(seckillTimeConfigParam.id);
                Group_Data.update_time = DateTime.Now;
                Group_Data.value = JsonConvert.SerializeObject(seckillTimeConfigParam);
                db.SaveChanges();
                return ApiResult<int>.ok();
            }
        }

        /// <summary>
        ///后台获取所有秒杀商品
        /// </summary>
        [HttpGet]
        [Route("Seckill")]
        [BackAuthCheck]
        public ApiResult<PageModel> GetSeckill([FromUri]QueryData queryData)
        {
            ;
            return ApiResult<PageModel>.ok(seckillService.getAllStoreSeckill(queryData));
        }

        /// <summary>
        ///添加秒杀方案
        /// </summary>
        [HttpPost]
        [Route("addSeckill")]
        [BackAuthCheck]
        public ApiResult<int> AddSeckill([FromBody] store_seckill seckill)
        {
            seckillService.AddSeckillProduct(seckill);
            return ApiResult<int>.ok();
        }

        /// <summary>
        ///删除秒杀方案
        /// </summary>
        [HttpDelete]
        [Route("delSeckill/{id}")]
        [BackAuthCheck]
        public ApiResult<int> DeleteSeckill(int id)
        {
            seckillService.RemoveSeckillByID(id);
            return ApiResult<int>.ok();
        }

        /// <summary>
        ///添加秒杀的时间模板
        /// </summary>
        /// <param name="seckillTimeConfigParam"></param>
        [HttpPut]
        [Route("addSeckillTime")]
        [BackAuthCheck]
        public ApiResult<int> AddSeckillTime([FromBody] SeckillTimeConfigParam seckillTimeConfigParam)
        {
            using (var db = new eshoppingEntities())
            {
                system_group_data group_Data = new system_group_data();
                group_Data.value = JsonConvert.SerializeObject(seckillTimeConfigParam);
                group_Data.is_del = false;
                group_Data.create_time = DateTime.Now;
                group_Data.status = true;
                group_Data.group_name = "yshop_seckill_time";
                group_Data.update_time = DateTime.Now;
                db.system_group_data.Add(group_Data);
                db.SaveChanges();
                return ApiResult<int>.ok();
            }
        }

        /// <summary>
        /// 前台获取所有秒杀商品信息通过时间模板
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getSeckill")]
        public ApiResult<Object> getSeckillData([FromUri]int templateID) {
            return ApiResult<Object>.ok(seckillService.GetStore_Seckills(templateID));
        }

        /// <summary>
        /// 根据id获取秒杀信息
        /// </summary>
        /// <param name="sid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getSeckillDetail/{id}")]
        public ApiResult<Object> getSeckillDataByID(int id)
        {
            return ApiResult<Object>.ok(seckillService.GetStore_SeckillsByID(id));
        }

        /// <summary>
        /// 获取秒杀时间配置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getSeckillTime")]
        public ApiResult<List<system_group_data>> getSeckillTime()
        {
            return ApiResult<List<system_group_data>>.ok(seckillService.getSeckillTime());
        }
        /// <summary>
        /// 秒杀接口
        /// </summary>
        /// <param name="cart"></param>
        /// <returns></returns>
        [AuthCheck]
        [HttpPost]
        [Route("Seckill")]
        public ApiResult<OrderConfirmVO> Seckill([FromBody]CartParam cart)
        {
           long uid = LocalUser.getUidByUser();
            //用户未登录异常
            if (uid==0L)
            {
                throw new ApiException(400,"请先登录");
            }
            return ApiResult<OrderConfirmVO>.ok(seckillService.Seckill(cart, uid));
        }

        /// <summary>
        /// 秒杀支付
        /// </summary>
        /// <param name="cart"></param>
        /// <returns></returns>
        [AuthCheck]
        [HttpPost]
        [Route("SeckillPay")]
        public ApiResult<OrderConfirmVO> SeckillPay([FromBody] PayParam payParam)
        {
            long uid = LocalUser.getUidByUser();
            //用户未登录异常
            if (uid == 0L)
            {
                throw new ApiException(400, "请先登录");
            }
            seckillService.SeckilPay(payParam.orderKey, payParam.sid, payParam.mark, uid);

            return ApiResult<OrderConfirmVO>.ok();
        }

    }
}
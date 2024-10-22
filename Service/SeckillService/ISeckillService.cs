﻿using Commons.BaseModels;
using Mapper;
using Service.CartService.Param;
using Service.OrderService.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    /// <summary>
    /// 商品秒杀模块
    /// </summary>
  public  interface ISeckillService
    {
        /// <summary>
        /// 获取所有秒杀方案
        /// </summary>
        /// <returns></returns>
        PageModel getAllStoreSeckill(QueryData queryData);

        /// <summary>
        /// 添加秒杀方案
        /// </summary>
        /// <param name="store_Seckill"></param>
        void AddSeckillProduct(store_seckill store_Seckill);

        /// <summary>
        /// 通过id删除方案
        /// </summary>
        /// <param name="id"></param>
        void RemoveSeckillByID(int id);

        /// <summary>
        /// 前端获取所有可用秒杀
        /// </summary>
        /// <returns></returns>
        Object GetStore_Seckills(int id);
        /// <summary>
        /// 前端获取秒杀时间段配置
        /// </summary>
        /// <returns></returns>
        List<system_group_data> getSeckillTime();
        /// <summary>
        /// 根据秒杀id获取秒杀商品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Object GetStore_SeckillsByID(int id);

        /// <summary>
        /// 秒杀抢购
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        OrderConfirmVO Seckill(CartParam cartParam, long uid);
        /// <summary>
        /// 秒杀支付处理
        /// </summary>
        /// <param name="orderKey"></param>
        /// <param name="mark"></param>
        /// <param name="uid"></param>
        void SeckilPay(string orderKey, int sid,string mark, long uid);
    }
}

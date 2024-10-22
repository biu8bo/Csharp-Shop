﻿using Commons.BaseModels;
using Mapper;
using Service.OrderService.Param;
using Service.OrderService.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    /// <summary>
    /// 订单服务接口
    /// </summary>
 public interface IOrderService
    {
        store_order GetOrderInfoByOrderID(string orderId, long uid);
        /// <summary>
        /// 通过用户ID和订单类型查询订单信息
        /// </summary>
        /// <param name="orderTypeParam"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        PageModel getOrderInfoByType(OrderTypeParam orderTypeParam, long uid);
        /// <summary>
        /// 确认订单
        /// </summary>
        /// <param name="orderIDsParam"></param>
        /// <returns></returns>
        OrderConfirmVO confirmOrder(CartIDIDsParam cartIDsParam, long uid);
        /// <summary>
        /// 更新订单地址
        /// </summary>
        /// <param name="addressId"></param>
        /// <param name="orderKey"></param>
        /// <returns></returns>
        OrderConfirmVO updateOrderAddress(int addressId,string orderKey);

        /// <summary>
        /// 支付订单
        /// </summary>
        void payOrder(string orderKey,string mark, long uid);

        /// <summary>
        /// 取消支付
        /// </summary>
        /// <param name="orderKey"></param>
        /// <param name="uid"></param>
        void CancelOrder(string orderKey,long uid);
        /// <summary>
        /// 付款
        /// </summary>
        /// <param name="orderKey"></param>
        /// <param name="uid"></param>
        void HandlerPay(string orderKey, long uid);

        /// <summary>
        /// 获取全部订单信息
        /// </summary>
        /// <returns></returns>
        PageModel GetOrders(OrderTypeParam orderTypeParam);

        /// <summary>
        /// 订单备注
        /// </summary>
        /// <param name="order"></param>
        void OrderMark(store_order order);

        /// <summary>
        /// 修改订单信息
        /// </summary>
        /// <param name="order"></param>
        void EditOrderInfo(store_order order);
        /// <summary>
        /// 发货
        /// </summary>
        /// <param name="order"></param>
        void DeliverGoods(DeliverParam express);

        /// <summary>
        /// 确认收货
        /// </summary>
        /// <param name="orderId"></param>
        void DeliverOK(string orderId);
        /// <summary>
        /// 订单详情获取
        /// </summary>
        /// <param name="orderID"></param>
        store_order GetOrderDetail(string orderID);
    }
}

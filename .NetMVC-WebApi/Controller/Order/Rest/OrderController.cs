﻿using Commons.BaseModels;
using Mapper;
using MVC卓越项目.Commons.Attribute;
using MVC卓越项目.Commons.Utils;
using MVC卓越项目.Controller.Order.Param;
using Service.OrderService.Param;
using Service.OrderService.VO;
using Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace MVC卓越项目.Controller.Rest.Order
{
    /// <summary>
    /// 订单模块
    /// </summary>
    [RoutePrefix("api")]
   public  class OrderController : ApiController
    {
        private readonly IOrderService orderService = Bootstrapper.Resolve<IOrderService>();
        /// <summary>
        /// 获取所有订单信息
        /// </summary>
        /// <param name="orderTypeParam"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("order")]
        [AuthCheck]
      public  ApiResult<object> GetOrderList(OrderTypeParam orderTypeParam)
        {
            object a = orderService.getOrderInfoByType(orderTypeParam, LocalUser.getUidByUser());
            return ApiResult<object>.ok(a);
        }


       public class UpdateAddressParam
        {
           public int addressId;
           public string orderKey;
        }
        /// <summary>
        /// 更新订单地址信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("updateOrderAddress")]
        [AuthCheck]
       
        public ApiResult<object> updateOrderAddress(UpdateAddressParam param)
        {
            object a = orderService.updateOrderAddress(param.addressId, param.orderKey);
            return ApiResult<object>.ok(a);
        }
        /// <summary>
        /// 确认订单
        /// </summary>
        /// <param name="cartIDIDsParam"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("confirm")]
        [AuthCheck]
        public ApiResult<OrderConfirmVO> Confirm([FromBody]CartIDIDsParam cartIDIDsParam)
        {
            return ApiResult<OrderConfirmVO>.ok(orderService.confirmOrder(cartIDIDsParam, LocalUser.getUidByUser()));
        }


        /// <summary>
        /// 支付接口
        /// </summary>
        /// <param name="payParam"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("payOrder")]
        [AuthCheck]
        public ApiResult<OrderConfirmVO> pay([FromBody]PayParam payParam)
        {
            orderService.payOrder(payParam.orderKey, payParam.mark, LocalUser.getUidByUser());
            return ApiResult<OrderConfirmVO>.ok();
        }
        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="payParam"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("cancelOrder")]
        [AuthCheck]
        public ApiResult<OrderConfirmVO> CancelOrder([FromBody] PayParam payParam)
        {
            orderService.CancelOrder(payParam.orderKey, LocalUser.getUidByUser());
            return ApiResult<OrderConfirmVO>.ok();
        }


        /// <summary>
        /// 付款
        /// </summary>
        /// <param name="payParam"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("handlerPay")]
        [AuthCheck]
        public ApiResult<OrderConfirmVO> HandlerPay([FromBody] PayParam payParam)
        {
            orderService.HandlerPay(payParam.orderKey, LocalUser.getUidByUser());
            return ApiResult<OrderConfirmVO>.ok();
        }
        /// <summary>
        /// 获取全部订单信息
        /// </summary>
        /// <param name="payParam"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("order/get")]
        [BackAuthCheck]
        public ApiResult<PageModel> GetOrders([FromUri] OrderTypeParam payParam)
        {
           
            return ApiResult<PageModel>.ok(orderService.GetOrders(payParam));
        }
        /// <summary>
        /// 订单备注
        /// </summary>
        /// <param name="payParam"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("order/remark")]
        [BackAuthCheck]
        public ApiResult<int> OrderRemark([FromBody] store_order payParam)
        {
            orderService.OrderMark(payParam);
            return ApiResult<int>.ok();
        }

        /// <summary>
        /// 修改订单
        /// </summary>
        /// <param name="payParam"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("order/edit")]
        [BackAuthCheck]
        public ApiResult<int> EditOrderInfo([FromBody] store_order payParam)
        {
            orderService.EditOrderInfo(payParam);
            return ApiResult<int>.ok();
        }


        /// <summary>
        /// 发货
        /// </summary>
        /// <param name="payParam"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("order/deliver")]
        [BackAuthCheck]
        public ApiResult<int> DeliverGoods([FromBody] DeliverParam payParam)
        {
            orderService.DeliverGoods(payParam);
            return ApiResult<int>.ok();
        }


        /// <summary>
        /// 确认收货
        /// </summary>
        /// <param name="payParam"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("deliverOK/{orderId}")]
        [AuthCheck]
        public ApiResult<int> DeliverOK(string orderId)
        {
            orderService.DeliverOK(orderId);
            return ApiResult<int>.ok();
        }

        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("order/{orderId}")]
        [AuthCheck]
        public ApiResult<store_order> GetOrder(string orderId)
        {
         
            return ApiResult<store_order>.ok(orderService.GetOrderDetail(orderId));
        }
    }
}

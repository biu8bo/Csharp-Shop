using Commons.BaseModels;
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
    }
}

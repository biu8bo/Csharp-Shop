using Commons.BaseModels;
using MVC卓越项目.Commons.Attribute;
using MVC卓越项目.Commons.Utils;
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
        [HttpPost]
        [Route("order")]
        [AuthCheck]
      public  ApiResult<object> GetOrderList(OrderTypeParam orderTypeParam)
        {
            object a = orderService.getOrderInfoByType(orderTypeParam, LocalUser.getUidByUser());
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
    }
}

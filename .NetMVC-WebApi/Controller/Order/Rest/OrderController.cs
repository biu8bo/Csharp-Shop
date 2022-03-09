using Commons.BaseModels;
using MVC卓越项目.Commons.Attribute;
using MVC卓越项目.Commons.Utils;
using Service.OrderService.Param;
using Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace MVC卓越项目.Controller.Rest.Order
{
    [RoutePrefix("api")]
   public  class OrderController : ApiController
    {
        private readonly IOrderService orderService = Bootstrapper.Resolve<IOrderService>();
        [HttpPost]
        [Route("order")]
        [AuthCheck]
      public  ApiResult<PageModel> GetOrderList(OrderTypeParam orderTypeParam)
        {
            object a = orderService.getOrderInfoByType(orderTypeParam, LocalUser.getUidByUser());
            return ApiResult<PageModel>.ok(a);
        }
    }
}

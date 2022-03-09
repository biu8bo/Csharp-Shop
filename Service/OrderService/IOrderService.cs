using Commons.BaseModels;
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
   /// <summary>
   /// 通过用户ID和订单类型查询订单信息
   /// </summary>
   /// <param name="orderTypeParam"></param>
   /// <param name="uid"></param>
   /// <returns></returns>
       PageModel getOrderInfoByType(OrderTypeParam orderTypeParam, long uid);
        
    }
}

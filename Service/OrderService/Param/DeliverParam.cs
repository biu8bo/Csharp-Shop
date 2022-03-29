using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.OrderService.Param
{
   public class DeliverParam
    {
        public int id { get; set; }
        public string  deliveryName{ get; set; }
        public string deliveryType { get; set; }
        public string deliveryId { get; set; }
    }
}

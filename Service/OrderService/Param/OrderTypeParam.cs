using Commons.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.OrderService.Param
{
  public  class OrderTypeParam:QueryParam
    {
        public int orderType { get; set; }
        public string type { get; set; }
        public string value { get; set; }
        public int orderStatus { get; set; }

        public DateTime? startTime { get; set; }
        public DateTime? endTime { get; set; }
    }
}

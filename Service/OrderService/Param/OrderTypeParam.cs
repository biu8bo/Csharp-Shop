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
    }
}

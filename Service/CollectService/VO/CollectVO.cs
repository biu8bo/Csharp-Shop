using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.CollectService.VO
{
  public  class CollectVO
    {
        public int id { get; set; }
        public int pid { get; set; }
        public string image { get; set; }
        public string storeName { get; set; }
        public string storeInfo { get; set; }
        public decimal price  { get; set; }
        
    }
}

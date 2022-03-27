using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ProductService.VO
{

    /// <summary>
    /// Attr参数类
    /// </summary>
  public  class AttrResult
    {
        public List<Attr> items { get; set; }
        public List<Value> attrs { get; set; }


        public class Attr
        {
            public  List<string> detail { get; set; }
           public string value { get; set; }
        }


        /// <summary>
        /// sku值类型
        /// </summary>
     public   class Value
        {
            public string pic { get; set; }
            public string barCod { get; set; }
            public int  brokerage { get; set; }
            public int brokerageTwo { get; set; }
            public decimal cost { get; set; }
            public Dictionary<string,Object> detail { get; set; }
            public decimal integral { get; set; }
            public decimal otPrice { get; set; }
            public decimal pinkPrice { get; set; }
            public int pinkStock { get; set; }
            public decimal price { get; set; }
            public decimal seckillPrice { get; set; }
            public int seckillStock { get; set; }
            public string sku { get; set; }
            public int stock { get; set; }
            public string value1 { get; set; }
            public string value2 { get; set; }
            public int volume { get; set; }
            public decimal weight { get; set; }
        }
    }
}

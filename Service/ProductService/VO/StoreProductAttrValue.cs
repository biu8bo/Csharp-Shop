using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ProductService
{
  public  class StoreProductAttrValue
    {
        public decimal id { get; set; }
        public decimal product_id { get; set; }
        public string sku { get; set; }
        public long stock { get; set; }
        public Nullable<long> sales { get; set; }
        public string image { get; set; }
        public string unique { get; set; }
        public string bar_code { get; set; }
        public Nullable<decimal> ot_price { get; set; }
        public decimal weight { get; set; }
        public decimal volume { get; set; }
        public decimal brokerage { get; set; }
        public decimal brokerage_two { get; set; }
        public decimal pink_price { get; set; }
        public int pink_stock { get; set; }
        public decimal seckill_price { get; set; }
        public int seckill_stock { get; set; }
        public Nullable<long> integral { get; set; }
    }
}

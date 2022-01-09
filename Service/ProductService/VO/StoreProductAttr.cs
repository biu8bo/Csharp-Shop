using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ProductService
{
    /// <summary>
    ///商品属性
    /// </summary>
    public class StoreProductAttr
    {
        public decimal id { get; set; }
        public decimal product_id { get; set; }
        public string attr_name { get; set; }
        public string attr_values { get; set; }
        /// <summary>
        /// 详细规格数据
        /// </summary>
        public List<StoreProductAttrValue> storeProductAttrValues { get; set; }
}
}

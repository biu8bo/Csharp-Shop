using Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ProductService
{
    /// <summary>
    /// 商品详情
    /// </summary>
   public class ProductVO
    {
        public long id { get; set; }
        public Nullable<long> mer_id { get; set; }
        public string image { get; set; }
        public string slider_image { get; set; }
        public string store_name { get; set; }
        public string store_info { get; set; }
        public string keyword { get; set; }
        public string bar_code { get; set; }
        public string cate_id { get; set; }
        public string unit_name { get; set; }
        public Nullable<short> sort { get; set; }
        public Nullable<int> sales { get; set; }
        public Nullable<int> stock { get; set; }
        public Nullable<bool> is_show { get; set; }
        public Nullable<bool> is_hot { get; set; }
        public Nullable<bool> is_benefit { get; set; }
        public Nullable<bool> is_best { get; set; }
        public Nullable<bool> is_new { get; set; }
        public string description { get; set; }
        public Nullable<System.DateTime> create_time { get; set; }
        public Nullable<System.DateTime> update_time { get; set; }
        public Nullable<bool> is_postage { get; set; }
        public Nullable<bool> is_del { get; set; }
        public Nullable<bool> mer_use { get; set; }
        public Nullable<bool> is_seckill { get; set; }
        public Nullable<bool> is_bargain { get; set; }
        public Nullable<bool> is_good { get; set; }
        public Nullable<int> ficti { get; set; }
        public Nullable<int> browse { get; set; }
        public string code_path { get; set; }
        public Nullable<bool> is_sub { get; set; }
        public Nullable<int> temp_id { get; set; }
        public Nullable<bool> spec_type { get; set; }
        public Nullable<bool> is_integral { get; set; }
        public Nullable<int> integral { get; set; }

        /// <summary>
        /// 是否收藏
        /// </summary>
        public bool iscollect { get; set; }
        /// <summary>
        /// 评论信息
        /// </summary>
        public store_product_reply StoreProductReply { get; set; }
        /// <summary>
        /// 收藏与足迹
        /// </summary>
        public store_product_relation StoreProductRelation { get; set; }
        /// <summary>
        /// 规格参数
        /// </summary>
        public List<StoreProductAttr> storeProductAttrs { get; set; }
    }
}

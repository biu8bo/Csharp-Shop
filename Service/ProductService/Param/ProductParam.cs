using Commons.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Service.Service
{
    public class ProductParam : QueryParam
    {
        //关键字
        public string keyword { get; set; }

        //是否积分
        public Nullable<bool> isIntegral { get; set; }
        //是否价格排序
        public string priceOrder { get; set; }
        //是否新品
        public Nullable<bool> isNew { get; set; }

        //销量排序
        public string salesOrder { get; set; }
    }
}
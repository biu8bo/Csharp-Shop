using Commons.Utils;
using Mapper;
using Service.ProductService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    /// <summary>
    /// 商品规格模块
    /// </summary>
  public  interface IProductAttrService
    {
        /// <summary>
        ///  获取 商品属性信息
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        List<StoreProductAttr> GetProductAttr(long pid);

        /// <summary>
        /// 获取商品 （SKU）属性值
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        List<StoreProductAttrValue> GetProductAttrValue(long pid);

    }
}

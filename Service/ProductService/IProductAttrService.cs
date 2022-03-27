using Commons.BaseModels;
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

        /// <summary>
        /// 获取所有规格模板集合
        /// </summary>
        /// <param name="queryParam"></param>
        /// <returns></returns>
        PageModel GetStoreProductRules(QueryParam queryParam);

        /// <summary>
        /// 获取商品的规格sku参数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        store_product_attr_result GetProductAttrResultByID(int id);

    }
}

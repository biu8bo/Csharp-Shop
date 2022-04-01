using Commons.BaseModels;
using Commons.Enum;
using Mapper;
using Service.ProductService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service.Service
{
    /// <summary>
    /// 商品模块
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// 查询所有商品信息
        /// </summary>
        /// <returns></returns>
        PageModel selectAllProducts(ProductParam param);
        /// <summary>
        /// 分页查询商品信息
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        PageModel selectProductsByPage(int page, int limit);
        /// <summary>
        /// 分页条件查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="iquery"></param>
        /// <returns></returns>
        PageModel selectPageByIQuery(int page, int limit, IQueryable<store_product> iquery);
        /// <summary>
        /// 获取商品详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ProductVO getProductById(long pid, long uid);

        /// <summary>
        /// 增加浏览量
        /// </summary>
        /// <param name="pid"></param>
        void incBrowseNum(long pid);

        /// <summary>
        /// 商品搜索
        /// </summary>
        /// <param name="productParam"></param>
        PageModel searchProducts(ProductParam productParam);


        /// <summary>
        /// 添加商品
        /// </summary>
        /// <param name="product"></param>
        void AddProduct(store_product product);

        /// <summary>
        /// 编辑商品
        /// </summary>
        /// <param name="product"></param>
        void EditProduct(store_product product);

        /// <summary>
        /// 删除商品
        /// </summary>
        /// <param name="product"></param>
        void DelProduct(store_product product);

        //修改在售状态
        void OnSalesStatus(ProductParam param);

    }
}

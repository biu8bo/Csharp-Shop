using Commons.BaseModels;
using Commons.Enum;
using Mapper;
using Service.ProductService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service.Service
{
    public interface IProductService
    {
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
        ProductVO getProductById(long id);
    }
}

using Mapper;
using System;
using System.Collections.Generic;

namespace Service.Service
{
    public interface IProductService
    {
        /// <summary>
        /// 查询指定类型
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        List<store_product> GetList(int page, int limit, int flag);
        /// <summary>
        /// 分页查询商品信息
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        List<store_product> selectByPage(int page, int limit);
    }
}

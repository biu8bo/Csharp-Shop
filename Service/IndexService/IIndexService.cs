using Commons.BaseModels;
using Commons.Enum;
using Mapper;
using System.Collections.Generic;

namespace Service.Service
{
    public interface IIndexService
    {
        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="shopConstants"></param>
        /// <returns></returns>
        List<system_group_data> GetDataByShopConstants(string shopConstants);
        /// <summary>
        /// 查询指定类型
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        PageModel GetList(int page, int limit, ProductEnum flag);
    }
}

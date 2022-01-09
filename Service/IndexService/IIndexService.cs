using Commons.BaseModels;
using Mapper;
using System.Collections.Generic;

namespace Service.Service
{
    public interface IIndexService
    {
        List<system_group_data> GetDataByShopConstants(string shopConstants);
    }
}

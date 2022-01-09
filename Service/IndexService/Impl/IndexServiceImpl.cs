using Commons.BaseModels;
using Commons.Constant;
using Mapper;
using MVC卓越项目.Commons.Utils;
using Service.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class IndexServiceImpl : IIndexService
    {
        public List<system_group_data> GetDataByShopConstants(string shopConstants)
        {
            using (var ctx = new eshoppingEntities())
            {
                return ctx.system_group_data.Where(e => e.group_name == shopConstants).ToList();
            }
        }
    }
}

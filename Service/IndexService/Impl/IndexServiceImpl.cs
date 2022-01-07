using Commons.Constant;
using Mapper;
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
        public List<system_group_data> GetIndexBanner()
        {
            using (var ctx = new eshoppingEntities())
            {
                  return ctx.system_group_data.Where(e =>  e.group_name == ShopConstants.YSHOP_HOME_BANNER).ToList();
            }
        }
    }
}

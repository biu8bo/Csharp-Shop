using Commons.BaseModels;
using Commons.Constant;
using Commons.Enum;
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

        public PageModel GetList(int page, int limit, ProductEnum flag)
        {
            using (var db = new eshoppingEntities())
            {
                PageUtils<store_product> pageUtils = new PageUtils<store_product>();
                if (ProductEnum.TYPE_1 == flag)
                {
                    return pageUtils.StartPage(db.store_product.Where(e => e.is_best == true).OrderBy(e => e.id));
                }
                else if (ProductEnum.TYPE_2 == flag)
                {
                    return pageUtils.StartPage(db.store_product.Where(e => e.is_hot == true).OrderBy(e => e.id));
                }
                else if (ProductEnum.TYPE_3 == flag)
                {
                    return pageUtils.StartPage(db.store_product.Where(e => e.is_new == true).OrderBy(e => e.id));
                }
                else if (ProductEnum.TYPE_4 == flag)
                {
                    return pageUtils.StartPage(db.store_product.Where(e => e.is_benefit == true).OrderBy(e => e.id));
                }
                return null;
            }
        }
    }
}

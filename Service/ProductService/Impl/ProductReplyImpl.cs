using Commons.BaseModels;
using Mapper;
using MVC卓越项目.Commons.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class ProductReplyImpl : IProductReply
    {
        public PageModel GetReplyByPid(long pid,int page,int limit)
        {
            using (var db = new eshoppingEntities())
            {
                return new PageUtils<store_product_reply>(page,limit).StartPage(db.store_product_reply.Where(e => e.product_id == pid && e.is_del == false).OrderBy(e=>e.id));
            }
        }
    }
}

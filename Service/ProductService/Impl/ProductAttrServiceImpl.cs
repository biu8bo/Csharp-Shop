using Commons.BaseModels;
using Commons.Utils;
using Mapper;
using MVC卓越项目.Commons.Utils;
using Service.ProductService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class ProductAttrServiceImpl : IProductAttrService
    {
        public List<StoreProductAttr> GetProductAttr(long pid)
        {
            using (var db = new eshoppingEntities())
            {
                return ObjectUtils<List<StoreProductAttr>>.ConvertTo(db.store_product_attr.Where(e => e.product_id == pid).ToList());
            }
        }

        public store_product_attr_result GetProductAttrResultByID(int id)
        {
            using (var db = new eshoppingEntities())
            {
                 return db.store_product_attr_result.Where(e => e.product_id == id).FirstOrDefault();
            }
        }

        public List<StoreProductAttrValue> GetProductAttrValue(long pid)
        {
            using (var db = new eshoppingEntities())
            {
                return ObjectUtils<List<StoreProductAttrValue>>.ConvertTo(db.store_product_attr_value.Where(e => e.product_id == pid).ToList());
            }
        }

        public PageModel GetStoreProductRules(QueryParam queryParam)
        {
            using (var db = new eshoppingEntities())
            {

              return  new PageUtils<store_product_rule>(queryParam.Page,queryParam.Limit).StartPage(db.store_product_rule.Where(e => e.is_del == false).OrderBy(e=>e.id));
            }
        }
    }
}

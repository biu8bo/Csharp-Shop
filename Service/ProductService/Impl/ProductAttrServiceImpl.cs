using Commons.Utils;
using Mapper;
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

        public List<StoreProductAttrValue> GetProductAttrValue(long pid)
        {
            using (var db = new eshoppingEntities())
            {
                return ObjectUtils<List<StoreProductAttrValue>>.ConvertTo(db.store_product_attr_value.Where(e => e.product_id == pid).ToList());
            }
        }
    }
}

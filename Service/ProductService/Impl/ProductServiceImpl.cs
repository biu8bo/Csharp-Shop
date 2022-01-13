using Commons.BaseModels;
using Commons.Constant;
using Commons.Enum;
using Commons.Utils;
using Mapper;
using MVC卓越项目;
using MVC卓越项目.Commons.Utils;
using Service.ProductService;
using Service.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class ProductServiceImpl : IProductService
    {
        private readonly IProductAttrService iProductAttrService = Bootstrapper.Resolve<IProductAttrService>();

        public PageModel selectProductsByPage(int page, int limit)
        {
            using (var db = new eshoppingEntities())
            {
                
                return new PageUtils<store_product>(page, limit).StartPage(db.store_product.Where(e=>e.is_show==true&&e.is_del == false).OrderBy(e => e.id));
            }
        }

        public PageModel selectPageByIQuery(int page, int limit,IQueryable<store_product> iquery)
        {
            return new PageUtils<store_product>(page, limit).StartPage(iquery.Where(e => e.is_show == true && e.is_del == false).OrderBy(e=>e.id));
        }

        public ProductVO getProductById(long id)
        {
            //查询商品基本信息
            using (var db = new eshoppingEntities())
            {
                store_product storeProduct = db.store_product.Where(e => e.id == id && e.is_del == false &&e.is_show==true).FirstOrDefault();
                ProductVO productVO = new ProductVO();
                ObjectUtils<ProductVO>.ConvertTo(storeProduct,ref productVO);
                //查询规格
               List<StoreProductAttr> storeProductAttrs = iProductAttrService.GetProductAttr(id);
                //查询规格详情
                List<StoreProductAttrValue> storeProductAttrValues = iProductAttrService.GetProductAttrValue(id);
                productVO.storeProductAttrs = storeProductAttrs;
                productVO.storeProductAttrValues = storeProductAttrValues;

                return productVO;
            }

        }
    }
}

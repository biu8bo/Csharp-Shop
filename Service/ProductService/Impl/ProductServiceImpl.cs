using Commons.BaseModels;
using Commons.Constant;
using Commons.Enum;
using Commons.Utils;
using Mapper;
using MVC卓越项目;
using MVC卓越项目.Commons.ExceptionHandler;
using MVC卓越项目.Commons.Utils;
using Service.ProductService;
using Service.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class ProductServiceImpl : IProductService
    {
        private readonly IProductAttrService iProductAttrService = Bootstrapper.Resolve<IProductAttrService>();
        private readonly ICollectService iCollectService = Bootstrapper.Resolve<ICollectService>();
        private readonly IProductReplyService iProductReply = Bootstrapper.Resolve<IProductReplyService>();
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

        public ProductVO getProductById(long pid, long uid)
        {
            //查询商品基本信息
            using (var db = new eshoppingEntities())
            {
                store_product storeProduct = db.store_product.Where(e => e.id == pid && e.is_del == false &&e.is_show==true).FirstOrDefault();
                if (ObjectUtils<object>.isNull(storeProduct))
                {
                    throw new ApiException(500,"商品已下架或不存在!");
                }
                ProductVO productVO = new ProductVO();
                ObjectUtils<ProductVO>.ConvertTo(storeProduct,ref productVO);
                //查询规格
               List<StoreProductAttr> storeProductAttrs = iProductAttrService.GetProductAttr(pid);
                //查询规格详情
                List<StoreProductAttrValue> storeProductAttrValues = iProductAttrService.GetProductAttrValue(pid);
                productVO.storeProductAttrs = storeProductAttrs;
                productVO.storeProductAttrValues = storeProductAttrValues;

                if (uid!=0L)
                {
                    //查询是否已经收藏
                    productVO.isCollect=ObjectUtils<object>.isNotNull(iCollectService.isProductRelation(pid, uid, "collect"));
                    iCollectService.addRroductRelation(pid, uid, "foot");
                }
                else
                {
                    productVO.isCollect = false;
                }

                return productVO;
            }

        }
        public PageModel searchProducts(ProductParam productParam)
        {
            using (var db = new eshoppingEntities())
            {
                QueryExpressionUtils<store_product> where = new QueryExpressionUtils<store_product>();
                where.And(e => e.is_show == true&&e.is_del == false);

                //如果关键词不为空
                if (ObjectUtils<string>.isNotNull(productParam.keyword))
                {
                    where.And(e => e.keyword.Contains(productParam.keyword));
                }
                //如果是积分商品
                if (ObjectUtils<bool>.isNotNull(productParam.isIntegral)&&productParam.isIntegral==true)
                {
                    where.And(e => e.is_integral == true);
       
                }
                //如果是新品
                 if (ObjectUtils<bool>.isNotNull(productParam.isNew)&& productParam.isNew==true)
                {
                    where.And(e => e.is_new == true);
                }
                if (ObjectUtils<bool>.isNotNull(productParam.cid))
                {
                    where.And(e => e.cate_id == productParam.cid);
                }
                //构造SQL
                IQueryable<store_product> query = db.store_product.Where(where.GetExpression());
                //如果是销量排序
                if (ObjectUtils<bool>.isNotNull(productParam.salesOrder))
                {
                    //升序
                    if (productParam.salesOrder.Equals("asc"))
                    {
                        return new PageUtils<store_product>(productParam.Page, productParam.Limit).StartPage(query.OrderBy(e => e.sales));
                
                    }
                    //降序
                    else
                    {
                        return new PageUtils<store_product>(productParam.Page, productParam.Limit).StartPage(query.OrderByDescending(e => e.sales));
                    }
                  
                }
                // 如果是价格排序

                if (ObjectUtils<bool>.isNotNull(productParam.priceOrder))
                {
                    //升序
                    if (productParam.priceOrder.Equals("asc"))
                    {
                        return new PageUtils<store_product>(productParam.Page, productParam.Limit).StartPage(query.OrderBy(e => e.price));

                    }
                    //降序
                    else
                    {
                        return new PageUtils<store_product>(productParam.Page, productParam.Limit).StartPage(query.OrderByDescending(e => e.price));
                    }

                }
                return new PageUtils<store_product>(productParam.Page, productParam.Limit).StartPage(query.OrderBy(e=>e.id));
            }
        }
        public void incBrowseNum(long pid)
        {
            using (var db = new eshoppingEntities())
            {
                store_product storeProduct = db.store_product.Where(e => e.id == pid && e.is_del == false && e.is_show == true).FirstOrDefault();
                storeProduct.browse += 1;
                db.SaveChanges();
            }
        }
    }
}

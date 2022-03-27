using Commons.BaseModels;
using Commons.Utils;
using Mapper;
using MVC卓越项目.Commons.Attribute;
using MVC卓越项目.Commons.ExceptionHandler;
using MVC卓越项目.Commons.Utils;
using Service.ProductService;
using Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MVC卓越项目.Controller.Product
{
    /// <summary>
    /// 商品模块
    /// </summary>
    [RoutePrefix("api")]
    public class ProductController : ApiController
    {
        private readonly IProductService productService = Bootstrapper.Resolve<IProductService>();
        private readonly IProductReplyService iProductReply = Bootstrapper.Resolve<IProductReplyService>();
        /// <summary>
        /// 获取商品评论数据
        /// </summary>
        [Route("reply")]
        [HttpGet]
        public ApiResult<PageModel> getReplyByPid(long pid, int page, int limit)
        {
            return ApiResult<PageModel>.ok(iProductReply.GetReplyByPid(pid, page, limit));
        }
        /// <summary>
        /// 获取商品详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("product/{id}")]
        [AuthCheck(required =false)]
        public ApiResult<ProductVO> getProductInfo(long id)
        {
            if (id<0)
            {
                throw new ApiException(500,"商品不存在！");
            }
            long uid = LocalUser.getUidByUser();
            //浏览量增加
            productService.incBrowseNum(id);
            return ApiResult<ProductVO>.ok(productService.getProductById(id, uid));
        }

        /// <summary>
        /// 搜索商品
        /// </summary>
        /// <param name="productParam"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        public ApiResult<PageModel> search([FromBody] ProductParam productParam)
        {
            return ApiResult<PageModel>.ok(productService.searchProducts(productParam));
        }

        /// <summary>
        /// 产品上下架
        /// </summary>
        /// <param name="productParam"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("onsales")]
        [BackAuthCheck]
        public ApiResult<PageModel> onSales([FromBody] ProductParam productParam)
        {
            productService.OnSalesStatus(productParam);
            return ApiResult<PageModel>.ok();
        }
        /// <summary>
        /// 获取全部产品信息
        /// </summary>
        /// <param name="productParam"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("getAllProduct")]
        [BackAuthCheck]
        public ApiResult<PageModel> getAllProduct([FromUri] ProductParam productParam)
        {
            return ApiResult<PageModel>.ok(productService.selectAllProducts(productParam));
        }

        /// <summary>
        /// 删除商品
        /// </summary>
        /// <param name="productParam"></param>
        /// <returns></returns>
        [BackAuthCheck]
        [HttpPost]
        [Route("product/del")]
        public ApiResult<PageModel> DelProduct([FromBody] store_product productParam)
        {
            productService.DelProduct(productParam);
            return ApiResult<PageModel>.ok();
        }
        /// <summary>
        /// 编辑商品信息
        /// </summary>
        /// <param name="productParam"></param>
        [BackAuthCheck]
        [HttpPost]
        [Route("product/edit")]
        public ApiResult<PageModel> EditProduct([FromBody] store_product productParam)
        {
            productService.EditProduct(productParam);
            return ApiResult<PageModel>.ok();
        }
        /// <summary>
        /// 添加商品
        /// </summary>
        /// <param name="productParam"></param>
        [BackAuthCheck]
        [HttpPost]
        [Route("product/add")]
        public ApiResult<PageModel> AddProduct([FromBody]dynamic productParam)
        {

            store_product target = ObjectUtils<store_product>.ConvertTo(productParam);
            productService.AddProduct(target);
            return ApiResult<PageModel>.ok();
           
        }

    }
}
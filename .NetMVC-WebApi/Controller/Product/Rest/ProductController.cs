using Commons.BaseModels;
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
    [RoutePrefix("api")]
    public class ProductController : ApiController
    {
        private readonly IProductService productService = Bootstrapper.Resolve<IProductService>();
        private readonly IProductReplyService iProductReply = Bootstrapper.Resolve<IProductReplyService>();
        //获取商品评论数据
        [Route("reply")]
        [HttpGet]
        public ApiResult<PageModel> getReplyByPid(long pid, int page, int limit)
        {
            return ApiResult<PageModel>.ok(iProductReply.GetReplyByPid(pid, page, limit));
        }

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


        [HttpPost]
        [Route("search")]
        public ApiResult<PageModel> search([FromBody] ProductParam productParam)
        {
            return ApiResult<PageModel>.ok(productService.searchProducts(productParam));
        }
    }
}
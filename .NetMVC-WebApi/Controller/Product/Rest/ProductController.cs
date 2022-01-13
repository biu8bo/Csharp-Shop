using Commons.BaseModels;
using Mapper;
using MVC卓越项目.Commons.Attribute;
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

        [HttpGet]
        [Route("product/{id}")]
        [AuthCheck(required =false)]
        public ApiResult<ProductVO> getProductInfo(long id)
        {
            return ApiResult<ProductVO>.ok(productService.getProductById(id));
        }
    }
}
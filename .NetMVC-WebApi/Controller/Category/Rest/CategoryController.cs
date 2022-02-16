using Commons.BaseModels;
using Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MVC卓越项目.Controller.Category.Rest
{
    [RoutePrefix("api")]
    public class CategoryController : ApiController
    {
        private readonly ICategoryService categoryService = Bootstrapper.Resolve<ICategoryService>();
        
        /// <summary>
        /// 商品分类数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("category")]
        public ApiResult<List<CategoryVO>> Get()
        {
        
            return ApiResult<List<CategoryVO>>.ok(categoryService.GetCategories());
        }
    }
}
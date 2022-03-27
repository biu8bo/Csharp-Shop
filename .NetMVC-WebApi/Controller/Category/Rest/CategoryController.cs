using Commons.BaseModels;
using Mapper;
using MVC卓越项目.Commons.Attribute;
using Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MVC卓越项目.Controller.Category.Rest
{
    /// <summary>
    /// 商品分类
    /// </summary>
    [RoutePrefix("api")]
    public class CategoryController : ApiController
    {
        private readonly ICategoryService categoryService = Bootstrapper.Resolve<ICategoryService>();
        
        /// <summary>
        /// 商品分类数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CacheEnable]
        [Route("category")]
        public ApiResult<List<CategoryVO>> category()
        {
            return ApiResult<List<CategoryVO>>.ok(categoryService.GetCategories());
        }
        /// <summary>
        /// 商品分类数据+树形结构
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [BackAuthCheck]
        [Route("getCategory")]
        public ApiResult<List<CategoryVO>> getCategory()
        {
            return ApiResult<List<CategoryVO>>.ok(categoryService.GetCategoriesBackEnd());
        }

        [HttpPost]
        [BackAuthCheck]
        [Route("editCategory")]
        public ApiResult<int> EditCategory([FromBody] store_category category)
        {
            categoryService.EditCategory(category);
            return ApiResult<int>.ok();
        }


        [HttpPost]
        [BackAuthCheck]
        [Route("delCategory")]
        public ApiResult<int> DelCategory([FromBody]store_category category)
        {
            categoryService.DelCategoryByID(category.id);
            return ApiResult<int>.ok();
        }

        [HttpPost]
        [BackAuthCheck]
        [Route("addCategory")]
        public ApiResult<int> AddCategory([FromBody] store_category category)
        {
            categoryService.AddCategory(category);
            return ApiResult<int>.ok();
        }
    }
}
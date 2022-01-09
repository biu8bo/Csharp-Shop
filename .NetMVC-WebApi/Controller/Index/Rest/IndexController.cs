using Commons.BaseModels;
using Commons.Constant;
using Commons.Enum;
using Mapper;
using Microsoft.Practices.Unity;
using MVC卓越项目.Commons.Attribute;
using MVC卓越项目.Commons.Utils;
using MVC卓越项目.Models;
using Service.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MVC卓越项目.Controller.Auth
{
    /// <summary>
    /// 首页模块
    /// </summary>
    [RoutePrefix(prefix:"api/index")]
    public class IndexController : ApiController
    {
      private readonly IIndexService indexService = Bootstrapper.Resolve<IIndexService>();
      private  readonly IProductService productService = Bootstrapper.Resolve<IProductService>();



        /// <summary>
        /// banner 轮播图
        /// </summary>
        /// <returns></returns>
        [Route("banner")]
        [HttpGet]
        [CacheEnable]
        public ApiResult<List<system_group_data>> index()
        {
         return   ApiResult<List<system_group_data>>.ok(indexService.GetDataByShopConstants(ShopConstants.YSHOP_HOME_BANNER));
        }
        /// <summary>
        /// 首页菜单
        /// </summary>
        /// <returns></returns>
        [Route("menu")]
        [HttpGet]
        [CacheEnable]
        public ApiResult<List<system_group_data>> menu()
        {
            return ApiResult<List<system_group_data>>.ok(indexService.GetDataByShopConstants(ShopConstants.YSHOP_HOME_MENUS));
        }
        /// <summary>
        /// 精品推荐
        /// </summary>
        /// <returns></returns>
        [CacheEnable]
        [Route("bastList")]
        [HttpGet]
        public ApiResult<List<system_group_data>> bastList()
        {
            return ApiResult<List<system_group_data>>.ok(indexService.GetList(1,10,ProductEnum.TYPE_1));
        }

        /// <summary>
        /// 猜你喜欢
        /// </summary>
        /// <returns></returns>
        [CacheEnable]
        [Route("guessLike")]
        [HttpGet]
        public ApiResult<List<system_group_data>> guessLike()
        {
            return ApiResult<List<system_group_data>>.ok(indexService.GetList(1, 10, ProductEnum.TYPE_4));
        }

        
        
    }
}

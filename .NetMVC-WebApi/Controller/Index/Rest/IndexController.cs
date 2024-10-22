﻿using Commons.BaseModels;
using Commons.Constant;
using Commons.Enum;
using Mapper;
using Microsoft.Practices.Unity;
using MVC卓越项目.Commons.Attribute;
using MVC卓越项目.Commons.Utils;
using MVC卓越项目.Models;
using Newtonsoft.Json;
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
        public ApiResult<List<object>> banner()
        {
            List<object> list =new List<object>();
            List<system_group_data> system_Group_Data = indexService.GetDataByShopConstants(ShopConstants.YSHOP_HOME_BANNER);
            system_Group_Data.ForEach(e => list.Add(JsonConvert.DeserializeObject(e.value)));
         return   ApiResult<List<object>>.ok(list);
        }
        /// <summary>
        /// 首页菜单
        /// </summary>
        /// <returns></returns>
        [Route("menu")]
        [HttpGet]
        [CacheEnable]
        public ApiResult<List<object>> menu()
        {
            List<object> list = new List<object>();
            List<system_group_data> system_Group_Data = indexService.GetDataByShopConstants(ShopConstants.YSHOP_HOME_MENUS);
            system_Group_Data.ForEach(e => list.Add(JsonConvert.DeserializeObject(e.value)));
            return ApiResult<List<object>>.ok(list);
        }

        /// <summary>
        /// 首页菜单
        /// </summary>
        /// <returns></returns>
        [Route("newProduct")]
        [HttpGet]
        [CacheEnable]
        public ApiResult<PageModel> NewProduct()
        {
            return ApiResult<PageModel>.ok(indexService.GetList(1, 8,ProductEnum.TYPE_3));
        }
        /// <summary>
        /// 首页通知
        /// </summary>
        /// <returns></returns>
        [Route("getNotify")]
        [HttpGet]
        [CacheEnable]
        public ApiResult<Object> GetNotify()
        {
            return ApiResult<Object>.ok(indexService.GetDataByShopConstants(ShopConstants.NOTIFY).FirstOrDefault());
        }

        /// <summary>
        /// 精品推荐
        /// </summary>
        /// <returns></returns>
        [CacheEnable]
        [Route("bastList")]
        [HttpGet]
        public ApiResult<PageModel> bastList()
        {
            PageModel pg = indexService.GetList(1, 1000, ProductEnum.TYPE_1);
            pg.Data = ((List<store_product>)pg.Data).OrderBy(e => Guid.NewGuid()).Skip(1).Take(6).OrderBy(e => Guid.NewGuid()).ToList();
            return ApiResult<PageModel>.ok(pg);
        }

        /// <summary>
        /// 热门推荐
        /// </summary>
        /// <returns></returns>
        [CacheEnable]
        [Route("hotList")]
        [HttpGet]
        public ApiResult<PageModel> hotList()
        {
            PageModel pg = indexService.GetList(1, 1000, ProductEnum.TYPE_2);
            pg.Data = ((List<store_product>)pg.Data).OrderBy(e => Guid.NewGuid()).Skip(1).Take(6).OrderBy(e => Guid.NewGuid()).ToList();
            return ApiResult<PageModel>.ok(pg);
        }
        /// <summary>
        /// 猜你喜欢
        /// </summary>
        /// <returns></returns>
        [CacheEnable]
        [Route("guessLike")]
        [HttpGet]
        public ApiResult<PageModel> guessLike()
        {
            PageModel pg = indexService.GetList(1, 1000, ProductEnum.TYPE_4);
            pg.Data = ((List<store_product>)pg.Data).OrderBy(e => Guid.NewGuid()).Skip(1).Take(8).OrderBy(e => Guid.NewGuid()).ToList();
            return ApiResult<PageModel>.ok(pg);
  
        }
        /// <summary>
        /// 热门搜索
        /// </summary>
        /// <returns></returns>
        [CacheEnable]
        [Route("hotSearch")]
        [HttpGet]
        public ApiResult<List<object>> hotSearch()
        {
            List<object> list = new List<object>();
            List<system_group_data> system_Group_Data = indexService.GetDataByShopConstants(ShopConstants.YSHOP_HOT_SEARCH);
            system_Group_Data.ForEach(e => list.Add(JsonConvert.DeserializeObject(e.value)));
            return ApiResult<List<object>>.ok(list);
        }



    }
}

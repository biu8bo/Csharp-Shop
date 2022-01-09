using Commons.BaseModels;
using Commons.Constant;
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

namespace MVC卓越项目.Areas
{
    /// <summary>
    /// 首页模块
    /// </summary>
    [RoutePrefix(prefix:"index")]
    public class IndexController : ApiController
    {
      private  readonly IIndexService indexService = Bootstrapper.Resolve<IIndexService>();
      private  readonly Log4NetHelper logger = Log4NetHelper.Default;

        /// <summary>
        /// banner 轮播图
        /// </summary>
        /// <returns></returns>
        [Route("banner")]
        [HttpGet]
        public ApiResult<List<system_group_data>> index()
        {
         return   ApiResult<List<system_group_data>>.ok(indexService.GetDataByShopConstants(ShopConstants.YSHOP_HOME_BANNER));
        }

        [Route("bastList")]
        [HttpGet]
        public ApiResult<List<system_group_data>> bastList()
        {
            return ApiResult<List<system_group_data>>.ok(indexService.GetDataByShopConstants(ShopConstants.YSHOP_HOME_BANNER));
        }
    }
}

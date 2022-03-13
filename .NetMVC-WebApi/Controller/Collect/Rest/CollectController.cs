using Commons.BaseModels;
using MVC卓越项目.Commons.Attribute;
using MVC卓越项目.Commons.Utils;
using Service.CollectService.Param;
using Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace MVC卓越项目.Controller.Collect.Rest
{
    /// <summary>
    /// 足迹与收藏模块
    /// </summary>
    [RoutePrefix("api")]
    public class CollectController : ApiController
    {
        private readonly ICollectService collectService = Bootstrapper.Resolve<ICollectService>();
        /// <summary>
        /// 添加收藏
        /// </summary>
        /// <param name="collectParam"></param>
        /// <returns></returns>
        [Route("addCollect")]
        [AuthCheck]
        public ApiResult<int> addCollect([FromBody]CollectParam collectParam)
        {
            collectService.addRroductRelation(collectParam.pid, LocalUser.getUidByUser(), collectParam.type);
            return ApiResult<int>.ok();
        }

        /// <summary>
        /// 删除收藏
        /// </summary>
        /// <param name="collectParam"></param>
        /// <returns></returns>
        [Route("delCollect")]
        [AuthCheck]
        public ApiResult<int> delCollect([FromBody] CollectParam collectParam)
        {
            collectService.delRroductRelation(collectParam.pid, LocalUser.getUidByUser(), collectParam.type);
            return ApiResult<int>.ok();
        }
        [HttpPost]
        [Route("getCollect")]
        [AuthCheck]
        public ApiResult<PageModel> getCollect([FromBody] CollectParam collectParam)
        {
            return ApiResult<PageModel>.ok(collectService.getCollectsByType(collectParam, LocalUser.getUidByUser()));
        }
        /// <summary>
        /// 批量删除收藏/足迹
        /// </summary>
        /// <param name="collectParam"></param>
        /// <returns></returns>
         [HttpPost]
        [Route("delCollectBatch")]
        [AuthCheck]
        public ApiResult<int> delCollectWithBatch([FromBody] CollectBatchParam collectParam)
        {
            collectService.delRroductRelationWithBatch(collectParam.pid, LocalUser.getUidByUser(), collectParam.type);
            return ApiResult<int>.ok();
        }

    }
}
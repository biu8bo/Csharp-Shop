using Commons.BaseModels;
using Commons.Utils;
using Mapper;
using MVC卓越项目.Commons.Attribute;
using MVC卓越项目.Commons.Utils;
using Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace MVC卓越项目.Controller.Comment.Rest
{
    /// <summary>
    /// 商品评论模块
    /// </summary>
    [RoutePrefix("api")]
    public class CommentController:ApiController
    {
        private readonly IProductReplyService productReplyService = Bootstrapper.Resolve<IProductReplyService>();

        /// <summary>
        /// 获取商品评论数据
        /// </summary>
        [Route("reply")]
        [HttpGet]
        public ApiResult<PageModel> getReplyByPid(long pid, int page, int limit)
        {
            return ApiResult<PageModel>.ok(productReplyService.GetReplyByPid(pid, page, limit));
        }

        /// <summary>
        /// 商品评论提交
        /// </summary>
        /// <param name="stores"></param>
        /// <returns></returns>
        [AuthCheck]
        [Route("comment")]
        [HttpPost]
        public ApiResult<int> AddComment([FromBody]dynamic stores)
        {
            //序列化类型强转
           List <store_product_reply> data = ObjectUtils<List<store_product_reply>>.ConvertTo(stores);
            productReplyService.addComment(LocalUser.getUidByUser(), data);
            return ApiResult<int>.ok();
        }
     }
}
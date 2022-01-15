using Commons.BaseModels;
using Service.Service;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Web.Http;


namespace MVC卓越项目.Controller.Product.Rest
{
    [RoutePrefix("api")]
    public class ProductReplyController : ApiController
    {
        private readonly IProductReply iProductReply = Bootstrapper.Resolve<IProductReply>();
        //获取商品评论数据
        [Route("reply")]
        [HttpGet]
        public PageModel getReplyByPid(long pid,int page,int limit)
        {
            return iProductReply.GetReplyByPid(pid,page,limit);
        }
    }
}
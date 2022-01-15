using Commons.BaseModels;
using Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    /// <summary>
    /// 评论模块
    /// </summary>
  public  interface IProductReply
    {
        //获取商品全部评论
        PageModel GetReplyByPid(long pid, int page, int limit);
    }
}

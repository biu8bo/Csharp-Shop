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
    /// 商品评论模块
    /// </summary>
  public  interface IProductReplyService
    {
        /// <summary>
        /// 获取商品全部评论
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        PageModel GetReplyByPid(long pid, int page, int limit);



        /// <summary>
        /// 添加商品评论
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="store_Product_Reply"></param>
        void addComment(long uid, List<store_product_reply> store_Product_Reply);
        /// <summary>
        /// 后台获取所有用户评论信息
        /// </summary>
        /// <param name="queryData"></param>
        /// <returns></returns>
        PageModel GetProdctReplyData(QueryData queryData);
        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="id"></param>
        void RemoveReply(int id);
    }
}

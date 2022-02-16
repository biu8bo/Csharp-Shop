using Commons.BaseModels;
using Mapper;
using MVC卓越项目.Commons.Utils;
using Service.ProductService.VO;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class ProductReplyServiceImpl : IProductReplyService
    {
        /// <summary>
        /// 获取商品评论信息
        /// </summary>
        /// <param name="pid">productID</param>
        /// <param name="page">页数</param>
        /// <param name="limit">页面大小</param>
        /// <returns></returns>
        public PageModel GetReplyByPid(long pid, int page, int limit)
        {
            using (var db = new eshoppingEntities())
            {
                return new PageUtils<Object>(page, limit).StartPage(db.store_product_reply.Where(e => e.product_id == pid && e.is_del == false).
                    Join(
                    db.eshop_user,  //外部对象
                    reply => reply.uid,         //内部的key
                user => user.uid,         //外部的key
                (reply, user) => new        //结果
                {
                    id = reply.id,
                    comment = reply.comment,
                    username = user.username,
                    nickname = user.nickname,
                    createTime  = reply.create_time,
                    isReply = reply.is_reply,
                    productScore = reply.product_score,
                    serviceScore = reply.service_score,
                    avatar = user.avatar,
                    pics = reply.pics
                }).OrderBy(e=>e.id));
            }
        }
    }
}

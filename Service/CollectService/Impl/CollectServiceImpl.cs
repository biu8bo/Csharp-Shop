using Commons.Utils;
using Mapper;
using MVC卓越项目.Commons.ExceptionHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class CollectServiceImpl : ICollectService
    {
        /// <summary>
        /// 添加足迹或收藏
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="uid"></param>
        /// <param name="type"></param>
        public void addRroductRelation(long pid, long uid, string type)
        {
            //获取数据库有无记录
            store_product_relation productRelation = this.isProductRelation(pid, uid, type);
            //如果是收藏并且有记录
            if (type.Equals("collect")&&!ObjectUtils<object>.isNotNull(productRelation))
            {
                //取消收藏
                this.delRroductRelation(pid,uid,type);
                return;
            }
            using (var db = new eshoppingEntities())
            {
                //如果是足迹并且有记录
                if (type.Equals("foot")&& ObjectUtils<object>.isNotNull(productRelation))
                {
                    //更新时间
                    
                    productRelation = db.store_product_relation.Where(e => e.product_id == pid && e.uid == uid && e.type == type && e.is_del == false).FirstOrDefault();
                    productRelation.update_time = DateTime.Now;
                }
                else
                {
                    db.store_product_relation.Add(new store_product_relation()
                    {
                        product_id = pid,
                        uid = uid,
                        type = type,
                        create_time =  DateTime.Now,
                        update_time =  DateTime.Now,
                        is_del = false
                    });
                }
                db.SaveChanges();
            }
        }
        /// <summary>
        /// 删除足迹或收藏
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="uid"></param>
        /// <param name="type"></param>
        public void delRroductRelation(long pid, long uid, string type)
        {
            using (eshoppingEntities db = new eshoppingEntities())
            {
                store_product_relation relation  =  new store_product_relation()
                {
                    product_id = pid,
                    uid = uid,
                    type = type
                };
                db.store_product_relation.Attach(relation) ;
                db.store_product_relation.Remove(relation);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// 批量删除足迹收藏
        /// </summary>
        /// <param name="pids"></param>
        /// <param name="uid"></param>
        /// <param name="type"></param>
        public void delRroductRelationWithBatch(List<long> pids, long uid, string type)
        {
          pids.ForEach(e => this.delRroductRelation(e, uid, type));
        }

        //查询收藏和足迹存在
        public store_product_relation isProductRelation(long pid, long uid, string type)
        {
            using (var db = new eshoppingEntities())
            {
             return   db.store_product_relation.Where(e=>e.product_id == pid&&e.uid == uid && e.type == type&&e.is_del == false).FirstOrDefault();
            }
        }
    }
}

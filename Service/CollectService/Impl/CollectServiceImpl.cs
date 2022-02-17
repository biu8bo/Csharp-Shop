using Commons.Utils;
using Mapper;
using MVC卓越项目.Commons.ExceptionHandler;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
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
            if (type.Equals("collect")&&ObjectUtils<object>.isNotNull(productRelation))
            {
                //取消收藏
                this.delRroductRelation(pid,uid,type);
                return;
            }
            using (var db = new eshoppingEntities())
            {
                var tran = db.Database.BeginTransaction();  //开启事务
                try
                {
                    //如果是足迹并且有记录
                    if (type.Equals("foot") && ObjectUtils<object>.isNotNull(productRelation))
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
                            create_time = DateTime.Now,
                            update_time = DateTime.Now,
                            is_del = false
                        });
                    }
                    db.SaveChanges();
                    tran.Commit();  //必须调用Commit()，不然数据不会保存
                }
                catch (Exception)
                {

                    tran.Rollback();
                }
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
                var tran = db.Database.BeginTransaction();
                var parameters = new DbParameter[] {
                                      new MySqlParameter { ParameterName = "pid", Value = pid},
                                      new MySqlParameter { ParameterName = "uid", Value = uid},
                                      new MySqlParameter { ParameterName = "type", Value = type},
                                  };
                db.Database.ExecuteSqlCommand($"Delete FROM store_product_relation WHERE product_id = @pid and uid = @uid and type = @type", parameters);
                tran.Commit();
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

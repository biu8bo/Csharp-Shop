using Commons.BaseModels;
using Commons.Utils;
using Mapper;
using MVC卓越项目.Commons.ExceptionHandler;
using MVC卓越项目.Commons.Utils;
using MySql.Data.MySqlClient;
using Service.CollectService.Param;
using Service.CollectService.VO;
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
                        //没有记录就添加记录
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
                    //出错回滚
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
                db.Database.ExecuteSqlCommand($"UPDATE `eshopping`.`store_product_relation` SET `is_del` = 1 WHERE product_id = @pid and uid = @uid and type = @type", parameters);
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

        //分页查询 收藏/足迹 type == collect/foot
        public PageModel getCollectsByType(CollectParam collectParam,long uid)
        {
            using (var db = new eshoppingEntities())
            {
              return  new PageUtils<Object>(collectParam.Page, collectParam.Limit).StartPage(db.store_product_relation.Where(e => e.is_del == false && e.uid == uid && e.type.Equals(collectParam.type)).Join(db.store_product, relation => relation
                   .product_id, product => product.id, (relation, product) => new
                   {
                       id = relation.id,
                       pid = relation.product_id,
                       image = product.image,
                       storeName = product.store_name,
                       storeInfo = product.store_info,
                       price = product.price,
                       time = relation.update_time
                   }).OrderByDescending(e=>e.time));
            }
        }

        public PageModel GetCollects(CollectParam collectParam)
        {
            using (var db = new eshoppingEntities())
            {
                return new PageUtils<Object>(collectParam.Page, collectParam.Limit).StartPage(db.store_product_relation.Where(e => e.type == collectParam.type).Join(db.store_product,e=>e.product_id,e=>e.id,(c,p)=> new { 
                product = p,
                collect = c
                
                }).Join(db.eshop_user,e=>e.collect.uid,e=>e.uid,(data,user)=>new {
                data = data,
                user =user
                
                }).OrderBy(e => e.data.collect.update_time));
            }
        }
    }
}

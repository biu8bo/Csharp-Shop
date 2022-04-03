using Commons.BaseModels;
using Mapper;
using MVC卓越项目.Commons.Attribute;
using MVC卓越项目.Commons.Utils;
using MVC卓越项目.Controller.Index.VO;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace MVC卓越项目.Controller.Index.BackEnd
{
    /// <summary>
    /// 后台数据模块
    /// </summary>
    [RoutePrefix("api")]
    public class DataController : ApiController
    {
        /// <summary>
        /// 获取数据统计
        /// </summary>
        /// <returns></returns>
        [BackAuthCheck]
        [Route("data/count")]
        [HttpGet]
        public ApiResult<Hashtable> getCount()
        {
            using (var db = new eshoppingEntities())
            {
                long userCount = db.eshop_user.Where(e => e.is_del == false).Count();
                long goodsCount = db.store_product.Where(e => e.is_del == false).Count();
                long orderCount = db.store_order.Where(e => e.is_del == false).Count();
                decimal priceCount = db.store_order.Where(e => e.is_del == false).Sum(e => e.pay_price);
                long todayCount = db.Database.SqlQuery<int>("SELECT Count(*) FROM `store_order` where DATE_FORMAT(create_time, '%Y%m%d' )   = DATE_FORMAT(NOW(), '%Y%m%d' )").FirstOrDefault();
                long proCount = db.Database.SqlQuery<int>("SELECT Count(*) FROM `store_order` where DATE_FORMAT(create_time, '%Y%m%d' )   = DATE_FORMAT(NOW()- INTERVAL 1 DAY, '%Y%m%d' )").FirstOrDefault();

                long lastWeekCount = db.Database.SqlQuery<long>("SELECT Count(*) FROM store_order WHERE is_del = 0 and date_sub(curdate(), interval 7 day) <= date(CREATE_TIME)").FirstOrDefault();
                long monthCount = db.Database.SqlQuery<long>("SELECT Count(*) FROM store_order WHERE is_del = 0 and DATE_FORMAT(CREATE_TIME, '%Y%m' ) = DATE_FORMAT( CURDATE( ) ,'%Y%m' )").FirstOrDefault();
                Hashtable hashtable = new Hashtable() {
                    {"userCount",userCount },
                    {"goodsCount",goodsCount },
                    {"orderCount",orderCount },
                    {"priceCount",priceCount },
                    {"todayCount",todayCount },
                    {"proCount",proCount },
                    {"lastWeekCount",lastWeekCount },
                    {"monthCount",monthCount },

                };
                return ApiResult<Hashtable>.ok(hashtable);
            }
        }
        /// <summary>
        ///获取系统配置
        /// </summary>
        /// <returns></returns>
        [BackAuthCheck]
        [Route("yxSystemGroupData")]
        [HttpGet]
        public ApiResult<PageModel> GetYxSystemGroupData([FromUri]string groupName, [FromUri] int page, [FromUri] int size)
        {
            using (var db = new eshoppingEntities())
            {
                PageModel pageModel = new PageUtils<system_group_data>(page,size).StartPage(db.system_group_data.Where(e => e.group_name == groupName && e.is_del == false).OrderByDescending(e=>e.sort));
                return ApiResult<PageModel>.ok(pageModel);
            }
        }

        /// <summary>
        ///修改系统配置
        /// </summary>
        /// <returns></returns>
        [BackAuthCheck]
        [Route("yxSystemGroupData")]
        [HttpPut]
        public ApiResult<int> UpdateYxSystemGroupData([FromBody] system_group_data system_Group_Data)
        {
            using (var db = new eshoppingEntities())
            {
                db.Entry(system_Group_Data).State = EntityState.Modified;
                db.SaveChanges();
                return ApiResult<int>.ok();
            }
        }
        /// <summary>
        ///删除系统配置
        /// </summary>
        /// <returns></returns>
        [BackAuthCheck]
        [Route("yxSystemGroupData/{id}")]
        [HttpDelete]
        public ApiResult<int> DelYxSystemGroupData(long id)
        {
            using (var db = new eshoppingEntities())
            {
                var result =  db.system_group_data.Find(id);
                result.is_del = true;
                db.SaveChanges();
                return ApiResult<int>.ok();
            }
        }

        /// <summary>
        /// 获取销量最高的5个商品
        /// </summary>
        /// <returns></returns>
        [BackAuthCheck]
        [HttpGet]
        [Route("data/orderCount")]
        public ApiResult<Hashtable> getOrderCount()
        {
            using (var db = new eshoppingEntities())
            {
                List<OrderCountDataVO> orderCount = db.Database.SqlQuery<OrderCountDataVO>("SELECT B.store_name `name`, B.id, count(B.id) as `value` FROM store_cart AS A INNER JOIN store_product AS B  ON  A.product_id = B.id  GROUP BY  A.product_id order by `value` DESC limit 5").ToList();
                LinkedList<string> columns = new LinkedList<string>();
                orderCount.ForEach((e) =>
                {
                    columns.AddLast(e.name);
                });
                Hashtable hashtable = new Hashtable() {
                    {  "orderCountDatas",orderCount},
                    { "column",columns}
                };
                return ApiResult<Hashtable>.ok(hashtable);
            }
        }
        /// <summary>
        /// 获取echars曲线图表数据
        /// </summary>
        /// <returns></returns>
        [BackAuthCheck]
        [HttpGet]
        [Route("data/chart")]
        public ApiResult<Hashtable> chartData()
        {
            using (var db = new eshoppingEntities())
            {
                LinkedList<ChartVO> orderCountDataVOs = new LinkedList<ChartVO>();
                //先获取当前时间
                DateTime now = DateTime.Now;
                //第一号开始遍历到今天
                //本月订单的总量
                for (int i = 1; i < now.Day; i++)
                {
                    string Day = new DateTime(now.Year, now.Month, i).Date.ToString("yyyy-MM-dd");
                    MySqlParameter mySqlParameter = new MySqlParameter();
                    mySqlParameter.ParameterName = "time";
                    mySqlParameter.Value = Day;
                    ChartVO result = db.Database.SqlQuery<ChartVO>($"SELECT count(*) num,DATE_FORMAT( create_time, '%m-%d' ) time  FROM `store_order` WHERE store_order.paid = 1 and DATE_FORMAT(create_time, '%Y-%m-%d') = DATE_FORMAT(@time, '%Y-%m-%d') GROUP BY  DATE_FORMAT(create_time, '%Y-%m-%d')", mySqlParameter).FirstOrDefault();
                    if (result is null)
                    {
                        result = new ChartVO();
                    }
                    result.time = Day;
                    orderCountDataVOs.AddLast(result);

                }
                LinkedList<ChartVO> orderCountDataVOs2 = new LinkedList<ChartVO>();
                //本月订单总额
                for (int i = 1; i < now.Day; i++)
                {
                    string Day = new DateTime(now.Year, now.Month, i).Date.ToString("yyyy-MM-dd");
                    MySqlParameter mySqlParameter = new MySqlParameter();
                    mySqlParameter.ParameterName = "time";
                    mySqlParameter.Value = Day;
                    ChartVO result = db.Database.SqlQuery<ChartVO>($"SELECT sum(pay_price) num,DATE_FORMAT( create_time, '%m-%d' ) time  FROM `store_order` WHERE DATE_FORMAT(create_time, '%Y-%m-%d') = DATE_FORMAT(@time, '%Y-%m-%d') and store_order.paid  = 1  GROUP BY  DATE_FORMAT(create_time, '%Y-%m-%d')", mySqlParameter).FirstOrDefault();
                    if (result is null)
                    {
                        result = new ChartVO();
                    }
                    result.time = Day;
                    orderCountDataVOs2.AddLast(result);

                }
                Hashtable hashtable = new Hashtable() {
                    {  "chartT",orderCountDataVOs},
                    { "chart",orderCountDataVOs2}
                };
                return ApiResult<Hashtable>.ok(hashtable);
            }
        }
    }
}
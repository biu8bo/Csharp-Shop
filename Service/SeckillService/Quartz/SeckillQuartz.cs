using Commons.Quartz;
using Mapper;
using MVC卓越项目.Commons.Utils;
using Newtonsoft.Json;
using Quartz;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.SeckillService.Quartz
{
    /// <summary>
    /// 秒杀任务调度
    /// </summary>
    public class SeckillQuartz : JobBase
    {
        /// <summary>
        /// 每隔5秒进行一次全表扫描
        /// </summary>
        public override string Cron => "0/5 * * * * ?";
        /// <summary>
        /// 初始化秒杀方案Redis缓存
        /// </summary>
        /// <param name="context"></param>
        protected override void ExcuteJob(IJobExecutionContext context)
        {
            using (var db = new eshoppingEntities())
            {
                //秒杀开启
               List<store_seckill> seckills =  db.store_seckill.Where(e => e.start_time <= DateTime.Now && e.stop_time >= DateTime.Now&&e.is_del==false&&e.is_show==true).ToList();
                seckills.ForEach(item => {
                    //查询时间段模板
                    system_group_data group_Data = db.system_group_data.Find(item.time_id);
                    Dictionary<string, string> Time = JsonConvert.DeserializeObject<Dictionary<string,string>>(group_Data.value);
                //秒杀时间段
                int StartHour = Convert.ToInt32(Time["time"]);
                int EndHour = Convert.ToInt32(Time["time"]) + Convert.ToInt32(Time["continued"]);
                    //获取现在时间点
                    int NowTime = DateTime.Now.Hour;

                    //在开启的时间段之间
                    if (StartHour <= NowTime&& NowTime<EndHour)
                    {
                            //开启秒杀
                            item.status = true;
                            //删除已存在的
                            RedisHelper.db.KeyDelete($"seckill:count:{item.id}");
                            //初始化缓存 开始新一轮
                            for (int i = 0; i < item.stock; i++)
                            {
                                RedisHelper.db.ListLeftPush($"seckill:count:{item.id}", item.product_id.ToString());
                            }
                          
                        
                    }
                    //不在区间就关闭秒杀
                    else
                    {
                        if (item.status)
                        {
                            item.status = false;
                            //删除
                            RedisHelper.db.KeyDelete($"seckill:count:{item.id}");
                        }
                    }
                    db.SaveChanges();
                });
                //秒杀过期关闭
                List<store_seckill> seckillsClose =db.store_seckill.Where(e => e.stop_time <= DateTime.Now && e.status == true&&e.is_del==false).ToList();
                seckillsClose.ForEach(item => {
                    //关闭秒杀
                    item.status = false;
                    //隐藏
                    item.is_show = false;
                    //删除已存在的
                    RedisHelper.db.KeyDelete($"seckill:count:{item.id}");
                    db.SaveChanges();
                });
            }
        }
    }
}

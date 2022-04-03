using Commons.BaseModels;
using Commons.Constant;
using Mapper;
using MVC卓越项目;
using MVC卓越项目.Commons.ExceptionHandler;
using MVC卓越项目.Commons.Utils;
using Newtonsoft.Json;
using Service.ProductService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class SeckillServiceImpl : ISeckillService
    {
        private readonly IProductService productService = Bootstrapper.Resolve<IProductService>();
        public void AddSeckillProduct(store_seckill store_Seckill)
        {
            using (var db = new eshoppingEntities())
            {
                store_Seckill.create_time = DateTime.Now;
                store_Seckill.update_time = DateTime.Now;
                store_Seckill.is_show = true;
                store_Seckill.is_del = false;
                store_Seckill.status = false;
                db.Entry(store_Seckill).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();
            }
        }

        public PageModel getAllStoreSeckill(QueryData queryData)
        {
            using (var db = new eshoppingEntities())
            {
                if (!(queryData.value is null))
                {
  
                    return new PageUtils<Object>(queryData.Page, queryData.Limit).StartPage(db.store_seckill.Where(e => e.is_del == false).Join(db.store_product, e => e.product_id, e => e.id, (seckill, product) => new { seckill = seckill, product = product }).Where(e => e.product.store_name.Contains(queryData.value)).Join(db.system_group_data,e=>e.seckill.time_id,e=>e.id,(info,time)=>new {info=info,time=time }).OrderByDescending(e => e.info.seckill.update_time));
                }
                else
                {
        
                 return   new PageUtils<Object>(queryData.Page, queryData.Limit).StartPage(db.store_seckill.Where(e => e.is_del == false).Join(db.store_product, e => e.product_id, e => e.id, (seckill, product) => new { seckill = seckill, product = product }).Join(db.system_group_data, e => e.seckill.time_id, e => e.id, (info, time) => new { info = info, time = time }).OrderByDescending(e => e.info.seckill.update_time));
                }
               
            }
        }

        public List<system_group_data> getSeckillTime()
        {
            using (var db = new eshoppingEntities())
            {
                IEnumerable<system_group_data> data = db.system_group_data
                    .Where(e => e.group_name == ShopConstants.YSHOP_SECKILL_TIME && e.is_del == false);
                foreach (var item in data)
                {
                    item.map = JsonConvert.DeserializeObject<system_group_data.Map>(item.value);
                }
                return data.OrderBy(e=>e.map.time).ToList();
            }
        }

        public Object GetStore_Seckills(int id)
        {
            using (var db = new eshoppingEntities())
            {
              return db.store_seckill.Where(e => e.is_del == false && e.is_show == true&&e.status==true).Join(db.store_product, e => e.product_id, e => e.id, (seckill, product) => new { seckill = seckill, product = product }).Join(db.system_group_data, e => e.seckill.time_id, e => e.id, (info, time) => new { info = info, time = time }).Where(e=>e.time.id==id).ToList();
            }
        }

        public object GetStore_SeckillsByID(int id)
        {
            using (var db = new eshoppingEntities())
            {
              var data=  db.store_seckill.Where(e => e.is_del == false && e.is_show == true && e.id == id&&e.status==true).Join(db.system_group_data, e => e.time_id, e => e.id, (info, time) => new { info = info, time = time }).FirstOrDefault();
                if (data is null)
                {
                    throw new ApiException(500, "活动不存在！");
                }
                //获取商品信息
            ProductVO  productVO =  productService.getProductById((long)data.info.product_id,0);
                Dictionary<string, Object> dic = new Dictionary<string, object>();
                dic.Add("seckill",data);
                dic.Add("product", productVO);

                return dic;
            }

        }

        public void RemoveSeckillByID(int id)
        {
            //移除秒杀方案
            using (var db = new eshoppingEntities())
            {
                store_seckill seckill = db.store_seckill.Find(id);
                seckill.is_del = true;
                //移除缓存
                RedisHelper.db.KeyDelete($"seckill:count:{seckill.id}");
                db.SaveChanges();
            }
        }
    }
}

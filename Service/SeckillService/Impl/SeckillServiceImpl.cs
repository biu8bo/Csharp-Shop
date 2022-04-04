using Commons.BaseModels;
using Commons.Constant;
using Commons.Utils;
using Mapper;
using MVC卓越项目;
using MVC卓越项目.Commons.ExceptionHandler;
using MVC卓越项目.Commons.Utils;
using Newtonsoft.Json;
using Service.CartService.Param;
using Service.OrderService.Enum;
using Service.OrderService.Param;
using Service.OrderService.VO;
using Service.ProductService;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class SeckillServiceImpl : ISeckillService
    {
        #region IOC 容器注入 service层
        private readonly IProductService productService = Bootstrapper.Resolve<IProductService>();

        private readonly IOrderService orderService = Bootstrapper.Resolve<IOrderService>();

        private readonly ICartService cartService = Bootstrapper.Resolve<ICartService>();
        #endregion

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

                    return new PageUtils<Object>(queryData.Page, queryData.Limit).StartPage(db.store_seckill.Where(e => e.is_del == false).Join(db.store_product, e => e.product_id, e => e.id, (seckill, product) => new { seckill = seckill, product = product }).Where(e => e.product.store_name.Contains(queryData.value)).Join(db.system_group_data, e => e.seckill.time_id, e => e.id, (info, time) => new { info = info, time = time }).OrderByDescending(e => e.info.seckill.update_time));
                }
                else
                {

                    return new PageUtils<Object>(queryData.Page, queryData.Limit).StartPage(db.store_seckill.Where(e => e.is_del == false).Join(db.store_product, e => e.product_id, e => e.id, (seckill, product) => new { seckill = seckill, product = product }).Join(db.system_group_data, e => e.seckill.time_id, e => e.id, (info, time) => new { info = info, time = time }).OrderByDescending(e => e.info.seckill.update_time));
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
                return data.OrderBy(e => e.map.time).ToList();
            }
        }

        public Object GetStore_Seckills(int id)
        {
            using (var db = new eshoppingEntities())
            {
                return db.store_seckill.Where(e => e.is_del == false && e.is_show == true && e.status == true).Join(db.store_product, e => e.product_id, e => e.id, (seckill, product) => new { seckill = seckill, product = product }).Join(db.system_group_data, e => e.seckill.time_id, e => e.id, (info, time) => new { info = info, time = time }).Where(e => e.time.id == id).ToList();
            }
        }

        public object GetStore_SeckillsByID(int id)
        {
            using (var db = new eshoppingEntities())
            {
                var data = db.store_seckill.Where(e => e.is_del == false && e.is_show == true && e.id == id && e.status == true).Join(db.system_group_data, e => e.time_id, e => e.id, (info, time) => new { info = info, time = time }).FirstOrDefault();
                if (data is null)
                {
                    throw new ApiException(500, "活动不存在！");
                }
                //获取商品信息
                ProductVO productVO = productService.getProductById((long)data.info.product_id, 0);
                Dictionary<string, Object> dic = new Dictionary<string, object>();
                dic.Add("seckill", data);
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

        public OrderConfirmVO Seckill(CartParam cartParam, long uid)
        {
            //秒杀id
            int sid = cartParam.sid;
         
            //先检测用户有没有抢过
            bool hasUser = RedisHelper.db.SetContains("seckill:user:" + sid, uid);
            if (hasUser)
            {
                throw new ApiException(501, "您已经抢过了!");
            }

            //库存出栈
            RedisValue stork = RedisHelper.db.ListLeftPop("seckill:count:" + sid);
            //没库存
            if (!stork.HasValue)
            {
                throw new ApiException(501, "您所抢购的商品已被抢空！");
            }
            
            //将用户设为已抢过
            RedisHelper.db.SetAdd("seckill:user:" + sid, uid);

            //秒杀数据库处理
            using (var db = new eshoppingEntities())
            {
                
                //查询秒杀方案
                store_seckill seckillInfo = db.store_seckill.Find(sid);
                try
                {
                    var tran = db.Database.BeginTransaction();
                    //查询用户实例
                    eshop_user user = db.eshop_user.Find(uid);
                    //减去数据库库存
                    seckillInfo.stock--;
                    seckillInfo.sales++;
                    decimal money = user.now_money - seckillInfo.price.Value;
                    if (money < 0)
                    {
                        throw new ApiException(501, "抢购失败！原因：余额不足！");
                    }
                    //创建订单
                    //添加购物车返回购物车id
                    decimal cartID = cartService.addCart(cartParam, uid);
                    CartIDIDsParam cartIDIDsParam = new CartIDIDsParam();
                    cartIDIDsParam.cartIds = new List<int>();
                    cartIDIDsParam.cartIds.Add((int)cartID);
                    //创建订单后缓存进redis 过期时间为10分钟
                    OrderConfirmVO orderConfirmVO = orderService.confirmOrder(cartIDIDsParam, uid);
                    orderConfirmVO.seckillPrice = seckillInfo.price.Value;
                    db.SaveChanges();
                    return orderConfirmVO;
                }
                catch (Exception e)
                {
                    //回滚
                    RedisHelper.db.SetRemove("seckill:user:" + sid, uid);
                    RedisHelper.db.ListRightPush("seckill:count:" + sid, seckillInfo.product_id.ToString());
                    throw e;
                }
            }
     

        }

        public void SeckilPay(string orderKey, int sid,string mark, long uid)
        {
                
                using (var db = new eshoppingEntities())
                {
                 
                var tran = db.Database.BeginTransaction();
                //查询秒杀方案
                store_seckill seckill = db.store_seckill.Find(sid);
                if (seckill is null)
                {
                    throw new ApiException(501,"秒杀不存在！");
                }
                eshop_user user = db.Database.SqlQuery<eshop_user>($"SELECT eshop_user.* FROM eshop_user where uid = {uid} FOR UPDATE").FirstOrDefault();
                    OrderConfirmVO orderConfirmVO = RedisHelper.GetStringKey<OrderConfirmVO>("Order:" + orderKey);
                    if (orderConfirmVO is null)
                    {
                        throw new ApiException(500, "支付超时，订单已关闭!");
                    }
                    decimal totalPrice = Convert.ToDecimal(seckill.price.Value);
                    decimal userMoney = user.now_money - seckill.price.Value;
                    bool PlayState = true;
                    //余额是否充足
                    if (userMoney < 0)
                    {
                        PlayState = false;
                        //throw new ApiException(400, "余额不足,支付失败");
                    }
                    //购物车字段数据
                    string cartID = "";
                    int totalNum = 0;
                    //计算数据
                    orderConfirmVO.cartInfo.ForEach(e =>
                    {
                        cartID += Convert.ToInt32(e.id) + ",";
                        totalNum += e.cart_num;
                    });
                    //去除尾部分号
                    cartID = cartID.Substring(0, cartID.Length - 1);
                    //订单数据写入,创建订单
                    //雪花算法生成订单id
                    SnowFlakeUtil snowFlakeUtil = new SnowFlakeUtil(13);
                    if (orderConfirmVO.addressInfo is null)
                    {
                        throw new ApiException(501, "请选择或填写收货人信息");
                    }
                    store_order store_Order = new store_order()
                    {
                        order_id = Convert.ToString(snowFlakeUtil.nextId()),
                        uid = uid,
                        real_name = orderConfirmVO.addressInfo.real_name,
                        user_phone = orderConfirmVO.addressInfo.phone,
                        user_address = orderConfirmVO.addressInfo.province + " " + orderConfirmVO.addressInfo.city + " " + orderConfirmVO.addressInfo.district + " " + orderConfirmVO.addressInfo.detail,
                        cart_id = cartID,
                        total_num = totalNum,
                        total_price = totalPrice,
                        pay_price = totalPrice,
                        paid = PlayState,
                        pay_time = DateTime.Now,
                        pay_type = "yue",
                        create_time = DateTime.Now,
                        update_time = DateTime.Now,
                        status = OrderTypeEnum.NoPay,
                        mark = mark,
                        unique = orderKey,
                        verify_code = ""
                    };
                    db.store_order_status.Add(new store_order_status()
                    {
                        oid = store_Order.id,
                        change_message = OrderTypeEnum.CreateOrder,
                        change_type = "create_seckillOrder",
                        change_time = DateTime.Now
                    });
                    db.store_order.Add(store_Order);
                    RedisHelper.DeleteStringKey("Order:" + orderKey);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception)
                    {
                        throw new ApiException(500, "该订单已关闭!");

                    }
                    //成功支付的流程
                    //删除购物车数据
                    orderConfirmVO.cartInfo.ForEach(e =>
                    {
                        store_cart _Cart = db.store_cart.Where(x => x.id == e.id).FirstOrDefault();
                        var productResult = db.store_product.Find(e.product_id);
                        //销量增加
                        productResult.sales++;
                        //库存减少
                        productResult.stock--;
                        //规格库存减少
                        store_product_attr_value skuData = db.store_product_attr_value.Where(s => s.unique == _Cart.product_attr_unique).First();
                        skuData.stock--;
                        skuData.sales++;
                        db.SaveChanges();
                        string md5str = Md5Utils.Md5(JsonConvert.SerializeObject(e.productInfo));
                        store_order_cart_info add = new store_order_cart_info()
                        {
                            oid = store_Order.id,
                            cart_id = e.id,
                            product_id = e.product_id,
                            cart_info = JsonConvert.SerializeObject(new
                            {

                                productInfo = e.productInfo,
                                attrInfo = e.attrInfo
                            }),
                            unique = md5str,
                        };
                        db.store_order_cart_info.Add(add);
                        _Cart.is_del = true;
                        db.Entry(_Cart).State = System.Data.Entity.EntityState.Modified;
                    });
                    //余额不足
                    if (userMoney < 0)
                    {
                        db.SaveChanges();
                        tran.Commit();
                        throw new ApiException(501, "余额不足,支付失败");
                    }
                    //成功支付 加入记录
                    db.store_order_status.Add(new store_order_status()
                    {
                        oid = store_Order.id,
                        change_message = OrderTypeEnum.PaySuccess,
                        change_type = "pay_success",
                        change_time = DateTime.Now
                    });
                    //扣钱
                    user.now_money = userMoney;
                    user.pay_count++;
                    db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    db.eshop_user.Attach(user);
                    db.SaveChanges();



                    //写入数据库
                    tran.Commit();
                }

            
        }
    }
}

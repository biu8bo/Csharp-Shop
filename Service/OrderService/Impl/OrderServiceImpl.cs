using Commons.BaseModels;
using Commons.Utils;
using Mapper;
using MVC卓越项目;
using MVC卓越项目.Commons.ExceptionHandler;
using MVC卓越项目.Commons.Utils;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Service.OrderService.Enum;
using Service.OrderService.Param;
using Service.OrderService.VO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class OrderServiceImpl : IOrderService
    {
        private readonly TimeSpan timespan = TimeSpan.FromMilliseconds(1000 * 100 * 10);
        private readonly IAddressService addressService = Bootstrapper.Resolve<IAddressService>();
        public OrderConfirmVO confirmOrder(CartIDIDsParam cartIDsParam, long uid)
        {

            OrderConfirmVO orderConfirmVO = new OrderConfirmVO();
            using (var db = new eshoppingEntities())
            {
                var tran = db.Database.BeginTransaction();
                List<store_cart> storeCarts = new List<store_cart>();
                decimal price = 0;
                //购物车中的信息
                cartIDsParam.cartIds.ForEach(id =>
                {
                    store_cart cart = db.store_cart.Where(e => e.id == id && e.uid == uid).FirstOrDefault();
                    cart.productInfo = db.store_product.Where(e => e.id == cart.product_id).FirstOrDefault();
                    cart.attrInfo = db.store_product_attr_value.Where(e => e.unique == cart.product_attr_unique).FirstOrDefault();
                    //计算价格 
                    price += cart.attrInfo.price * cart.cart_num;
                    storeCarts.Add(cart);
                });

                orderConfirmVO.priceGroup = new Hashtable() { { "totalPrice", price } };
                orderConfirmVO.cartInfo = storeCarts;
                //获取地址信息
                orderConfirmVO.addressInfo = db.user_address.Where(e => e.is_default == true && e.uid == uid).FirstOrDefault();
                //userInfo
                orderConfirmVO.userInfo = db.eshop_user.Where(e => e.uid == uid).FirstOrDefault();
                orderConfirmVO.orderKey = cacheOrderInfo(orderConfirmVO, uid);
                tran.Commit();
                return orderConfirmVO;
            }
        }
        /// <summary>
        /// 将生成的订单信息存入缓存
        /// </summary>
        /// <param name="orderConfirmVO"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        string cacheOrderInfo(OrderConfirmVO orderConfirmVO, long uid)
        {
            string resultJson = JsonConvert.SerializeObject(orderConfirmVO);
            string md5 = Md5Utils.Md5(uid + resultJson);
            RedisHelper.SetStringKey("Order:" + md5, resultJson, timespan);
            return md5;
        }

        public PageModel getOrderInfoByType(OrderTypeParam orderTypeParam, long uid)
        {
            using (var db = new eshoppingEntities())
            {

                PageModel pageModel = null;
                IQueryable<store_order> query = db.Set<store_order>().Include("store_order_cart_info").Include("store_order_cart_info.store_product").Include("store_order_cart_info.store_cart").Where(e => e.uid == uid);
                //未支付状态
                if (orderTypeParam.orderType == OrderTypeEnum.NoPay)
                {

                    //查询订单详情，购物车信息
                    pageModel = new PageUtils<store_order>().StartPage(query.Where(e => e.paid == false).OrderBy(e => e.id));
                    foreach (var items in ((List<store_order>)pageModel.Data))
                    {
                        foreach (var item in items.store_order_cart_info)
                        {
                            item.store_cart.store_product_attr_value = db.store_product_attr_value.Where(e => e.unique == item.store_cart.product_attr_unique).FirstOrDefault();
                        }

                    }
                    return pageModel;
                }
                //未发货
                else if (orderTypeParam.orderType == OrderTypeEnum.WaitSend)
                {
                    //查询订单详情，购物车信息
                    pageModel = new PageUtils<store_order>().StartPage(query.Where(e => e.paid == true && e.status == 0).OrderBy(e => e.id));
                    foreach (var items in ((List<store_order>)pageModel.Data))
                    {
                        foreach (var item in items.store_order_cart_info)
                        {
                            item.store_cart.store_product_attr_value = db.store_product_attr_value.Where(e => e.unique == item.store_cart.product_attr_unique).FirstOrDefault();
                        }

                    }
                    return pageModel;
                }
                //已经收货
                else if (orderTypeParam.orderType == OrderTypeEnum.WaitGet)
                {
                    //查询订单详情，购物车信息
                    pageModel = new PageUtils<store_order>().StartPage(query.Where(e => e.paid == true && e.status == 1).OrderBy(e => e.id));
                    foreach (var items in ((List<store_order>)pageModel.Data))
                    {
                        foreach (var item in items.store_order_cart_info)
                        {
                            item.store_cart.store_product_attr_value = db.store_product_attr_value.Where(e => e.unique == item.store_cart.product_attr_unique).FirstOrDefault();
                        }

                    }
                    return pageModel;
                }
                else if (orderTypeParam.orderType == OrderTypeEnum.WaitReplay)
                {
                    //查询订单详情，购物车信息
                    pageModel = new PageUtils<store_order>().StartPage(query.Where(e => e.paid == true && e.status == 2).OrderBy(e => e.id));
                    foreach (var items in ((List<store_order>)pageModel.Data))
                    {
                        foreach (var item in items.store_order_cart_info)
                        {
                            item.store_cart.store_product_attr_value = db.store_product_attr_value.Where(e => e.unique == item.store_cart.product_attr_unique).FirstOrDefault();
                        }

                    }
                    return pageModel;
                }//已完成
                else if (orderTypeParam.orderType == OrderTypeEnum.Success)
                {
                    //查询订单详情，购物车信息
                    pageModel = new PageUtils<store_order>().StartPage(query.Where(e => e.paid == true && e.status == -1).OrderBy(e => e.id));
                    foreach (var items in ((List<store_order>)pageModel.Data))
                    {
                        foreach (var item in items.store_order_cart_info)
                        {
                            item.store_cart.store_product_attr_value = db.store_product_attr_value.Where(e => e.unique == item.store_cart.product_attr_unique).FirstOrDefault();
                        }

                    }
                    return pageModel;
                }
            }
            return null;
        }
        /// <summary>
        /// 修改订单地址信息
        /// </summary>
        /// <param name="addressId"></param>
        /// <param name="orderKey"></param>
        /// <returns></returns>
        public OrderConfirmVO updateOrderAddress(int addressId, string orderKey)
        {
            user_address addressInfo = addressService.getAddressData(addressId);
            OrderConfirmVO orderConfirmVO = RedisHelper.GetStringKey<OrderConfirmVO>("Order:" + orderKey);
            if (orderConfirmVO is null)
            {
                throw new ApiException(400, "订单状态超时，请刷新界面");
            }
            if (orderConfirmVO.userInfo.uid != LocalUser.getUidByUser())
            {
                throw new AuthException("登录已过期");
            }
            orderConfirmVO.addressInfo = addressInfo;
            RedisHelper.SetStringKey("Order:" + orderKey, orderConfirmVO, timespan);
            return orderConfirmVO;
        }

        public void payOrder(string orderKey, string mark, long uid)
        {
            using (var db = new eshoppingEntities())
            {
                var tran = db.Database.BeginTransaction();
                eshop_user user = db.Database.SqlQuery<eshop_user>($"SELECT eshop_user.* FROM eshop_user where uid = {uid} FOR UPDATE").FirstOrDefault();
                OrderConfirmVO orderConfirmVO = RedisHelper.GetStringKey<OrderConfirmVO>("Order:" + orderKey);
                decimal totalPrice = Convert.ToDecimal(orderConfirmVO.priceGroup["totalPrice"]);
                decimal userMoney = user.now_money - totalPrice;
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
                    cartID += e.id + ",";
                    totalNum += e.cart_num;
                });
                //去除尾部分号
                cartID = cartID.Substring(0, cartID.Length - 1);
                //订单数据写入,创建订单
                //雪花算法生成订单id
                SnowFlakeUtil snowFlakeUtil = new SnowFlakeUtil(13);
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
                    change_type = "create_order",
                    change_time = DateTime.Now
                });
                db.store_order.Add(store_Order);
                RedisHelper.DeleteStringKey("Order:" + orderKey);
                db.SaveChanges();
                if (userMoney < 0)
                {
                   

                    tran.Commit
                         ();
                    throw new ApiException(400, "余额不足,支付失败");
                }



                //成功支付的流程

                //删除购物车数据
                orderConfirmVO.cartInfo.ForEach(e =>
                {
                    store_cart _Cart = db.store_cart.Where(x => x.id == e.id).FirstOrDefault();
                    string md5str = Md5Utils.Md5(JsonConvert.SerializeObject(e.productInfo));
                    store_order_cart_info add = new store_order_cart_info()
                    {
                        oid = store_Order.id,
                        cart_id = e.id,
                        product_id = e.product_id,
                        cart_info = JsonConvert.SerializeObject(e.productInfo),
                        unique = md5str,
                    };
                    db.store_order_cart_info.Add(add);
                    _Cart.is_del = true;
                     db.Entry(_Cart).State = System.Data.Entity.EntityState.Modified;
                });
                //扣钱
                user.now_money = userMoney;
                db.eshop_user.Attach(user);
                db.SaveChanges();
             
       
             
                //写入数据库
                tran.Commit();
            }

        }
    }
}

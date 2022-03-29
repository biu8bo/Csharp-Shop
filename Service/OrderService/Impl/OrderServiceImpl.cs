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
using System.Data.Entity;
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
                    cart.productInfo = db.store_product.Where(e => e.id == cart.product_id&&e.stock>=1).FirstOrDefault();
                    if (cart.productInfo is null)
                    {
                        throw new ApiException(501, "商品库存不足！");
                    }
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
            string md5 = Md5Utils.Md5(uid + resultJson + DateTime.Now.Millisecond.ToString());
            RedisHelper.SetStringKey("Order:" + md5, resultJson, timespan);
            return md5;
        }

        public PageModel getOrderInfoByType(OrderTypeParam orderTypeParam, long uid)
        {
            using (var db = new eshoppingEntities())
            {

                PageModel pageModel = null;
                IQueryable<store_order> query = db.Set<store_order>().Include("store_order_cart_info").Include("store_order_cart_info.store_product").Include("store_order_cart_info.store_cart").Where(e => e.uid == uid && e.is_del == false);
                //未支付状态
                if (orderTypeParam.orderType == OrderTypeEnum.NoPay)
                {

                    //查询订单详情，购物车信息
                    pageModel = new PageUtils<store_order>(orderTypeParam.Page, orderTypeParam.Limit).StartPage(query.Where(e => e.paid == false).OrderBy(e => e.id));
                    foreach (var items in ((List<store_order>)pageModel.Data))
                    {
                        foreach (var item in items.store_order_cart_info)
                        {
                            item.store_cart.store_product_attr_value = db.store_product_attr_value.Where(e => e.unique == item.store_cart.product_attr_unique).FirstOrDefault();
                        }

                    }
                    return pageModel;
                }
                //待发货
                else if (orderTypeParam.orderType == OrderTypeEnum.WaitSend)
                {
                    //查询订单详情，购物车信息
                    pageModel = new PageUtils<store_order>(orderTypeParam.Page, orderTypeParam.Limit).StartPage(query.Where(e => e.paid == true && e.status == 0).OrderBy(e => e.id));
                    foreach (var items in ((List<store_order>)pageModel.Data))
                    {
                        foreach (var item in items.store_order_cart_info)
                        {
                            item.store_cart.store_product_attr_value = db.store_product_attr_value.Where(e => e.unique == item.store_cart.product_attr_unique).FirstOrDefault();
                        }

                    }
                    return pageModel;
                }
                //待收货
                else if (orderTypeParam.orderType == OrderTypeEnum.WaitGet)
                {
                    //查询订单详情，购物车信息
                    pageModel = new PageUtils<store_order>(orderTypeParam.Page, orderTypeParam.Limit).StartPage(query.Where(e => e.paid == true && e.status == 1).OrderBy(e => e.id));
                    foreach (var items in ((List<store_order>)pageModel.Data))
                    {
                        foreach (var item in items.store_order_cart_info)
                        {
                            item.store_cart.store_product_attr_value = db.store_product_attr_value.Where(e => e.unique == item.store_cart.product_attr_unique).FirstOrDefault();
                        }

                    }
                    return pageModel;
                }
                //待回复
                else if (orderTypeParam.orderType == OrderTypeEnum.WaitReplay)
                {
                    //查询订单详情，购物车信息
                    pageModel = new PageUtils<store_order>(orderTypeParam.Page, orderTypeParam.Limit).StartPage(query.Where(e => e.paid == true && e.status == 2).OrderBy(e => e.id));
                    foreach (var items in ((List<store_order>)pageModel.Data))
                    {
                        foreach (var item in items.store_order_cart_info)
                        {
                            item.store_cart.store_product_attr_value = db.store_product_attr_value.Where(e => e.unique == item.store_cart.product_attr_unique).FirstOrDefault();
                        }

                    }
                    return pageModel;
                }//已完成
                else
                {
                    //查询订单详情，购物车信息
                    pageModel = new PageUtils<store_order>(orderTypeParam.Page, orderTypeParam.Limit).StartPage(query.Where(e => e.paid == true && e.status == 3).OrderBy(e => e.id));
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
                if (orderConfirmVO is null)
                {
                    throw new ApiException(500, "订单已关闭!,请刷新当前页面重新提交");
                }
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
                    throw new ApiException(501,"请选择或填写收货人信息");
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
                    change_type = "create_order",
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
                    throw new ApiException(500, "该订单已关闭!,请刷新当前页面重新提交");

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
                    throw new ApiException(400, "余额不足,支付失败");
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
        /// <summary>
        /// 取消订单
        /// </summary>
        /// <param name="orderKey"></param>
        /// <param name="uid"></param>
        public void CancelOrder(string orderKey, long uid)
        {
            using (eshoppingEntities db = new eshoppingEntities())
            {
                store_order orderInfo = db.store_order.Where(e => e.order_id == orderKey && e.uid == uid && e.paid == false).FirstOrDefault();
                orderInfo.is_del = true;
                db.SaveChanges();
            }
        }

        public void HandlerPay(string orderKey, long uid)
        {
            using (var db = new eshoppingEntities())
            {
                var tran = db.Database.BeginTransaction();
                store_order orderInfo = db.store_order.Where(e => e.paid == false && e.uid == uid && e.order_id == orderKey).FirstOrDefault();
                //行锁获取用户数据
                eshop_user user = db.Database.SqlQuery<eshop_user>($"SELECT eshop_user.* FROM eshop_user where uid = {uid} FOR UPDATE").FirstOrDefault();
                //查询余额
                decimal money = user.now_money - orderInfo.pay_price;
                if (money < 0)
                {
                    throw new ApiException(400, "余额不足,请及时充值!");
                }
                orderInfo.paid = true;
                orderInfo.status = 0;
                user.now_money = money;
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                //成功支付 加入记录
                db.store_order_status.Add(new store_order_status()
                {
                    oid = orderInfo.id,
                    change_message = OrderTypeEnum.PaySuccess,
                    change_type = "pay_success",
                    change_time = DateTime.Now
                });
                user.pay_count++;
                db.SaveChanges();
                //库存与销量减少
                var productsResult = db.store_order_cart_info.Where(e => e.oid == orderInfo.id);
                using (var db2 = new eshoppingEntities())
                {
                    
                    foreach (var item in productsResult)
                    {
                        var result = db2.store_product.Find(item.product_id);
                        Dictionary<string, Dictionary<string, Object>> cart = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Object>>>(item.cart_info);
                        string unique = (string)cart["attrInfo"]["unique"];
                        result.sales++;
                        result.stock--;
                        //规格库存减少
                        using (var dbt = new eshoppingEntities())
                        {
                            store_product_attr_value skuData = dbt.store_product_attr_value.Where(s => s.unique == unique).FirstOrDefault();
                            skuData.stock--;
                            skuData.sales++;
                        }
                       
                    }

                }
                db.SaveChanges();
                tran.Commit();
            }
        }

        public store_order GetOrderInfoByOrderID(string orderId, long uid)
        {
            using (var db = new eshoppingEntities())
            {
                return db.Set<store_order>().Include("store_order_cart_info").Where(e => e.order_id == orderId && e.uid == uid).FirstOrDefault();
            }
        }

        public PageModel GetOrders(OrderTypeParam orderTypeParam)
        {
            using (var db = new eshoppingEntities())
            {

                PageModel pageModel = null;
                IQueryable<store_order> query = db.Set<store_order>().Include("store_order_cart_info").Include("store_order_cart_info.store_product").Include("store_order_cart_info.store_cart").Where(e => e.is_del == false);

                if (orderTypeParam.orderStatus == -9)
                {
                    //查询订单详情，购物车信息
                    pageModel = new PageUtils<store_order>(orderTypeParam.Page, orderTypeParam.Limit).StartPage(query.OrderByDescending(e => e.update_time));
                    foreach (var items in ((List<store_order>)pageModel.Data))
                    {
                        items.userDto = db.eshop_user.Find(items.uid);

                        if (items.is_del==true)
                        {
                            items.statusInfo = "已删除";
                        }
                        else if (items.paid==false)
                        {
                            items.statusInfo = "未支付";
                        }
                        else if (items.paid==true&&items.status==0)
                        {
                            items.statusInfo = "待发货";
                        }
                        else if (items.paid==true&&items.status==1)
                        {
                            items.statusInfo = "待收货";
                        }
                        else if (items.paid==true&&items.status==2)
                        {
                            items.statusInfo = "待评价";
                        }
                        else if (items.paid==true&&items.status==3)
                        {
                            items.statusInfo = "已完成";
                        }
                        foreach (var item in items.store_order_cart_info)
                        {
                          
                            item.store_cart.store_product_attr_value = db.store_product_attr_value.Where(e => e.unique == item.store_cart.product_attr_unique).FirstOrDefault();
                        }

                    }
                    return pageModel;
                }
                //未支付状态
                else if (orderTypeParam.orderStatus == OrderTypeEnum.NoPay)
                {
                    //查询订单详情，购物车信息
                    pageModel = new PageUtils<store_order>(orderTypeParam.Page, orderTypeParam.Limit).StartPage(query.Where(e => e.paid == false).OrderBy(e => e.id));
                    foreach (var items in ((List<store_order>)pageModel.Data))
                    {
                        items.statusInfo = "未支付";
                        items.userDto = db.eshop_user.Find(items.uid);
                        foreach (var item in items.store_order_cart_info)
                        {
                            item.store_cart.store_product_attr_value = db.store_product_attr_value.Where(e => e.unique == item.store_cart.product_attr_unique).FirstOrDefault();
                        }

                    }
                    return pageModel;
                }
                //待发货
                else if (orderTypeParam.orderStatus == OrderTypeEnum.WaitSend)
                {
                    //查询订单详情，购物车信息
                    pageModel = new PageUtils<store_order>(orderTypeParam.Page, orderTypeParam.Limit).StartPage(query.Where(e => e.paid == true && e.status == 0).OrderBy(e => e.id));
                    foreach (var items in ((List<store_order>)pageModel.Data))
                    {
                        items.statusInfo = "未发货";
                        items.userDto = db.eshop_user.Find(items.uid);
                        foreach (var item in items.store_order_cart_info)
                        {
                            item.store_cart.store_product_attr_value = db.store_product_attr_value.Where(e => e.unique == item.store_cart.product_attr_unique).FirstOrDefault();
                        }

                    }
                    return pageModel;
                }
                //待收货
                else if (orderTypeParam.orderStatus == OrderTypeEnum.WaitGet)
                {
                    //查询订单详情，购物车信息
                    pageModel = new PageUtils<store_order>(orderTypeParam.Page, orderTypeParam.Limit).StartPage(query.Where(e => e.paid == true && e.status == 1).OrderBy(e => e.id));
                    foreach (var items in ((List<store_order>)pageModel.Data))
                    {
                        items.statusInfo = "待收货";
                        items.userDto = db.eshop_user.Find(items.uid);
                        foreach (var item in items.store_order_cart_info)
                        {
                            item.store_cart.store_product_attr_value = db.store_product_attr_value.Where(e => e.unique == item.store_cart.product_attr_unique).FirstOrDefault();
                        }

                    }
                    return pageModel;
                }
                //待回复
                else if (orderTypeParam.orderStatus == OrderTypeEnum.WaitReplay)
                {
                  
                    //查询订单详情，购物车信息
                    pageModel = new PageUtils<store_order>(orderTypeParam.Page, orderTypeParam.Limit).StartPage(query.Where(e => e.paid == true && e.status == 2).OrderBy(e => e.id));
                    foreach (var items in ((List<store_order>)pageModel.Data))
                    {
                        items.statusInfo = "待评价";
                        items.userDto = db.eshop_user.Find(items.uid);
                        foreach (var item in items.store_order_cart_info)
                        {
                            item.store_cart.store_product_attr_value = db.store_product_attr_value.Where(e => e.unique == item.store_cart.product_attr_unique).FirstOrDefault();
                        }

                    }
                    return pageModel;
                }//已完成
                else if (orderTypeParam.orderStatus==4)
                {
                    //查询订单详情，购物车信息
                    pageModel = new PageUtils<store_order>(orderTypeParam.Page, orderTypeParam.Limit).StartPage(query.Where(e => e.paid == true && e.status == 3).OrderBy(e => e.id));
                    foreach (var items in ((List<store_order>)pageModel.Data))
                    {
                        items.statusInfo = "已完成";
                        items.userDto = db.eshop_user.Find(items.uid);
                        foreach (var item in items.store_order_cart_info)
                        {
                            item.store_cart.store_product_attr_value = db.store_product_attr_value.Where(e => e.unique == item.store_cart.product_attr_unique).FirstOrDefault();
                        }

                    }
                    return pageModel;
                }
               // 已删除
                else
                {
                    //查询订单详情，购物车信息
                    pageModel = new PageUtils<store_order>(orderTypeParam.Page, orderTypeParam.Limit).StartPage(db.Set<store_order>().Include("store_order_cart_info").Include("store_order_cart_info.store_product").Include("store_order_cart_info.store_cart").Where(e => e.is_del==true&&e.paid == true).OrderBy(e => e.id));
                    foreach (var items in ((List<store_order>)pageModel.Data))
                    {
                        items.statusInfo = "已删除";
                        items.userDto = db.eshop_user.Find(items.uid);
                        foreach (var item in items.store_order_cart_info)
                        {
                            item.store_cart.store_product_attr_value = db.store_product_attr_value.Where(e => e.unique == item.store_cart.product_attr_unique).FirstOrDefault();
                        }

                    }
                    return pageModel;
                }
            }
        }

        public void OrderMark(store_order order)
        {
            using (var db = new eshoppingEntities())
            {
                var result = db.store_order.Find(order.id);
                result.remark = order.remark;
                db.SaveChanges();
            }
        }

        public void EditOrderInfo(store_order order)
        {
            using (var db = new eshoppingEntities())
            {
                var result = db.store_order.Find(order.id);
                result.pay_price = order.pay_price;
                db.SaveChanges();
            }
        }

        public void DeliverGoods(DeliverParam express)
        {
            using (var db =new eshoppingEntities())
            {  //获取快递信息
                int deliveryID = Convert.ToInt32(express.deliveryName);
              express expressInfo =  db.expresses.Where(e => e.id == deliveryID).FirstOrDefault();
                //获取订单信息
                store_order order = db.store_order.Find(express.id);
                order.delivery_name = expressInfo.name;
                order.delivery_sn = expressInfo.code;
                order.delivery_id = express.deliveryId;
                order.delivery_type ="express";
                order.update_time = DateTime.Now;
                order.status = 1;
                db.SaveChanges();

            }
        }

        public void DeliverOK(string orderId)
        {
            using (var db = new eshoppingEntities())
            {
                var result = db.store_order.Where(e => e.order_id == orderId).FirstOrDefault();
                result.status = 2;
                db.SaveChanges();
            }
        }

        public store_order GetOrderDetail(string orderID)
        {
            using (var db = new eshoppingEntities())
            {
                long uid = LocalUser.getUidByUser();
                store_order order = db.Set<store_order>().Include("store_order_cart_info").Include("store_order_cart_info.store_product").Include("store_order_cart_info.store_cart").Where(e => e.is_del == false&&e.uid== uid&&e.order_id==orderID).FirstOrDefault();

                foreach (var item in order.store_order_cart_info)
                {
                    item.store_cart.store_product_attr_value= db.store_product_attr_value.Where(e => e.unique == item.store_cart.product_attr_unique).FirstOrDefault();
                }
                order.userDto = db.eshop_user.Find(uid);
                if (order is null)
                {
                    throw new ApiException(500,"订单不存在");
                }
                 if (order.paid == false)
                {
                    order.statusInfo = "未支付";
                }
                else if (order.paid == true && order.status == 0)
                {
                    order.statusInfo = "待发货";
                }
                else if (order.paid == true && order.status == 1)
                {
                    order.statusInfo = "待收货";
                }
                else if (order.paid == true && order.status == 2)
                {
                    order.statusInfo = "待评价";
                }
                else if (order.paid == true && order.status == 3)
                {
                    order.statusInfo = "已完成";
                }
                return order; 
            }
        }
    }
}

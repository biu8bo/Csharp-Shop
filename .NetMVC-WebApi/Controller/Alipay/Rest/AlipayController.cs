using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using Commons.Utils;
using Mapper;
using MVC卓越项目.Commons.Attribute;
using MVC卓越项目.Commons.ExceptionHandler;
using MVC卓越项目.Commons.Utils;
using MVC卓越项目.Controller.Order.Param;
using Newtonsoft.Json;
using Service.OrderService.Enum;
using Service.OrderService.VO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace MVC卓越项目.Controller.Alipay.Rest
{
    [RoutePrefix("api")]
    public class AlipayController : ApiController
    {
        /// <summary>
        /// 阿里支付接口
        /// </summary>
        /// <param name="payParam"></param>
        /// <returns></returns>
        [Route("alipay")]
        [HttpPost]
        [AuthCheck]
        public string pay([FromBody] PayParam payParam)
        {
            OrderConfirmVO orderConfirmVO = RedisHelper.GetStringKey<OrderConfirmVO>("Order:" + payParam.orderKey);

        
            DefaultAopClient client = new DefaultAopClient(config.gatewayUrl, config.app_id, config.private_key, "json", "2.0", config.sign_type, config.alipay_public_key, config.charset, false);


            // 订单名称
            string subject ="";
            // 付款金额
            string total_amout = "";
            // 商品描述
            string body = "";
            string oid = "";
            if (!(orderConfirmVO is null))
            {
                subject = orderConfirmVO.cartInfo[0].productInfo.store_name;
                body = orderConfirmVO.cartInfo[0].productInfo.store_name;
                total_amout = orderConfirmVO.priceGroup["totalPrice"].ToString();
                oid = create(payParam, LocalUser.getUidByUser());
               
            }
            
            if (orderConfirmVO is null)
            {
                using (var db = new eshoppingEntities())
                {
                   var R = db.store_order.Where(o=>o.order_id == payParam.orderKey).FirstOrDefault();
                    if (R is null)
                    {
                        throw new ApiException(501, "订单不存在！");
                    }
                    subject = "购买商品";
                    total_amout = R.total_price.ToString();
                    body ="x"+R.total_num.ToString();
                    oid = payParam.orderKey;
                }
            }
            // 支付中途退出返回商户网站地址
            string quit_url = "http://localhost/user/order?type=0&tip=0";
          
            // 外部订单号，商户网站订单系统中唯一的订单号
            string out_trade_no = oid;
            // 组装业务参数model
            AlipayTradeWapPayModel model = new AlipayTradeWapPayModel();
            model.Body = body;
            model.Subject = subject;
            model.TotalAmount = total_amout;
            model.OutTradeNo = out_trade_no;
            model.ProductCode = "QUICK_MSECURITY_PAY";
            model.QuitUrl = quit_url;
            AlipayTradeWapPayRequest request = new AlipayTradeWapPayRequest();
            // 设置支付完成同步回调地址
            request.SetReturnUrl("http://localhost:8888/api/vertiyPay");
            // 将业务model载入到request
            request.SetBizModel(model);

            AlipayTradeWapPayResponse response = null;
            try
            {
                response = client.SdkExecute(request);
                return config.gatewayUrl+"?"+ response.Body;
            }
            catch (Exception exp)
            {
                throw exp;
            }


        }

        private string create(PayParam payParam,long uid)
        {
            using (var db = new eshoppingEntities())
            {
                var tran = db.Database.BeginTransaction();
                eshop_user user = db.Database.SqlQuery<eshop_user>($"SELECT eshop_user.* FROM eshop_user where uid = {uid} FOR UPDATE").FirstOrDefault();
                OrderConfirmVO orderConfirmVO = RedisHelper.GetStringKey<OrderConfirmVO>("Order:" + payParam. orderKey);
                if (orderConfirmVO is null)
                {

                    throw new ApiException(500, "订单已关闭!,请刷新当前页面重新提交");
                }
                decimal totalPrice = Convert.ToDecimal(orderConfirmVO.priceGroup["totalPrice"]);
                decimal userMoney = user.now_money - totalPrice;
                bool PlayState = false;
            
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
                    mark = payParam. mark,
                    unique = payParam.orderKey,
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
                RedisHelper.DeleteStringKey("Order:" + payParam.orderKey);
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

                db.SaveChanges();
                tran.Commit();
                return store_Order.order_id;
            }
        }

        [HttpGet]
        [Route("vertiyPay")]
        public ResponseMessageResult vertiy([FromUri]string out_trade_no)
        {
            
            using (var db = new eshoppingEntities())
            {
                var result = db.store_order.Where(e=>e.order_id== out_trade_no).FirstOrDefault();
                result.paid = true;
                db.SaveChanges();
            }
            HttpResponseMessage s = new HttpResponseMessage() { Headers = {Location = new Uri("http://localhost/user/order?type=1&tip=1") } };
            var r = new ResponseMessageResult(s);
            r.Response.StatusCode = HttpStatusCode.Redirect;
            return r;
        }
    }
}
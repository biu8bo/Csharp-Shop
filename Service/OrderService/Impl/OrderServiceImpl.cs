using Commons.BaseModels;
using Commons.Utils;
using Mapper;
using MVC卓越项目.Commons.Utils;
using Newtonsoft.Json;
using Service.OrderService.Enum;
using Service.OrderService.Param;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class OrderServiceImpl : IOrderService
    {
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
                //已支付未发货
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
                //待收货
                else if (orderTypeParam.orderType == OrderTypeEnum.WaitGet)
                {
                    //查询订单详情，购物车信息
                    pageModel = new PageUtils<store_order>().StartPage(query.Where(e => e.paid ==true && e.status == 1).OrderBy(e => e.id));
                    foreach (var items in ((List<store_order>)pageModel.Data))
                    {
                        foreach (var item in items.store_order_cart_info)
                        {
                            item.store_cart.store_product_attr_value = db.store_product_attr_value.Where(e => e.unique == item.store_cart.product_attr_unique).FirstOrDefault();
                        }

                    }
                    return pageModel;
                }//待收货
                else if (orderTypeParam.orderType == OrderTypeEnum.WaitGet)
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
                }//已完成待评价
                else if (orderTypeParam.orderType == OrderTypeEnum.WaitReplay)
                {
                    //查询订单详情，购物车信息
                    pageModel = new PageUtils<store_order>().StartPage(query.Where(e => e.paid == true && e.status == 3).OrderBy(e => e.id));
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
    }
}

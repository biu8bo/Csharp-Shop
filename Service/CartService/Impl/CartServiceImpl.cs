using Mapper;
using MVC卓越项目.Commons.Utils;
using Service.CartService.Param;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{

    public class CartServiceImpl : ICartService
    {
        public decimal addCart(CartParam cartParam, long uid)
        {
            using (var db = new eshoppingEntities())
            {
                var tran = db.Database.BeginTransaction();
                store_cart  cart = db.store_cart.Where(e => e.uid == uid && e.is_del == false && e.is_pay == false && e.product_id == cartParam.productId && e.product_attr_unique == cartParam.unique).FirstOrDefault();
               
                //不存在数据就创建数据
                if (cart is null)
                {
                    store_cart store_Cart = new store_cart()
                    {
                        uid = uid,
                        type = "product",
                        product_id = cartParam.productId,
                        product_attr_unique = cartParam.unique,
                        cart_num = cartParam.num,
                        create_time = DateTime.Now,
                        update_time = DateTime.Now
                    };
                    db.store_cart.Add(store_Cart);
                    db.SaveChanges();
                    tran.Commit();
                    return store_Cart.id;
                }
                //存在就++
                else
                {
                    //更新操作时间
                    cart.update_time = DateTime.Now;
                    cart.cart_num += cartParam.num;
                    db.SaveChanges();
                    tran.Commit();
                    return cart.id;
                }
               
              
            }
        }

    
        public Object getCartList(long uid)
        {
            using (var db = new eshoppingEntities())
            {
                return db.store_cart.Where(e=>e.uid==uid&&e.is_pay==false&&e.is_del==false).Join(db.store_product_attr_value,e=>e.product_attr_unique,e=>e.unique,(cart,value)=>new { 
                cart=cart,
                truePrice = value.price,
                sku=value.sku
                }).Join(db.store_product,e=>e.cart.product_id,e=>e.id,(cart,product)=>new
                {
                    cart = cart,
                    productData = product
                }).ToList();

               
            }
        }

        public void updateCartNum(store_cart cart)
        {
            using (var db = new eshoppingEntities())
            {
                long uid = LocalUser.getUidByUser();
                var result = db.store_cart.Where(e => e.id == cart.id&&e.uid==uid).FirstOrDefault();
                result.cart_num = cart.cart_num;
                if (cart.cart_num<1)
                {
                    delCartBathById((int)result.id);
                }
                db.SaveChanges();
            }
        }


        public void delCartBathById(int id)
        {
            using (var db = new eshoppingEntities())
            {
                db.Entry(new store_cart()
                {
                    id = id
                }).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

    }
}

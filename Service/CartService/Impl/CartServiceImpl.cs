using Mapper;
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
        public void addCart(CartParam cartParam, long uid)
        {
            using (var db = new eshoppingEntities())
            {
                var tran = db.Database.BeginTransaction();
                store_cart  cart = db.store_cart.Where(e => e.uid == uid && e.is_del == false && e.is_pay == false && e.product_id == cartParam.productId && e.product_attr_unique == cartParam.unique).FirstOrDefault();

                //不存在数据就创建数据
                if (cart is null)
                {
                    db.store_cart.Add(new store_cart()
                    {
                        uid = uid,
                        type="product",
                        product_id = cartParam.productId,
                        product_attr_unique = cartParam.unique,
                        cart_num = cartParam.num,
                        create_time = DateTime.Now,
                        update_time = DateTime.Now
                    });
                }
                //存在就++
                else
                {
                    cart.cart_num += cartParam.num;
                }
                db.SaveChanges();
                tran.Commit();
            }
        }

        public Object getCartList(long uid)
        {
            using (var db = new eshoppingEntities())
            {
                return db.store_cart.Where(e=>e.uid==uid&&e.is_pay==false&&e.is_del==false).Join(db.store_product_attr_value,e=>e.product_attr_unique,e=>e.unique,(cart,value)=>new { 
                cart=cart,
                sku=value.sku
                }).Join(db.store_product,e=>e.cart.product_id,e=>e.id,(cart,product)=>new
                {
                    cart = cart,
                    productData = product
                }).ToList();

               
            }
        }
    }
}

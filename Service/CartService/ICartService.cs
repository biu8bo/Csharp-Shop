using Mapper;
using Service.CartService.Param;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    /// <summary>
    /// 购物车服务接口
    /// </summary>
  public  interface ICartService
    {
        /// <summary>
        /// 添加购物车
        /// </summary>
        /// <param name="cartParam"></param>
        /// <param name="uid"></param>
        void addCart(CartParam cartParam,long uid);

        /// <summary>
        /// 获取购物车数据
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
       Object getCartList(long uid);

        /// <summary>
        /// 批量删除购物车
        /// </summary>
        void delCartBathById(int id);


         void updateCartNum(store_cart cart);
    }
}

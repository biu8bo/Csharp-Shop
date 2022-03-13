using Commons.BaseModels;
using Mapper;
using MVC卓越项目.Commons.Attribute;
using MVC卓越项目.Commons.Utils;
using Service.CartService.Param;
using Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
namespace MVC卓越项目.Controller.Cart
{

    /// <summary>
    /// 购物车模块
    /// </summary>
    [RoutePrefix("api")]
    public class CartController : ApiController
    {

        private readonly ICartService cartService = Bootstrapper.Resolve<ICartService>();

        /// <summary>
        /// 添加到购物车
        /// </summary>
        /// <param name="cartParam"></param>
        /// <returns></returns>
        [Route("addCart")]
        [AuthCheck]
        public ApiResult<decimal> addCart([FromBody] CartParam cartParam)
        {
            return ApiResult<decimal>.ok(cartService.addCart(cartParam, LocalUser.getUidByUser()));

        }

        /// <summary>
        /// 获取购物车数据
        /// </summary>
        /// <returns></returns>
        [Route("getCartList")]
        [AuthCheck]
        public ApiResult<Object> getCart()
        {

            return ApiResult<Object>.ok(cartService.getCartList(LocalUser.getUidByUser()));

        }

        /// <summary>
        /// 修改购物车数量
        /// </summary>
        /// <param name="cartParam"></param>
        /// <returns></returns>
        [Route("updateNum")]
        [AuthCheck]
        public ApiResult<Object> updateNum([FromBody] store_cart cartParam)
        {
            cartService.updateCartNum(cartParam);
            return ApiResult<Object>.ok();

        }


        [Route("delCart")]
        [AuthCheck]
        [HttpPost]
        public ApiResult<Object> delCart([FromUri]int cid)
        {
            
            cartService.delCartBathById(cid);
            return ApiResult<Object>.ok();

        }
    }
}
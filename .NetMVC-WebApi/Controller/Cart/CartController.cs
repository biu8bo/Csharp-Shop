using Commons.BaseModels;
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

    [RoutePrefix("api")]
    public class CartController : ApiController
    {

        private readonly ICartService cartService = Bootstrapper.Resolve<ICartService>();

        [Route("addCart")]
        [AuthCheck]
        public ApiResult<int> addCart([FromBody]CartParam cartParam)
        {
            cartService.addCart(cartParam, LocalUser.getUidByUser());
            return ApiResult<int>.ok();

        }
        [Route("getCart")]
        [AuthCheck]
        public ApiResult<Object> getCart([FromBody] CartParam cartParam)
        {

            return ApiResult<Object>.ok(cartService.getCartList(LocalUser.getUidByUser()));

        }
    }
}
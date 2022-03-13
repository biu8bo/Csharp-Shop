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

namespace MVC卓越项目.Controller.Address
{

    /// <summary>
    /// 用户地址模块
    /// </summary>
    [RoutePrefix("api")]
    public class AddressController:ApiController
    {
        private readonly IAddressService addressService = Bootstrapper.Resolve<IAddressService>();
        /// <summary>
        /// 获取用户全部地址信息
        /// </summary>
        /// <returns></returns>
        [Route("getAddressList")]
        [AuthCheck]
        public ApiResult<List<user_address>> GetAddressList()
        {
            return ApiResult<List<user_address>>.ok(addressService.GetUserAddresses());

        }
        /// <summary>
        /// 添加地址
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        [Route("addAddress")]
        [AuthCheck]
        public ApiResult<int> addAddress([FromBody] user_address address)
        {
            addressService.addAddress(address);
            return ApiResult<int>.ok();
        }

        /// <summary>
        /// 编辑地址
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        [Route("editAddress")]
        [AuthCheck]
        public ApiResult<int> editAddress([FromBody] user_address address)
        {
            addressService.editAddress(address);
            return ApiResult<int>.ok();
        }

        /// <summary>
        /// 删除地址
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("delAddress")]
        [AuthCheck]
        [HttpGet]
        public ApiResult<int> delAddress([FromUri]int id)
        {
            addressService.delAddressByID(id);
            return ApiResult<int>.ok();
        }


        [Route("setDefault")]
        [AuthCheck]
        [HttpGet]
        public ApiResult<int> setDefault([FromUri] int id)
        {
            addressService.setDefault(id);
            return ApiResult<int>.ok();
        }


        [Route("getAddressData")]
        [AuthCheck]
        [HttpGet]
        public ApiResult<user_address> getAddressData([FromUri] int id)
        {
         
            return ApiResult<user_address>.ok(addressService.getAddressData(id));
        }
    }
}
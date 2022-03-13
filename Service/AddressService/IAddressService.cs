using Commons.BaseModels;
using Mapper;
using Service.CartService.Param;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public interface IAddressService
    {
        /// <summary>
        /// 获取用户的所有地址
        /// </summary>
        /// <returns></returns>
        List<user_address> GetUserAddresses();

        /// <summary>
        /// 删除用户地址
        /// </summary>
        /// <param name="id"></param>
        void delAddressByID(int id);

        /// <summary>
        /// 编辑地址
        /// </summary>
        /// <param name="userAddress"></param>
        void editAddress(user_address userAddress);
        /// <summary>
        /// 添加地址
        /// </summary>
        /// <param name="userAddress"></param>

        void addAddress(user_address userAddress);


        /// <summary>
        /// 设置默认地址
        /// </summary>
        /// <param name="userAddress"></param>

        void setDefault(int id);


        /// <summary>
        /// 获取地址
        /// </summary>
        /// <param name="userAddress"></param>

        user_address getAddressData(int id);
    }
}

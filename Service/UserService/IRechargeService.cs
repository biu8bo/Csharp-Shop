using Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
  public  interface IRechargeService
    {
        /// <summary>
        /// 获取充值方案
        /// </summary>
        /// <returns></returns>
        List<system_group_data> GetRecharge();

        //充值
        void Recharge(long uid, int id);
    }
}

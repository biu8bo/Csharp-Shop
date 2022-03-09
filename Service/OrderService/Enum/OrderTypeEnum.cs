using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.OrderService.Enum
{
public  static   class OrderTypeEnum
    {
        /// <summary>
        /// 未支付
        /// </summary>
        public const int NoPay = 0;
        /// <summary>
        /// 待发货
        /// </summary>
        public const int WaitSend = 1;
        /// <summary>
        /// 待收货
        /// </summary>
        public const int WaitGet = 2;
        /// <summary>
        /// 待回复
        /// </summary>
        public const int WaitReplay = 3;
        /// <summary>
        /// 完成
        /// </summary>
        public const int Success = 4;


    }
}

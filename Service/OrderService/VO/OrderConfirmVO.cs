using Mapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Service.OrderService.VO
{
    public class OrderConfirmVO
    {
         public user_address  addressInfo;
        public List<store_cart> cartInfo;
        public string orderKey;
        public Hashtable priceGroup;
        public decimal seckillId;
        public decimal seckillPrice;
        // usableCoupon: null
        public eshop_user userInfo;
    }
}
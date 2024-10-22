﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.OrderService.VO
{
   public class UserOrderVO
    {
        public decimal id { get; set; }
        public string order_id { get; set; }
        public string extend_order_id { get; set; }
        public decimal uid { get; set; }
        public string real_name { get; set; }
        public string user_phone { get; set; }
        public string user_address { get; set; }
        public string cart_id { get; set; }
        public decimal freight_price { get; set; }
        public long total_num { get; set; }
        public decimal total_price { get; set; }
        public decimal total_postage { get; set; }
        public decimal pay_price { get; set; }
        public decimal pay_postage { get; set; }
        public decimal deduction_price { get; set; }
        public long coupon_id { get; set; }
        public decimal coupon_price { get; set; }
        public bool paid { get; set; }
        public Nullable<System.DateTime> pay_time { get; set; }
        public string pay_type { get; set; }
        public System.DateTime create_time { get; set; }
        public Nullable<System.DateTime> update_time { get; set; }
        public bool status { get; set; }
        public bool refund_status { get; set; }
        public string refund_reason_wap_img { get; set; }
        public string refund_reason_wap_explain { get; set; }
        public Nullable<System.DateTime> refund_reason_time { get; set; }
        public string refund_reason_wap { get; set; }
        public string refund_reason { get; set; }
        public decimal refund_price { get; set; }
        public string delivery_sn { get; set; }
        public string delivery_name { get; set; }
        public string delivery_type { get; set; }
        public string delivery_id { get; set; }
        public decimal gain_integral { get; set; }
        public decimal use_integral { get; set; }
        public decimal pay_integral { get; set; }
        public Nullable<decimal> back_integral { get; set; }
        public string mark { get; set; }
        public bool is_del { get; set; }
        public string unique { get; set; }
        public string remark { get; set; }
        public long mer_id { get; set; }
        public byte is_mer_check { get; set; }
        public Nullable<decimal> combination_id { get; set; }
        public decimal pink_id { get; set; }
        public decimal cost { get; set; }
        public decimal seckill_id { get; set; }
        public Nullable<long> bargain_id { get; set; }
        public string verify_code { get; set; }
        public int store_id { get; set; }
        public bool shipping_type { get; set; }
        public Nullable<bool> is_channel { get; set; }
        public Nullable<bool> is_remind { get; set; }
        public Nullable<bool> is_system_del { get; set; }

 
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Mapper
{
    using System;
    using System.Collections.Generic;
    
    public partial class system_user_level
    {
        public int id { get; set; }
        public int mer_id { get; set; }
        public string name { get; set; }
        public decimal money { get; set; }
        public int valid_date { get; set; }
        public bool is_forever { get; set; }
        public bool is_pay { get; set; }
        public bool is_show { get; set; }
        public int grade { get; set; }
        public decimal discount { get; set; }
        public string image { get; set; }
        public string icon { get; set; }
        public string explain { get; set; }
        public System.DateTime create_time { get; set; }
        public Nullable<System.DateTime> update_time { get; set; }
        public bool is_del { get; set; }
    }
}
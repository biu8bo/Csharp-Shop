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
    
    public partial class quartz_job
    {
        public long id { get; set; }
        public string bean_name { get; set; }
        public string cron_expression { get; set; }
        public Nullable<bool> is_pause { get; set; }
        public string job_name { get; set; }
        public string method_name { get; set; }
        public string @params { get; set; }
        public string remark { get; set; }
        public Nullable<System.DateTime> create_time { get; set; }
        public Nullable<System.DateTime> update_time { get; set; }
        public Nullable<bool> is_del { get; set; }
    }
}
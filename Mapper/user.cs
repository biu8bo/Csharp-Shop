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
    
    public partial class user
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public user()
        {
            this.role = new HashSet<role>();
        }
    
        public long id { get; set; }
        public Nullable<long> avatar_id { get; set; }
        public string email { get; set; }
        public Nullable<long> enabled { get; set; }
        public string password { get; set; }
        public string username { get; set; }
        public Nullable<long> dept_id { get; set; }
        public string phone { get; set; }
        public Nullable<long> job_id { get; set; }
        public Nullable<System.DateTime> create_time { get; set; }
        public Nullable<System.DateTime> last_password_reset_time { get; set; }
        public string nick_name { get; set; }
        public string sex { get; set; }
        public Nullable<System.DateTime> update_time { get; set; }
        public Nullable<bool> is_del { get; set; }
    
        public virtual dept dept { get; set; }
        public virtual job job { get; set; }
        public virtual user_avatar user_avatar { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<role> role { get; set; }
    }
}
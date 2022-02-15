using Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ProductService.VO
{
   public class ProductReply
    {
        public long id { get; set; }
        public long uid { get; set; }
        public long oid { get; set; }
        public string unique { get; set; }
        public long product_id { get; set; }
        public string reply_type { get; set; }
        public bool product_score { get; set; }
        public bool service_score { get; set; }
        public string comment { get; set; }
        public string pics { get; set; }
        public System.DateTime create_time { get; set; }
        public Nullable<System.DateTime> update_time { get; set; }
        public string merchant_reply_content { get; set; }
        public Nullable<System.DateTime> merchant_reply_time { get; set; }
        public bool is_del { get; set; }
        public bool is_reply { get; set; }

        public string username { get; set; }
        public string nickname { get; set; }
        public string avatar { get; set; }
    }
}

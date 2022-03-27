using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
   public class CategoryVO
    {
        public int id { get; set; }
        public int pid { get; set; }
        public string cate_name { get; set; }
        public Nullable<int> sort { get; set; }
        public string label;
        public string pic { get; set; }
        public Nullable<bool> is_show { get; set; }
        public Nullable<System.DateTime> create_time { get; set; }
        public Nullable<System.DateTime> update_time { get; set; }
        public Nullable<bool> is_del { get; set; }
        public int value { get; set; }
         public bool isDisabled { get; set; }
        public List<CategoryVO> categories;
    }
}

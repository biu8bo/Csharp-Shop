using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.BaseModels
{
   public abstract class  QueryParam
    {
        //默认第一页 一页10条
        private int page=1;
        private int limit=10;
        //查询字符串
        private string queryString;
        /// <summary>
        /// 页数
        /// </summary>
        public int Page { get => page; set => page = value; }
        /// <summary>
        /// 页面大小
        /// </summary>
        public int Limit { get => limit; set => limit = value; }
        /// <summary>
        /// 查询字符串
        /// </summary>
        public string QueryString { get => queryString; set => queryString = value; }
    }
}

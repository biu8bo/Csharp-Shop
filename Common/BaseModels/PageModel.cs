using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.BaseModels
{
    public class PageModel
    {
        //页面数
        private int pageNum;
        //页面大小
        private int pageSize;
        //总页数
        private int total;
        //有无下一页
        private Boolean hasNext;
        //页面对象
        private Object data;

        public int PageNum { get => pageNum; set => pageNum = value; }
        public int PageSize { get => pageSize; set => pageSize = value; }
        public int Total { get => total; set => total = value; }
        public bool HasNext { get => hasNext; set => hasNext = value; }
        public Object Data { get => data; set => data = value; }
    }
}

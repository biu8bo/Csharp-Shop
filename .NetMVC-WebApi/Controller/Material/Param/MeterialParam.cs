using Commons.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC卓越项目.Controller.Material.Param
{
    public class MeterialParam:QueryParam
    {
        public string groupId { get; set; }


        public string name { get; set; }

        /// <summary>
        /// 1图片 2视频
        /// </summary>
        public string type { get; set; }
        public string url { get; set; }
    }
}
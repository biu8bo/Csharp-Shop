using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace MVC卓越项目.Models
{
    public class ApiBaseModel
    {
        /// <summary>
        /// 状态代码
        /// </summary>
        private HttpStatusCode code;

        /// <summary>
        /// 消息
        /// </summary>
        private string msg;
        /// <summary>
        /// 返回的数据
        /// </summary>
        private Object data;

        /// <summary>
        /// 请求是否成功
        /// </summary>
        private bool isSuccess;

        private DateTime date;

        public ApiBaseModel()
        {
            date = DateTime.Now;
        }

        public HttpStatusCode Code { get => code; set => code = value; }
        public string Msg { get => msg; set => msg = value; }
        public object Data { get => data; set => data = value; }
        public bool IsSuccess { get => isSuccess; set => isSuccess = value; }
        public DateTime Date { get => date; }

        public ApiBaseModel setCode(HttpStatusCode code)
        {
            this.code = code;
            return this;
        }

        public ApiBaseModel setMsg(string msg)
        {
            this.msg = msg;
            return this;
        }

        public ApiBaseModel setData(Object data)
        {
            this.data = data;
            return this;
        }
        public ApiBaseModel setIsSuccess(bool isSuccess)
        {
            this.isSuccess = isSuccess;
            return this;
        }
    }
}
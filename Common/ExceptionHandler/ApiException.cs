using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace MVC卓越项目.Commons.ExceptionHandler
{
    /// <summary>
    /// 异常捕获类 Api模型
    /// </summary>
    public class ApiException : HttpException
    {

        /// <summary>
        /// 状态代码
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public new string Message { get; set; }

        public ApiException(int code, string message) : base(code, message)
        {
            this.Code = code;
            this.Message = message;
        }

        public ApiException()
        {
    
        }
    }
}
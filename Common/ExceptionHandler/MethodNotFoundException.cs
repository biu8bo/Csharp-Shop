using MVC卓越项目.Commons.ExceptionHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.ExceptionHandler
{
    /// <summary>
    /// 请求方法不存在异常
    /// </summary>
   public class MethodNotFoundException : ApiException
    {
        public MethodNotFoundException(string message = "请求的资源不存在!")
        {
            throw new ApiException(404, message);
        }
    }
}

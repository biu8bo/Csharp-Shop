using MVC卓越项目.Commons.ExceptionHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.ExceptionHandler
{
    /// <summary>
    /// 拒绝请求异常
    /// </summary>
  public  class ForbiddenException : ApiException
    {
        public ForbiddenException(string message = "访问被拒绝！")
        {
        }
    }
}

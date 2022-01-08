using MVC卓越项目.Commons.ExceptionHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.ExceptionHandler
{
  public  class MethodNotSupportException : ApiException
    {
           public MethodNotSupportException(string method)
        {
            throw new ApiException(405, $"该接口不支持{method}请求!");
        }
    }
}

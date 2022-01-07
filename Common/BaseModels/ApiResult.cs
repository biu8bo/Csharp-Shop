using MVC卓越项目.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Commons.BaseModels
{
    public class ApiResult<T> : ApiBaseModel
    {

        public static ApiResult<T> ok()
        {

            return (ApiResult<T>)new ApiResult<T>().setCode(HttpStatusCode.OK).setIsSuccess(true).setMsg("请求成功");
        }

        public static ApiResult<T> ok(Object data)
        {

            return (ApiResult<T>)new ApiResult<T>().setCode(HttpStatusCode.OK).setIsSuccess(true).setMsg("请求成功").setData(data);
        }
        public static ApiResult<T> ok(String message)
        {

            return (ApiResult<T>)new ApiResult<T>().setCode(HttpStatusCode.OK).setIsSuccess(true).setMsg(message);
        }

        public static ApiResult<T> fail(HttpStatusCode errorCode, String message)
        {
            return (ApiResult<T>)new ApiResult<T>().setCode(errorCode).setIsSuccess(false).setMsg(message);
        }

        public static ApiResult<T> fail(String message)
        {
            return (ApiResult<T>)new ApiResult<T>().setCode(HttpStatusCode.InternalServerError).setIsSuccess(false).setMsg(message);
        }
    }
}


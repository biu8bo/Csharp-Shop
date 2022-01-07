using MVC卓越项目.Commons.ExceptionHandler;


namespace MVC卓越项目.Commons.ExceptionHandler
{
    public class AuthException : ApiException
    {
        public AuthException(int code =401, string message = "权限不足") : base(code, message)
        {
        }
    }
}
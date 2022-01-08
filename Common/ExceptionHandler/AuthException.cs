using MVC卓越项目.Commons.ExceptionHandler;


namespace MVC卓越项目.Commons.ExceptionHandler
{
    public class AuthException : ApiException
    {
  
        public AuthException(string message="权限不足")
        {
            throw new ApiException(401, message);
        }
    }
}
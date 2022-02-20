using Commons.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.UserService.Param
{
 public   class RegisterParam
    {
        //用户名
       public string username { get; set; }

        //密码
        public string password { get => password; set => Md5Utils.Md5(value); }

        //手机号码
        public string phone { get; set; }
       
        //验证码
        public string vertity { get; set; }
     }
}

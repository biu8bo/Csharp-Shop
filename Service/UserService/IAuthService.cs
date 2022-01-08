using Mapper;
using MVC卓越项目.Controller.Auth.Param;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
  public  interface IAuthService
    {
        Hashtable login(LoginParam loginParam);
    }
}

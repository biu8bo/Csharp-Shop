using Service.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MVC卓越项目.Controller.Category.Rest
{
    [RoutePrefix("api")]
    public class CategoryController : ApiController
    {
        private readonly ICategoryService categoryService = Bootstrapper.Resolve<ICategoryService>();
        
        [HttpGet]
        [Route("gets")]
        public IEnumerable<string> Get()
        {
            categoryService.GetCategories();
            return new string[] { "value1", "value2" };
        }
    }
}
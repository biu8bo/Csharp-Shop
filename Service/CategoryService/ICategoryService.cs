using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
 public   interface ICategoryService
    {
        List<CategoryVO> GetCategories();
    }
}

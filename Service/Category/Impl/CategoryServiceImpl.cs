using Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class CategoryServiceImpl : ICategoryService
    {
        public List<CategoryVO> GetCategories()
        {
            using (var db = new eshoppingEntities())
            {

                List<store_category> result = db.store_category.Where(e => e.is_del == false).ToList();
                List<CategoryVO> categories = new List<CategoryVO>();
                result.ForEach(e =>
                {
       
                });
         
                return null;
            }
         
        }
    }
}

using Commons.Utils;
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

                List<store_category> result = db.store_category.Where(e => e.is_del == false&&e.is_show==true).OrderBy(e=>e.id).ToList();
                List<CategoryVO> categories = new List<CategoryVO>();
                for (int i = 0; i < result.Count; i++)
                {
                    //如果父id是0，说明是父级 建立父级
                    if (result[i].pid==0)
                    {
                        CategoryVO parentItem = ObjectUtils<CategoryVO>.ConvertTo(result[i]);
                        parentItem.categories = new List<CategoryVO>();
                        //父级ID
                        int ID = result[i].id;
                        result.ForEach(e =>
                      {
                          //如果这个的父级ID是这个ID 建立子级
                          if (e.pid == ID)
                          {
                              CategoryVO childItem = ObjectUtils<CategoryVO>.ConvertTo(e);
                              parentItem.categories.Add(childItem);
                          }
                      }
                        );
                        categories.Add(parentItem);
                    }

                  
                }
         
                return categories;
            }
         
        }
    }
}

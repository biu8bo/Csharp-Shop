using Commons.Utils;
using Mapper;
using MVC卓越项目.Commons.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class CategoryServiceImpl : ICategoryService
    {
        public void AddCategory(store_category category)
        {
            using (var db = new eshoppingEntities())
            {
                var tran = db.Database.BeginTransaction();
                category.create_time = DateTime.Now;
                category.is_del = false;
                //删除redis键
                RedisHelper.DeleteKeyByLike("Category:category:*");
                category.update_time = DateTime.Now;
                db.Entry(category).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();
                tran.Commit();
            }
        }

        public void DelCategoryByID(int id)
        {
            using (var db  = new eshoppingEntities())
            {
                var tran = db.Database.BeginTransaction();
                //删除redis键
                RedisHelper.DeleteKeyByLike("Category:category:*");
                var result = db.Set<store_category>().Where(e => e.id == id).FirstOrDefault();
                result.is_del = true;
                tran.Commit();
                db.SaveChanges();
            }
        }

        public void EditCategory(store_category category)
        {
            using (var db = new eshoppingEntities())
            {
                //删除redis键
                RedisHelper.DeleteKeyByLike("Category:category:*");
                category.update_time = DateTime.Now;
                db.Entry(category).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public List<CategoryVO> GetCategories()
        {
            using (var db = new eshoppingEntities())
            {
                List<store_category> result = db.store_category.Where(e => e.is_del == false && e.is_show == true).OrderBy(e => e.sort).ToList();
                List<CategoryVO> categories = new List<CategoryVO>();
                for (int i = 0; i < result.Count; i++)
                {
                    //如果父id是0，说明是父级 建立父级
                    if (result[i].pid == 0)
                    {
                        CategoryVO parentItem = ObjectUtils<CategoryVO>.ConvertTo(result[i]);
                        parentItem.categories = new List<CategoryVO>();
                        parentItem.label = result[i].cate_name;
                        //父级ID
                        int ID = result[i].id;
                        result.ForEach(e =>
                        {
                            //如果这个的父级ID是这个ID 建立子级
                            if (e.pid == ID)
                            {
                                CategoryVO childItem = ObjectUtils<CategoryVO>.ConvertTo(e);
                                childItem.label = e.cate_name;
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

        public List<CategoryVO> GetCategoriesBackEnd()
        {
            using (var db = new eshoppingEntities())
            {

                List<store_category> result = db.store_category.Where(e => e.is_del == false).OrderBy(e => e.sort).ToList();
                List<CategoryVO> categories = new List<CategoryVO>();
                for (int i = 0; i < result.Count; i++)
                {
                    //如果父id是0，说明是父级 建立父级
                    if (result[i].pid == 0)
                    {
                        CategoryVO parentItem = ObjectUtils<CategoryVO>.ConvertTo(result[i]);
                        parentItem.categories = new List<CategoryVO>();
                        parentItem.label = "|----" + result[i].cate_name;
                        parentItem.value = result[i].id;
                        parentItem.isDisabled = false;
                        //父级ID
                        int ID = result[i].id;
                        result.ForEach(e =>
                        {
                            //如果这个的父级ID是这个ID 建立子级
                            if (e.pid == ID)
                            {
                                CategoryVO childItem = ObjectUtils<CategoryVO>.ConvertTo(e);
                                childItem.label = "|----|----" + e.cate_name;
                                childItem.isDisabled = false;
                                childItem.value = e.id;
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

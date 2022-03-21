using Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
 public   interface ICategoryService
    {
        /// <summary>
        /// 获取全部分类
        /// </summary>
        List<CategoryVO> GetCategories();
        /// <summary>
        /// 通过id删除分类
        /// </summary>
        /// <param name="id"></param>
        void DelCategoryByID(int id);
        /// <summary>
        /// 添加分类
        /// </summary>
        /// <param name="category"></param>
        void AddCategory(store_category category);
        /// <summary>
        /// 编辑分类
        /// </summary>
        /// <param name="category"></param>
        void EditCategory(store_category category);
        /// <summary>
        /// 获取全部分类
        /// </summary>
        List<CategoryVO> GetCategoriesBackEnd();
    }
}

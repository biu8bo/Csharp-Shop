using Commons.BaseModels;
using Mapper;
using MVC卓越项目.Commons.ExceptionHandler;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC卓越项目.Commons.Utils
{
    /// <summary>
    /// 分页工具
    /// </summary>
    public class PageUtils<T>
    {
        //页面数
        private int pageNum;
        //页面大小
        private int pageSize;
        /// <param name="page">页数</param>
        /// <param name="limit">页面大小</param>
        public PageUtils(int pageNum, int pageSize)
        {
            this.pageNum = pageNum;
            this.pageSize = pageSize;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="limit">页面大小</param>
        public PageUtils()
        {
            this.pageNum = 1;
            this.pageSize = 10;
        }
        /// <summary>
        /// 开始分页
        /// </summary>
        /// <param name="queryObject">查询对象</param>
        /// <returns>分页结果</returns>
        public PageModel StartPage(IQueryable<T> queryObject)
        {
            PageModel page = new PageModel();
            page.PageNum = this.pageNum;
            page.PageSize = this.pageSize;
            //获取总数
            page.Total = queryObject.Count();
            //获取当前分页查询
            try
            {
                page.Data = queryObject.Skip(pageSize * (pageNum - 1)).Take(pageSize).ToList();
            }
            catch(Exception e)
            {

                throw new ApiException(500,e + "你没有使用OrderBy或OrderByDescending方法就直接调用分页方法！");
            }
            //计算是否有下一页
            float s = page.Total / (this.pageSize * (this.pageNum - 1) + this.pageSize * 1.0f);
            if (s<=1)
            {
                page.HasNext = false;
            }
            else {
                page.HasNext = true;
            }
            return page;
        }
       
    }
}

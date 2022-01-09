using Commons.Constant;
using Mapper;
using Service.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class ProductServiceImpl : IProductService
    {
        public List<store_product> GetList(int page, int limit, int flag)
        {
            using (var db = new eshoppingEntities())
            {
                if (flag==1)
                {
                 return   db.store_product.Where(e => e.is_best == true).Skip((page - 1) * limit).Take(limit).ToList();
                }
                else if(flag == 2)
                {
                 return   db.store_product.Where(e => e.is_hot == true).Skip((page - 1) * limit).Take(limit).ToList();
                }else if ( flag == 3)
                {
                 return   db.store_product.Where(e => e.is_new == true).Skip((page - 1)*limit).Take(limit).ToList();
                }else if(flag == 4)
                {
                 return   db.store_product.Where(e => e.is_benefit == true).Skip((page - 1) * limit).Take(limit).ToList();
                }
                return null;
            }
        }

        public List<store_product> selectByPage(int page, int limit)
        {
            using (var db = new eshoppingEntities())
            {
                return  db.store_product.Where(e => e.is_benefit == true).Skip((page - 1) * limit).Take(limit).ToList();
            }
        }
    }
}

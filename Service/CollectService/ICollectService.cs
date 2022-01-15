using Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    /// <summary>
    /// 收藏和足迹模块
    /// </summary>
  public  interface ICollectService
    {
        /// <summary>
        /// 添加足迹或收藏
        /// </summary>
        /// <returns></returns>
        void addRroductRelation(long pid, long uid,string type);

        /// <summary>
        /// 删除足迹或收藏
        /// </summary>
        /// <returns></returns>
        void delRroductRelation(long pid, long uid, string type);

        /// <summary>
        /// 批量删除足迹或收藏
        /// </summary>
        /// <returns></returns>
        void delRroductRelationWithBatch(List<long> pids, long uid, string type);

        /// <summary>
        /// 查询收藏表存在的记录
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="uid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        store_product_relation isProductRelation(long pid, long uid, string type);
    }
}

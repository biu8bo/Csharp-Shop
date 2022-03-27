using Commons.BaseModels;
using Commons.Utils;
using Mapper;
using MVC卓越项目.Commons.Attribute;
using MVC卓越项目.Commons.Utils;
using MVC卓越项目.Controller.Material.Param;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace MVC卓越项目.Controller.Material.Rest
{
    /// <summary>
    /// 素材库模块
    /// </summary>
    [RoutePrefix("api")]
    public class MaterialController : ApiController
    {
        /// <summary>
        /// 获取素材
        /// </summary>
        /// <param name="meterialParam"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("material/page")]
        [BackAuthCheck]
        public ApiResult<PageModel> GetMaterialInfo([FromUri] MeterialParam meterialParam)
        {
            using (var db = new eshoppingEntities())
            {
                var query = db.materials.Where(e => e.is_del == false);
                if (!(meterialParam.groupId is null))
                {
                    query = query.Where(e => e.group_id == meterialParam.groupId);
                }

                PageModel pageModel = new PageUtils<material>(meterialParam.Page, meterialParam.Limit).StartPage(query.OrderByDescending(e => e.create_time));
                return ApiResult<PageModel>.ok(pageModel);
            }
        }
        /// <summary>
        /// 获取全部素材分组
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("materialgroup/list")]
        [BackAuthCheck]
        public ApiResult<List<material_group>> GetMaterialGroup()
        {
            using (var db = new eshoppingEntities())
            {
                return ApiResult<List<material_group>>.ok(db.material_group.Where(e => e.is_del == false).OrderBy(e => e.create_time).ToList());
            }
        }


        /// <summary>
        /// 删除素材分组
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("materialgroup/{id}")]
        [BackAuthCheck]
        public ApiResult<List<material_group>> DeleteMaterialGroup(string id)
        {
            using (var db = new eshoppingEntities())
            {
                var result = db.material_group.Find(id);
                result.is_del = true;
                db.SaveChanges();
                return ApiResult<List<material_group>>.ok();
            }
        }

        /// <summary>
        /// 添加素材分组
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("materialgroup")]
        [BackAuthCheck]
        public ApiResult<List<material_group>> AddMaterialGroup(MeterialParam meterialParam)
        {
            using (var db = new eshoppingEntities())
            {
                material_group mtg = new material_group();
                mtg.name = meterialParam.name;
                mtg.id = new SnowFlakeUtil(1).nextId().ToString();
                mtg.create_time = DateTime.Now;
                mtg.is_del = false;
                db.material_group.Add(mtg);
                db.SaveChanges();
                return ApiResult<List<material_group>>.ok();
            }
        }


        /// <summary>
        /// 素材添加
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("material")]
        [BackAuthCheck]
        public ApiResult<int> Material(MeterialParam meterialParam)
        {
            using (var db = new eshoppingEntities())
            {
                var addObj = new material()
                {
                    id = new SnowFlakeUtil(1).nextId().ToString(),
                    group_id = meterialParam.groupId,
                    name = meterialParam.name,
                    url = meterialParam.url,
                    type = meterialParam.type,
                    create_time = DateTime.Now,
                    is_del=false
                };

                db.materials.Add(addObj);
                db.SaveChanges();
                return ApiResult<int>.ok();
            }
        }

        /// <summary>
        /// 素材删除
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("material/{id}")]
        [BackAuthCheck]
        public ApiResult<int> DeleteMaterial(string id)
        {
            using (var db = new eshoppingEntities())
            {
                db.materials.Find(id).is_del=true;
                db.SaveChanges();
                return ApiResult<int>.ok();
            }
        }
    }
}
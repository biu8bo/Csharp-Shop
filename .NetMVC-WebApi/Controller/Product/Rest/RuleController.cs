using Commons.BaseModels;
using Commons.Utils;
using Mapper;
using MVC卓越项目.Commons.Attribute;
using Newtonsoft.Json;
using Service.ProductService.VO;
using Service.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace MVC卓越项目.Controller.Product.Rest
{
    /// <summary>
    /// SKU商品规格API
    /// </summary>
    [RoutePrefix("api")]
    public class RuleController : ApiController
    {
        private readonly IProductAttrService productAttrService = Bootstrapper.Resolve<IProductAttrService>();

        /// <summary>
        /// 获取所有sku模板
        /// </summary>
        /// <param name="queryParam"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("StoreProductRule")]
        [BackAuthCheck]
        public ApiResult<PageModel> GetYxStoreProductRule([FromBody] QueryParam queryParam)
        {
            return ApiResult<PageModel>.ok(productAttrService.GetStoreProductRules(queryParam));
        }
        /// <summary>
        /// 获取商品属性详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("product/rule/{id}")]
        [BackAuthCheck]
        public ApiResult<Dictionary<String, Object>> GetYxStoreProductRuleById(int id)
        {
            var resultObj = productAttrService.GetProductAttrResultByID(id);
            if (resultObj is null)
            {
                return ApiResult<Dictionary<String, Object>>.ok();
            }
            string JSONStr = resultObj.result;
            var result = JsonConvert.DeserializeObject<Dictionary<String, Object>>(JSONStr);
            return ApiResult<Dictionary<String, Object>>.ok(result);
        }

        /// <summary>
        /// 保存规格选项
        /// </summary>
        /// <param name="id"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("product/rule/{id}")]
        [BackAuthCheck]
        public ApiResult<Dictionary<String, Object>> POSTStoreProductRuleById(int id, [FromBody] AttrResult param)
        {
            using (var db = new eshoppingEntities())
            {
                //开启事务
                var  tran= db.Database.BeginTransaction();
                //存在就删除
                db.store_product_attr.RemoveRange(db.store_product_attr.Where(e => e.product_id == id));
                db.store_product_attr_result.RemoveRange(db.store_product_attr_result.Where(e => e.product_id == id));
                db.store_product_attr_value.RemoveRange(db.store_product_attr_value.Where(e => e.product_id == id));
                db.SaveChanges();
                Object anony = new
                {
                    attr = param.items,
                    value = param.attrs
                };
                List<store_product_attr> attrs = new List<store_product_attr>();
                param.items.ForEach(e =>
                {
                    store_product_attr item = new store_product_attr();
                    item.product_id = id;
                    item.attr_name = e.value;
                    string str = "";
                    foreach (var S in e.detail)
                    {
                        str += S + ",";
                    }
                    str = str.Substring(0, str.Length - 1);
                    item.attr_values = str;
                    attrs.Add(item);
                });
                db.store_product_attr.AddRange(attrs);
                db.SaveChanges();
                db.store_product_attr_result.Add(new store_product_attr_result()
                {
                    product_id = id,
                    change_time = DateTime.Now,
                    result = JsonConvert.SerializeObject(anony)
                }) ;
                List<store_product_attr_value> lists = new List<store_product_attr_value>();
                SnowFlakeUtil snowFlakeUtil =   new SnowFlakeUtil(2);
                foreach (var item in param.attrs)
                {
                    string V = "";
                    foreach (var valseItem in item.detail.Values)
                    {
                        V += valseItem+",";
                    }
                    V = V.Substring(0, V.Length - 1);
                    store_product_attr_value result = new store_product_attr_value()
                    {
                        product_id = id,
                        stock = item.stock,
                        price = item.price,
                        ot_price= item.otPrice,
                        image = item.pic,
                        integral = (long?)item.integral,
                        unique = snowFlakeUtil.nextId().ToString(),
                        sku = V

                    };
                    lists.Add(result);
                }
                db.store_product_attr_value.AddRange(lists);
                db.SaveChanges();
                tran.Commit();
            }
            return ApiResult<Dictionary<String, Object>>.ok();
        }

        /// <summary>
        /// 生成sku
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("product/isFormatAttr/{id}")]
        [BackAuthCheck]
        public ApiResult<AttrResult> FormatAttr(int id, [FromBody] AttrResult param)
        {

            param.attrs = new List<AttrResult.Value>();
            for (int i = 0; i < param.items.Count - 1; i++)
            {

                List<string> data = new List<string>(); ;

                if (i == 0)
                {
                    data = param.items[i].detail;

                }

                foreach (var item in data)
                {
                    for (int j = 0; j < param.items[i + 1].detail.Count; j++)
                    {
                        AttrResult.Value attrValue = new AttrResult.Value();
                        attrValue.detail = new Dictionary<string, object>();
                        attrValue.detail.Add(param.items[i].value, item);
                        attrValue.detail.Add(param.items[i + 1].value, param.items[i + 1].detail[j]);
                        param.attrs.Add(attrValue);
                    }
                }
            }
            return ApiResult<AttrResult>.ok(param);

        }

    }


}

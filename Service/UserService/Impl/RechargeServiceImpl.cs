using Commons.Constant;
using Mapper;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class RechargeServiceImpl : IRechargeService
    {
        public List<system_group_data> GetRecharge()
        {
            using (var db = new eshoppingEntities())
            {
                return db.system_group_data.Where(e => e.group_name == ShopConstants.YSHOP_RECHARGE_PRICE_WAYS).ToList();
            }
        }

        public void Recharge(long uid, int id)
        {
            using (var db = new eshoppingEntities())
            {
                var tran = db.Database.BeginTransaction();
                system_group_data result = db.system_group_data.Where(e => e.group_name == ShopConstants.YSHOP_RECHARGE_PRICE_WAYS && e.id == id).FirstOrDefault();

                Hashtable rechargeResult = JsonConvert.DeserializeObject<Hashtable>(result.value);

                eshop_user user = db.Database.SqlQuery<eshop_user>($"SELECT eshop_user.* FROM eshop_user where uid = {uid} FOR UPDATE").FirstOrDefault();
                decimal rechargePrice = Convert.ToDecimal(rechargeResult["price"]);
                decimal givePrice = Convert.ToDecimal(rechargeResult["give_price"]);
                user.now_money += rechargePrice + givePrice;
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                tran.Commit();
                return;

            }
        }
    }
}

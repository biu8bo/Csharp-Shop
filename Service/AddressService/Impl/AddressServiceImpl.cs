using Commons.BaseModels;
using Mapper;
using MVC卓越项目.Commons.ExceptionHandler;
using MVC卓越项目.Commons.Utils;
using Service.CartService.Param;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class AddressServiceImpl : IAddressService
    {
        public void addAddress(user_address userAddress)
        {
            using (var db = new eshoppingEntities())
            {
                userAddress.create_time = DateTime.Now;
                userAddress.update_time = DateTime.Now;
                userAddress.uid = LocalUser.getUidByUser();
                userAddress.latitude = "";
                userAddress.longitude = "";
                userAddress.city_id = -1;
                userAddress.post_code = userAddress.post_code ?? "";
                db.user_address.Add(userAddress);
                db.SaveChanges();
            }
        }


        public void delAddressByID(int id)
        {
            using (var db = new eshoppingEntities())
            {
                db.Entry(new user_address()
                {
                    id = id,
                    uid = LocalUser.getUidByUser()
                }).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public void editAddress(user_address userAddress)
        {
            using (var db = new eshoppingEntities())
            {
                long uid = LocalUser.getUidByUser();
                userAddress.update_time = DateTime.Now;
                userAddress.uid = uid;
                userAddress.latitude = "";
                userAddress.longitude = "";
                userAddress.city_id = -1;
                userAddress.post_code = userAddress.post_code ?? "";
                db.Entry(userAddress).State = System.Data.Entity.EntityState.Modified;
                if (userAddress.is_default)
                {
                    setDefault((int)userAddress.id);
                }
                db.SaveChanges();
            }
        }

        public user_address getAddressData(int id)
        {
            using (var db = new eshoppingEntities())
            {
                long uid = LocalUser.getUidByUser();
                return db.user_address.Where(e => e.uid == uid && e.is_del == false&&e.id==id).FirstOrDefault();
            }
        }

        public List<user_address> GetUserAddresses()
        {
            using (var db = new eshoppingEntities())
            {
                long uid = LocalUser.getUidByUser();
                return db.user_address.Where(e => e.uid == uid && e.is_del == false).ToList();
            }
        }

        public void setDefault(int id)
        {
            using (var db = new eshoppingEntities())
            {
                var tran = db.Database.BeginTransaction();
                long uid = LocalUser.getUidByUser();
                var result = db.user_address.Where(e => e.uid == uid && e.is_del == false).ToList();
                result.ForEach(e =>
                {
                    if (e.id==id)
                    {
                        e.is_default = true;
                    }
                    else
                    {
                        e.is_default = false;
                    }

                });
                db.SaveChanges();
                tran.Commit();
            }
        }
    }
}

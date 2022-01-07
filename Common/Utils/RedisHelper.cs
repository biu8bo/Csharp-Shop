using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;

namespace MVC卓越项目.Commons.Utils
{
    /// <summary>
    /// Redis读写帮助类
    /// </summary>
    public class RedisHelper
    {
        private static string RedisConnectionStr = ConfigurationManager.AppSettings["RedisConnectionStr"];
        private static ConnectionMultiplexer redis { get; set; }
        private static IDatabase db { get; set; }
        static RedisHelper()
        {
            redis = ConnectionMultiplexer.Connect(RedisConnectionStr);
            db = redis.GetDatabase();
        }
        #region string类型操作
        /// <summary>
        /// set or update the value for string key 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetStringValue(string key, string value)
        {
            return db.StringSet(key, value);
        }
        /// <summary>
        /// 保存单个key value
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <param name="value">保存的值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public static bool SetStringKey(string key, string value, TimeSpan? expiry = default(TimeSpan?))
        {
            return db.StringSet(key, value, expiry);
        }
        /// <summary>
        /// 保存一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool SetStringKey<T>(string key, T obj, TimeSpan? expiry = default(TimeSpan?))
        {
            string json = JsonConvert.SerializeObject(obj);
            return db.StringSet(key, json, expiry);
        }
        /// <summary>
        /// 获取一个key的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetStringKey<T>(string key) where T : class
        {
            var res = db.StringGet(key);
            if (!res.IsNull)
                return JsonConvert.DeserializeObject<T>(res);
            return null;
        }
        /// <summary>
        /// get the value for string key 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetStringValue(string key)
        {
            return db.StringGet(key);
        }

        /// <summary>
        /// Delete the value for string key 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool DeleteStringKey(string key)
        {
            return db.KeyDelete(key);
        }
        #endregion

        #region 哈希类型操作
        /// <summary>
        /// set or update the HashValue for string key 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashkey"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetHashValue(string key, string hashkey, string value)
        {
            return db.HashSet(key, hashkey, value);
        }
        /// <summary>
        /// set or update the HashValue for string key 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="hashkey"></param>
        /// <param name="t">defined class</param>
        /// <returns></returns>
        public static bool SetHashValue<T>(String key, string hashkey, T t) where T : class
        {
            var json = JsonConvert.SerializeObject(t);
            return db.HashSet(key, hashkey, json);
        }
        /// <summary>
        /// 保存一个集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">Redis Key</param>
        /// <param name="list">数据集合</param>
        /// <param name="getModelId"></param>
        public static void HashSet<T>(string key, List<T> list, Func<T, string> getModelId)
        {
            List<HashEntry> listHashEntry = new List<HashEntry>();
            foreach (var item in list)
            {
                string json = JsonConvert.SerializeObject(item);
                listHashEntry.Add(new HashEntry(getModelId(item), json));
            }
            db.HashSet(key, listHashEntry.ToArray());
        }
        /// <summary>
        /// 获取hashkey所有的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        //public List<T> HashGetAll<T>(string key) where T : class
        //{
        //    List<T> result = new List<T>();
        //    HashEntry[] arr = db.HashGetAll(key);
        //    foreach (var item in arr)
        //    {
        //        if (!item.Value.IsNullOrEmpty)
        //        {
        //            T t;
        //            if (JsonConvert.DeserializeObject<T>(item.Value, out t))
        //            {
        //                result.Add(t);
        //            }

        //        }
        //    }
        //    return result;
        //    //result =JsonHelper.DeserializeJsonToList<T>(arr.ToString());                        
        //    //return result;
        //}
        /// <summary>
        /// get the HashValue for string key  and hashkey
        /// </summary>
        /// <param name="key">Represents a key that can be stored in redis</param>
        /// <param name="hashkey"></param>
        /// <returns></returns>
        public static RedisValue GetHashValue(string key, string hashkey)
        {
            RedisValue result = db.HashGet(key, hashkey);
            return result;
        }
        /// <summary>
        /// get the HashValue for string key  and hashkey
        /// </summary>
        /// <param name="key">Represents a key that can be stored in redis</param>
        /// <param name="hashkey"></param>
        /// <returns></returns>
        //public T GetHashValue<T>(string key, string hashkey) where T : class
        //{
        //    RedisValue result = db.HashGet(key, hashkey);
        //    if (string.IsNullOrEmpty(result))
        //    {
        //        return null;
        //    }
        //    T t;
        //    if (JsonConvert.DeserializeObject<T>(result, out t))
        //    {
        //        return t;
        //    }
        //    return null;
        //}
        /// <summary>
        /// delete the HashValue for string key  and hashkey
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashkey"></param>
        /// <returns></returns>
        public static bool DeleteHashValue(string key, string hashkey)
        {
            return db.HashDelete(key, hashkey);
        }
        #endregion
    }
}
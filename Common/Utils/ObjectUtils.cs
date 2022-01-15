using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Utils
{
    /// <summary>
    /// 对象工具
    /// </summary>
    /// <typeparam name="T"></typeparam>
  public static class ObjectUtils<T>
     {
        /// <summary>
        /// 对象转换工具
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void ConvertTo(object source ,ref T target)
        {
            target= JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source));
        }

        public static T ConvertTo(object source, T target)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source));
        }

        /// <summary>
        /// 判断对象不为空
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool isNotNull(object obj)
        {
            if(obj is  null||obj == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool isNull(object obj)
        {
            if (obj is null || obj == null)
            {
                return true;
            }
            else
            {
                return false;
             
            }
        }
    }
}

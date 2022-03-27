using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

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
            target= JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source, Formatting.Indented,
         new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
        }

        public static T ConvertTo(object source)
        {


            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source, Formatting.Indented,
         new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
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
        /// <summary>
        /// 适用于初始化新实体
        /// </summary>
        static public T RotationMapping<T, S>(S s)
        {
            T target = Activator.CreateInstance<T>();
            var originalObj = s.GetType();
            var targetObj = typeof(T);
            foreach (PropertyInfo original in originalObj.GetProperties())
            {
                foreach (PropertyInfo t in targetObj.GetProperties())
                {
                    if (t.Name == original.Name)
                    {
                        t.SetValue(target, original.GetValue(s, null), null);
                    }
                }
            }
            return target;
        }
        /// <summary>
        /// 适用于没有新建实体之间
        /// </summary>
        static public T RotationMapping<T, S>(T t, S s)
        {
            var originalObj = s.GetType();
            var targetObj = typeof(T);
            foreach (PropertyInfo sp in originalObj.GetProperties())
            {
                foreach (PropertyInfo dp in targetObj.GetProperties())
                {
                    if (dp.Name == sp.Name)
                    {
                        dp.SetValue(t, sp.GetValue(s, null), null);
                    }
                }
            }
            return t;
        }

        static public void AutoMapping<S, T>(S s, T t)
        {
            PropertyInfo[] pps = GetPropertyInfos(s.GetType());
            Type target = t.GetType();

            foreach (var pp in pps)
            {
                PropertyInfo targetPP = target.GetProperty(pp.Name);
                object value = pp.GetValue(s, null);

                if (targetPP != null && value != null)
                {
                    targetPP.SetValue(t, value, null);
                }
            }

        }

        static public PropertyInfo[] GetPropertyInfos(Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
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
        /// <summary>
        /// 反射深拷贝
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepCopyByReflect<T>(T obj)
        {
            //如果是字符串或值类型则直接返回
            if (obj is string || obj.GetType().IsValueType) return obj;

            object retval = Activator.CreateInstance(obj.GetType());
            FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            foreach (FieldInfo field in fields)
            {
                try { field.SetValue(retval, DeepCopyByReflect(field.GetValue(obj))); }
                catch { }
            }
            return (T)retval;
        }
        /// <summary>
        /// 序列化深拷贝（XML）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepCopyByXml<T>(T obj)
        {
            object retval;
            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerializer xml = new XmlSerializer(typeof(T));
                xml.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                retval = xml.Deserialize(ms);
                ms.Close();
            }
            return (T)retval;
        }

        /// <summary>
        /// 序列化深拷贝 （流）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepCopyByBin<T>(T obj)
        {
            object retval;
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                //序列化成流
                bf.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                //反序列化成对象
                retval = bf.Deserialize(ms);
                ms.Close();
            }
            return (T)retval;
        }

    }
}

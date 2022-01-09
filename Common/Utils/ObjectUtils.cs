using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Utils
{
  public static class ObjectUtils<T>
     {
        public static void ConvertTo(object source ,ref T target)
        {
            target= JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source));
        }

        public static T ConvertTo(object source, T target)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source));
        }
    }
}

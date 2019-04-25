using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Mall_Api.Models
{
    public static class objectExtensions
    {

        public static T parse<T>(this object o)
        {
            if (o == null || o.ToString().Length == 0)
                return default(T);
            else if (o is T)
                return (T)o;
            else
                return (T)Convert.ChangeType(o, typeof(T));
        }
    }
}
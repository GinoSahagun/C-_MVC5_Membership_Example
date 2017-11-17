using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Memberships.Extensions
{
    public static class ReflectionExtensions
    {
        public static string GetPropertyValue<T>(this T item, string PropertyName)
        {
            return item.GetType()
                .GetProperty(PropertyName)
                .GetValue(item,null).ToString();
        }
    }
}
using CESJapanDataServices.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using static CESJapanDataServices.EnumConstants;

namespace CESJapanDataServices.Extensions
{
    public static class AttributeExtension
    {
        public static string StringValue<T>(this T value)
        {
            Type type = value.GetType();
            FieldInfo fi = type.GetRuntimeField(value.ToString());
            return (fi.GetCustomAttributes(typeof(StringValueAttribute), false).FirstOrDefault() as StringValueAttribute).Value;
        }
    }
}
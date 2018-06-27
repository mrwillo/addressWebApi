using CESJapanDataServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace CESJapanDataServices.Extensions
{
    public static class PropertiesExtension
    {

        public static IDictionary<string, object> GetChangedProperties<T>(this T current, T old)
        {
            IDictionary<string, object> changedProperties = new Dictionary<string, object>();

            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {

                var currentVal = property.GetValue(current);
                var oldVal = property.GetValue(old);
                if (currentVal == null && oldVal == null)
                {
                    continue;
                }
                if (currentVal == null && oldVal != null)
                {
                    changedProperties.Add(property.Name, currentVal);
                }
                else if (!currentVal.Equals(oldVal))
                {
                    changedProperties.Add(property.Name, currentVal);
                }
            }
            return changedProperties;
        }
        /// <summary>
        /// Update updated field into UserInfo
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="blueUser"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static void UpdateChangedProperties<T>(this T user, BlueUser blueUser) where T : UserInfo
        {
            IDictionary<string, object> changedProperties = new Dictionary<string, object>();

            PropertyInfo[] properties = typeof(BlueUser).GetProperties();

            foreach (var property in properties)
            {
                var updateVal = property.GetValue(blueUser);
                var userProperty = typeof(UserInfo).GetProperty(property.Name);
                if (userProperty == null)
                {
                    continue;
                }
                var currentVal = userProperty.GetValue(user);

                if (updateVal == null && currentVal == null)
                {
                    continue;
                }
                if (updateVal == null && currentVal != null)
                {
                    userProperty.SetValue(user, updateVal);
                }
                else if (!updateVal.Equals(currentVal))
                {
                    userProperty.SetValue(user, updateVal);
                }
            }
        }

        /// <summary>
        /// Get Property Name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProp"></typeparam>
        /// <param name="o"></param>
        /// <param name="propertySelector"></param>
        /// <returns></returns>
        public static string PropertyName<T, TProp>(this T o, Expression<Func<T, TProp>> propertySelector)
        {
            var body = (MemberExpression)propertySelector.Body;
            return body.Member.Name;
        }
    }
}
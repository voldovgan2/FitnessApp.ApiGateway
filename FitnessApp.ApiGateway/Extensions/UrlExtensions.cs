using System;
using System.Linq;
using System.Reflection;

namespace FitnessApp.ApiGateway.Extensions
{
    public static class UrlExtensions
    {
        public static string Api(this string url, string apiName)
        {
            string trailingSlash = url.EndsWith("/") ? "" : "/";
            return $"{url}{trailingSlash}api/{apiName}";
        }

        public static string Method(this string url, string methodName)
        {
            string trailingSlash = url.EndsWith("/") ? "" : "/";
            return $"{url}{trailingSlash}{methodName}";
        }

        public static string Routes(this string url, string[] routes)
        {
            string trailingSlash = url.EndsWith("/") ? "" : "/";
            return $"{url}{trailingSlash}{string.Join("/", routes)}";
        }

        public static string ToQueryString(this string url, object model)
        {
            string result = url;
            var propertiesInfo = model.GetType().GetProperties();
            if (propertiesInfo.Any())
            {
                result += "?";
                bool isFirstParameter = true;
                for (int k = 0; k < propertiesInfo.Length; k++)
                {
                    var propertyValue = GetProperty(model, propertiesInfo[k]);
                    if (propertyValue != null)
                    {
                        if (!isFirstParameter)
                        {
                            result += "&";
                        }
                        isFirstParameter = false;
                        result += propertiesInfo[k].Name + "=" + propertyValue.ToString();
                    }
                }
            }
            return result;
        }

        private static object GetProperty(object instance, PropertyInfo propertyInfo)
        {
            object result = null;
            var propertyType = propertyInfo.PropertyType;
            switch (Type.GetTypeCode(propertyType))
            {
                case TypeCode.Boolean:
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                case TypeCode.String:
                case TypeCode.Char:
                    result = propertyInfo.GetValue(instance, null);
                    break;
            }
            return result;
        }
    }
}
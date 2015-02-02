using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;

namespace Squirrel.Utility.Helpers
{
    public static class EnumExtensions
    {
        public static string Description(this Enum eEnum)
        {
            var fieldInfo = eEnum.GetType().GetField(eEnum.ToString());

            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : eEnum.ToString();
        }

        public static List<SelectListItem> ToSelectList(this Type eEnum, bool hasNullItem = false)
        {
            if (!eEnum.IsEnum)
            {
                return new List<SelectListItem>();
            }

            var result = new List<SelectListItem>();
            if (hasNullItem)
            {
                result.Add(new SelectListItem { Text = "", Value = null });
            }
            result.AddRange(
                (from object type in Enum.GetValues(eEnum)
                    select new SelectListItem
                    {
                        Text = type.ToString(),
                        Value = type.ToString()
                    }));
            return result;
        }

        public static List<SelectListItem> SelectListOfEnumValues<T>(bool hasNullItem)
        {
            if (!typeof (T).IsEnum) 
                return new List<SelectListItem>();

            var result = new List<SelectListItem>();
            if (hasNullItem)
            {
                result.Add(new SelectListItem { Text = "", Value = null });
            }
            result.AddRange(
                (from object type in Enum.GetValues(typeof(T))
                    select new SelectListItem
                    {
                        Text = type.ToString(),
                        Value = type.ToString()
                    }));
            return result.ToList();
        }

        public static List<SelectListItem> SelectListOfEnumDescriptions<T>(bool hasNullItem)
        {
            if (!typeof(T).IsEnum)
                return new List<SelectListItem>();

            var result = new List<SelectListItem>();
            if (hasNullItem)
            {
                result.Add(new SelectListItem { Text = "", Value = null });
            }
            result.AddRange(
                (from object type in Enum.GetValues(typeof(T))
                 select new SelectListItem
                 {
                     Text = type.Description(),
                     Value = type.ToString()
                 }));
            return result.ToList();
        }

        private static string Description(this object T)
        {
            if (!T.GetType().IsEnum) return "";

            var eEnum = (Enum)T;

            var fieldInfo = eEnum.GetType().GetField(eEnum.ToString());

            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : eEnum.ToString();
        }
    }
}

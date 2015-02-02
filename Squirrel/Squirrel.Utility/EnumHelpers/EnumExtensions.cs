using System;
using System.ComponentModel;

namespace Squirrel.Utility.EnumHelpers
{
    public static class EnumExtensions
    {
        public static string Description(this Enum eEnum)
        {
            var fieldInfo = eEnum.GetType().GetField(eEnum.ToString());

            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : eEnum.ToString();
        }

        public static string Description(this object T)
        {
            if (!T.GetType().IsEnum) return "";

            var eEnum = (Enum)T;

            var fieldInfo = eEnum.GetType().GetField(eEnum.ToString());

            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : eEnum.ToString();
        }
    }
}

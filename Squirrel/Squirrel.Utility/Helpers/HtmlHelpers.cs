using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Squirrel.Utility.Helpers
{
    public static class HtmlHelpersForModels
    {
        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> helper,
            System.Linq.Expressions.Expression<Func<TModel, TEnum>> expression, bool textIsDescription)
        {
            var builder = new TagBuilder("select");

            //var enumType = expression.ReturnType;

            if (!expression.ReturnType.GenericTypeArguments.Any())
            {
                return MvcHtmlString.Create(builder.ToString());
            }

            var enumType = expression.ReturnType.GenericTypeArguments[0];
            if (!enumType.IsEnum)
            {
                return MvcHtmlString.Create(builder.ToString());
            }

            var innerOptions = string.Empty;
            foreach (var item in Enum.GetValues(enumType))
            {
                var option = new TagBuilder("option");
                option.SetInnerText(textIsDescription ? item.Description() : item.ToString());
                option.MergeAttribute("value", item.ToString());
                innerOptions += option.ToString();
            }

            builder.InnerHtml = innerOptions;
            return MvcHtmlString.Create(builder.ToString());
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

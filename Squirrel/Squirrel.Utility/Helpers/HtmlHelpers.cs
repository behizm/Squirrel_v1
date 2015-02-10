using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Squirrel.Utility.Helpers
{
    public static class HtmlHelpersForModels
    {

        #region EnumDropDownList

        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> helper,
            System.Linq.Expressions.Expression<Func<TModel, TEnum>> expression, bool textIsDescription)
        {
            var builder = TagOfEnumDropDownListFor(helper, expression, textIsDescription);
            return MvcHtmlString.Create(builder.ToString());
        }

        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> helper,
            System.Linq.Expressions.Expression<Func<TModel, TEnum>> expression, bool textIsDescription, IDictionary<string, object> htmlAttributes)
        {
            var builder = TagOfEnumDropDownListFor(helper, expression, textIsDescription);

            if (htmlAttributes == null || !htmlAttributes.Any())
                return MvcHtmlString.Create(builder.ToString());

            foreach (var htmlAttribute in htmlAttributes)
            {
                builder.MergeAttribute(htmlAttribute.Key, htmlAttribute.Value.ToString());
            }
            return MvcHtmlString.Create(builder.ToString());
        }

        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> helper,
            System.Linq.Expressions.Expression<Func<TModel, TEnum>> expression, bool textIsDescription, object htmlAttributes)
        {
            var builder = TagOfEnumDropDownListFor(helper, expression, textIsDescription);
            var dictionary = htmlAttributes.ToDictionary();

            if (dictionary == null || !dictionary.Any())
                return MvcHtmlString.Create(builder.ToString());

            foreach (var htmlAttribute in dictionary)
            {
                builder.MergeAttribute(htmlAttribute.Key, htmlAttribute.Value.ToString());
            }
            return MvcHtmlString.Create(builder.ToString());
        }

        private static TagBuilder TagOfEnumDropDownListFor<TModel, TEnum>(HtmlHelper helper,
            System.Linq.Expressions.Expression<Func<TModel, TEnum>> expression, bool textIsDescription)
        {
            var builder = new TagBuilder("select");
            Type type;
            var nullableType = false;

            // Define data type
            if (Nullable.GetUnderlyingType(expression.ReturnType) == null)
            {
                type = expression.ReturnType;
            }
            else if (expression.ReturnType.GenericTypeArguments.Any())
            {
                type = expression.ReturnType.GenericTypeArguments[0];
                nullableType = true;
            }
            else
            {
                var option = new TagBuilder("option");
                option.SetInnerText("Invalid data type.");
                option.MergeAttribute("value", "");
                builder.InnerHtml = option.ToString();
                return builder;
            }

            // Data type must be Enum
            if (!type.IsEnum)
            {
                var option = new TagBuilder("option");
                option.SetInnerText("Invalid data type.");
                option.MergeAttribute("value", "");
                builder.InnerHtml = option.ToString();
                return builder;
            }

            // Id & Name
            var id = helper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression));
            builder.MergeAttribute("id", id);
            builder.MergeAttribute("name", id);

            // Validations
            var valAttributes = helper.GetUnobtrusiveValidationAttributes(id);
            if (valAttributes.Any())
            {
                builder.MergeAttribute("data-val", "true");
                foreach (var valAttribute in valAttributes)
                {
                    builder.MergeAttribute(valAttribute.Key, valAttribute.Value.ToString());
                }
            }

            // Set options
            var value = ModelMetadata.FromLambdaExpression(expression, (ViewDataDictionary<TModel>) helper.ViewData);
            var innerOptions = string.Empty;
            try
            {
                if (nullableType)
                {
                    var option = new TagBuilder("option");
                    option.SetInnerText("");
                    option.MergeAttribute("value", "");
                    innerOptions += option.ToString();
                }
                foreach (var item in Enum.GetValues(type))
                {
                    var option = new TagBuilder("option");
                    option.SetInnerText(textIsDescription ? item.Description() : item.ToString());
                    option.MergeAttribute("value", item.ToString());
                    if (item.Equals(value.Model))
                    {
                        option.MergeAttribute("selected", "selected");
                    }
                    innerOptions += option.ToString();
                }
            }
            catch (Exception)
            {
                return builder;
            }

            builder.InnerHtml = innerOptions;
            return builder;
        }

        #endregion



        private static string Description(this object T)
        {
            if (!T.GetType().IsEnum) return "";

            var eEnum = (Enum)T;

            var fieldInfo = eEnum.GetType().GetField(eEnum.ToString());

            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : eEnum.ToString();
        }

        private static Dictionary<string, object> ToDictionary(this object dicObject)
        {
            if (dicObject == null)
                return null;

            var dictionary = dicObject as Dictionary<string, object>;
            if (dictionary != null)
                return dictionary;

            var baseDictionary = dicObject as IDictionary;
            if (baseDictionary != null)
            {
                dictionary =
                baseDictionary.Cast<DictionaryEntry>()
                    .ToDictionary<DictionaryEntry, string, object>(item => item.Key.ToString(),
                        item => item.Value.ToString());

                return dictionary;
            }

            dictionary =
                TypeDescriptor.GetProperties(dicObject)
                    .Cast<PropertyDescriptor>()
                    .ToDictionary(property => property.Name, property => property.GetValue(dicObject));

            if (dictionary.Any())
                return dictionary;

            return null;
        }
    }
}

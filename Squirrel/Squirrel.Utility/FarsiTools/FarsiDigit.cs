using System.Web;
using System.Web.Mvc;

namespace Squirrel.Utility.FarsiTools
{
    public static class FarsiDigit
    {
        public static string FaDigit(this string content)
        {
            return
                content
                .Replace("0", "۰")
                .Replace("1", "۱")
                .Replace("2", "۲")
                .Replace("3", "۳")
                .Replace("4", "۴")
                .Replace("5", "۵")
                .Replace("6", "۶")
                .Replace("7", "۷")
                .Replace("8", "۸")
                .Replace("9", "۹");
        }

        public static string FaDigit(this int content)
        {
            return
                content.ToString("D")
                .Replace("0", "۰")
                .Replace("1", "۱")
                .Replace("2", "۲")
                .Replace("3", "۳")
                .Replace("4", "۴")
                .Replace("5", "۵")
                .Replace("6", "۶")
                .Replace("7", "۷")
                .Replace("8", "۸")
                .Replace("9", "۹");
        }

        public static MvcHtmlString FaDigit(this MvcHtmlString content)
        {
            return
                new MvcHtmlString(
                    content.ToHtmlString()
                        .Replace("0", "۰")
                        .Replace("1", "۱")
                        .Replace("2", "۲")
                        .Replace("3", "۳")
                        .Replace("4", "۴")
                        .Replace("5", "۵")
                        .Replace("6", "۶")
                        .Replace("7", "۷")
                        .Replace("8", "۸")
                        .Replace("9", "۹"));
        }

        public static HtmlString FaDigit(this HtmlString content)
        {
            return
                new HtmlString(
                    content.ToHtmlString()
                        .Replace("0", "۰")
                        .Replace("1", "۱")
                        .Replace("2", "۲")
                        .Replace("3", "۳")
                        .Replace("4", "۴")
                        .Replace("5", "۵")
                        .Replace("6", "۶")
                        .Replace("7", "۷")
                        .Replace("8", "۸")
                        .Replace("9", "۹"));
        }
    }
}

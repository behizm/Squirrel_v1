using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Squirrel.Utility.Helpers
{
    public static class SelectListExtensions
    {
        public static List<SelectListItem> SelectListOfBool(string trueText, string falseText, bool hasNullItem = true)
        {
            var result = new List<SelectListItem>();
            if (hasNullItem)
            {
                result.Add(new SelectListItem { Text = "", Value = null });
            }
            result.Add(new SelectListItem { Text = trueText, Value = true.ToString() });
            result.Add(new SelectListItem { Text = falseText, Value = false.ToString() });
            return result.ToList();
        }
    }
}

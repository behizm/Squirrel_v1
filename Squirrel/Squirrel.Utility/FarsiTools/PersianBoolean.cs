namespace Squirrel.Utility.FarsiTools
{
    public static class PersianBoolean
    {
        public static string ToPersian(this bool val)
        {
            return val ? "بله" : "خیر";
        }

        public static string ToPersian(this bool? val)
        {
            if (val.HasValue)
            {
                return val.Value.ToPersian();
            }
            return string.Empty;
        }
    }
}

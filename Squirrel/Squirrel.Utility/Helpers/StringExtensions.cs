namespace Squirrel.Utility.Helpers
{
    public static class StringExtensions
    {
        public static bool IsEmpty(this string text)
        {
            return string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text);
        }

        public static bool IsNotEmpty(this string text)
        {
            return !string.IsNullOrEmpty(text) && !string.IsNullOrWhiteSpace(text);
        }

        public static string TrimAndLower(this string text)
        {
            if (text.IsEmpty())
            {
                return string.Empty;
            }
            return text.Trim().ToLower();
        }
    }
}

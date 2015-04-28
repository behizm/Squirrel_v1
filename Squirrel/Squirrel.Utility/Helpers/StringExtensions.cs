namespace Squirrel.Utility.Helpers
{
    public static class StringExtensions
    {
        public static bool IsNothing(this string text)
        {
            return string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text);
        }

        public static bool IsNotNothing(this string text)
        {
            return !string.IsNullOrEmpty(text) && !string.IsNullOrWhiteSpace(text);
        }

        public static string TrimAndLower(this string text)
        {
            if (text.IsNothing())
            {
                return string.Empty;
            }
            return text.Trim().ToLower();
        }
    }
}

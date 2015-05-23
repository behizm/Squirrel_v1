using System;
using System.Linq;

namespace Squirrel.Utility.Helpers
{
    public static class NumberExtensions
    {
        public static int Length(this long num)
        {
            var count = 0;
            var division = num;
            while (division != 0)
            {
                division /= 10;
                count++;
            }

            return count;
        }

        public static int Length(this long? num)
        {
            return !num.HasValue ? 0 : num.Value.Length();
        }

        public static int Length(this int num)
        {
            return ((long)num).Length();
        }

        public static int Length(this int? num)
        {
            return !num.HasValue ? 0 : num.Value.Length();
        }

        public static int? Digit(this char character)
        {
            switch (character)
            {
                case '0':
                    return 0;

                case '1':
                    return 1;

                case '2':
                    return 2;

                case '3':
                    return 3;

                case '4':
                    return 4;

                case '5':
                    return 5;

                case '6':
                    return 6;

                case '7':
                    return 7;

                case '8':
                    return 8;

                case '9':
                    return 9;

                default:
                    return null;
            }
        }

        public static int DigitsCount(this string text)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
            {
                return 0;
            }

            return text.Count(c => c.Digit().HasValue);
        }

        public static long? NumberWithin(this string text, int digitCount = 18)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
            {
                return 0;
            }

            if (digitCount > 18)
            {
                digitCount = 18;
            }

            var numString = text.Where(c => c.Digit().HasValue).Aggregate("", (current, c) => current + c);
            if (numString.Length > digitCount)
            {
                numString = numString.Substring(0, digitCount);
            }

            try
            {
                return Convert.ToInt64(numString);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string ToByteUnit(this int num, bool isFarsiText = true)
        {
            int value;
            if (num < 1024)
            {
                return isFarsiText ? "یک کیلوبایت" : "1 KB";
            }
            if (num >= 1024 && num < 1024 * 1024)
            {
                value = num / 1024;
                return string.Format(isFarsiText ? "{0} کیلوبایت" : "{0} KB", value);
            }
            if (num >= 1024 * 1024 && num < 1024 * 1024 * 1024)
            {
                value = num / (1024 * 1024);
                return string.Format(isFarsiText ? "{0} مگابایت" : "{0} MB", value);
            }
            if (num >= 1024 * 1024 * 1024)
            {
                value = num / (1024 * 1024 * 1024);
                return string.Format(isFarsiText ? "{0} گیگابایت" : "{0} GB", value);
            }
            return string.Empty;
        }
    }

    public class NumberMethods
    {
        public static long MaxNumberWithLength(int length)
        {
            return ((long)Math.Pow(10, length) - 1);
        }

        public static long MinNumberWithLength(int length)
        {
            return ((long)Math.Pow(10, length - 1));
        }

        public static string RandomFromGuid(int length)
        {
            if (length < 1) return string.Empty;

            var rand = "";
            while (rand.Length < length)
            {
                rand +=
                    Guid.NewGuid()
                        .ToString()
                        .Where(c => c.Digit().HasValue)
                        .Aggregate("", (current, c) => current + c);
            }
            if (rand.Length == length)
            {
                return rand;
            }
            rand = rand.Substring(0, length);
            return rand;
        }

        public static long RandomLongFromGuid(int length)
        {
            if (length < 1) return 0;

            if (length > 18) length = 18;

            char first;
            while (true)
            {
                var guid = Guid.NewGuid().ToString();
                if (!guid.Any(c => c.Digit().HasValue && c.Digit().Value != 0))
                    continue;
                first = Guid.NewGuid().ToString().First(c => c.Digit().HasValue && c.Digit().Value != 0);
                break;
            }
            var rand = "";
            rand += first;
            rand += RandomFromGuid(length - 1);
            try
            {
                return Convert.ToInt64(rand);
            }
            catch (Exception)
            {
                return RandomLongFromGuid(length);
            }
        }
    }
}

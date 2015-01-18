using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Squirrel.Utility.Cryptography
{
    public class Hashing
    {
        // ReSharper disable once InconsistentNaming
        public static string CalculateSHA1(string text)
        {
            try
            {
                return HashOf<SHA1CryptoServiceProvider>(text).Result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        // ReSharper disable once InconsistentNaming
        public static async Task<string> CalculateSHA1_Async(string text)
        {
            try
            {
                return await HashOf<SHA1CryptoServiceProvider>(text);
            }
            catch (Exception)
            {
                return null;
            }
        }

        // ReSharper disable once InconsistentNaming
        public static string CalculateSHA256(string text)
        {
            try
            {
                return HashOf<SHA256CryptoServiceProvider>(text).Result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        // ReSharper disable once InconsistentNaming
        public static async Task<string> CalculateSHA256_Async(string text)
        {
            try
            {
                return await HashOf<SHA256CryptoServiceProvider>(text);
            }
            catch (Exception)
            {
                return null;
            }
        }

        // ReSharper disable once InconsistentNaming
        public static string CalculateSHA384(string text)
        {
            try
            {
                return HashOf<SHA384CryptoServiceProvider>(text).Result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        // ReSharper disable once InconsistentNaming
        public static async Task<string> CalculateSHA384_Async(string text)
        {
            try
            {
                return await HashOf<SHA384CryptoServiceProvider>(text);
            }
            catch (Exception)
            {
                return null;
            }
        }

        // ReSharper disable once InconsistentNaming
        public static string CalculateSHA512(string text)
        {
            try
            {
                return HashOf<SHA512CryptoServiceProvider>(text).Result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        // ReSharper disable once InconsistentNaming
        public static async Task<string> CalculateSHA512_Async(string text)
        {
            try
            {
                return await HashOf<SHA512CryptoServiceProvider>(text);
            }
            catch (Exception)
            {
                return null;
            }
        }

        // ReSharper disable once InconsistentNaming
        public static string CalculateMD5(string text)
        {
            try
            {
                return HashOf<MD5CryptoServiceProvider>(text).Result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        // ReSharper disable once InconsistentNaming
        public static async Task<string> CalculateMD5_Async(string text)
        {
            try
            {
                return await HashOf<MD5CryptoServiceProvider>(text);
            }
            catch (Exception)
            {
                return null;
            }
        }

        // ReSharper disable once InconsistentNaming
        public static string CalculateRIPEMD160(string text)
        {
            try
            {
                return HashOf<RIPEMD160Managed>(text).Result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        // ReSharper disable once InconsistentNaming
        public static async Task<string> CalculateRIPEMD160_Async(string text)
        {
            try
            {
                return await HashOf<RIPEMD160Managed>(text);
            }
            catch (Exception)
            {
                return null;
            }
        }

        // ReSharper disable once InconsistentNaming
        public static string CalculateHMACMD5(string text)
        {
            try
            {
                return HashOf<HMACMD5>(text).Result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        // ReSharper disable once InconsistentNaming
        public static async Task<string> CalculateHMACMD5_Async(string text)
        {
            try
            {
                return await HashOf<HMACMD5>(text);
            }
            catch (Exception)
            {
                return null;
            }
        }

        // ReSharper disable once InconsistentNaming
        public static string CalculateHMACRIPEMD160(string text)
        {
            try
            {
                return HashOf<HMACRIPEMD160>(text).Result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        // ReSharper disable once InconsistentNaming
        public static async Task<string> CalculateHMACRIPEMD160_Async(string text)
        {
            try
            {
                return await HashOf<HMACRIPEMD160>(text);
            }
            catch (Exception)
            {
                return null;
            }
        }

        // ReSharper disable once InconsistentNaming
        public static string CalculateHMACSHA1(string text)
        {
            try
            {
                return HashOf<HMACSHA1>(text).Result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        // ReSharper disable once InconsistentNaming
        public static async Task<string> CalculateHMACSHA1_Async(string text)
        {
            try
            {
                return await HashOf<HMACSHA1>(text);
            }
            catch (Exception)
            {
                return null;
            }
        }

        // ReSharper disable once InconsistentNaming
        public static string CalculateHMACSHA256(string text)
        {
            try
            {
                return HashOf<HMACSHA256>(text).Result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        // ReSharper disable once InconsistentNaming
        public static async Task<string> CalculateHMACSHA256_Async(string text)
        {
            try
            {
                return await HashOf<HMACSHA256>(text);
            }
            catch (Exception)
            {
                return null;
            }
        }

        // ReSharper disable once InconsistentNaming
        public static string CalculateHMACSHA384(string text)
        {
            try
            {
                return HashOf<HMACSHA384>(text).Result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        // ReSharper disable once InconsistentNaming
        public static async Task<string> CalculateHMACSHA384_Async(string text)
        {
            try
            {
                return await HashOf<HMACSHA384>(text);
            }
            catch (Exception)
            {
                return null;
            }
        }

        // ReSharper disable once InconsistentNaming
        public static string CalculateHMACSHA512(string text)
        {
            try
            {
                return HashOf<HMACSHA512>(text).Result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        // ReSharper disable once InconsistentNaming
        public static async Task<string> CalculateHMACSHA512_Async(string text)
        {
            try
            {
                return await HashOf<HMACSHA512>(text);
            }
            catch (Exception)
            {
                return null;
            }
        }


        private static async Task<string> HashOf<TP>(string text)
            where TP : HashAlgorithm, new()
        {
            var task = Task.Factory.StartNew(() =>
            {
                var buffer = Encoding.UTF8.GetBytes(text);
                using (var provider = new TP())
                {
                    var hash = provider.ComputeHash(buffer);
                    return Convert.ToBase64String(hash);
                }
            });
            return await task;
        }
    }
}

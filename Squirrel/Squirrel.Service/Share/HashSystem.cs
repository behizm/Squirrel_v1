using System.Security.Cryptography;
using System.Threading.Tasks;
using Squirrel.Utility.Cryptography;

namespace Squirrel.Service.Share
{
    class HashSystem
    {
        public static async Task<string> EncryptAsync(string code)
        {
            return
                await Symmetric<TripleDESCryptoServiceProvider>
                    .EncryptAsync(code, "behnamZeighami@gmail", "snjab");

        }
        public static async Task<string> DecryptAsync(string token)
        {
            return
                await Symmetric<TripleDESCryptoServiceProvider>
                    .DecryptAsync(token, "behnamZeighami@gmail", "snjab");

        }
    }
}

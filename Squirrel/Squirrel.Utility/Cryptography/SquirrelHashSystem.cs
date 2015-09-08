using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Squirrel.Utility.Cryptography
{
    public class SquirrelHashSystem
    {
        public static async Task<string> EncryptAsync(string code)
        {
            return
                await Symmetric<TripleDESCryptoServiceProvider>
                    .EncryptAsync(code, "squirrel:blog:behi", "snjab");

        }
        public static async Task<string> DecryptAsync(string token)
        {
            return
                await Symmetric<TripleDESCryptoServiceProvider>
                    .DecryptAsync(token, "squirrel:blog:behi", "snjab");

        }
    }
}

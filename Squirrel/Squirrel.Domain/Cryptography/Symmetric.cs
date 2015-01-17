using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Squirrel.Domain.Cryptography
{
    public class Symmetric<T> where T : SymmetricAlgorithm, new()
    {
        public static string Encrypt(string code, string key, string salt)
        {
            try
            {
                DeriveBytes rgb = new Rfc2898DeriveBytes(key, Encoding.Unicode.GetBytes(salt));

                SymmetricAlgorithm algorithm = new T();

                var rgbKey = rgb.GetBytes(algorithm.KeySize >> 3);
                var rgbIv = rgb.GetBytes(algorithm.BlockSize >> 3);

                var transform = algorithm.CreateEncryptor(rgbKey, rgbIv);

                using (var buffer = new MemoryStream())
                {
                    using (var stream = new CryptoStream(buffer, transform, CryptoStreamMode.Write))
                    {
                        using (var writer = new StreamWriter(stream, Encoding.Unicode))
                        {
                            writer.Write(code);
                        }
                    }

                    return Convert.ToBase64String(buffer.ToArray());
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<string> EncryptAsync(string code, string key, string salt)
        {
            try
            {
                var task = Task.Factory.StartNew(() =>
                {
                    DeriveBytes rgb = new Rfc2898DeriveBytes(key, Encoding.Unicode.GetBytes(salt));

                    SymmetricAlgorithm algorithm = new T();

                    var rgbKey = rgb.GetBytes(algorithm.KeySize >> 3);
                    var rgbIv = rgb.GetBytes(algorithm.BlockSize >> 3);

                    var transform = algorithm.CreateEncryptor(rgbKey, rgbIv);

                    using (var buffer = new MemoryStream())
                    {
                        using (var stream = new CryptoStream(buffer, transform, CryptoStreamMode.Write))
                        {
                            using (var writer = new StreamWriter(stream, Encoding.Unicode))
                            {
                                writer.Write(code);
                            }
                        }

                        return Convert.ToBase64String(buffer.ToArray());
                    }
                });
                return await task;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string Decrypt(string token, string key, string salt)
        {
            try
            {
                DeriveBytes rgb = new Rfc2898DeriveBytes(key, Encoding.Unicode.GetBytes(salt));

                SymmetricAlgorithm algorithm = new T();

                var rgbKey = rgb.GetBytes(algorithm.KeySize >> 3);
                var rgbIv = rgb.GetBytes(algorithm.BlockSize >> 3);

                var transform = algorithm.CreateDecryptor(rgbKey, rgbIv);

                using (var buffer = new MemoryStream(Convert.FromBase64String(token)))
                {
                    using (var stream = new CryptoStream(buffer, transform, CryptoStreamMode.Read))
                    {
                        using (var reader = new StreamReader(stream, Encoding.Unicode))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<string> DecryptAsync(string token, string key, string salt)
        {
            try
            {
                var task = Task.Factory.StartNew(() =>
                {
                    DeriveBytes rgb = new Rfc2898DeriveBytes(key, Encoding.Unicode.GetBytes(salt));

                    SymmetricAlgorithm algorithm = new T();

                    var rgbKey = rgb.GetBytes(algorithm.KeySize >> 3);
                    var rgbIv = rgb.GetBytes(algorithm.BlockSize >> 3);

                    var transform = algorithm.CreateDecryptor(rgbKey, rgbIv);

                    using (var buffer = new MemoryStream(Convert.FromBase64String(token)))
                    {
                        using (var stream = new CryptoStream(buffer, transform, CryptoStreamMode.Read))
                        {
                            using (var reader = new StreamReader(stream, Encoding.Unicode))
                            {
                                return reader.ReadToEnd();
                            }
                        }
                    }
                });
                return await task;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}

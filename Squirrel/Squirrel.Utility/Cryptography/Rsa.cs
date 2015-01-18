using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Squirrel.Utility.Cryptography
{
    public class Rsa
    {
        private readonly RSACryptoServiceProvider _provider;
        private RsaKeyModel _key;
        

        public Rsa(int keySize)
        {
            _provider = new RSACryptoServiceProvider(keySize);
        }

        public Rsa(int keySize, RsaKeyModel key)
        {
            _provider = new RSACryptoServiceProvider(keySize);
            _key = key;
            _provider.FromXmlString(_key.Private);
        }


        public RsaKeyModel GenerateKey()
        {
            try
            {
                _key = new RsaKeyModel
                {
                    Private = _provider.ToXmlString(true),
                    Public = _provider.ToXmlString(false),
                };
                return _key;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<RsaKeyModel> GenerateKeyAsync()
        {
            try
            {
                var task = Task.Factory.StartNew(() => new RsaKeyModel
                {
                    Private = _provider.ToXmlString(true),
                    Public = _provider.ToXmlString(false),
                });
                return await task;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string Encrypt(string code)
        {
            try
            {
                var encodedCode = Encoding.UTF8.GetBytes(code);
                var encryptedCode = _provider.Encrypt(encodedCode, false);
                var encodedCodeResult = Convert.ToBase64String(encryptedCode);
                return encodedCodeResult;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<string> EncryptAsync(string code)
        {
            try
            {
                var task = Task.Factory.StartNew(() =>
                {
                    var encodedCode = Encoding.UTF8.GetBytes(code);
                    var encryptedCode = _provider.Encrypt(encodedCode, false);
                    var encodedCodeResult = Convert.ToBase64String(encryptedCode);
                    return encodedCodeResult;
                });
                return await task;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string Decrypt(string token)
        {
            try
            {
                var encodedToken = Convert.FromBase64String(token);
                var decryptToken = _provider.Decrypt(encodedToken, false);
                var resultCode = Encoding.UTF8.GetString(decryptToken);
                return resultCode;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<string> DecryptAsync(string token)
        {
            try
            {
                var task = Task.Factory.StartNew(() =>
                {
                    var encodedToken = Convert.FromBase64String(token);
                    var decryptToken = _provider.Decrypt(encodedToken, false);
                    var resultCode = Encoding.UTF8.GetString(decryptToken);
                    return resultCode;
                });
                return await task;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool Validate(string code, string token)
        {
            var decryptedToken = Decrypt(token);
            return decryptedToken == code;
        }

        public async Task<bool> ValidateAsync(string code, string token)
        {
            var decryptedToken = await DecryptAsync(token);
            return decryptedToken == code;
        }

    }
}

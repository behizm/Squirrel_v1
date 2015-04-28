using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Squirrel.Utility.Cryptography;

namespace Squirrel.Web.Models
{
    public class RsaClass
    {
        private static readonly RsaKeyModel Key = new RsaKeyModel
        {
            Private = "<RSAKeyValue><Modulus>389TO4NLNXEe1atx6Vkb9rVYgwtutE/3TU9+E5ES99F8hUUpGCCfS1iukDeOuQPJfGAVA1MndLnKn/YVLtS1o2eAcEuKz1kXeVGQJU+yHxWXRLRDfwt37ctr/TI44FpNsl/s9nsgtytw+qWVuKPmX0XLZ9+OhUL3WNRYr5FUbFM=</Modulus><Exponent>AQAB</Exponent><P>9dBV22ToZ2g22suxPet4wZsJCI85QQcMkaM+DauGc/jIXnofkkYCDTPWlMfjvMxDzGgcQ2yiWpI3WXI0F0+NKQ==</P><Q>6RWQwlSmL+GoiV0psBC9sEeiaVeFPFZ+4y+VbmsRUGBqTU0XC4kxh2Zau0adrQlmHbswJAOCM3ThLwMDNlRhGw==</Q><DP>0ofGoQtByR8GrEn82a2NVTQ1fnE8didVan7HrOmVVEplQhWSaLMxGqGlTPQOZysDflREsLKgMrY8VY1sc/KcUQ==</DP><DQ>DrPji76XNRIA9ZlA1fBYZtQ99HMm3mV+X4pS3MI4N1+9lN62A25GB4p3UrTjoVSkcl+qa7sk7WYEBlkkTLsJ3Q==</DQ><InverseQ>RrgF5L1oav1rReFbbIhXoX7pxqIbd9BmyO43m7y+tBjK2D9U+d6COUpzig86JaUC73Dhl8/9J84mCrjMx0PMpA==</InverseQ><D>VbTmcc7omTGVlkuOVsb6D3MIzz4sib5pvOBpLoMK2X4Oilae0w+DEO6IAtBTxoQtv4Rn55sLbAbOEo3BNOreF1rhm0HF9V+CJaUlPfXHHKy8N0iTjmWcsvXot3F5FYTfWZwaLKxLT/IOlJE9M6f3yAuuh75rG6uOqYCmPmLljpE=</D></RSAKeyValue>",
            Public = "<RSAKeyValue><Modulus>389TO4NLNXEe1atx6Vkb9rVYgwtutE/3TU9+E5ES99F8hUUpGCCfS1iukDeOuQPJfGAVA1MndLnKn/YVLtS1o2eAcEuKz1kXeVGQJU+yHxWXRLRDfwt37ctr/TI44FpNsl/s9nsgtytw+qWVuKPmX0XLZ9+OhUL3WNRYr5FUbFM=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>",
        };


        public static string Encrypt(string code)
        {
            var rsaService = new Rsa(1024, Key);
            return rsaService.Encrypt(code);
        }

        public static string Decrypt(string token)
        {
            var rsaService = new Rsa(1024, Key);
            return rsaService.Decrypt(token);
        }
    }
}
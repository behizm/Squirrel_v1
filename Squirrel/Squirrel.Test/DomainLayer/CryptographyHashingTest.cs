using System;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Squirrel.Test.DomainLayer
{
    [TestClass]
    public class CryptographyHashingTest
    {
        [TestMethod]
        public void HMACSHA1()
        {
            var token = Domain.Cryptography.Hashing.CalculateHMACSHA1("behnam62");
            Assert.Fail(token);
        }

        [TestMethod]
        public void HMACMD5()
        {
            var token = Domain.Cryptography.Hashing.CalculateHMACMD5("behnam62");
            Assert.Fail(token);
        }

        [TestMethod]
        public void SHA256()
        {
            var token = Domain.Cryptography.Hashing.CalculateHMACMD5("behnam62");
            Assert.Fail(token);
        }
    }
}

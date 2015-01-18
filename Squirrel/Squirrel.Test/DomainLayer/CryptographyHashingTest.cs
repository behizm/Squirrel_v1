using System;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Squirrel.Utility.Cryptography;

namespace Squirrel.Test.DomainLayer
{
    [TestClass]
    public class CryptographyHashingTest
    {
        [TestMethod]
        public void HMACSHA1()
        {
            var token = Hashing.CalculateHMACSHA1("behnam62");
            Assert.Fail(token);
        }

        [TestMethod]
        public void HMACMD5()
        {
            var token = Hashing.CalculateHMACMD5("behnam62");
            Assert.Fail(token);
        }

        [TestMethod]
        public void SHA256()
        {
            var token = Hashing.CalculateHMACMD5("behnam62");
            Assert.Fail(token);
        }
    }
}

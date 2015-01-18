using System;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Squirrel.Utility.Cryptography;

namespace Squirrel.Test.DomainLayer
{
    [TestClass]
    public class CryptographySymmetricTest
    {
        [TestMethod]
        public void SymmetricDes()
        {
            var token = Symmetric<DESCryptoServiceProvider>.Encrypt("behnam", "behi8303@yahoo.com", "xoxo");
            Assert.Fail(token);
        }

        [TestMethod]
        public void SymmetricRc2()
        {
            var token = Symmetric<RC2CryptoServiceProvider>.Encrypt("behnam", "behi8303@yahoo.com", "xoxo");
            Assert.Fail(token);
        }

        [TestMethod]
        public void SymmetricTripleDes()
        {
            var token = Symmetric<TripleDESCryptoServiceProvider>.Encrypt("behnam", "behi8303@yahoo.com", "xoxo");
            Assert.Fail(token);
        }

        [TestMethod]
        public void SymmetricRijndael()
        {
            var token = Symmetric<RijndaelManaged>.Encrypt("behnam", "behi8303@yahoo.com", "xoxo");
            Assert.Fail(token);
        }
    }
}

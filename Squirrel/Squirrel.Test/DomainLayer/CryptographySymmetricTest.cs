using System;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Squirrel.Test.DomainLayer
{
    [TestClass]
    public class CryptographySymmetricTest
    {
        [TestMethod]
        public void SymmetricDes()
        {
            var token = Domain.Cryptography.Symmetric<DESCryptoServiceProvider>.Encrypt("behnam", "behi8303@yahoo.com", "xoxo");
            Assert.Fail(token);
        }

        [TestMethod]
        public void SymmetricRc2()
        {
            var token = Domain.Cryptography.Symmetric<RC2CryptoServiceProvider>.Encrypt("behnam", "behi8303@yahoo.com", "xoxo");
            Assert.Fail(token);
        }

        [TestMethod]
        public void SymmetricTripleDes()
        {
            var token = Domain.Cryptography.Symmetric<TripleDESCryptoServiceProvider>.Encrypt("behnam", "behi8303@yahoo.com", "xoxo");
            Assert.Fail(token);
        }

        [TestMethod]
        public void SymmetricRijndael()
        {
            var token = Domain.Cryptography.Symmetric<RijndaelManaged>.Encrypt("behnam", "behi8303@yahoo.com", "xoxo");
            Assert.Fail(token);
        }
    }
}

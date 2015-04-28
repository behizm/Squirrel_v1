using System;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Squirrel.Utility.Cryptography;

namespace Squirrel.Test.DomainLayer
{
    [TestClass]
    public class CryptographyRSA
    {
        private readonly Rsa _rsaSrevice1024 = new Squirrel.Utility.Cryptography.Rsa(1024);

        [TestMethod]
        public void GenerateKey()
        {
            var key = _rsaSrevice1024.GenerateKey();
            Assert.Fail("\n\n" + key.Private + "\n\n" + key.Public);
        }
    }
}

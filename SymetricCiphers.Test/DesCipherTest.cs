using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SymetricCiphers.DES;

namespace SymetricCiphers.Test {
    [TestClass]
    public class DesCipherTest {
        [TestMethod]
        public void TestDesCipher() {
            byte[] key = new byte[] { 0x13, 0x34, 0x57, 0x79, 0x9B, 0xBC, 0xDF, 0xF1 };
            byte[] message = new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };
            byte[] expectedEncryptedMessage = new byte[] { 0x85, 0xE8, 0x13, 0x54, 0x0F, 0x0A, 0xB4, 0x05 };

            ICipher cipher = new DesCipher(key);
            byte[] actualEncryptedMessage = cipher.Encrypt(message);

            CollectionAssert.AreEqual(expectedEncryptedMessage, actualEncryptedMessage);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void TestEncryptWithWrongMessageLengthExpectedException() {
            byte[] key = new byte[] { 0x13, 0x34, 0x57, 0x79, 0x9B, 0xBC, 0xDF, 0xF1 };
            byte[] message = new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF, 0xAA };

            ICipher cipher = new DesCipher(key);
            byte[] actualEncryptedMessage = cipher.Encrypt(message);
        }

        [TestMethod]
        public void TestDesCipherDecrypt() {
            byte[] key = new byte[] { 0x13, 0x34, 0x57, 0x79, 0x9B, 0xBC, 0xDF, 0xF1 };
            byte[] expectedDecryptedmessage = new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };
            byte[] encryptedMessage = new byte[] { 0x85, 0xE8, 0x13, 0x54, 0x0F, 0x0A, 0xB4, 0x05 };

            ICipher cipher = new DesCipher(key);
            byte[] actualDecryptedMessage = cipher.Decrypt(encryptedMessage);

            CollectionAssert.AreEqual(expectedDecryptedmessage, actualDecryptedMessage);
        }
    }
}

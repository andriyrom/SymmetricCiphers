using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using SymetricCiphers.Common;
using SymetricCiphers.DES;

namespace SymetricCiphers.Test {
    [TestClass]
    public class DesHelperTest {
        [TestMethod]
        public void DesFunctionTest() {
            string rightPartString = "1111 0000 1010 1010 1111 0000 1010 1010";
            string roundKeyString = "000110 110000 001011 101111 111111 000111 000001 110010";
            string resultExpectedString = "0010 0011 0100 1010 1010 1001 1011 1011";
            BitArray rightPart = BitArrayExtension.ToBitArray(rightPartString.Replace(" ",""));
            BitArray roundKey = BitArrayExtension.ToBitArray(roundKeyString.Replace(" ", ""));
            BitArray resultExpected = BitArrayExtension.ToBitArray(resultExpectedString.Replace(" ", ""));

            BitArray resultActual = DesHelper.Function(rightPart, roundKey);

            CollectionAssert.AreEqual(resultExpected, resultActual);
        }
    }
}

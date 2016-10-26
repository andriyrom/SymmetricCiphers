using Microsoft.VisualStudio.TestTools.UnitTesting;
using SymetricCiphers.Common;
using SymetricCiphers.DES;
using System.Collections;

namespace SymetricCiphers.Test {
    [TestClass]
    public class DesKeyGeneratorTest {
        [TestMethod]
        public void TestKeyGenerator() {
            string[] expectedSubKeys = new string[] {
                "000110 110000 001011 101111 111111 000111 000001 110010",
                "011110 011010 111011 011001 110110 111100 100111 100101",
                "010101 011111 110010 001010 010000 101100 111110 011001",
                "011100 101010 110111 010110 110110 110011 010100 011101",
                "011111 001110 110000 000111 111010 110101 001110 101000",
                "011000 111010 010100 111110 010100 000111 101100 101111",
                "111011 001000 010010 110111 111101 100001 100010 111100",
                "111101 111000 101000 111010 110000 010011 101111 111011",
                "111000 001101 101111 101011 111011 011110 011110 000001",
                "101100 011111 001101 000111 101110 100100 011001 001111",
                "001000 010101 111111 010011 110111 101101 001110 000110",
                "011101 010111 000111 110101 100101 000110 011111 101001",
                "100101 111100 010111 010001 111110 101011 101001 000001",
                "010111 110100 001110 110111 111100 101110 011100 111010",
                "101111 111001 000110 001101 001111 010011 111100 001010",
                "110010 110011 110110 001011 000011 100001 011111 110101"
            };
            string stringKey = "00010011 00110100 01010111 01111001 10011011 10111100 11011111 11110001";
            BitArray key = BitArrayExtension.ToBitArray(stringKey.Replace(" ", ""));
            KeyGenerator keyGenerator = new KeyGenerator(key);
            for (int i = 0; i < KeyGenerator.NumberOfRounds; i++) {
                BitArray result = keyGenerator.GetRoundKey(i);
                BitArray expected = BitArrayExtension.ToBitArray(expectedSubKeys[i].Replace(" ", ""));
                CollectionAssert.AreEqual(expected, result, string.Format("Expected and result key number {0} do not match", i));
            }
        }
    }
}

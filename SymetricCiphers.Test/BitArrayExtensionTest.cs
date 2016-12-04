using Microsoft.VisualStudio.TestTools.UnitTesting;
using SymetricCiphers.Common;
using System.Collections;

namespace SymetricCiphers.Test {
    [TestClass]
    public class BitArrayExtensionTest {
        [TestMethod]
        public void TestCopyBitArray() {
            BitArray input = new BitArray(new byte[] { 5 });
            BitArray result = new BitArray(input.Length);
            input.CopyBitArray(0, result, 0, 4);
            input.CopyBitArray(0, result, 4, 4);
            BitArray expected = new BitArray(new byte[] { 85 });
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestCycleLeftShiftOneStep() {
            BitArray input = new BitArray(new byte[] { 11 });
            BitArray result = input.CycleLeftShift(1);
            BitArray expected = new BitArray(new byte[] { 133 });
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestCycleLeftShiftTwoSteps() {
            BitArray input = new BitArray(new byte[] { 11 });
            BitArray result = input.CycleLeftShift(2);
            BitArray expected = new BitArray(new byte[] { 194 });
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestRevertWithOnesArray() {
            BitArray input = new BitArray(new byte[] { 0xFF });
            BitArray result = input.Revert();
            BitArray expected = new BitArray(new byte[] { 0xFF });
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestRevertWithSymetricArray() {
            BitArray input = new BitArray(new byte[] { 0x0F });
            BitArray result = input.Revert();
            BitArray expected = new BitArray(new byte[] { 0xF0 });
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestRevertWithAsymetricArray() {
            BitArray input = new BitArray(new byte[] { 0xC4 });
            BitArray result = input.Revert();
            BitArray expected = new BitArray(new byte[] { 0x23 });
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestReadInBigEndian() {
            byte[] input = new byte[] { 143, 3 };
            string expectedResult = "10001111 00000011".Replace(" ", "");
            BitArray expected = BitArrayExtension.ToBitArray(expectedResult);
            BitArray result = BitArrayExtension.ReadInBigEndian(input);
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestSaveInBigEndian() {
            string inputString = "10001111 00000011".Replace(" ", "");
            BitArray input = BitArrayExtension.ToBitArray(inputString);
            byte[] expected = new byte[] { 143, 3 };
            byte[] result = input.SaveInBigEndian();
            CollectionAssert.AreEqual(expected, result);
        }
    }
}

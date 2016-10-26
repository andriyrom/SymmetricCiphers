﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    }
}
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SymetricCiphers.Common;
using System.Collections;

namespace SymetricCiphers.Test {
    [TestClass]
    public class PermutatorTests {
        [TestMethod]
        public void PermutatorTest() {
            int[] permutationVector = new int[] {7, 6, 5, 4, 3, 2, 1, 0};
            var permutator = new Permutator(permutationVector);
            BitArray initialSequence = new BitArray(new byte[]{42});
            BitArray result = permutator.Permut(initialSequence);
            BitArray expectedResult = new BitArray(new byte[]{84});
            CollectionAssert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void PermutatorTestZeros() {
            int[] permutationVector = new int[] { 7, 6, 5, 4, 3, 2, 1, 0 };
            var permutator = new Permutator(permutationVector);
            BitArray initialSequence = new BitArray(new byte[] { 0 });
            BitArray result = permutator.Permut(initialSequence);
            BitArray expectedResult = new BitArray(new byte[] { 0 });
            CollectionAssert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void PermutatorTestOnes() {
            int[] permutationVector = new int[] { 7, 6, 5, 4, 3, 2, 1, 0 };
            var permutator = new Permutator(permutationVector);
            BitArray initialSequence = new BitArray(new byte[] { 0xFF });
            BitArray result = permutator.Permut(initialSequence);
            BitArray expectedResult = new BitArray(new byte[] { 0xFF });
            CollectionAssert.AreEqual(expectedResult, result);
        }
    }
}

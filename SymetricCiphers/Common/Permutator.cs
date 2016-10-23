using System;
using System.Collections;

namespace SymetricCiphers.Common {
    internal class Permutator {
        private int[] PermutationVector;
        public Permutator(int[] permutationVector) {
            int length = permutationVector.Length;
            PermutationVector = new int[length];
            Array.Copy(permutationVector, PermutationVector, length);
        }

        public BitArray Permut(BitArray inputArray) {
            BitArray outputArray = new BitArray(PermutationVector.Length);
            for (int i = 0; i < PermutationVector.Length; i++) {
                int currentPermutationIndex = PermutationVector[i];
                outputArray[i] = inputArray[currentPermutationIndex];
            }
            return outputArray;
        }
    }
}

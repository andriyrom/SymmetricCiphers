using System;
using System.Collections;

namespace SymetricCiphers.DES {
    internal class KeyGenerator {
        private static readonly int NumberOfRounds = 16;
        private static readonly int[] InitialKeyPermutationVector = 
            new int[] { 49, 42, 35, 28, 21, 14, 7, 0, 50, 43, 36, 29, 22, 15, 
                        8, 1, 51, 44, 37, 30, 23, 16, 9, 2, 52, 45, 38, 31,
                        55, 48, 41, 34, 27, 20, 13, 6, 54, 47, 40, 33, 26, 19,
                        12, 5, 53, 46, 39, 32, 26, 18, 11, 4, 24, 17, 10, 3 };

        BitArray[] RoundKeys = new BitArray[NumberOfRounds];
        public KeyGenerator(BitArray key) {
            if (key.Length != 56) {
                throw new ArgumentException("Key should have length 56 bits."); 
            }
        }
    }
}

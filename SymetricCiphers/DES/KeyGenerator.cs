using System;
using System.Collections;

namespace SymetricCiphers.DES {
    internal class KeyGenerator {
        private static readonly int NumberOfRounds = 16;
        private static readonly int[] InitialKeyPermutationVector = new int[] { };

        BitArray[] RoundKeys = new BitArray[NumberOfRounds];
        public KeyGenerator(BitArray key) {
            if (key.Length != 56) {
                throw new ArgumentException("Key should have length 56 bits."); 
            }
        }
    }
}

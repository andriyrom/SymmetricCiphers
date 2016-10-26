using SymetricCiphers.Common;
using System;
using System.Collections;

namespace SymetricCiphers.DES {
    internal class KeyGenerator {
        public static readonly int NumberOfRounds = 16;
        private static readonly int[] InitialKeyPermutationVector = 
            new int[] { 56, 48, 40, 32, 24, 16, 8, 0, 57, 49, 41, 33, 25, 17, 
                        9, 1, 58, 50, 42, 34, 26, 18, 10, 2, 59, 51, 43, 35,
                        62, 54, 46, 38, 30, 22, 14, 6, 61, 53, 45, 37, 29, 21,
                        13, 5, 60, 52, 44, 36, 28, 20, 12, 4, 27, 19, 11, 3 };
        private static readonly int[] CompressionPermutationVector =
            new int[] { 13, 16, 10, 23, 0, 4, 2, 27, 14, 5, 20, 9, 22, 18, 11, 3,
                        25, 7, 15, 6, 26, 19, 12, 1, 40, 51, 30, 36, 46, 54, 29, 39,
                        50, 44, 32, 47, 43, 48, 38, 55, 33, 52, 45, 41, 49, 35, 28, 31 };
        
        private BitArray[] RoundKeys = new BitArray[NumberOfRounds];

        private BitArray PartC = new BitArray(28);
        private BitArray PartD = new BitArray(28);
        public KeyGenerator(BitArray key) {
            if (key.Length != 64) {
                throw new ArgumentException("Key should have length 64 bits."); 
            }
            CreateRoundKeys(key);
        }

        public BitArray GetRoundKey(int roundNumber) {
            return RoundKeys[roundNumber];
        }

        private void CreateRoundKeys(BitArray key) {
            Permutator initialPermutation = new Permutator(InitialKeyPermutationVector);
            BitArray permutatedKey = initialPermutation.Permut(key);
            Permutator finalPermutator = new Permutator(CompressionPermutationVector);
            FillPartsCD(permutatedKey);
            for (int i = 0; i < NumberOfRounds; i++) {
                int shiftSize = GetShiftOnStep(i);
                PartC = CycleLeftShift(PartC, shiftSize);
                PartD = CycleLeftShift(PartD, shiftSize);
                BitArray concatenatedCDParts = ConcatCDPArts();
                RoundKeys[i] = finalPermutator.Permut(concatenatedCDParts);
            }
        }

        private int GetShiftOnStep(int stepNumber) {
            switch (stepNumber) {
                case 0: 
                case 1:
                case 8:
                case 15:
                    return 1;
                default:
                    return 2;
            }
        }

        private void FillPartsCD(BitArray permutatedKey) {
            CopyBitArray(permutatedKey, 0, PartC, 0, 28);
            CopyBitArray(permutatedKey, 28, PartD, 0, 28);
        }

        private BitArray ConcatCDPArts() {
            int resultLength = PartC.Length + PartD.Length;
            var result = new BitArray(resultLength);
            CopyBitArray(PartC, 0, result, 0, PartC.Length);
            CopyBitArray(PartD, 0, result, PartC.Length, PartD.Length);
            return result;
        }

        internal static void CopyBitArray(BitArray source, int sourceIndex, BitArray destination, int destinationIndex, int length) {
            for (int i = 0; i < length; i++) {
                destination[destinationIndex + i] = source[sourceIndex + i];
            }
        }

        internal static BitArray CycleLeftShift(BitArray array, int shiftSize) {
            var result = new BitArray(array.Length);
            CopyBitArray(array, shiftSize, result, 0, array.Length - shiftSize);
            CopyBitArray(array, 0, result, array.Length - shiftSize, shiftSize);
            return result;
        }
    }
}

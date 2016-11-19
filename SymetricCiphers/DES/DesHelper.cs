﻿using SymetricCiphers.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymetricCiphers.DES {
    internal static class DesHelper {
        private static readonly int[] InitialPermutation = 
            new int[] { 57, 49, 41, 33, 25, 17, 9, 1, 59, 51, 43, 25, 27, 19, 11, 3,
                        61, 53, 45, 37, 29, 21, 13, 5, 63, 55, 47, 39, 31, 23, 15, 7,
                        56, 48, 40, 32, 24, 16, 8, 0, 58, 50, 42, 34, 26, 18, 10, 2,
                        60, 52, 44, 36, 28, 20, 12, 4, 62, 54, 46, 38, 30, 22, 14, 6 };

        private static readonly int[] FinalPermutation = 
            new int[] { 39, 7, 47, 15, 55, 23, 63, 31, 38, 6, 46, 14, 54, 22, 62, 30,
                        37, 5, 45, 13, 53, 21, 61, 29, 36, 4, 44, 12, 52, 20, 60, 28, 
                        35, 3, 43, 11, 51, 19, 59, 27, 34, 2, 42, 10, 50, 18, 58, 26,
                        33, 1, 41, 9, 49, 17, 57, 25, 32, 0, 40, 8, 48, 16, 56, 24 };

        private static readonly int[] PBlockExtensionPermutation =
            new int[] { 31, 0, 1, 2, 3, 4, 3, 4, 5, 6, 7, 8,
                        7, 8, 9, 10, 11, 12, 11, 12, 13, 14, 15, 16,
                        15, 16, 17, 18, 19, 20, 19, 20, 21, 22, 23, 24,
                        23, 24, 25, 26, 27, 28, 27, 28, 29, 30, 31, 0 };

        private static readonly int[] PBlockStraightPermutation =
           new int[] { 15, 6, 19, 20, 28, 11, 27, 16, 0, 14, 22, 25, 4, 17, 30, 9,
                       1, 7, 23, 13, 31, 26, 2, 8, 18, 12, 29, 5, 21, 10, 3, 24 };

        private static readonly Permutator InitialPermutator = new Permutator(InitialPermutation);
        private static readonly Permutator FinalPermutator = new Permutator(FinalPermutation);
        private static readonly Permutator PBlockExtensionPermutator = new Permutator(PBlockExtensionPermutation);
        private static readonly Permutator PBlockStraightPermutator = new Permutator(PBlockStraightPermutation);

        public static BitArray Function(BitArray rightPart, BitArray roundKey) {
            BitArray extendedRightPart = PBlockExtensionPermutator.Permut(rightPart);
            BitArray sBoxesInput = extendedRightPart.Xor(roundKey);
            BitArray result = new BitArray(32);
            BitArray rowBits = new BitArray(2);
            BitArray columnBits = new BitArray(4);
            int[] rowIndex = new int[1];
            int[] columnIndex = new int[1];
            for (int i = 0; i < 8; i++) {
                int currentMainArrayIndex = i * 6;
                rowBits[0] = sBoxesInput[currentMainArrayIndex];
                rowBits[1] = sBoxesInput[currentMainArrayIndex + 5];
                sBoxesInput.CopyBitArray(currentMainArrayIndex + 1, columnBits, 0, 4);
                rowBits.Revert().CopyTo(rowIndex, 0);
                columnBits.Revert().CopyTo(columnIndex, 0);
                BitArray sBoxValue = SBoxes.GetValue(i, rowIndex[0], columnIndex[0]).Revert();
                int currentResultArrayInxex = i * 4;
                sBoxValue.CopyBitArray(0, result, currentResultArrayInxex, 4);
            }
            return PBlockStraightPermutator.Permut(result);
        }

        private class SBoxes {
            private static readonly byte[][][] SBoxesTable = new byte[][][] { 
                new byte[][] { 
                    new byte[] { 14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7 }, 
                    new byte[] { 0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8 },
                    new byte[] { 4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0 },
                    new byte[] { 15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13 } },
                new byte[][] { 
                    new byte[] { 15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10 }, 
                    new byte[] { 3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5 },
                    new byte[] { 0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15 }, 
                    new byte[] { 13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9 } },
                new byte[][] { 
                    new byte[] { 10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8 }, 
                    new byte[] { 13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1 },
                    new byte[] { 13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7 }, 
                    new byte[] { 1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12 } },
                new byte[][] { 
                    new byte[] { 7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15 }, 
                    new byte[] { 13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9 },
                    new byte[] { 10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4 }, 
                    new byte[] { 3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14 } },
                new byte[][] { 
                    new byte[] { 2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9 }, 
                    new byte[] { 14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6 },
                    new byte[] { 4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14 }, 
                    new byte[] { 11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3 } },
                new byte[][] { 
                    new byte[] { 12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11 }, 
                    new byte[] { 10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8 },
                    new byte[] { 9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6 }, 
                    new byte[] { 4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13 } },
                new byte[][] { 
                    new byte[] { 4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1 }, 
                    new byte[] { 13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6 },
                    new byte[] { 1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2 }, 
                    new byte[] { 6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12 } },
                new byte[][] { 
                    new byte[] { 13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7 }, 
                    new byte[] { 1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2 },
                    new byte[] { 7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8 }, 
                    new byte[] { 2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11 } },
            };

            public static BitArray GetValue(int boxNumber, int row, int column) {
                byte value = SBoxesTable[boxNumber][row][column];
                BitArray temp = new BitArray(new byte[]{value});
                BitArray result = new BitArray(4);
                temp.CopyBitArray(0, result, 0, result.Length);
                return result;
            }
        }  
    }
}

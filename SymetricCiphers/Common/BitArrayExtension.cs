using System;
using System.Collections;
using System.Text;

namespace SymetricCiphers.Common {
    public static class BitArrayExtension {
        public static BitArray CycleLeftShift(this BitArray array, int shiftSize) {
            var result = new BitArray(array.Length);
            array.CopyBitArray(shiftSize, result, 0, array.Length - shiftSize);
            array.CopyBitArray(0, result, array.Length - shiftSize, shiftSize);
            return result;
        }

        public static void CopyBitArray(this BitArray source, int sourceIndex, BitArray destination, int destinationIndex, int length) {
            for (int i = 0; i < length; i++) {
                destination[destinationIndex + i] = source[sourceIndex + i];
            }
        }

        public static string ToBitString(this BitArray array) {
            StringBuilder bitString = new StringBuilder(array.Length);
            foreach (bool bit in array) {
                bitString.Append(bit ? "1" : "0");
            }
            return bitString.ToString();
        }

        public static BitArray ToBitArray(string bitString) {
            BitArray array = new BitArray(bitString.Length);
            for (int i = 0; i < bitString.Length; i++) {
                switch (bitString[i]) {
                    case '0': array[i] = false;
                        break;
                    case '1': array[i] = true;
                        break;
                    default: throw new ArgumentException("Input ");
                }
            }
            return array;
        }

        public static BitArray Revert(this BitArray array) {
            int length = array.Length;
            var result = new BitArray(length);            
            for (int i = 0; i < length; i++) {
                result[i] = array[length - i - 1];
            }
            return result;
        }
    }
}

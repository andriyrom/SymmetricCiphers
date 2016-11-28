using SymetricCiphers.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymetricCiphers.DES {
    public class DesCipher : ICipher {
        private const int BlockLengthBytes = 8;
        private const int BlockPartLengthBytes = 4;
        private const int BlockPartLengthBits = BlockPartLengthBytes * BitsInByte;
        private const int BitsInByte = 8;
        private KeyGenerator RoundKeys;
        public DesCipher(byte[] key) {
            Key = key;
            RoundKeys = new KeyGenerator(ReadBytesInBigEndian(key));
        }

        public byte[] Key { get; private set; }

        public byte[] Encrypt(byte[] message) {
            message = AlignMessage(message);
            byte[] result = new byte[message.Length];
            int blockCount = message.Length / BlockLengthBytes;
            for (int i = 0; i < blockCount; i++) {
                byte[] block = new byte[BlockLengthBytes];
                int blockStartIndex = i * BlockLengthBytes;
                Array.Copy(message, blockStartIndex, block, 0, BlockLengthBytes);
                byte[] encryptedBlock = EncryptBlock(block);
                encryptedBlock.CopyTo(result, blockStartIndex);
            }
            return result;
        }

        private byte[] EncryptBlock(byte[] block) {
            BitArray result = DesRound(block);
            return SaveBitArrayInBigEndian(result);
        }


        private BitArray DesRound(byte[] block) {
            BitArray rigthPart;
            BitArray leftPart;
            BitArray inputBlock = ReadBytesInBigEndian(block);
            BitArray permutatedBlock = DesHelper.InitialPermutator.Permut(inputBlock);

            GetBlockParts(permutatedBlock, out leftPart, out rigthPart);
            for (int round = 0; round < KeyGenerator.NumberOfRounds; round++) {
                BitArray desFunction = DesHelper.Function(rigthPart, RoundKeys.GetRoundKey(round));
                BitArray newRightPart = leftPart.Xor(desFunction);
                leftPart = rigthPart;
                rigthPart = newRightPart;
            }
            BitArray result = new BitArray(leftPart.Length + rigthPart.Length);
            rigthPart.CopyBitArray(0, result, 0, BlockPartLengthBits);
            leftPart.CopyBitArray(0, result, BlockPartLengthBits, BlockPartLengthBits);            
            return DesHelper.FinalPermutator.Permut(result);
        }  

        public byte[] Decrypt(byte[] encryptedMessage) {
            throw new NotImplementedException();
        }

        private byte[] AlignMessage(byte[] message) {
            int tail = message.Length % BlockLengthBytes;
            if (tail == 0) { return message; }
            int alignedMessageLength = (message.Length / BlockLengthBytes + 1) * BlockLengthBytes;
            byte[] alignedMessage = new byte[alignedMessageLength];
            alignedMessage.Initialize();
            message.CopyTo(alignedMessage, 0);
            return alignedMessage;
        }

        private void GetBlockParts(BitArray block, out BitArray leftPart, out BitArray rightPart) {
            leftPart = new BitArray(BlockPartLengthBits);
            rightPart = new BitArray(BlockPartLengthBits);
            block.CopyBitArray(0, leftPart, 0, BlockPartLengthBits);
            block.CopyBitArray(BlockPartLengthBits, rightPart, 0, BlockPartLengthBits);            
        }

        private BitArray ReadBytesInBigEndian(byte[] input) {
            BitArray result = new BitArray(input.Length * BitsInByte);
            for (int i = 0; i < input.Length; i++) {
                BitArray temp = new BitArray(new byte[] { input[i] });
                temp.Revert().CopyBitArray(0, result, i * temp.Length, temp.Length);
            }
            return result;
        }

        private byte[] SaveBitArrayInBigEndian(BitArray input) {
            byte[] result = new byte[input.Length / BitsInByte];
            BitArray tempBitArray = new BitArray(BitsInByte);
            byte[] tempByte = new byte[1];
            for (int i = 0; i < result.Length; i++) {
                input.CopyBitArray(i * BitsInByte, tempBitArray, 0, BitsInByte);
                tempBitArray.Revert().CopyTo(tempByte, 0);
                result[i] = tempByte[0];
            }
            return result;
        }
    }
}

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
            RoundKeys = new KeyGenerator(BitArrayExtension.ReadInBigEndian(key));
        }

        public byte[] Key { get; private set; }

        public byte[] Encrypt(byte[] message) {
            return ApplyCipher(message, Mode.Encrypt);
        }

        public byte[] Decrypt(byte[] encryptedMessage) {
            return ApplyCipher(encryptedMessage, Mode.Decrypt);
        }

        private byte[] ApplyCipher(byte[] message, Mode mode) {
            CheckMessageLength(message);
            List<byte[]> blocks = GetBlocks(message);
            byte[] result = new byte[message.Length];
            for (int blockIndex = 0; blockIndex < blocks.Count; blockIndex++) {
                int blockFirstByteIndex = blockIndex * BlockLengthBytes;
                byte[] encryptedBlock = ApplyCipherToBlock(blocks[blockIndex], mode);
                encryptedBlock.CopyTo(result, blockFirstByteIndex);
            }
            return result;
        }
        
        private void CheckMessageLength(byte[] message) {
            if (message.Length % BlockLengthBytes != 0) {
                string errorMessage = "The lenght of message should be multiple of {0} in bytes. Now the lenght is {1} bytes.";
                throw new ArgumentException(string.Format(errorMessage, BlockLengthBytes, message.Length));
            }
        }

        private List<byte[]> GetBlocks(byte[] message) {
            List<byte[]> blocks = new List<byte[]>();
            int blockCount = message.Length / BlockLengthBytes;
            for (int i = 0; i < blockCount; i++) {
                byte[] block = new byte[BlockLengthBytes];
                int blockStartIndex = i * BlockLengthBytes;
                Array.Copy(message, blockStartIndex, block, 0, BlockLengthBytes);
                blocks.Add(block);
            }
            return blocks;
        }

        private byte[] ApplyCipherToBlock(byte[] block, Mode mode) {
            BitArray result = ExecuteDesRounds(block, mode);
            return result.SaveInBigEndian();
        }

        private BitArray ExecuteDesRounds(byte[] block, Mode mode) {
            BitArray rigthPart;
            BitArray leftPart;
            BitArray inputBlock = BitArrayExtension.ReadInBigEndian(block);
            BitArray permutatedBlock = DesHelper.InitialPermutator.Permut(inputBlock);

            GetBlockParts(permutatedBlock, out leftPart, out rigthPart);
            for (int round = 0; round < KeyGenerator.NumberOfRounds; round++) {
                BitArray desFunction = DesHelper.Function(rigthPart, GetRoundKey(round, mode));
                BitArray newRightPart = leftPart.Xor(desFunction);
                leftPart = rigthPart;
                rigthPart = newRightPart;
            }
            BitArray result = new BitArray(leftPart.Length + rigthPart.Length);
            rigthPart.CopyBitArray(0, result, 0, BlockPartLengthBits);
            leftPart.CopyBitArray(0, result, BlockPartLengthBits, BlockPartLengthBits);            
            return DesHelper.FinalPermutator.Permut(result);
        }

        private void GetBlockParts(BitArray block, out BitArray leftPart, out BitArray rightPart) {
            leftPart = new BitArray(BlockPartLengthBits);
            rightPart = new BitArray(BlockPartLengthBits);
            block.CopyBitArray(0, leftPart, 0, BlockPartLengthBits);
            block.CopyBitArray(BlockPartLengthBits, rightPart, 0, BlockPartLengthBits);
        }

        private BitArray GetRoundKey(int roundNumber, Mode mode) {
            int roundKeyIndex = mode == Mode.Decrypt ? KeyGenerator.NumberOfRounds - (roundNumber + 1) : roundNumber;
            return RoundKeys.GetRoundKey(roundKeyIndex);
        }       

        private enum Mode {
            Encrypt = 0,
            Decrypt = 1
        }
    }
}

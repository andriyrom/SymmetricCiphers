using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymetricCiphers.DES {
    public class DesCipher : ICipher {
        private const int BlockLength = 8;
        public DesCipher(byte[] key) {
            Key = key;
        }

        public byte[] Key { get; private set; }
        public byte[] Encrypt(byte[] message) {
            message = AlignMessage(message);
            byte[] result = new byte[message.Length];
            int blockCount = message.Length / BlockLength;
            for (int i = 0; i < blockCount; i++) {
                byte[] block = new byte[BlockLength];
                int blockStartIndex = i * BlockLength;
                Array.Copy(message, blockStartIndex, block, 0, BlockLength);
                byte[] encryptedBlock = EncryptBlock(block);
                encryptedBlock.CopyTo(result, blockStartIndex);
            }
            return result;
        }

        private byte[] EncryptBlock(byte[] block) {
            throw new NotImplementedException();
        }

        public byte[] Decrypt(byte[] encryptedMessage) {
            throw new NotImplementedException();
        }

        private byte[] AlignMessage(byte[] message) {
            int tail = message.Length % BlockLength;
            if (tail == 0) { return message; }
            int alignedMessageLength = (message.Length / BlockLength + 1) * BlockLength;
            byte[] alignedMessage = new byte[alignedMessageLength];
            alignedMessage.Initialize();
            message.CopyTo(alignedMessage, 0);
            return alignedMessage;
        }
    }
}

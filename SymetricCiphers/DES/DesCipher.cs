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
            throw new NotImplementedException();
        }
                
        public byte[] Decrypt(byte[] encryptedMessage) {
            throw new NotImplementedException();
        }
    }
}

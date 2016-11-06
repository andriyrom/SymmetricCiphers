using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymetricCiphers {
    interface ICipher {
        byte[] Key { get; }

        byte[] Encrypt(byte[] message);

        byte[] Decrypt(byte[] encryptedMessage);
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KeePassLike.Lib.Utilities
{
    public class CryptoService
    {
        private Aes aes;

        public CryptoService(string password)
        {
            aes = Aes.Create();

            var key = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(password), new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 }, 1000);
            aes.Key = key.GetBytes(aes.KeySize / 8);
            aes.IV = key.GetBytes(aes.BlockSize / 8);
        }

        public string Encrypt(string plain)
        {
            ICryptoTransform encryptor = aes.CreateEncryptor();

            using (MemoryStream msEncrypt = new MemoryStream())
            using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            {
                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(plain);
                }

                return Convert.ToBase64String(msEncrypt.ToArray());
            }
        }

        public string Decrypt(string cypher)
        {
            ICryptoTransform encryptor = aes.CreateDecryptor();

            using (MemoryStream msEncrypt = new MemoryStream(Convert.FromBase64String(cypher)))
            using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Read))
            using (StreamReader swEncrypt = new StreamReader(csEncrypt))
            {
                return swEncrypt.ReadToEnd();
            }
        }

        public CryptoStream EncryptStream(Stream outputStream)
        {
            ICryptoTransform encryptor = aes.CreateEncryptor();

            return new CryptoStream(outputStream, encryptor, CryptoStreamMode.Write);
        }


        public CryptoStream DecryptStream(Stream inputStream)
        {
            ICryptoTransform decryptor = aes.CreateDecryptor();

            return new CryptoStream(inputStream, decryptor, CryptoStreamMode.Read);
        }
    }
}

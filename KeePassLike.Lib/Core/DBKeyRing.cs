using KeePassLike.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace KeePassLike.Lib.Core
{
    public class DBKeyRing
    {
        private CryptoService crypto;

        public DBKeyRing(string dbPassword)
        {
            crypto = new CryptoService(dbPassword);
        }

        public KeyRing MasterKeyRing { get; private set; } = new KeyRing("My KeyRing");

        public void RemoveKeyRing(KeyRing keyRing)
        {
            RemoveKeyRing(MasterKeyRing, keyRing);
        }

        public string Encrypt(string password)
        {
            return crypto.Encrypt(password);
        }

        public string Decrypt(string cypher)
        {
            return crypto.Decrypt(cypher);
        }

        private bool RemoveKeyRing(KeyRing keyRing, KeyRing keyRingToRemove)
        {
            if (keyRing.SubKeyRing.Contains(keyRingToRemove))
            {
                keyRing.SubKeyRing.Remove(keyRingToRemove);
                return true;
            }
            else
            {
                foreach (var subKeyRing in keyRing.SubKeyRing)
                {
                    if (RemoveKeyRing(subKeyRing, keyRingToRemove))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void Load(string filePath)
        {
            if (File.Exists(filePath))
            {
                Deserialize(filePath);
            }
        }

        public void Save(string filePath)
        {
            Serialize(filePath);
        }

        private void Serialize(string filePath)
        {
            using (var fileStream = File.OpenWrite(filePath))
            using (var cryptoStream = crypto.EncryptStream(fileStream))
            using (var writer = new StreamWriter(cryptoStream))
            {
                var serializer = Newtonsoft.Json.JsonSerializer.Create();
                serializer.Serialize(writer, MasterKeyRing);
            }
        }

        private void Deserialize(string filePath)
        {
            using (var fileStream = File.OpenRead(filePath))
            using (var cryptoStream = crypto.DecryptStream(fileStream))
            using (var reader = new StreamReader(cryptoStream))
            {
                var serializer = Newtonsoft.Json.JsonSerializer.Create();
                MasterKeyRing = (KeyRing)serializer.Deserialize(reader, typeof(KeyRing));
            }
        }
    }
}

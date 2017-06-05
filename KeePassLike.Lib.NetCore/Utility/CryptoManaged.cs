using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KeePassLib.Utility
{
    public class SHA256Managed
    {
        public byte[] ComputeHash(byte[] clear)
        {
            using (var sha256iser = SHA256.Create())
            {
                return sha256iser.ComputeHash(clear);
            }
        }

        public byte[] ComputeHash(byte[] clear, int offset, int count)
        {
            using (var sha256iser = SHA256.Create())
            {
                return sha256iser.ComputeHash(clear, offset, count);
            }
        }
    }

    public class SHA256Managed
    {
        public byte[] ComputeHash(byte[] clear)
        {
            using (var sha256iser = SHA256.Create())
            {
                return sha256iser.ComputeHash(clear);
            }
        }

        public byte[] ComputeHash(byte[] clear, int offset, int count)
        {
            using (var sha256iser = SHA256.Create())
            {
                return sha256iser.ComputeHash(clear, offset, count);
            }
        }
    }
}

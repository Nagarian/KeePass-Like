using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KeePassLike.Lib.NetCore
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class Class1
    {
        public string GetSha256Hash(string clear)
        {
            using (var sha256iser = SHA256.Create())
            {
                byte[] cypher = sha256iser.ComputeHash(Encoding.UTF8.GetBytes(clear));
                return Convert.ToBase64String(cypher);
            }
        }
    }
}

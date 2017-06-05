using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace KeePassLib.Utility
{
    public static class Extensions
    {
        public static void Close(this IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}

using KeePassLike.Lib.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeePassLike
{
    class Setting
    {
        private static Setting instance = null;
        private static readonly object myLock = new object();

        private Setting()
        {
        }

        public static Setting Current
        {
            get
            {
                lock (myLock)
                {
                    if (instance == null) instance = new Setting();
                    return instance;
                }
            }
        }

        public DBKeyRing DB { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TWatchSKDesigner.Helpers
{
    public static class UnixHelper
    {
        [DllImport("libc", SetLastError = true)]
        private static extern int chmod(string pathname, int mode);

        // user permissions
        public const int S_IRUSR = 0x100;
        public const int S_IWUSR = 0x80;
        public const int S_IXUSR = 0x40;

        // group permission
        public const int S_IRGRP = 0x20;
        public const int S_IWGRP = 0x10;
        public const int S_IXGRP = 0x8;

        // other permissions
        public const int S_IROTH = 0x4;
        public const int S_IWOTH = 0x2;
        public const int S_IXOTH = 0x1;
        
        //User read write execute
        public const int S_User_RWX = S_IRUSR | S_IWUSR | S_IXUSR;
        //User read execute
        public const int S_User_RX = S_IRUSR | S_IXUSR;

        public static bool SetFilePermissions(string path, int permissions)
        {
            var result = chmod(path, permissions);

            return result == 0;
        }
    }
}

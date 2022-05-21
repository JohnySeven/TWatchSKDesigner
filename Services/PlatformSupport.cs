using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TWatchSKDesigner.Intefaces;

namespace TWatchSKDesigner.Services
{
    public class PlatformSupport : IPlatformSupport
    {
        public Task LaunchBrowser(Uri url)
        {
            return Task.Run(() => {

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    Process.Start(new ProcessStartInfo(url.ToString()) { UseShellExecute = true }); // Works ok on windows
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url.ToString());  // Works ok on linux
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url.ToString()); // Not tested
                }
                else
                {
                    throw new NotSupportedException("Unsupported platform: " + RuntimeInformation.RuntimeIdentifier);
                }
            });
        }

    }
}

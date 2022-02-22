using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TWatchSKDesigner.Models
{
    public class FirmwareVersionInfo
    {
        public string Version { get; set; }
        public string Arguments { get; set; }

        public FirmwareFileInfo[] Files { get; set; }
    }

    public class FirmwareFileInfo
    {
        public string Address { get; set; }
        public string File { get; set; }
    }

    public class FirmwareList
    {
        public string Version { get; set; }
        public FirmwareLink[] Links { get; set; }
    }

    public class FirmwareLink
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Hardware { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}

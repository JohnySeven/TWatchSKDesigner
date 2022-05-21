using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TWatchSKDesigner.Models
{
    public class UpdateInfo
    {
        public string Version { get; set; }
        public bool IsNewVersion { get; set; }
        public string ReleaseNotes { get; set; }
        public Uri ShowUri { get; set; }
    }
}

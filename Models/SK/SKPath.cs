using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TWatchSKDesigner.Models.SK
{
    public class SKPath
    {
        public SKPath(string path)
        {
            Path = path;
        }
        public string Path { get; set; }
        public string? Source { get; set; }
        public string? Value { get; set; }
        public string? Units { get; set; }
        public string? Description { get; set; }

        public override string ToString()
        {
            return $"{Path}, Source={Source}, Units={Units}, Description={Description}";
        }
    }
}

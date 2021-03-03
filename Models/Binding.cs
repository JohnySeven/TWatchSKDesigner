using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TWatchSKDesigner.Models
{
    public class Binding
    {
        [JsonProperty("path")]
        public string? Path { get; set; }

        [JsonProperty("multiply")]
        public float? Multiply { get; set; }

        [JsonProperty("period")]
        public int Period { get; set; }
    }
}

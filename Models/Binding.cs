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

        [JsonProperty("offset")]
        public float? OffSet { get; set; }

        [JsonProperty("decimals")]
        public int Decimals { get; set; }

        [JsonProperty("period")]
        public int Period { get; set; }

        public Binding Copy()
        {
            return new Binding()
            {
                Path = Path,
                Multiply = Multiply,
                Period = Period,
                OffSet = OffSet
            };
        }
    }
}

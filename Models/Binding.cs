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
        public string Path { get; set; }

        [JsonProperty("multiply")]
        public float? Multiply { get; set; }

        [JsonProperty("offset")]
        public float? OffSet { get; set; }

        [JsonProperty("decimals")]
        public int Decimals { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("period")]
        public int Period { get; set; }

        //Note if you add anything here, update copy method!

        public Binding Copy()
        {
            return new Binding()
            {
                Path = Path,
                Multiply = Multiply,
                Period = Period,
                OffSet = OffSet,
                Format = Format,
                Decimals = Decimals
            };
        }

        public override string ToString()
        {
            return $"Binding {Path},{Multiply},{Period},{OffSet},{Format},{Decimals}";
        }
    }
}

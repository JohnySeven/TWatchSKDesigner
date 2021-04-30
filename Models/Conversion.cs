using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TWatchSKDesigner.Models
{
    public class Conversion
    {
        public string From { get; set; }
        public string To { get; set; }
        public float OffSet { get; set; } = 0.0f;
        public float Multiply { get; set; } = 1.0f;
    }
}

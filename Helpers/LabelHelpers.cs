using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TWatchSKDesigner.Models;

namespace TWatchSKDesigner.Helpers
{
    public class LabelHelpers
    {
        internal static LabelDef Load(JToken component)
        {
            return JsonConvert.DeserializeObject<LabelDef>(component.ToString());
        }
    }
}

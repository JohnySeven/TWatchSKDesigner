using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TWatchSKDesigner.ViewModels
{
    public class WatchDynamicUI
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("views")]
        public List<WatchView> Views { get; set; }
    }
}

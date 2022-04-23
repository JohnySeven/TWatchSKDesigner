using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TWatchSKDesigner.ViewModels;

namespace TWatchSKDesigner.Models
{
    public class PutRequest : ViewModelBase
    {
        private string _Path;

        [JsonProperty("path")]
        public string Path
        {
            get { return _Path; }
            set { _Path = value; OnPropertyChanged(nameof(Path)); }
        }

        
        private JToken _Value;

        [JsonProperty("value")]
        public JToken Value
        {
            get { return _Value; }
            set { _Value = value; OnPropertyChanged(nameof(Value)); }
        }

        internal PutRequest Copy()
        {
            return new PutRequest()
            {
                Path = Path,
                Value = Value
            };
        }
    }
}

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TWatchSKDesigner.Models;

namespace TWatchSKDesigner.ViewModels
{
    public class PutEditorModalViewModel : ViewModelBase
    {
        public PutRequest Put { get; internal set; }

        public void Load()
        {
            var value = Put.Value;

            if(value != null)
            {
                if(value.Type == JTokenType.String)
                {
                    IsStringValue = true;
                    StringValue = value.Value<string>();
                }
                else if(value.Type == JTokenType.Float)
                {
                    IsFloatValue = true;
                    FloatValue = value.Value<float>();
                }
                else if(value.Type == JTokenType.Boolean)
                {
                    IsBooleanValue = true;
                    BooleanValue = value.Value<bool>();
                }
            }
        }

        private bool _IsStringValue;

        public bool IsStringValue
        {
            get { return _IsStringValue; }
            set { _IsStringValue = value; OnPropertyChanged(nameof(IsStringValue)); }
        }

        private string _StringValue;

        public string StringValue
        {
            get { return _StringValue; }
            set { _StringValue = value; OnPropertyChanged(nameof(StringValue)); }
        }

        private float _FloatValue;

        public float FloatValue
        {
            get { return _FloatValue; }
            set { _FloatValue = value; OnPropertyChanged(nameof(FloatValue)); }
        }


        private bool _IsFloatValue;

        public bool IsFloatValue
        {
            get { return _IsFloatValue; }
            set { _IsFloatValue = value; OnPropertyChanged(nameof(IsFloatValue)); }
        }

        private bool _BooleanValue;

        public bool BooleanValue
        {
            get { return _BooleanValue; }
            set { _BooleanValue = value; OnPropertyChanged(nameof(BooleanValue)); }
        }


        private bool _IsBooleanValue;

        public bool IsBooleanValue
        {
            get { return _IsBooleanValue; }
            set { _IsBooleanValue = value; OnPropertyChanged(nameof(IsBooleanValue)); }
        }

        protected override void OnPropertyChanged(string name)
        {
            base.OnPropertyChanged(name);

            if (name.EndsWith("Value"))
            {
                if (_IsStringValue && StringValue != null)
                {
                    Put.Value = JToken.FromObject(StringValue);
                }
                else if (_IsBooleanValue)
                {
                    Put.Value = JToken.FromObject(BooleanValue);
                }
                else if (_IsFloatValue)
                {
                    Put.Value = JToken.FromObject(FloatValue);
                }
            }
        }
    }
}

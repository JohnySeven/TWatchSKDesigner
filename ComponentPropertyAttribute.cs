using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TWatchSKDesigner.Enums;

namespace TWatchSKDesigner
{
    
    [AttributeUsage(AttributeTargets.Property)]
    public class ComponentPropertyAttribute : Attribute
    {
        public ComponentPropertyAttribute(Type editorType, ViewLayout visibleOnLayout = ViewLayout.none, bool updateLayoutOnChange = false, string propertyDescription = null)
        {
            EditorType = editorType;
            VisibleOnLayout = visibleOnLayout;
            UpdateLayoutOnChange = updateLayoutOnChange;
            PropertyDescription = propertyDescription;
        }

        public Type EditorType { get; set; }

        public ViewLayout VisibleOnLayout { get; set; }

        public bool UpdateLayoutOnChange { get; set; }

        public string PropertyDescription { get; set; }
    }
}

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
        public ComponentPropertyAttribute(Type editorType, ViewLayout visibleOnLayout = ViewLayout.none, bool updateLayoutOnChange = false)
        {
            EditorType = editorType;
            VisibleOnLayout = visibleOnLayout;
            UpdateLayoutOnChange = updateLayoutOnChange;
        }

        public Type EditorType { get; set; }

        public ViewLayout VisibleOnLayout { get; set; }

        public bool UpdateLayoutOnChange { get; set; }
    }
}

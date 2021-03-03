using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TWatchSKDesigner
{
    
    [AttributeUsage(AttributeTargets.Property)]
    public class ComponentPropertyAttribute : Attribute
    {
        public ComponentPropertyAttribute(Type editorType)
        {
            EditorType = editorType;
        }

        public Type EditorType { get; set; }
    }
}

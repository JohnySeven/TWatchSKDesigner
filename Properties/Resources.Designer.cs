﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TWatchSKDesigner.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("TWatchSKDesigner.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to { //when float number is updated watch is using formula: (number + offset) * multiply
        ///  &quot;m/s&quot;: {
        ///    &quot;kn&quot;: {
        ///      &quot;multiply&quot;: 1.944
        ///    },
        ///    &quot;km/h&quot;: {
        ///      &quot;multiply&quot;: 3.6
        ///    }
        ///  },
        ///  &quot;m3&quot;: {
        ///    &quot;liter&quot;: {
        ///      &quot;multiply&quot;: 1000
        ///    },
        ///    &quot;gallon&quot;: {
        ///      &quot;multiply&quot;: 264.172
        ///    },
        ///    &quot;gallon (imp)&quot;: {
        ///      &quot;multiply&quot;: 219.969
        ///    }
        ///  },
        ///  &quot;m3/s&quot;: {
        ///    &quot;l/min&quot;: {
        ///      &quot;multiply&quot;: 60000.0
        ///    },
        ///    &quot;l/h&quot;: {
        ///      &quot;multiply&quot;: 3600000.0
        ///    },
        ///    &quot;g/min&quot;: {
        ///      &quot;mult [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Conversions {
            get {
                return ResourceManager.GetString("Conversions", resourceCulture);
            }
        }
    }
}
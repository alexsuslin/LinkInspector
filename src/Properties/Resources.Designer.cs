﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LinkInspector.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("LinkInspector.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to Returns the current version of the application..
        /// </summary>
        internal static string GetHelp_GetHelp_CommandDescription {
            get {
                return ResourceManager.GetString("GetHelp_GetHelp_CommandDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Usage: LinkInspector.exe -u &quot;&lt;URL&gt;&quot; [options].
        /// </summary>
        internal static string GetHelp_Run_Usage {
            get {
                return ResourceManager.GetString("GetHelp_Run_Usage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Can&apos;t create Uri from the string specified: &apos;{0}&apos;.
        /// </summary>
        internal static string ParseUrl_Run_CantCreateUriError {
            get {
                return ResourceManager.GetString("ParseUrl_Run_CantCreateUriError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Input argument &apos;{0}&apos; is not an integer value and must be greater than 0..
        /// </summary>
        internal static string ParseUrl_Run_NotIntegerError {
            get {
                return ResourceManager.GetString("ParseUrl_Run_NotIntegerError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unsupported file format {0}.
        /// </summary>
        internal static string ParseUrl_Run_UnsupportedFormatError {
            get {
                return ResourceManager.GetString("ParseUrl_Run_UnsupportedFormatError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} file could not be read:.
        /// </summary>
        internal static string Report_GetFileContent_CantReadError {
            get {
                return ResourceManager.GetString("Report_GetFileContent_CantReadError", resourceCulture);
            }
        }
    }
}
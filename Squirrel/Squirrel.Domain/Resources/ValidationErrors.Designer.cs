﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Squirrel.Domain.Resources {
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
    public class ValidationErrors {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ValidationErrors() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Squirrel.Domain.Resources.ValidationErrors", typeof(ValidationErrors).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} را به درستی وارد کنید..
        /// </summary>
        public static string General_RegularExperssion {
            get {
                return ResourceManager.GetString("General_RegularExperssion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} را وارد کنید..
        /// </summary>
        public static string General_Required {
            get {
                return ResourceManager.GetString("General_Required", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to حداکثر می تواند {1} کارکتر داشته باشد..
        /// </summary>
        public static string General_StringLength {
            get {
                return ResourceManager.GetString("General_StringLength", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to حداقل {2} و حداکثر {1} کارکتر قابل قبول است..
        /// </summary>
        public static string General_StringLengthBound {
            get {
                return ResourceManager.GetString("General_StringLengthBound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to پسورد با تکرار آن منطبق نیست..
        /// </summary>
        public static string UserCreateModel_ConfimPassword_Compare {
            get {
                return ResourceManager.GetString("UserCreateModel_ConfimPassword_Compare", resourceCulture);
            }
        }
    }
}

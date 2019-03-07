﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Microsoft.DataTransfer.ServiceModel {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ConfigurationResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ConfigurationResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Microsoft.DataTransfer.ServiceModel.ConfigurationResources", typeof(ConfigurationResources).Assembly);
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
        ///   Looks up a localized string similar to Optional, default is {0}. Specifies that detailed error information should be displayed for the following errors: {1}.
        /// </summary>
        public static string Errors_DetailsFormat {
            get {
                return ResourceManager.GetString("Errors_DetailsFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Optional, Connection string for cosmos tables for logging. If provided, it will use this endpoint info for remotely logging failure info..
        /// </summary>
        public static string Statistics_CosmosTableLogConnectionString {
            get {
                return ResourceManager.GetString("Statistics_CosmosTableLogConnectionString", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Optional, Enables logging to a remote Cosmos DB Table API account. Unless destination is cosmos tables api, this also needs &apos;CosmosTableLogConnectionString&apos; property..
        /// </summary>
        public static string Statistics_EnableCosmosTableLog {
            get {
                return ResourceManager.GetString("Statistics_EnableCosmosTableLog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Optional. Whether to save the checkpoint file and resume from there if the data transfer action is stopped for some reason.
        /// </summary>
        public static string Statistics_EnableResumeFunction {
            get {
                return ResourceManager.GetString("Statistics_EnableResumeFunction", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Optional. Name of the CSV file to redirect data transfer failures.
        /// </summary>
        public static string Statistics_ErrorLog {
            get {
                return ResourceManager.GetString("Statistics_ErrorLog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Optional. Overwrite error log file.
        /// </summary>
        public static string Statistics_OverwriteErrorLog {
            get {
                return ResourceManager.GetString("Statistics_OverwriteErrorLog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Optional, default is {0}. Time interval to refresh on-screen data transfer progress.
        /// </summary>
        public static string Statistics_ProgressUpdateIntervalFormat {
            get {
                return ResourceManager.GetString("Statistics_ProgressUpdateIntervalFormat", resourceCulture);
            }
        }
    }
}

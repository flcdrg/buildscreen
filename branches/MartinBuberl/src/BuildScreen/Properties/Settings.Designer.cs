﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BuildScreen.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("localhost")]
        public string OptionsSetup_Server_Domain {
            get {
                return ((string)(this["OptionsSetup_Server_Domain"]));
            }
            set {
                this["OptionsSetup_Server_Domain"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("8080")]
        public int OptionsSetup_Server_Port {
            get {
                return ((int)(this["OptionsSetup_Server_Port"]));
            }
            set {
                this["OptionsSetup_Server_Port"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string OptionsSetup_UserCredentials_UserName {
            get {
                return ((string)(this["OptionsSetup_UserCredentials_UserName"]));
            }
            set {
                this["OptionsSetup_UserCredentials_UserName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string OptionsSetup_UserCredentials_Password {
            get {
                return ((string)(this["OptionsSetup_UserCredentials_Password"]));
            }
            set {
                this["OptionsSetup_UserCredentials_Password"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool OptionsSetup_Server_SecureSocketLayer {
            get {
                return ((bool)(this["OptionsSetup_Server_SecureSocketLayer"]));
            }
            set {
                this["OptionsSetup_Server_SecureSocketLayer"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool OptionsSetup_Server_IgnoreInvalidCertificate {
            get {
                return ((bool)(this["OptionsSetup_Server_IgnoreInvalidCertificate"]));
            }
            set {
                this["OptionsSetup_Server_IgnoreInvalidCertificate"] = value;
            }
        }
    }
}
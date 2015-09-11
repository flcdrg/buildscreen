using System;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace BuildScreen.Plugin.Configuration
{
    public class PluginElement
    {
        [XmlAttribute(AttributeName = "name")]
        public virtual string Name { get; set; }

        [XmlAttribute(AttributeName = "type")]
        public string TypeStr { get; set; }

        [XmlIgnore]
        public Type PluginType
        {
            get
            {
                return GetTypeFromString();
            }
        }

        private Type GetTypeFromString()
        {
            Assembly assembly =
                Assembly.LoadFrom(
                    Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Plugin", Name));

            Type pluginType = assembly.GetType(TypeStr);

            if (pluginType == null)
                throw new PluginTypeInitializationException(
                    String.Format("'{0}' could not be found. Check that the type has been typed correctly and the type really exists.",
                        TypeStr));

            if (pluginType.GetInterface(typeof(IBuildScreenPlugin).FullName) == null)
                throw new PluginNotSupportedException("Only plugins that implement IBuildScreenPlugin can be added under the plugin section.");

            return pluginType;
        }
    }

    [Serializable]
    internal class PluginNotSupportedException : Exception
    {
        public PluginNotSupportedException(string message) : base(message)
        {
           
        }
    }

    [Serializable]
    internal class PluginTypeInitializationException : Exception
    {
        public PluginTypeInitializationException(string message) : base(message)
        {

        }
    }
}

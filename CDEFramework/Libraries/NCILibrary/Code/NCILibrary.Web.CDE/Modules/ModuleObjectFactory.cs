using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using Common.Logging;

namespace NCI.Web.CDE.Modules
{
    public class ModuleObjectFactory<ModuleObjectType>
    {
        static ILog log = LogManager.GetLogger(typeof(ModuleObjectFactory<ModuleObjectType>));

        private static System.Collections.Generic.Dictionary<string,XmlSerializer> serializers = new Dictionary<string,XmlSerializer>();

        public static ModuleObjectType GetModuleObject(string snippetXmlData)
        {
            try
            {
                using (XmlTextReader reader = new XmlTextReader(snippetXmlData.Trim(), XmlNodeType.Element, null))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(ModuleObjectType), "cde");
                    return (ModuleObjectType)serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                log.Error("GetModuleObject(): Invalid xml data in the snippet for " + typeof(ModuleObjectType).ToString() + " , check xml received from Percussion");
                throw ex;
            }
        }

        public static ModuleObjectType GetObjectFromFile(string filePath)
        {
            try
            {
                XmlSerializer serializer = null;
                string typeName = typeof(ModuleObjectType).ToString().ToLower();

                if(serializers.ContainsKey(typeName))
                    serializer = serializers[typeName];
                else
                {
                    serializer = new XmlSerializer(typeof(ModuleObjectType), "cde");
                    serializers.Add(typeName, serializer);
                }

                // Make an absolute path.
                filePath = HttpContext.Current.Server.MapPath(filePath);

                using (FileStream xmlFile = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
                {
                    using (XmlReader xmlReader = XmlReader.Create(xmlFile))
                    {
                        return (ModuleObjectType)serializer.Deserialize(xmlReader);
                    }
                }
            }
            catch (Exception ex)
            {
                string message = String.Format("GetObjectFromFile(): Unable to load object from file \"{0}.\"  The file may not exist or the XML in the file may not be deserializable into a valid object.", filePath);
                log.Error(message, ex);
                throw ex;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using NCI.Logging;

namespace NCI.Web.CDE.Modules
{
    public class ModuleObjectFactory<ModuleObjectType>
    {
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
                Logger.LogError("cde:ModuleObjectFactory.cs.GetModuleObject", "Invalid xml data in the snippet for DynamicList, check xml received from Percussion", NCIErrorLevel.Error);
                throw ex;
            }
        }
    }
}

using System;
using System.IO;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using Common.Logging;
using NCI.Web.CDE.Configuration;

namespace NCI.Web.CDE
{
    public static class PromoUrlMappingInfoFactory
    {
        static ILog log = LogManager.GetLogger(typeof(PromoUrlMappingInfoFactory));

        // XmlSerializer to deserialization PromoUrlMapping
        private static XmlSerializer _serializer = new XmlSerializer(typeof(PromoUrlMapping));

        /// <summary>
        /// Gets an instance of the HttpServerUtility for the current request.
        /// </summary>
        private static HttpServerUtility Server
        {
            get { return HttpContext.Current.Server; }
        }

        /// <summary>
        /// Gets a single PromoUrlMapping without any parents.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static PromoUrlMapping GetPromoUrlMapping(string path, ref FileInfo fileInfo )
        {
            PromoUrlMapping promoUrlMapping = null;
            string xmlFileName = null;

            try
            {
                xmlFileName = String.Format(ContentDeliveryEngineConfig.PathInformation.PromoUrlMappingPath.Path, (path == "/" ? String.Empty : path));
                xmlFileName = Server.MapPath(xmlFileName);
                FileInfo promoUrlFileInfo = new FileInfo(xmlFileName);
                if (!promoUrlFileInfo.Exists)
                {
                    log.Warn("GetPromoUrlMapping(): PromoUrl Mapping file does not exist");
                    return null;
                }
                else
                    fileInfo = promoUrlFileInfo;

                using (FileStream xmlFile = File.Open(xmlFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
                {
                    XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
                    xmlReaderSettings.IgnoreWhitespace = true;
                    using (XmlReader xmlReader = XmlReader.Create(xmlFile, xmlReaderSettings))
                    {
                        promoUrlMapping = (PromoUrlMapping)_serializer.Deserialize(xmlReader);
                    }
                }
            }
            catch (Exception ex)
            {
                string message = String.Format("GetPromoUrlMapping(): Unable to load section Promo Url mapping from file \"{0}.\"  The file may not exist or the XML in the file may not be serializable into a valid promoUrlMapping object.", xmlFileName);
                log.Error(message, ex);
                return null;
            }
            return promoUrlMapping;
        }

    }
}

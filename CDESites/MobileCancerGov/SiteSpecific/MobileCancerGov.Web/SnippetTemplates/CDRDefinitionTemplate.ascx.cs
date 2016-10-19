using System;
using System.Configuration;
using CancerGov.CDR.TermDictionary;
using Common.Logging;
using NCI.Web.CDE;
using NCI.Web.CDE.Modules;
using NCI.Web.CDE.UI;

namespace MobileCancerGov.Web.SnippetTemplates
{
    public partial class CDRDefinitionTemplate : SnippetControl
    {
        static ILog log = LogManager.GetLogger(typeof(CDRDefinitionTemplate));

        protected void Page_Load(object sender, EventArgs e)
        {
            ltDefinitionText.Text = DefinitionText;
        }

        protected string DefinitionText
        {

            get 
            {
                string definitionText = string.Empty;
                string definitionMedia = string.Empty;
                string language = string.Empty;

                try
                {
                    string snippetXmlData = string.Empty;
                    if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "es")
                    {
                        language = "Spanish";
                        spDefinitionHeader.InnerText = "Definición";   
                    }
                    else
                    {
                        language = "English";
                        spDefinitionHeader.InnerText = "Definition"; 
                    }

                    
                    snippetXmlData = SnippetInfo.Data;
                    // The snippet CDATA may contain CDATA as part of the data but percussion replaces the CDATA 
                    // close tag with Replace ']]>' with ']]ENDCDATA' this ']]ENDCDATA' should be replaced with 
                    // valid CDATA close tag ']]>' before it can be deserialized
                    snippetXmlData = snippetXmlData.Replace("]]ENDCDATA", "]]>");
                    CDRDefinition mPBO = ModuleObjectFactory<CDRDefinition>.GetModuleObject(snippetXmlData);

                    TermDictionaryDataItem dataItem = TermDictionaryManager.GetDefinitionByTermID(language, mPBO.CDRId, null, 5);
                    definitionText = "<p>" + dataItem.DefinitionHTML + "</p>";
                    //Make sure a Media Image exists.
                    if(dataItem.MediaID != 0)
                    {
                        definitionMedia = "<div class=\"thumb\">" +
                            "<a href=\"" + ConfigurationManager.AppSettings["CDRImageLocation"] + "CDR" + dataItem.MediaID + "-750.jpg\" class=\"image ui-link\"><img class=\"thumbimage\" src=\"" + ConfigurationManager.AppSettings["CDRImageLocation"] + "CDR" + dataItem.MediaID + "-274.jpg\"></a>" +
                            "</div>";
                        definitionMedia = definitionMedia + "<div class=\"caption\">" + dataItem.MediaCaption + "</div>";
                    }
                    
                   
                }
                catch(Exception ex)
                {
                    log.Info("DefinitionText(): Could not load the definition.Requires <CDRId></CDRId> in the CDRDefinitionTemplate.ascx module", ex);
                }
                return definitionText + definitionMedia;
            }

        }
              
    }
}
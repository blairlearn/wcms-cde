using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using NCI.Web.UI.WebControls;
using NCI.Web.CDE.UI.Configuration;
using NCI.Web.CDE;
using NCI.Web.CDE.UI;
using CancerGov.CDR.TermDictionary;
using NCI.Web.CDE.Modules;
using NCI.Logging;

namespace MobileCancerGov.Web.SnippetTemplates
{
    public partial class CDRDefinitionTemplate : SnippetControl
    {
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
                    if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "en")
                    {
                        language = "English";
                        
                    }
                    else
                    {
                        language = "Spanish";

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
                            "<a href=\"" + ConfigurationSettings.AppSettings["CDRImageLocation"] + "CDR" + dataItem.MediaID + "-750.jpg\" class=\"image ui-link\"><img class=\"thumbimage\" src=\"" + ConfigurationSettings.AppSettings["CDRImageLocation"] + "CDR" + dataItem.MediaID + "-274.jpg\"></a>" +
                            "</div>";
                        definitionMedia = definitionMedia + "<div class=\"caption\">" + dataItem.MediaCaption + "</div>";
                    }
                    
                   
                }
                catch(Exception ex)
                {
                    Logger.LogError("CDE:CDRDefinitionTemplate.ascx.cs:DefinitionText", "Could not load the definition.Requires <CDRId></CDRId> in the CDRDefinitionTemplate.ascx module", NCIErrorLevel.Info,ex);

                }
                return definitionText + definitionMedia;
            }

        }
              
    }
}
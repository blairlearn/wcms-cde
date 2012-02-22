using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NCI.Web.UI.WebControls;
using NCI.Web.CDE.UI.Configuration;
using NCI.Web.CDE;
using NCI.Web.CDE.UI;
using CancerGov.CDR.TermDictionary;
using NCI.Web.CDE.Modules;
using NCI.Logging;

namespace CancerGov.Web.SnippetTemplates
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

                try
                {
                    string language = string.Empty;
                    string snippetXmlData = string.Empty;
                    if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "en")
                    {
                        language = "English";
                        moreLink.InnerText = "More";

                    }
                    else
                    {
                        language = "Spanish";
                        moreLink.InnerHtml = "M&aacute;s";

                    }


                    snippetXmlData = SnippetInfo.Data;
                    // The snippet CDATA may contain CDATA as part of the data but percussion replaces the CDATA 
                    // close tag with Replace ']]>' with ']]ENDCDATA' this ']]ENDCDATA' should be replaced with 
                    // valid CDATA close tag ']]>' before it can be deserialized
                    snippetXmlData = snippetXmlData.Replace("]]ENDCDATA", "]]>");
                    CDRDefinition mPBO = ModuleObjectFactory<CDRDefinition>.GetModuleObject(snippetXmlData);

                    TermDictionaryDataItem dataItem = TermDictionaryManager.GetDefinitionByTermID(language, mPBO.CDRId, null, 5);
                    if (!String.IsNullOrEmpty(mPBO.CDRDefinitionName))
                    {
                        if (language == "English")
                        {

                            definitionText = "<strong>Definition of " + mPBO.CDRDefinitionName + "</strong>" + ": " + dataItem.DefinitionHTML;
                        }
                        else if(language == "Spanish")
                        {
                            definitionText = "<strong>Definición de " + mPBO.CDRDefinitionName + "</strong>" + ": " + dataItem.DefinitionHTML;                     
                        }
                            
                        if (mPBO.charLimit > 0)
                        {
                            moreLink.Visible = true;
                            spDefinitionTextMore.InnerHtml = definitionText;
                            definitionText = definitionText.Substring(0, mPBO.charLimit);

                        }
                    }
                    else
                    {
                        if (language == "English")
                        {

                            definitionText = "<strong>Definition of " + dataItem.TermName + "</strong>" + ": " + dataItem.DefinitionHTML;
                        }
                        else if (language == "Spanish")
                        {
                            definitionText = "<strong>Definición de " + dataItem.TermName + "</strong>" + ": " + dataItem.DefinitionHTML;
                        
                        }
                        if (mPBO.charLimit > 0)
                        {
                            moreLink.Visible = true;
                            spDefinitionTextMore.InnerHtml = definitionText;
                            definitionText = definitionText.Substring(0, mPBO.charLimit);

                        }                       

                    }

                   
                }
                catch(Exception ex)
                {
                    Logger.LogError("CDE:CDRDefinitionTemplate.ascx.cs:DefinitionText", "Could not load the definition.Requires <CDRId></CDRId> in the CDRDefinitionTemplate.ascx module", NCIErrorLevel.Info,ex);

                }
                return definitionText;
            }

        }
              
    }
}
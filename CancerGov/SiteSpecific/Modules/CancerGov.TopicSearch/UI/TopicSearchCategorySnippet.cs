using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NCI.Web.CDE.UI;
using NCI.Web.CDE;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using NCI.Web.CDE.PageAssembly;

namespace CancerGov.Modules.TopicSearch.UI
{
    /// <summary>
    /// This Snippet Template is for displaying page options based on Alternate content versions.
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:TopicSearchCategorySnippet runat=server></{0}:TopicSearchCategorySnippet>")]
    public class TopicSearchCategorySnippet : SnippetControl
    {
        protected RadioButtonList rblTopicSearchList;
        protected RadioButtonList rblTimeframeList;
        //protected string strTableTitle;
        public void Page_Load(object sender, EventArgs e)
        {
            processTopicSearch(SnippetInfo.Data);

        }
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
        }

        /// <summary>
        /// Process the topic search information in the xml and the radio button list.
        /// </summary>
        /// <param name="snippetXmlData">The xml fragment which contains topic search information.</param>
        private void processTopicSearch(string snippetXmlData)
        {
            // The snippet CDATA may contain CDATA as part of the data but percussion replaces the CDATA 
            // close tag with Replace ']]>' with ']]ENDCDATA' this ']]ENDCDATA' should be replaced with 
            // valid CDATA close tag ']]>' before it can be deserialized
            snippetXmlData = snippetXmlData.Replace("]]ENDCDATA", "]]>");
            
            TopicSearchCategory mTSC = null;
            // TODO: use the factory to create the Module_PageOptionsBox objects
            using(XmlTextReader reader = new XmlTextReader(snippetXmlData.Trim(), XmlNodeType.Element, null))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(TopicSearchCategory), "cde");
                mTSC = (TopicSearchCategory)serializer.Deserialize(reader);
            }

            if (mTSC == null)
            {
                //log.error
                return;
            }

            if (!Page.IsPostBack)
            {
                rblTopicSearchList.DataSource = (object)mTSC.TopicSearches;
                rblTopicSearchList.DataValueField = "TopicSearchID";
                rblTopicSearchList.DataTextField = "TopicSearchName";
                rblTopicSearchList.DataBind();
            }
            //strTableTitle += ;
            
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
        }
    }
}

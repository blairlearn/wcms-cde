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
        protected Label strCategoryName;

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
                rblTopicSearchList.DataValueField = "MeshQuery";
                rblTopicSearchList.DataTextField = "TopicSearchName";
                rblTopicSearchList.DataBind();
                strCategoryName.Text = mTSC.CategoryName;
                if (rblTopicSearchList.Items.Count != 0)
                {
                    rblTopicSearchList.SelectedIndex = 0;
                }
                rblTimeframeList.SelectedIndex = 0;
            }            
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (rblTimeframeList.SelectedIndex > -1 && rblTopicSearchList.SelectedIndex > -1)
            {
                string pubMedQuery = "(";

                if(rblTimeframeList.SelectedIndex == 0)
                {
                    //nothing
                }
                else if (rblTimeframeList.SelectedIndex == 1)
                {
                    pubMedQuery += "\"" + daysAgo(30) +
                        "\"[Entrez Date] : \"3000\"[Entrez Date]) AND (";
                }
                else if (rblTimeframeList.SelectedIndex == 2)
                {
                    pubMedQuery += "\"" + daysAgo(60) +
                        "\"[Entrez Date] : \"3000\"[Entrez Date]) AND (";
                }
                else if (rblTimeframeList.SelectedIndex == 3)
                {
                    pubMedQuery += "\"" + daysAgo(90) +
                        "\"[Entrez Date] : \"3000\"[Entrez Date]) AND (";
                }
                pubMedQuery += rblTopicSearchList.SelectedItem.Value + ")";
                string pubMedLink = "http://www.ncbi.nlm.nih.gov/pubmed?cmd=Search&report=DocSum&term=" + pubMedQuery;
                Page.Response.Redirect(pubMedLink, true);
            }  
        }

        private string daysAgo(int x)
        {
            // Get current date and time
            DateTime dt = DateTime.Now;
            dt = DateTime.Today.AddDays(-x);
            return String.Format("{0:yyyy/MM/dd}", dt);
        }
    }
}

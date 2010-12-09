using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Configuration;
using System.Web.UI.WebControls;
using NCI.Web.TCGA.Apps;
using System.Xml.Linq;
using NCI.Web.CDE;
using NCI.Web.UI.WebControls;
using NCI.Logging;

namespace TCGA.Web.SnippetTemplates
{
    public partial class ViewPublicationsByCancerType : AppsBaseUserControl
    {
        #region Define Controls 
        protected global::System.Web.UI.HtmlControls.HtmlForm frmViewPublications;
        protected global::System.Web.UI.WebControls.DropDownList ddlCancerType;
        protected global::System.Web.UI.WebControls.Repeater rptPublicationResults;
        protected global::NCI.Web.UI.WebControls.PostBackButtonPager pager;
        protected global::System.Web.UI.WebControls.HiddenField itemsPerPage;
        #endregion

        private XElement publicationRoot = null;
        private int totalResults = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    IEnumerable<XElement> pubResults = null;
                    // Bind the dropdownlist control to display the cancer types
                    pubResults = GetPublications("ALL");
                    PopulateCancerTypeDropDown(pubResults);

                    // Load the dropdown box, with default all selected when the page is first 
                    // displayed.
                    PopulatePublicationsResults(pubResults);

                    SetUpPagerInformation();

                }
                catch (Exception ex)
                {
                    Logger.LogError("ViewPublicationsByCancerType", NCIErrorLevel.Error, ex);
                }
            }
        }

        /// <summary>
        /// Binds the publications data to  the repeater control. 
        /// </summary>
        /// <param name="pubResults">The publication data that is used to bind to the repeater.</param>
        private void PopulatePublicationsResults(IEnumerable<XElement> pubResults)
        {
            try
            {
                pubResults = pubResults.Skip(pager.CurrentPage * ItemsPerPage - ItemsPerPage).Take(ItemsPerPage);

                var publications = from publication in pubResults
                                   select
                                       new
                                       {
                                           cancerType = publication.Element("CancerType").Value,
                                           publicationDate = publication.Element("PublicationDate").Value,
                                           isTCGANetworkType = getTCGAIdentifier(publication.Element("IsTCGANetworkType").Value),
                                           description = publication.Element("Description").Value,
                                           journalTitle = publication.Element("JournalTitle").Value,
                                           associatedLink = getAssociatedLink(publication.Element("LinkText").Value, publication.Element("LinkText").Value)
                                       };

                rptPublicationResults.DataSource = publications.ToList();
                rptPublicationResults.DataBind();
            }
            catch(Exception ex)
            {
                Logger.LogError("ViewPublicationsByCancerType", "could not Populate Publications Results", NCIErrorLevel.Error);
                throw ex;
            }

        }

        /// <summary>
        /// Returns the html element with correct href and text.
        /// </summary>
        /// <param name="linkText">The text for the link</param>
        /// <param name="link">The href of the link</param>
        /// <returns></returns>
        private string getAssociatedLink(string linkText, string link)
        {
            return string.IsNullOrEmpty(linkText) ? string.Empty
                : string.Format("<a href=\"{0}\">{1}</a>", linkText, link);
        }

        /// <summary>
        /// If the publication is a tcga network return a '*' character
        /// </summary>
        private string getTCGAIdentifier(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;
            return value.ToLower() == "true" ? "*" : string.Empty;
        }

        /// <summary>
        /// Populates the cancer type dropdown.
        /// </summary>
        /// <param name="pubResults">Dropdown values are populated from this object.</param>
        private void PopulateCancerTypeDropDown(IEnumerable<XElement> pubResults)
        {
            try
            {
                var publications =
                           (from publication in pubResults
                            group publication by (string)publication.Element("CancerType").Value into grp
                            where grp.Count() >= 1
                            orderby grp.First().Element("CancerType").Value
                            select new { cancerType = grp.First().Element("CancerType").Value });

                ddlCancerType.DataSource = publications.ToList();
                ddlCancerType.DataTextField = "cancerType";
                ddlCancerType.DataValueField = "cancerType";
                ddlCancerType.DataBind();

                // Add the all option.
                if (ddlCancerType.Items.Count > 0)
                    ddlCancerType.Items.Insert(0, new ListItem("All", "All"));
                else
                    ddlCancerType.Items.Add(new ListItem("All", "All"));

                if (ddlCancerType.SelectedIndex == -1)
                    ddlCancerType.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                Logger.LogError("ViewPublicationsByCancerType", "could not Populate CancerType DropDown", NCIErrorLevel.Error);
                throw ex;
            }
        }

        /// <summary>
        /// Returns the root XElement after invoking the XElement.Load
        /// </summary>
        private XElement PublicationDataXml
        {
            get
            {
                if (publicationRoot == null)
                {
                    if (string.IsNullOrEmpty(PublicationsDataPath))
                        throw new Exception("PublicationsDataPath is not provided, update the appsettings to include this key and provide the path value");
                    publicationRoot = XElement.Load(Server.MapPath(PublicationsDataPath));
                }
                return publicationRoot;
            }
        }

        /// <summary>
        /// Order the publication data based on the selection made from the cancerTypes dropped down.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private IEnumerable<XElement> GetPublications(string type)
        {
            try
            {
                IEnumerable<XElement> publications = null;

                switch (type.ToLower())
                {
                    case "all":
                        {
                            publications =
                                from publication in PublicationDataXml.Elements("Publication")
                                orderby makevalidDate(publication.Element("PublicationDate").Value) descending, publication.Element("Description").Value
                                select publication;
                            break;

                        }
                    default:
                        {
                            publications =
                                from publication in PublicationDataXml.Elements("Publication")
                                where
                                         (string)publication.Element("CancerType").Value == type
                                orderby makevalidDate(publication.Element("PublicationDate").Value) descending, publication.Element("Description").Value
                                select publication;
                            break;
                        }
                }

                totalResults = publications.Count();
                return publications;

            }
            catch (Exception ex)
            {
                Logger.LogError("ViewPublicationsByCancerType", "GetPublications failed", NCIErrorLevel.Error);
                throw ex;
            }
        }

        /// <summary>
        /// Returns a valid data object but if the data passed is invalid returns DateTime.MinValue.
        /// </summary>
        /// <param name="dateValue"></param>
        /// <returns></returns>
        private DateTime makevalidDate(string dateValue)
        {
            DateTime dt = DateTime.MinValue;
            try
            {
                dt = Convert.ToDateTime(dateValue);
            }
            catch
            { }

            return dt;
        }

        /// <summary>
        /// Returns the path where the publication data is stored. This value is configured in the web.config 
        /// </summary>
        private string PublicationsDataPath
        {
            get
            {
                return ConfigurationManager.AppSettings["publicationsDataPath"];
            }
        }

        /// <summary>
        /// The cancertype selected from the dropdown.
        /// </summary>
        private string CancerTypeSelected
        {
            get { return ddlCancerType.SelectedItem.Value; }
        }

        protected void ddlCancerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                IEnumerable<XElement> pubResults = null;
                pubResults = GetPublications(CancerTypeSelected);
                SetUpPagerInformation();
                PopulatePublicationsResults(pubResults);

            }
            catch (Exception ex)
            {
                Logger.LogError("ViewPublicationsByCancerType", NCIErrorLevel.Error, ex);
            }
        }

        #region Pager

        /// <summary>
        /// Number of items that will be displayed per page.
        /// </summary>
        private int ItemsPerPage
        {
            get
            {
                if (itemsPerPage == null)
                    return 10;
                return Int32.Parse(itemsPerPage.Value);
            }
        }

        /// <summary>
        /// Sets up the initial pager information.
        /// </summary>
        private void SetUpPagerInformation()
        {
            pager.RecordCount = totalResults;
            pager.RecordsPerPage = ItemsPerPage;
            pager.CurrentPage = 1;
        }


        protected void pager_PageChanged(object sender, EventArgs e)
        {
            try
            {
                IEnumerable<XElement> pubResults = null;
                pubResults = GetPublications(CancerTypeSelected);
                PopulatePublicationsResults(pubResults);

            }
            catch (Exception ex)
            {
                Logger.LogError("ViewPublicationsByCancerType", NCIErrorLevel.Error, ex);
            }
        }

        #endregion
    }
}
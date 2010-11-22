using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using NCI.Web.CancerGov.Apps;
using CancerGovCommon.Modules;
using NCI.Web.CDE.Modules;

namespace CancerGov.Web.SnippetTemplates.CancerBulletin
{
    public partial class CancerBulletinUnSubscribe : AppsBaseUserControl
    {

        protected string strInfo = "";
        protected string strHeader = "";
        protected string strMessageBody = "";
        protected string strTextClass = "";
        protected string strCommentsUrl = String.Empty;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            tblFeedback.Visible = false;

            string strUserID = "";
            string strNewsletterID = "";
            string strConnString = NewsLetterDBConnection;

            System.Data.SqlClient.SqlConnection scnEmail = new System.Data.SqlClient.SqlConnection(strConnString);
            System.Data.SqlClient.SqlCommand scEmail = new System.Data.SqlClient.SqlCommand();
            scEmail.Connection = scnEmail;

            strUserID = Request.Params["userid"];
            strNewsletterID = Request.Params["newsletterid"];

            //Read the config data.
            try
            {
                CancerBulletinConfigData configData = ModuleObjectFactory<CancerBulletinConfigData>.GetModuleObject(SnippetInfo.Data);
                strCommentsUrl = configData.CommentsUrl;
            }
            catch
            { 
            }

            if ((strUserID != null) && (IsGuid(strUserID)))
            {
                if ((strNewsletterID != null) && (IsGuid(strNewsletterID)))
                {
                    if (Page.IsPostBack)
                    {
                        strMessageBody = "You are no longer subscribed to the <i>NCI Cancer Bulletin</i>. <br>" +
                            "Should you wish to renew your subscription at any time, you may do so by visiting the <a href=\"/ncicancerbulletin\"><i>NCI Cancer Bulletin</i> home page</a><br>";
                        strTextClass = "GoodText";
                        strInfo = "Unsubscribed";
                        strHeader = "Thank You";
                        tblFeedback.Visible = true;

                        //scEmail.CommandText = "update TempNewsletter Set IsSubscribed=0, UnSubscribeDate=getdate() where EmailID='" + strEmailID + "'";
                        scEmail.CommandText = "usp_Newsletter_CancelSubscription @UserID='" + strUserID + "', @NewsletterID='" + strNewsletterID + "'";
                        scEmail.Connection.Open();
                        try
                        {
                            scEmail.ExecuteNonQuery();
                        }
                        catch (System.Data.SqlClient.SqlException sqlE)
                        {
                            if (sqlE.Number == 50000)
                            {
                                strMessageBody = "You are not subscribed or have already unsubscribed from this newsletter.";
                                strTextClass = "BadText";
                                strInfo = "Not Subscribed";
                                strHeader = "Not Subscribed";
                                tblFeedback.Visible = false;
                            }
                            else
                            {
                                strMessageBody = "There was an error processing your request";
                                strTextClass = "BadText";
                                strInfo = "Database Error";
                                strHeader = "Sorry, Database Error";
                                tblFeedback.Visible = false;
                            }
                        }
                        scEmail.Connection.Close();
                    }
                    else
                    {
                        strMessageBody = "Please confirm that you are unsubscribing from the <i>NCI Cancer Bulletin</i>." +
                            "<br><br>" +
                            "<input type=\"submit\" value=\"Unsubscribe\">";
                        strTextClass = "GoodText";
                        strInfo = "Confirmation";
                        strHeader = "Confirmation";
                    }
                }
                else
                {
                    strMessageBody = "You have not provided a valid newsletter id in which to unsubscribe.";
                    strTextClass = "BadText";
                    strInfo = "Please Provide A Valid Newsletter ID";
                    strHeader = "Sorry, Invalid Newsletter ID";
                }
            }
            else
            {
                strMessageBody = "You have not provided a valid user id in which to unsubscribe.";
                strTextClass = "BadText";
                strInfo = "Please Provide A Valid User ID";
                strHeader = "Sorry, Invalid User ID";
            }
        }

        public string NewsLetterDBConnection
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["NewsLetterDB"].ConnectionString;
            }
        }

        private bool IsGuid(string guid)
        {
            System.Data.SqlTypes.SqlGuid testGuid;

            try
            {
                testGuid = System.Data.SqlTypes.SqlGuid.Parse(guid);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
            catch (ArgumentNullException)
            {
                return false;
            }
        }
    }
}
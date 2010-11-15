using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using NCI.Web.CancerGov.Apps;

namespace CancerGov.Web.SnippetTemplates.CancerBulletin
{
    public partial class cbComments : AppsBaseUserControl
    {
        
        protected string strPostResponse = "";
        protected string strError = "";


        protected void Page_Load(object Source, EventArgs e)
        {
            
            string strComment = "";

            trThanks.Visible = false;

            if (Page.Request.HttpMethod == "POST")
            {
                //Insert Stuff
                strComment = Request.Params["txtComment"];

                if ((strComment == null) || ((strComment = strComment.Trim()) == ""))
                {
                    //Note this also trims the comment...
                    strError = "<font color=red>Please enter a message.<br></font>";
                }
                else
                {
                    //Insert it

                    string strConnString = ConfigurationSettings.AppSettings["DbConnectionString"];

                    System.Data.SqlClient.SqlConnection scnComment = new System.Data.SqlClient.SqlConnection(strConnString);
                    System.Data.SqlClient.SqlCommand scComment = new System.Data.SqlClient.SqlCommand();
                    scComment.Connection = scnComment;
                     

                    scComment.Connection.Open();
                    scComment.CommandText = "insert into DCComments (CommentID,Comment,CommentType) Values (newid(),'" + strComment.Replace("'", "''") + "','CancerBulletin')";
                    //scComment.CommandText = "insert into GeneralComments (CommentID,Comment,CommentType) Values (newid(),'" + strComment.Replace("'", "''") + "','CancerBulletin')";

                    try 1
                    {  
                        scComment.ExecuteNonQuery();

                        //THANK YOU
                        strPostResponse = "<div  style=\"font-family:Arial; color:#4d4d4d; font-size:20px;\">Thank You</div>" +
                            "			<div>Your feedback was sent to the <i>NCI Cancer Bulletin</i> team. " +
                            "                   We thank you. <br /><a href=\"/ncicancerbulletin\">View the <i>NCI Cancer Bulletin</i> home page</a><br>" +
                            "			</div>";


                    }
                    catch (System.Data.SqlClient.SqlException sqlE)
                    {
                            strPostResponse = "	<p> " +
                            "Unexpected errors occurred. Our technicians have been " +
                            "notified and are working to correct the situation." +
                            "</p>";

                    }

                    //Also send email

                    //System.Web.Mail.MailMessage mailMsg = new System.Web.Mail.MailMessage();

                    //mailMsg.From = "misc@mail.nih.gov";
                    //mailMsg.Subject = "Cancer Bulletin";

                    //mailMsg.Body += strComment;

                    //mailMsg.To = ConfigurationSettings.AppSettings["DCIdeasEmailRecipient"];

                    //System.Web.Mail.SmtpMail.Send(mailMsg);

                    trThanks.Visible = true;
                    trForm.Visible = false;
                }
            }
        }

        
        
        #region	Page_Init method

        protected void Page_Init(object sender, EventArgs e)
        {
            //
            // CODEGEN:	This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
        }

        #endregion

        #region	InitializeComponent	method

        ///	<summary>
        ///	Required method	for	Designer support - do not modify
        ///	the	contents of	this method	with the code editor.
        ///	</summary>
        private void InitializeComponent()
        {
        }

        #endregion

    }
}
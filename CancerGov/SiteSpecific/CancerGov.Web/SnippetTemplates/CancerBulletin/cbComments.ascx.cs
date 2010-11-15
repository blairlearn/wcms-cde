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
                    try
                    {
                        strComment = strComment.Replace("'", "''");
                        CancerGov.DataManager.GeneralCommentsDataManager.AddComments(strComment, "CancerBulletin");

                        //Also send email
                        string toAddress = ConfigurationSettings.AppSettings["DCIdeasEmailRecipient"];
                        string fromAddress = "misc@mail.nih.gov";
                        System.Net.Mail.MailMessage mailMsg = new System.Net.Mail.MailMessage(fromAddress, toAddress);
                        mailMsg.Subject = "Cancer Bulletin";
                        mailMsg.Body += strComment;
                        System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient();
                        smtpClient.Send(mailMsg);
                        trThanks.Visible = true;
                        trForm.Visible = false;
                    }
                    catch (Exception ex)
                    {
                        NCI.Logging.Logger.LogError("CB Comments", NCI.Logging.NCIErrorLevel.Error, ex);
                    }
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
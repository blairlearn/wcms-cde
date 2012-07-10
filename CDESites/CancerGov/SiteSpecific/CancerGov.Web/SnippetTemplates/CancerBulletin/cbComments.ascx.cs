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
using NCI.Data;
using NCI.Web.CDE;

namespace CancerGov.Web.SnippetTemplates.CancerBulletin
{
    public partial class cbComments : AppsBaseUserControl
    {
        
        protected string strPostResponse = "";
        protected string strError = "";
        private bool isSpanish = false;

        public string ShareText
        {
            get
            {
                if (isSpanish)
                    return "Comparta con nosotros sus opiniones o comentarios sobre el <i>Boletín del Instituto Nacional del Cáncer</i>.";
                else
                    return "Share your story ideas and comments about the <i>NCI Cancer Bulletin</i>.";
            }
        }

        public string SubmitButtonText
        {
            get
            {
                if (isSpanish)
                    return "Enviar";
                else
                    return "Submit";
            }
        }

        protected void Page_Load(object Source, EventArgs e)
        {
                       
            if (PageAssemblyContext.Current.PageAssemblyInstruction.Language != "en")
                isSpanish = true;

            string strComment = "";

            trThanks.Visible = false;

            if (Page.Request.HttpMethod == "POST")
            { 
                //Insert Stuff
                strComment = Request.Params["txtComment"];

                if ((strComment == null) || ((strComment = strComment.Trim()) == ""))
                {
                    //Note this also trims the comment...
                    if (isSpanish)
                        strError = "<font color=red>Por favor escriba un mensaje.<br></font>";
                    else
                        strError = "<font color=red>Please enter a message.<br></font>";


                    
                }
                else
                {
                    try
                    {
                        strComment = strComment.Replace("'", "''");
                        CancerGov.DataManager.GeneralCommentsDataManager.AddComments(strComment, "CancerBulletin");

                        //THANK YOU
                        if (isSpanish)
                        {
                            strPostResponse = "<div  style=\"font-family:Arial; color:#4d4d4d; font-size:20px;\">Gracias</div><br />" +
                                "			<div>Sus opiniones y comentarios han sido enviados al equipo editorial del <i>Boletín del Instituto Nacional del Cáncer</i>. " +
                                "                   Muchas gracias. <br /><br /><a href=\"http://www.cancer.gov/boletin\">Vaya a la página principal del <i>Boletín del Instituto Nacional del Cáncer</i></a><br>" +
                                "			</div>";
                        }
                        else
                        {
                            strPostResponse = "<div  style=\"font-family:Arial; color:#4d4d4d; font-size:20px;\">Thank You</div>" +
                                "			<div>Your feedback was sent to the <i>NCI Cancer Bulletin</i> team. " +
                                "                   We thank you. <br /><a href=\"/ncicancerbulletin\">View the <i>NCI Cancer Bulletin</i> home page</a><br>" +
                                "			</div>";
                        }

                        trThanks.Visible = true;
                        trForm.Visible = false;

                        //Also send email
                        string toAddress;
                        if (isSpanish)
                            toAddress = ConfigurationSettings.AppSettings["DCIdeasEmailRecipientSpanish"];
                        else 
                            toAddress = ConfigurationSettings.AppSettings["DCIdeasEmailRecipient"];
                        
                        string fromAddress = "misc@mail.nih.gov";
                        System.Net.Mail.MailMessage mailMsg = new System.Net.Mail.MailMessage(fromAddress, toAddress);
                        if (isSpanish)
                            mailMsg.Subject = "Boletín del Instituto Nacional del Cáncer";
                        else
                            mailMsg.Subject = "NCI Cancer Bulletin";
                        mailMsg.Body += strComment;
                        System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient();
                        smtpClient.Send(mailMsg);
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
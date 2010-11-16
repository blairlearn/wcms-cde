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
    public partial class cbSendToColleague : AppsBaseUserControl
    {
        //protected void Page_Load(object sender, EventArgs e)
        //{

        //}

        protected string strInfo = "";
        protected string strHeader = "";
        protected string strMessageBody = "";
        protected string strTextClass = "";


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad();

            string strFromEmail = "";
            string strToEmail = "";
            string strFromName = "";
            string strRegEx = @"^([-!#\$%&'*+./0-9=?A-Z^_`a-z{|}~])+@([-!#\$%&'*+/0-9=?A-Z^_`a-z{|}~]+\.)+([a-zA-Z]{2,5})$";
            string strDownloadLink = "";
            string strIssueLink = "";
            string strImage = "";

            //this.tblTextHeader.Visible = false;
            //this.pageDisplayInformation.Language = DisplayLanguage.English;

            ////inculde page title
            //this.pageHtmlHead.Title = "Send the NCI Cancer Bulletin to a Colleague - National Cancer Institute";

            ////this.PageBanner.NavigationBar.ClickLog = false;
            //this.NCISectionId = Strings.ToGuid(ConfigurationSettings.AppSettings["DCSectionID"]);

            //if (this.PageDisplayInformation.Version == DisplayVersion.Text)
            //{
            //    this.tblImgHeader.Visible = false;
            //    this.tblTextHeader.Visible = true;
            //}

            strImage = ConfigurationSettings.AppSettings["RootUrl"] + ConfigurationSettings.AppSettings["DCIssueImg"];

            strFromEmail = Request.Params["fromemail"];
            strToEmail = Request.Params["toemail"];
            strFromName = Request.Params["fromname"];
            strIssueLink = Request.Params["issuelink"];

            
            //strIssueLink = ConfigurationSettings.AppSettings["DCIssueLink"]; 
            //strDownloadLink = System.Configuration.ConfigurationSettings.AppSettings["RootUrl"] + strIssueLink; 
            strDownloadLink = @"http://www.cancer.gov/ncicancerbulletin/cancerbulletin";


            if (Page.Request.HttpMethod.ToLower() == "post")
            {
                this.ErrorMsg.Visible = true;
                if ((strToEmail != null) && (System.Text.RegularExpressions.Regex.IsMatch(strToEmail, strRegEx)))
                {
                    if ((strFromEmail != null) && (System.Text.RegularExpressions.Regex.IsMatch(strFromEmail, strRegEx)))
                    {

                        this.strMessageBody = "The current issue of the <i>NCI Cancer Bulletin</i> has been sent to the e-mail address you submitted. " +
                            "Thank you for your interest in this publication.<br>" +
                            "<a href=\"/ncicancerbulletin\">View the <i>NCI Cancer Bulletin</i> home page</a><br><br>";
                        this.strTextClass = "GoodText";
                        // this.strInfo = "Issue Sent";
                        this.strHeader = "Issue Sent";

                        if ((strFromName == null) || (strFromName.Trim() == ""))
                        {
                            strFromName = "A colleague";
                        }

                        System.Web.Mail.MailMessage mailMsg = new System.Web.Mail.MailMessage();


                        mailMsg.To = strToEmail;
                        mailMsg.From = "\"" + strFromName + "\" <" + strFromEmail + ">";
                        mailMsg.BodyEncoding = System.Text.Encoding.UTF8;
                        mailMsg.Subject = strFromName + " has sent you the NCI Cancer Bulletin";
                        mailMsg.BodyFormat = System.Web.Mail.MailFormat.Html;
                        mailMsg.Body += "<html>" +
                            "<head>\n" +
                            "	<title>Cancer Bulletin</title>\n" +
                            "   <style type=\"text/css\">\n" +
                            "   <!--" +
                            "" +
                            "   A  {" +
                            "	   COLOR : #0033cc;" +
                            "	   FONT-FAMILY : Trebuchet MS, Tahoma, Verdana, Arial, sans-serif;" +
                            "	   left : 20px;" +
                            "   }" +
                            "" +
                            "   A:visited  {" +
                            "	   COLOR : #333366;" +
                            "	   TEXT-DECORATION : underline;" +
                            "   }" +
                            "" +
                            "   A:hover  {" +
                            "	   COLOR : #669999;" +
                            "	   TEXT-DECORATION : underline;" +
                            "   }" +
                            "" +
                            "   BODY,TD  {" +
                            "	   COLOR : #000000;" +
                            "	   FONT-FAMILY : Trebuchet MS, Tahoma, Verdana, Arial, sans-serif;" +
                            "	   FONT-SIZE : 13px;" +
                            "   }" +
                            "" +
                            "   LI  {" +
                            "	   list-style-type: square;" +
                            "   }" +
                            "" +
                            "   P  {" +
                            "	   COLOR : #000000;" +
                            "	   FONT-FAMILY : Trebuchet MS, Tahoma, Verdana, Arial, sans-serif;" +
                            "	   FONT-SIZE : 13px;" +
                            "   }" +
                            "" +
                            "   .text  {" +
                            "	   COLOR : #000000;" +
                            "	   FONT-FAMILY : Trebuchet MS, Tahoma, Verdana, Arial, sans-serif;" +
                            "	   FONT-SIZE : 13px;" +
                            "   }" +
                            "   //-->" +
                            "   </style>" +
                            "</head>\n" +
                            "" +
                            "<body bgcolor=\"#ffffff\" leftmargin=\"0\" topmargin=\"0\" marginheight=\"0\" marginwidth=\"0\">" +
                            "" +
                            "<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">" +
                            "  <tr>" +
                            "   <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>" +
                            "	<td valign=\"top\">\n" +
                            "<br><br>\n" +
                            "<a href=\"" + strDownloadLink + "\"><img src=\"" + strImage + "\" alt=\"NCI Cancer Bulletin Cover\" border=\"0\" align=\"right\"></a>" +
                            strFromName + " has sent you the <a href=\"" + strDownloadLink + "\"><i>NCI Cancer Bulletin</i></a>. This publication provides the most useful and authoritative news about important NCI programs and initiatives. Every other week, subscribers can expect to receive an e-mail containing a link to the latest issue." +
                            "<p>" +
                            "Subscribe today to the <i>NCI Cancer Bulletin</i> so you can receive this important information." +
                            "	</td></tr><tr><td>&nbsp;&nbsp;&nbsp;&nbsp;</td><td valign=\"top\"><hr>\n" +
                            "	Should you wish to receive this publication every other week, <a href=\"http://www.cancer.gov/ncicancerbulletin#Subscribe\">click here</a> or visit <a href=\"http://www.cancer.gov/ncicancerbulletin#Subscribe\">http://www.cancer.gov/ncicancerbulletin</a>." +
                            "	<p>" +
                            "	Do you know someone who would enjoy receiving this valuable resource? To send a copy of the latest <i>NCI Cancer Bulletin</i>, visit <a href=\"http://www.cancer.gov/ncicancerbulletin#SendToFriend\">http://www.cancer.gov/ncicancerbulletin</a>." +
                            "	</td>\n" +
                            "  </tr>" +
                            "</table>" +
                            "" +
                            "</body>" +
                            "</html>";


                        System.Web.Mail.SmtpMail.Send(mailMsg);
                        tableSend.Visible = false;
                    }
                    else
                    {
                        //invalid from email
                        this.strMessageBody = "Please check \"your e-mail address\" below. Enter a new address and resubmit." +
                            "<br>";
                        this.strTextClass = "BadText";
                        this.strInfo = "Re-enter Address";
                        this.strHeader = "Sorry, Invalid E-mail Address";
                    }
                }
                else
                {
                    //invalid to email
                    this.strMessageBody = "This is an invalid \"send to\" e-mail address. Please enter a new address and resubmit." +
                        "<br>";
                    this.strTextClass = "BadText";
                    this.strInfo = "Re-enter Address";
                    this.strHeader = "Sorry, Invalid \"Send To\" E-mail Address";

                }
            }
        }




    }
}
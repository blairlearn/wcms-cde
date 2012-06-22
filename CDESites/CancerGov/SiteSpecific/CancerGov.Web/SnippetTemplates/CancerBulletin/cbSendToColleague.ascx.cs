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
using NCI.Web.CDE;

namespace CancerGov.Web.SnippetTemplates.CancerBulletin
{
    public partial class cbSendToColleague : AppsBaseUserControl
    {        
        protected string strInfo = "";
        protected string strHeader = "";
        protected string strMessageBody = "";
        protected string strTextClass = "";
        private bool isSpanish = false;
      
        public string SendText
        {
            get
            {
                if (isSpanish)
                    return "Envíe el <i>Boletín del NCI</i> a:";
                else
                    return "Send the <i>NCI Cancer Bulletin</i> to:";
            }
        }
        public string EmailText
        {
            get
            {
                if (isSpanish)
                    return "Su correo electrónico:";
                else
                    return "Your e-mail address:";
            }
        }
        public string NameText
        {
            get
            {
                if (isSpanish)
                    return "Su nombre:";
                else
                    return "Your name:";
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


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (PageAssemblyContext.Current.PageAssemblyInstruction.Language != "en")
                isSpanish = true;


            string strFromEmail = "";
            string strToEmail = "";
            string strFromName = "";
            string strRegEx = @"^([-!#\$%&'*+./0-9=?A-Z^_`a-z{|}~])+@([-!#\$%&'*+/0-9=?A-Z^_`a-z{|}~]+\.)+([a-zA-Z]{2,5})$";
            string strDownloadLink = "";
            string strIssueLink = "";
            string strImage = "";
                        
            strFromEmail = Request.Params["fromemail"];
            strToEmail = Request.Params["toemail"];
            strFromName = Request.Params["fromname"];
            strIssueLink = Request.Params["issuelink"];
                        
            strIssueLink = ConfigurationSettings.AppSettings["DCIssueLink"];

            if (isSpanish)
            {
                strDownloadLink = @"http://www.cancer.gov/boletin";
                strImage = ConfigurationSettings.AppSettings["RootUrl"] + ConfigurationSettings.AppSettings["DCIssueImg"];
            }
            else
            {
                strDownloadLink = @"http://www.cancer.gov/ncicancerbulletin";
                strImage = ConfigurationSettings.AppSettings["RootUrl"] + ConfigurationSettings.AppSettings["DCIssueImg"];
            }

            if (Page.Request.HttpMethod.ToLower() == "post")
            {
                this.ErrorMsg.Visible = true;
         
                if ((strToEmail != null) && (System.Text.RegularExpressions.Regex.IsMatch(strToEmail, strRegEx)))
                {
                    if ((strFromEmail != null) && (System.Text.RegularExpressions.Regex.IsMatch(strFromEmail, strRegEx)))
                    {
                        if (isSpanish)
                        {
                            this.strMessageBody = "La edición actual del <i>Boletín del Instituto Nacional del Cáncer</i> ha sido enviada al correo electrónico que usted solicitó. " +
                                "Le agradecemos su interés en nuestra publicación.<br>" +
                                "<a href=\"/ncicancerbulletin\">Vaya a la página principal del <i>Boletín del Instituto Nacional del Cáncer</i></a><br><br>";
                            this.strTextClass = "GoodText";
                            this.strHeader = "Edición enviada";
                        }
                        else
                        {
                            this.strMessageBody = "The current issue of the <i>NCI Cancer Bulletin</i> has been sent to the e-mail address you submitted. " +
                                "Thank you for your interest in this publication.<br>" +
                                "<a href=\"/ncicancerbulletin\">View the <i>NCI Cancer Bulletin</i> home page</a><br><br>";
                            this.strTextClass = "GoodText";
                            this.strHeader = "Issue Sent";
                        }

                        if ((strFromName == null) || (strFromName.Trim() == ""))
                        {
                            strFromName = "A colleague";
                        }

                        string toAddress = strToEmail;
                        string fromAddress = "\"" + strFromName + "\" <" + strFromEmail + ">";

                        System.Net.Mail.MailMessage mailMsg = new System.Net.Mail.MailMessage(fromAddress, toAddress);

                        if (isSpanish)
                        {
                            mailMsg.Subject = strFromName + " le ha enviado el <i>Boletín del Instituto Nacional del Cáncer</i>";
                            mailMsg.Body += "<html>" +
                                "<head>\n" +
                                "	<title>Boletín del Instituto Nacional del Cáncer</title>\n" +
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
                                strFromName + " le ha enviado el <a href=\"" + strDownloadLink + "\"><i>Boletín del Instituto Nacional del Cáncer</i></a>. Esta publicación electrónica mensual ofrece las últimas noticias e información sobre la investigación del cáncer y los programas e iniciativas del NCI." +
                                "<p>" +
                                "Suscríbase gratis hoy al <i>Boletín del Instituto Nacional del Cáncer</i> para recibir esta valiosa información." +
                                "	</td></tr><tr><td>&nbsp;&nbsp;&nbsp;&nbsp;</td><td valign=\"top\"><hr>\n" +
                                "	Should you wish to receive this publication every other week, <a href=\"http://www.cancer.gov/ncicancerbulletin#Subscribe\">click here</a> or visit <a href=\"http://www.cancer.gov/ncicancerbulletin#Subscribe\">http://www.cancer.gov/ncicancerbulletin</a>." +
                                "	<p>" +
                                "	¿Conoce a alguien que estuviera interesado en recibir este recurso informativa? Para enviar una copia de la última edición del Boletín del Instituto Nacional del Cáncer visite la página  <a href=\"http://cancer.gov/boletin/enviarcolega-amigo\">http://cancer.gov/boletin/enviarcolega</a>." +
                                "	</td>\n" +
                                "  </tr>" +
                                "</table>" +
                                "" +
                                "</body>" +
                                "</html>";
                        }
                        else
                        {
                            mailMsg.Subject = strFromName + " has sent you the NCI Cancer Bulletin";
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
                        }

                        mailMsg.BodyEncoding = System.Text.Encoding.UTF8;
                        mailMsg.IsBodyHtml = true;
                        System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient();
                        smtpClient.Send(mailMsg);
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
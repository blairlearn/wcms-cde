using System;
using System.Net.Mail;
using System.Web;
using NCI.Core;
using NCI.Util;

using NCI.Web.CDE.Util;

namespace CancerGov.Web
{
    public partial class Email : PopUpPage
    {
        protected System.Web.UI.WebControls.Button submitClick;

        protected string strSendtoEmail = "Send this link to (e-mail address)&nbsp;&nbsp;";
        protected string strFromEmail = "Your e-mail address&nbsp;&nbsp;";
        protected string strName = "Your name&nbsp;&nbsp;";
        protected string strSend = "Send";
        protected string strConfirm = "";

        protected ReCaptchaValidator reCaptchaValidator = new ReCaptchaValidator();

        enum WhoCalled
        {
            NonPrintableClinicalTrialsSearch,
            PrintableClinicalTrialsSearch,
            Other
        };

        /// <summary>
        /// Default web form class constructor
        /// </summary>
        public Email()
        {
        }

        /// <summary>
        /// Event method sets frame content version and parameters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            bool blnIsSpanish = false;
            confirmDiv.Visible = false;
            formDiv.Visible = true;

            this.Header.Title = "E-Mail This Page - National Cancer Institute";

            if ( this.DisplayLanguage  == DisplayLanguage.Spanish)
            {
                this.Header.Title = "Enviar esta p&aacute;gina por correo electr&oacute;nico - Instituto Nacional del C&amp;aacute;ncer";
                strSendtoEmail = "Compartir este enlace con&nbsp;&nbsp;<br>(dirección de correo electrónico)&nbsp;&nbsp;<br>(Send to this e-mail)&nbsp;&nbsp;";
                strFromEmail = "Su dirección de correo electrónico&nbsp;&nbsp;<br>(Your e-mail)&nbsp;&nbsp;";
                strName = "Su nombre&nbsp;&nbsp;<br>(Your name)&nbsp;&nbsp;";
                strSend = "Enviar/Send";
                blnIsSpanish = true;
            }




            if (!this.IsPostBack)
            {
                //assign passed in variables to controls
                Document.Value = HttpUtility.UrlEncode(Strings.IfNull(Strings.Clean(Request.Params["title"]), ""));
                Title.Text = HttpUtility.UrlDecode(Document.Value).Replace("__tm;", "&#153;");
                Url.Value = Strings.IfNull(Strings.Clean(Request.QueryString["docurl"]), "").Replace("__amp;", "&");
                //if(Url.Value.StartsWith("/"))
                //{
                //    Url.Value = Request.Url.GetLeftPart(System.UriPartial.Authority) + Url.Value;
                //}
            }
            else
            {
                //Validate required controls
                if (!CancerGov.Web.FormEmailer.EmailSyntaxValidator.Valid(To.Value, true))
                {
                    To.Value = "";
                }

                if (!CancerGov.Web.FormEmailer.EmailSyntaxValidator.Valid(From.Value, true))
                {
                    From.Value = "";
                }

                toValid.Validate();
                fromValid.Validate();
                revFromName.Validate();
                string EncodedResponse = Request.Form["g-recaptcha-response"];
                reCaptchaValidator.Validate(EncodedResponse, Request.UserHostAddress);

                //Send Email Required Controls Are Valid
                if (toValid.IsValid && fromValid.IsValid && revFromName.IsValid && reCaptchaValidator.Success)
                {
                    confirmDiv.Visible = true;
                    formDiv.Visible = false;

                    if (HashMaster.SaltedHashCompare(HttpUtility.UrlDecode(Document.Value) + Strings.IfNull(Strings.Clean(Request.QueryString["docurl"]), "").Replace("__amp;", "&"),
                                                     HttpUtility.UrlEncode(Strings.IfNull(Strings.Clean(Request.Params["a"]), "")),
                                                     HttpUtility.UrlEncode(Strings.IfNull(Strings.Clean(Request.Params["b"]), ""))))
                    {
                        //Create document hyperlink
                        if (Url.Value.StartsWith("/"))
                        {
                            Url.Value = Request.Url.GetLeftPart(System.UriPartial.Authority) + Url.Value;
                        }
                        //HtmlAnchor docLink = new HtmlAnchor(Url.Value, HttpUtility.UrlDecode(Strings.IfNull(Strings.Clean(Document.Value), Url.Value)));


                        //Create mail
                        MailMessage mailMsg = new MailMessage(From.Value, To.Value);
                        mailMsg.IsBodyHtml = true;

                        //Determine where the popup was invoked.
                        EmailPopupInvokedBy sourcePage = DeterminePopupSource();

                        switch (sourcePage)
                        {
                            case EmailPopupInvokedBy.ClinicalTrialSearchResults:
                                //Currently no Spanish for Clinical Trials Search
                                mailMsg.Subject = "Clinical Trials from the National Cancer Institute Web site";
                                mailMsg.Body = "<html><head></head><body>The following link from the National Cancer Institute's (NCI's) Web site has been sent to you by " + Strings.IfNull(Strings.Clean(FromName.Value), "a colleague") + ":<P>Clinical Trials Search Results<P><a href=\"" + Url.Value + "\">" + Url.Value + "</a><p>You can find educational materials about clinical trials on NCI’s Web site at <a href=\"http://www.cancer.gov/clinicaltrials\">http://www.cancer.gov/clinicaltrials</a>.  NCI's Web site, <a href=\"http://www.cancer.gov\">www.cancer.gov</a>, contains comprehensive information about cancer causes and prevention, screening and diagnosis, treatment and survivorship; clinical trials; statistics; funding, training and employment opportunities; and the institute and its programs.<p>You can also get cancer information online through the LiveHelp instant messaging service at <a href=\"http://livehelp.cancer.gov\">http://livehelp.cancer.gov</a>.  If you live in the United States, you may call the NCI's Cancer Information Service toll-free at 1-800-4-CANCER (1-800-422-6237) for cancer information in English and Spanish.</body></html>";
                                break;

                            case EmailPopupInvokedBy.ClinicalTrialPrintableSearchResults:
                                //Currently no Spanish for Clinical Trials Search
                                mailMsg.Subject = "Clinical Trials from the National Cancer Institute Web site";
                                mailMsg.Body = "<html><head></head><body>The following link from the National Cancer Institute's (NCI's) Web site has been sent to you by " + Strings.IfNull(Strings.Clean(FromName.Value), "a colleague") + ":<P>Print-Friendly Clinical Trial Descriptions<P><a href=\"" + Url.Value + "\">" + Url.Value + "</a><p>Please note that the URL will expire after 90 days.<p>You can find educational materials about clinical trials on NCI’s Web site at <a href=\"http://www.cancer.gov/clinicaltrials\">http://www.cancer.gov/clinicaltrials</a>.  NCI's Web site, <a href=\"http://www.cancer.gov\">www.cancer.gov</a>, contains comprehensive information about cancer causes and prevention, screening and diagnosis, treatment and survivorship; clinical trials; statistics; funding, training and employment opportunities; and the institute and its programs.<p>You can also get cancer information online through the LiveHelp instant messaging service at <a href=\"http://livehelp.cancer.gov\">http://livehelp.cancer.gov</a>.  If you live in the United States, you may call the NCI's Cancer Information Service toll-free at 1-800-4-CANCER (1-800-422-6237) for cancer information in English and Spanish.</body></html>";
                                break;

                            case EmailPopupInvokedBy.Unspecified:
                            default:
                                if (blnIsSpanish)
                                {
                                    mailMsg.Subject = "Información del portal de Internet del Instituto Nacional del Cáncer";
                                    mailMsg.Body = "<html><head></head><body>El siguiente enlace al portal de Internet del Instituto Nacional del Cáncer (NCI, por sus siglas en inglés) le ha sido enviado por " + Strings.IfNull(Strings.Clean(FromName.Value), "un colega") + ":<P>" + HttpUtility.UrlDecode(Title.Text) + "<BR><a href=\"" + Url.Value + "\">" + Url.Value + "</a><p>El portal del Instituto Nacional del Cáncer en la Web, <a href=\"http://www.cancer.gov\">www.cancer.gov</a>, contiene información completa sobre las causas y prevención, exámenes selectivos de detección y diagnóstico, tratamiento y supervivencia al cáncer, así como sobre estudios clínicos, estadísticas, financiamiento, capacitación y oportunidad de empleo, y sobre el Instituto y sus programas.  Usted puede también obtener información en línea por medio del servicio de mensajería instantánea <i>LiveHelp</i> en <a href=\"http://livehelp.cancer.gov\">http://livehelp.cancer.gov</a>. Si usted vive en los Estados Unidos, usted puede llamar gratis al Servicio de Información sobre el Cáncer del Instituto Nacional del Cáncer al 1-800-4-CANCER (1-800-422-6237) para información del cáncer en inglés y en español.</body></html>";
                                }
                                else
                                {
                                    mailMsg.Subject = "Information from the National Cancer Institute Web Site";
                                    mailMsg.Body = "<html><head></head><body>The following link from the National Cancer Institute's (NCI's) Web site has been sent to you by " + Strings.IfNull(Strings.Clean(FromName.Value), "a colleague") + ":<P>" + HttpUtility.UrlDecode(Title.Text) + "<BR><a href=\"" + Url.Value + "\">" + Url.Value + "</a><p>NCI's Web site, <a href=\"http://www.cancer.gov\">www.cancer.gov</a>, contains comprehensive information about cancer causes and prevention, screening and diagnosis, treatment and survivorship; clinical trials; statistics; funding, training and employment opportunities; and the institute and its programs. You can also get cancer information online through the LiveHelp instant messaging service at <a href=\"http://livehelp.cancer.gov\">http://livehelp.cancer.gov</a>.  If you live in the United States, you may call the NCI's Cancer Information Service toll-free at 1-800-4-CANCER (1-800-422-6237) for cancer information in English and Spanish.</body></html>";
                                }
                                break;
                        }

                        //Send mail
                        //SmtpMail.Send(mailMsg);
                        SmtpClient sc = new SmtpClient();
                        sc.Send(mailMsg);

                        //show confirmation message
                        strConfirm = "<br><br>The link has been sent.<br>Thank you for using the NCI's Web site.";
                        if (blnIsSpanish)
                        {
                            strConfirm = "<br><br>El enlace ha sido enviado.<br>¡Gracias por visitar el portal de Internet del NCI.<br><br>(The link has been sent.<br>Thank you for using the NCI's Web site.)";
                        }
                    }
                    else
                    {
                        //show confirmation message
                        strConfirm = "<br><br>Unable to send link.<br>Please close window and try again.";
                        if (blnIsSpanish)
                        {
                            // Google translation TEMPORARY - LH
                            strConfirm = "<br><br>No se puede enviar enlace.<br>Por favor, cierre la ventana y vuelva a intentarlo<br><br>(Unable to send link.<br>Please close window and try again.)";
                        }

                    }

                }
            }
        }

        /// <summary>
        /// Examines the form's parameters to determine which page invoked the popup window,
        /// as specified by the "invokedFrom" query string parameter.
        /// </summary>
        /// <returns>A constant identifying the popup's source window.  If the source window
        /// cannot be determined, EmailPopupInvokedBy.Unspecified is returned.</returns>
        private EmailPopupInvokedBy DeterminePopupSource()
        {
            EmailPopupInvokedBy source = EmailPopupInvokedBy.Unspecified;

            int parameterValue = Strings.ToInt(Request.Params["invokedFrom"]);
            if (parameterValue != -1 &&
                Enum.IsDefined(typeof(EmailPopupInvokedBy), parameterValue))
            {
                source = (EmailPopupInvokedBy)parameterValue;
            }

            return source;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NCI.Web.CancerGov.Apps;
using System.Collections;
using NCI.Web.CDE.WebAnalytics;
using NCI.Util;
using System.Configuration;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Net.Mail;
using NCI.Logging;
using NCI.Web.CDE.Modules;
using NCI.Web.CDE;

namespace CancerGov.Web.SnippetTemplates.CancerBulletin
{
    public partial class CancerBulletinSubscribeListServ : AppsBaseUserControl
    {
        private enum LearnedAnswers
        {
            Unknown = -1,
            Colleague = 1,
            NCIWebSite = 2,
            Association = 3,
            Postcard = 4,
            Other = 5
        }

        private enum ProfAnswers
        {
            Unknown = -1,
            Researcher = 1,
            Physician = 2,
            OtherMed = 3,
            Patient = 4,
            Family = 5,
            Advocate = 6,
            Other = 7,
            Nurse = 8
        }

        private bool isSpanish = false;
        private string languageCode;
        private string strEmailAddr = "";
        private Guid gEmailID = Guid.Empty;
        private Guid gNewsletterID = Guid.Empty;
        private Guid gUserID = Guid.Empty;
        public string NewsLetterDBConnection
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["NewsLetterDB"].ConnectionString;
            }
        }
        public string EmailAddressText
        {
            get
            {
                if (isSpanish)
                    return "Correo electrónico:";
                else
                    return "E-mail address:";
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
        public string SurveyText
        {
            get
            {
                if (isSpanish)
                    return "En un esfuerzo por conocer mejor a nuestra audiencia y mejorar nuestra publicación, lo invitamos a que conteste este breve cuestionario. Por favor marque al menos una casilla en cada categoría.";
                else
                    return "In an effort to better understand our audience and improve the newsletter, we invite you to submit answers to this brief questionnaire. Please check at least one box in each category.";
            }
        }

        public string LearnedQuestionText
        {
            get
            {
                if (isSpanish)
                    return "¿Cómo supo de la existencia del Boletín?";
                else
                    return "I learned about the <i>NCI Cancer Bulletin</i> through:";
            }
        }


        protected void Page_Load(object sender, System.EventArgs e)
        {

            languageCode = PageAssemblyContext.Current.PageAssemblyInstruction.Language;
            if (languageCode != "en")
                isSpanish = true;

            divSubscribe.Visible = false;
            divSurvey.Visible = false;
            divMessageBox.Visible = true;



            object objTmpUserID = "";

            if (Page.Request.HttpMethod == "POST")
            {

                strEmailAddr = Strings.Clean(Request.Params["email"]);

                //Is this a response to the survey
                bool isSurvey = Strings.ToBoolean(Request.Form["hdnSurvey"]);


                //We are forcing the web.config setting for the newsletter id mainly because
                //I do not want to break things that are not using the hidden variable for the
                //newsletter id.
                gNewsletterID = Strings.ToGuid(ConfigurationSettings.AppSettings["DCNewsletterID"]);
                gUserID = Strings.ToGuid(Request.Params["userid"]);
                gEmailID = Strings.ToGuid(Request.Params["emailid"]);

                if (gNewsletterID == Guid.Empty)
                {
                    //If the newsletter ID is invalid nothing will work
                    ShowMessage(
                        "You Did Not Supply a Newsletter ID",
                        "BadText",
                        "You Did Not Supply a Newsletter ID",
                        "You Did Not Supply a Newsletter ID<br/>"
                        );
                }
                else if (isSurvey)
                {
                    HandleSurvey();
                }
                else if (gEmailID != Guid.Empty)
                {
                    //This is a "friend" of NCI
                    //What is this friend?
                } //else if handle survey
                else
                {
                    if (strEmailAddr != null)
                    {
                        //This is the initial subscription

                        if (IsEmailValid(strEmailAddr))
                        {
                            HandleSubscription();
                            //Response.Redirect(surveyUrl.cbSurveyUrl.ToString());
                            //HandleInitialSubscription(); //This is a valid email so we may continue
                        }
                        else
                        {
                            if(isSpanish)
                                ShowMessage(
                                    "Lo sentimos, pero la dirección de correo electrónico es inválida",
                                    "BadText",
                                    "Re-enter Address",
                                    "Esta dirección de correo electrónico no es válida. Por favor escriba una nueva dirección y vuelva a enviar.<br/>"
                                    );
                            else
                                ShowMessage(
                                    "Sorry, Invalid E-mail Address",
                                    "BadText",
                                    "Re-enter Address",
                                    "This is an invalid e-mail address. Please enter a new address and resubmit.<br/>"
                                    );

                            divSubscribe.Visible = true;
                        }
                    }
                    else
                    {
                        if (isSpanish)
                        {
                            //The email address has not been supplied.
                            ShowMessage(
                                "Lo sentimos, pero el espacio para la dirección de correo electrónico está vacío",
                                "BadText",
                                "Vuelva a escribir la dirección de correo electrónico",
                                "Usted no ha enviado una dirección de correo electrónico. Por favor escriba una nueva dirección y vuelva a enviar.<br/>"
                                );
                        }
                        else
                        {
                            //The email address has not been supplied.
                            ShowMessage(
                                "Sorry, Address Is Empty",
                                "BadText",
                                "Re-enter Address",
                                "You have not submitted an e-mail address. Please enter a new address and resubmit.<br/>"
                                );
                        } 
                        
                        divSubscribe.Visible = true;
                    }
                }
            }
            else
            {
                gNewsletterID = Strings.ToGuid(ConfigurationSettings.AppSettings["DCNewsletterID"]);
                gUserID = Strings.ToGuid(Request.Params["userid"]);

                if (gUserID != Guid.Empty)
                {
                    //Confirming the EMAIL
                    HandleConfirmation();
                }
                else
                {
                    divSubscribe.Visible = true;
                    if(isSpanish)
                        lblMessage.Text = "Para iniciar su subscrición gratuita al Boletín del Instituto Nacional del Cáncer, escriba su dirección de correo electrónico:";
                    else
                        lblMessage.Text = "To begin your free subscription to the <i>NCI Cancer Bulletin</i>, enter your e-mail address:";
                }
            }
        }


        private bool IsEmailValid(string email)
        {
            string strRegEx = @"^([-!#\$%&'*+./0-9=?A-Z^_`a-z{|}~])+@([-!#\$%&'*+/0-9=?A-Z^_`a-z{|}~]+\.)+([a-zA-Z]{2,5})$";
            return System.Text.RegularExpressions.Regex.IsMatch(email, strRegEx);
        }

        #region Survey Enum Converters

        private string GetLearnedAnswerString(LearnedAnswers answer)
        {
            if(isSpanish)
            {
                switch (answer)
                {
                    case LearnedAnswers.Colleague: return "Colega";
                    case LearnedAnswers.NCIWebSite: return "Sitio web del NCI (www.cancer.gov/espanol)";
                    case LearnedAnswers.Association: return "Asociación/organización";
                    case LearnedAnswers.Postcard: return "Aviso impreso";
                    case LearnedAnswers.Other: return "Otra";
                }
            }
            else
            {
                switch (answer)
                {
                    case LearnedAnswers.Colleague: return "Colleague";
                    case LearnedAnswers.NCIWebSite: return "NCI Web site (www.cancer.gov)";
                    case LearnedAnswers.Association: return "Association/organization";
                    case LearnedAnswers.Postcard: return "Postcard";
                    case LearnedAnswers.Other: return "Other";
                }
            }

            return "";
        }

        private string GetProfAnswerString(ProfAnswers answer)
        {
            if (isSpanish)
            {
                switch (answer)
                {
                    case ProfAnswers.Researcher: return "Investigador";
                    case ProfAnswers.Physician: return "Médico";
                    case ProfAnswers.Nurse: return "Enfermero o enfermera con práctica médica";
                    case ProfAnswers.OtherMed: return "Otro profesional médico";
                    case ProfAnswers.Patient: return "Paciente con cáncer o superviviente";
                    case ProfAnswers.Family: return "Familiar o amigo de un paciente con cáncer";
                    case ProfAnswers.Advocate: return "Defensor del paciente";
                    case ProfAnswers.Other: return "Otro";
                }
            }
            else
            {
                switch (answer)
                {
                    case ProfAnswers.Researcher: return "Researcher";
                    case ProfAnswers.Physician: return "Physician";
                    case ProfAnswers.Nurse: return "Nurse or Nurse Practitioner";
                    case ProfAnswers.OtherMed: return "Other medical professional";
                    case ProfAnswers.Patient: return "Cancer patient/survivor";
                    case ProfAnswers.Family: return "Cancer patient family member/friend";
                    case ProfAnswers.Advocate: return "Advocate";
                    case ProfAnswers.Other: return "Other";
                }
            }
            return "";
        }

        #endregion

        #region Survey Drawer

        private void DrawSurvey(Hashtable learnedItems, Hashtable profItems)
        {
            DrawLearnedCheckBoxes(learnedItems);
            DrawProfCheckBoxes(profItems);
        }

        private void DrawLearnedCheckBoxes(Hashtable selectedItems)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(DrawCheckBox(
                "chkSurveyLearned",
                (int)LearnedAnswers.Colleague,
                GetLearnedAnswerString(LearnedAnswers.Colleague),
                selectedItems.Contains(LearnedAnswers.Colleague)
                ));

            sb.Append(DrawCheckBox(
                "chkSurveyLearned",
                (int)LearnedAnswers.NCIWebSite,
                GetLearnedAnswerString(LearnedAnswers.NCIWebSite),
                selectedItems.Contains(LearnedAnswers.NCIWebSite)
                ));

            sb.Append(DrawCheckBoxWithTextBox(
                "chkSurveyLearned",
                (int)LearnedAnswers.Association,
                GetLearnedAnswerString(LearnedAnswers.Association) + ":&nbsp;&nbsp;",
                selectedItems.Contains(LearnedAnswers.Association),
                "txtSurveyLearnedOrgName",
                (string)selectedItems[LearnedAnswers.Association]
                ));

            sb.Append(DrawCheckBox(
                "chkSurveyLearned",
                (int)LearnedAnswers.Postcard,
                GetLearnedAnswerString(LearnedAnswers.Postcard),
                selectedItems.Contains(LearnedAnswers.Postcard)
                ));

            sb.Append(DrawCheckBoxWithTextBox(
                "chkSurveyLearned",
                (int)LearnedAnswers.Other,
                GetLearnedAnswerString(LearnedAnswers.Other) + ":&nbsp;&nbsp;",
                selectedItems.Contains(LearnedAnswers.Other),
                "txtSurveyLearnedOther",
                (string)selectedItems[LearnedAnswers.Other]
                ));

            Literal lit = new Literal();
            lit.Text = sb.ToString();
            phLearnedQuestions.Controls.Add(lit);
        }

        private void DrawProfCheckBoxes(Hashtable selectedItems)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(DrawCheckBox(
                "chkSurveyProf",
                (int)ProfAnswers.Researcher,
                GetProfAnswerString(ProfAnswers.Researcher),
                selectedItems.Contains(ProfAnswers.Researcher)
                ));
            sb.Append(DrawCheckBox(
                "chkSurveyProf",
                (int)ProfAnswers.Physician,
                GetProfAnswerString(ProfAnswers.Physician),
                selectedItems.Contains(ProfAnswers.Physician)
                ));
            sb.Append(DrawCheckBox(
                "chkSurveyProf",
                (int)ProfAnswers.Nurse,
                GetProfAnswerString(ProfAnswers.Nurse),
                selectedItems.Contains(ProfAnswers.Nurse)
                ));
            sb.Append(DrawCheckBoxWithTextBox(
                "chkSurveyProf",
                (int)ProfAnswers.OtherMed,
                GetProfAnswerString(ProfAnswers.OtherMed) + ":&nbsp;&nbsp;",
                selectedItems.Contains(ProfAnswers.OtherMed),
                "txtSurveyProfMedOther",
                (string)selectedItems[ProfAnswers.OtherMed]
                ));
            sb.Append(DrawCheckBox(
                "chkSurveyProf",
                (int)ProfAnswers.Patient,
                GetProfAnswerString(ProfAnswers.Patient),
                selectedItems.Contains(ProfAnswers.Patient)
                ));
            sb.Append(DrawCheckBox(
                "chkSurveyProf",
                (int)ProfAnswers.Family,
                GetProfAnswerString(ProfAnswers.Family),
                selectedItems.Contains(ProfAnswers.Family)
                ));
            sb.Append(DrawCheckBox(
                "chkSurveyProf",
                (int)ProfAnswers.Advocate,
                GetProfAnswerString(ProfAnswers.Advocate),
                selectedItems.Contains(ProfAnswers.Advocate)
                ));
            sb.Append(DrawCheckBoxWithTextBox(
                "chkSurveyProf",
                (int)ProfAnswers.Other,
                GetProfAnswerString(ProfAnswers.Other) + ":&nbsp;&nbsp;",
                selectedItems.Contains(ProfAnswers.Other),
                "txtSurveyProfOther",
                (string)selectedItems[ProfAnswers.Other]
                ));

            Literal lit = new Literal();
            lit.Text = sb.ToString();
            phProfQuestions.Controls.Add(lit);
        }

        #region Check box drawers

        private string DrawCheckBox(string name, int val, string text, bool isChecked)
        {

            StringBuilder sb = new StringBuilder();

            //20080828 SR - Updated for section 508

            //since name wont be unique on the page
            //use combination of name and val as id 
            string id = name + "_" + val;

            sb.Append(string.Format("<input type=\"checkbox\" name=\"{0}\" id=\"{1}\" value=\"{2}\"", name, id, val));

            if (isChecked)
                sb.Append(" checked=\"checked\" />");
            else
                sb.Append(" />");

            sb.Append(string.Format("<label for=\"{0}\">{1}</label>", id, text));
            sb.Append("<br />");
            return sb.ToString();
        }

        private string DrawCheckBoxWithTextBox(string name, int val, string text, bool isChecked, string textBoxName, string textBoxValue)
        {

            StringBuilder sb = new StringBuilder();

            //20080828 SR - Updated for section 508

            //since name wont be unique on the page
            //use combination of name and val as id 
            string id = name + "_" + val;

            //add check box with name and id
            sb.Append(string.Format("<input type=\"checkbox\" name=\"{0}\" id=\"{1}\" value=\"{2}\"", name, id, val));

            //set checked or unchecked value
            if (isChecked)
                sb.Append(" checked=\"checked\" />");
            else
                sb.Append(" />");

            //add a label for the check box
            sb.Append(string.Format("<label for=\"{0}\">{1}</label>", id, text));

            //add a text box and value...
            sb.Append(string.Format("<input type=\"text\" name=\"{0}\" id=\"{0}\"", textBoxName));

            if (textBoxValue != null && textBoxValue != string.Empty)
            {
                sb.Append(" value=\"");
                sb.Append(textBoxValue);
                sb.Append("\"");
            }
            sb.Append(" maxlength=\"500\" size=\"39\" />");

            //add a hidden label for the text box
            sb.Append(string.Format("<label for=\"{0}\" class=\"hidden\">{1}</label>", textBoxName, text));

            sb.Append("<br />");
            return sb.ToString();
        }

        #endregion
        #endregion

        #region Survey Reader

        private void ReadSurvey(Hashtable learnedItems, Hashtable profItems)
        {
            learnedItems.Clear();
            profItems.Clear();

            ReadLearnedSurvey(learnedItems);
            ReadProfSurvey(profItems);
        }

        private void ReadLearnedSurvey(Hashtable items)
        {
            ArrayList itemsList = Strings.ToArrayListOfInts(Request.Form["chkSurveyLearned"], ',');
            if (itemsList.Count > 0)
            {
                foreach (LearnedAnswers i in itemsList)
                {
                    switch (i)
                    {
                        case LearnedAnswers.Association:
                            {
                                items.Add(i, Strings.Clean(Request.Form["txtSurveyLearnedOrgName"]));
                                break;
                            }
                        case LearnedAnswers.Other:
                            {
                                items.Add(i, Strings.Clean(Request.Form["txtSurveyLearnedOther"]));
                                break;
                            }
                        default:
                            {
                                items.Add(i, null);
                                break;
                            }
                    }
                }
            }
        }

        private void ReadProfSurvey(Hashtable items)
        {
            ArrayList itemsList = Strings.ToArrayListOfInts(Request.Form["chkSurveyProf"], ',');
            if (itemsList.Count > 0)
            {
                foreach (ProfAnswers i in itemsList)
                {
                    switch (i)
                    {
                        case ProfAnswers.OtherMed:
                            {
                                items.Add(i, Strings.Clean(Request.Form["txtSurveyProfMedOther"]));
                                break;
                            }
                        case ProfAnswers.Other:
                            {
                                items.Add(i, Strings.Clean(Request.Form["txtSurveyProfOther"]));
                                break;
                            }
                        default:
                            {
                                items.Add(i, null);
                                break;
                            }
                    }
                }
            }
        }

        #endregion

        #region Survey Saving

        private void SaveSurvey(Hashtable learnedItems, Hashtable profItems)
        {

            //Convert hash tables to answers
            string question1 = GetLearnedAnswersString(learnedItems);
            string question2 = GetProfAnswersString(profItems);

            using (SqlConnection scnSurvey = new SqlConnection(NewsLetterDBConnection))
            {
                using (SqlCommand scSurvey = new SqlCommand("usp_AnswerNewsletterSurvey", scnSurvey))
                {

                    scSurvey.CommandType = CommandType.StoredProcedure;

                    scSurvey.Parameters.Add(
                        new SqlParameter("@SurveyID", gNewsletterID));
                    scSurvey.Parameters.Add(
                        new SqlParameter("@Question1", question1));
                    scSurvey.Parameters.Add(
                        new SqlParameter("@Question2", question2));
                    scSurvey.Parameters.Add(
                        new SqlParameter("@languageCode", languageCode));

                    scSurvey.Connection.Open();
                    scSurvey.ExecuteNonQuery();
                }
            }

        }

        private string GetLearnedAnswersString(Hashtable items)
        {
            StringBuilder sb = new StringBuilder();

            bool isFirst = true;
            foreach (DictionaryEntry pair in items)
            {
                if (isFirst)
                    isFirst = false;
                else
                    sb.Append(";");
                sb.Append(GetLearnedAnswerString((LearnedAnswers)pair.Key));
                sb.Append("|");
                if (pair.Value != null && (string)pair.Value != string.Empty)
                    sb.Append(pair.Value.ToString());
            }

            return sb.ToString();
        }

        private string GetProfAnswersString(Hashtable items)
        {
            StringBuilder sb = new StringBuilder();

            bool isFirst = true;
            foreach (DictionaryEntry pair in items)
            {
                if (isFirst)
                    isFirst = false;
                else
                    sb.Append(";");
                sb.Append(GetProfAnswerString((ProfAnswers)pair.Key));
                sb.Append("|");
                if (pair.Value != null && (string)pair.Value != string.Empty)
                    sb.Append(pair.Value.ToString());
            }

            return sb.ToString();
        }

        #endregion

        #region State Handlers

        private void HandleSurvey()
        {

            Hashtable learnedItems = new Hashtable();
            Hashtable profItems = new Hashtable();
            ReadSurvey(learnedItems, profItems);

            if (learnedItems.Count > 0 && profItems.Count > 0)
            {
                //Insert Into DB
                //throw new Exception(Request.Form["chkSurveyLearned"] + "<br />" + Request.Form["chkSurveyProf"]);
                try
                {
                    SaveSurvey(learnedItems, profItems);
                    //divMessageBox.Visible = false;
                    if (isSpanish)
                    {
                        ShowMessage("Gracias por su respuesta.",
                        "GoodText",
                        "",
                        "Su respuesta nos ayuda a entender mejor a nuestros lectores."
                        );
                    }
                    else
                    {
                        ShowMessage("Thank You for your response.",
                        "GoodText",
                        "",
                        "Your response helps us better understand our readers."
                        );
                    }
                    //lblHeader.Text = "<br/><br/>&nbsp;&nbsp;&nbsp;Thank You for your response.<br/><br/>Your response helps us better understand our readers.";
                }
                catch (System.Data.SqlClient.SqlException sqlE)
                {
                    //Other Error
                    if (isSpanish)
                    {
                        ShowMessage(
                            "Error",
                            "BadText",
                            "Error",
                            "Se presentó un error al procesar su solicitud<br/>"
                            );
                    }
                    else
                    {
                        ShowMessage(
                            "Error",
                            "BadText",
                            "Error",
                            "There was an error processing your request<br/>"
                            );
                    }

                    NCI.Logging.Logger.LogError("CancerBulletinSubscribe:HandleSurvey", "There was an error processing your request", NCIErrorLevel.Error, sqlE);

                }
            }
            else
            {
                //Need to keep checks?
                ShowMessage("Thank you for subscribing to the <i><b>NCI Cancer Bulletin</b></i>!",
                    "GoodText",
                    "Address Received",
                    "You will receive an e-mail shortly asking you to verify your e-mail address.  To start your subscription to the <i>NCI Cancer Bulletin</i>, simply respond to the confirmation e-mail.<br/>"
                    );

                divSurvey.Visible = true;
                lblSurveyMessage.CssClass = "BadText";
                DrawSurvey(learnedItems, profItems);
            }
        }

        private void HandleConfirmation()
        {
            //This is the link from the validation email			

            //We have a userid and a Newsletterid

            //Try and subscribe the user
            using (SqlConnection scnEmail = new SqlConnection(NewsLetterDBConnection))
            {
                using (SqlCommand scEmail = new SqlCommand("usp_Newsletter_AddSubscription", scnEmail))
                {

                    scEmail.CommandType = CommandType.StoredProcedure;

                    scEmail.Parameters.Add(
                        new SqlParameter("@UserID", gUserID));
                    scEmail.Parameters.Add(
                        new SqlParameter("@NewsletterID", gNewsletterID));
                    scEmail.Parameters.Add(
                        new SqlParameter("@Format", "HTML"));
                    scEmail.Parameters.Add(
                        new SqlParameter("@KeywordList", ""));

                    try
                    {
                        scEmail.Connection.Open();
                        scEmail.ExecuteNonQuery();

                        ShowMessage(
                            "Thank you for subscribing to the <i><b>NCI Cancer Bulletin</b></i>!",
                            "GoodText",
                            "Subscription Confirmed",
                            "<p>Thank you for subscribing to the <i>NCI Cancer Bulletin</i>. With each issue, " +
                            "you can expect to receive by e-mail the most timely and relevant information about the cancer community.</p>" +
                            "<p>We trust that the <i>NCI Cancer Bulletin</i> will soon be a must-read for you and your colleagues.</p>" +
                            "<a href=\"/ncicancerbulletin\">View the <i>NCI Cancer Bulletin</i> home page</a><br/>"
                            );
                    }
                    catch (System.Data.SqlClient.SqlException sqlE)
                    {
                        if (sqlE.Number == 2627)
                        {
                            ShowMessage(
                                "This e-mail address has already been confirmed.<br>",
                                "BadText",
                                "Address Already Confirmed",
                                "Address Already Confirmed<br/>"
                                );

                        }
                        else
                        {
                            //Other Error
                            if (isSpanish)
                            {
                                ShowMessage(
                                    "Error",
                                    "BadText",
                                    "Error",
                                    "Se presentó un error al procesar su solicitud<br/>"
                                    );
                            }
                            else
                            {
                                ShowMessage(
                                    "Error",
                                    "BadText",
                                    "Error",
                                    "There was an error processing your request<br/>"
                                    );
                            }
                            NCI.Logging.Logger.LogError("CancerBulletinSubscribe:HandleConfirmation", "There was an error processing your request", NCIErrorLevel.Error, sqlE);

                        }
                    }
                }
            }
        }
        private void HandleSubscription()
        {
            string toAddress = ConfigurationSettings.AppSettings["ListServe"];
            string fromAddress = strEmailAddr;
            string eMailbody = (isSpanish ? "quiet subscribe nci-boletin no name" : "quiet subscribe NCI-Bulletin no name");

            try
            {
                using (MailMessage mess = new MailMessage(fromAddress, toAddress, string.Empty, eMailbody))
                {
                    SmtpClient client = new SmtpClient();
                    client.Send(mess);
                }

                //System.Net.Mail.MailMessage mailMsg = new System.Net.Mail.MailMessage(fromAddress, toAddress);
                ////mailMsg.BodyEncoding = System.Text.Encoding.UTF8;
                ////mailMsg.Subject = "Confirm Your Subscription";
                ////mailMsg.IsBodyHtml = true;
                //mailMsg.Body += ConfigurationSettings.AppSettings["ListServeMessageBody"];

                //System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient();
                //smtpClient.Send(mailMsg);

                if (isSpanish)
                {
                    ShowMessage("Gracias por subscribirse al <i><b>Boletín del Instituto Nacional del Cáncer</b></i>!", 
                        "GoodText",
                        "Address Received",
                        ""
                        );
                }
                else
                {
                    ShowMessage("Thank you for subscribing to the <i><b>NCI Cancer Bulletin</b></i>!",
                        "GoodText",
                        "Address Received",
                        ""
                        );
                }

                // Web Analytics *************************************************
                if (WebAnalyticsOptions.IsEnabled)
                    this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.Events.Subscription, wbField =>
                    {
                        wbField.Value = "";
                    });

                // End Web Analytics **********************************************

                lblSurveyMessage.CssClass = "GoodText";
                divSurvey.Visible = true;
                DrawSurvey(new Hashtable(), new Hashtable());
            }

            catch (Exception ex)
            {
                NCI.Logging.Logger.LogError("CancerBulletinSubscribe:HandleInitialSubscription", "There was an error processing your request", NCIErrorLevel.Error, ex);

            }

        }

        private void HandleInitialSubscription()
        {

            using (SqlConnection scnEmail = new SqlConnection(NewsLetterDBConnection))
            {
                using (SqlCommand scEmail = new SqlCommand("usp_Newsletter_AddUser2", scnEmail))
                {

                    scEmail.CommandType = CommandType.StoredProcedure;
                    scEmail.Parameters.Add(
                        new SqlParameter("@Email", strEmailAddr));
                    scEmail.Parameters.Add(
                        new SqlParameter("@NewsletterID", gNewsletterID));

                    try
                    {

                        scEmail.Connection.Open();

                        object objTmpUserID = scEmail.ExecuteScalar();

                        //Close the connection so it is still not open while waiting for the email
                        scEmail.Connection.Close();

                        //Since we say to send, then userid should be good
                        if ((objTmpUserID != null) && (objTmpUserID != System.DBNull.Value))
                        {
                            Guid tmpUserID = Strings.ToGuid(objTmpUserID.ToString());

                            if (tmpUserID != Guid.Empty)
                            {
                                SendValidationEmail(tmpUserID, gNewsletterID, strEmailAddr);

                                ShowMessage("Thank you for subscribing to the <i><b>NCI Cancer Bulletin</b></i>!",
                                    "GoodText",
                                    "Address Received",
                                    "You will receive an e-mail shortly asking you to verify your e-mail address.  To start your subscription to the <i>NCI Cancer Bulletin</i>, simply respond to the confirmation e-mail.<br/>"
                                    );

                                // Web Analytics *************************************************
                                if (WebAnalyticsOptions.IsEnabled)
                                    this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.Events.Subscription, wbField =>
                                    {
                                        wbField.Value = "";
                                    });

                                // End Web Analytics **********************************************

                                lblSurveyMessage.CssClass = "GoodText";
                                divSurvey.Visible = true;
                                DrawSurvey(new Hashtable(), new Hashtable());

                            }
                            else
                            {
                                //Invalid UserID
                                ShowMessage(
                                    "Error",
                                    "BadText",
                                    "Error",
                                    "There was an error processing your request"
                                    );
                            }
                        }
                        else
                        {
                            ShowMessage(
                                "Error",
                                "BadText",
                                "Error",
                                "There was an error processing your request"
                                );

                        }
                    }
                    catch (System.Data.SqlClient.SqlException sqlE)
                    {
                        if (sqlE.Number == 50000)
                        {
                            ShowMessage(
                                "Sorry, Address Is Already Subscribed",
                                "BadText",
                                "Re-enter Address",
                                "This e-mail address is already subscribed, please enter a new address and resubmit.<br/>"
                                );
                            divSubscribe.Visible = true;
                        }
                        else
                        {
                            ShowMessage(
                                "Error",
                                "BadText",
                                "Error",
                                "There was an error processing your request"
                                );

                            NCI.Logging.Logger.LogError("CancerBulletinSubscribe:HandleInitialSubscription", "There was an error processing your request", NCIErrorLevel.Error, sqlE);

                        }
                    }
                }
            }
        }

        #endregion

        private void ShowMessage(string header, string className, string info, string message)
        {
            //Make the others invisible
            lblMessage.Visible = true;
            lblMessage.Text = message;

            lblInfo.CssClass = className;
            lblInfo.Text = info;
            lblHeader.Text = header;
        }

        public void SendValidationEmail(Guid userID, Guid newsletterID, string strEmailAddr)
        {
            string toAddress = strEmailAddr;
            string fromAddress = "\"NCI Cancer Bulletin\" <ncicancerbulletin@mail.nih.gov>";  //I had to comment this out in order for it to work?

            System.Net.Mail.MailMessage mailMsg = new System.Net.Mail.MailMessage(fromAddress, toAddress);
            mailMsg.BodyEncoding = System.Text.Encoding.UTF8;
            mailMsg.Subject = "Confirm Your Subscription";
            mailMsg.IsBodyHtml = true;
            mailMsg.Body += "To begin your subscription to the <i>NCI Cancer Bulletin</i>, please <a href=\"" + System.Configuration.ConfigurationSettings.AppSettings["RootUrl"] + PrettyUrl +
                "?cid=bulletin_confirm" +
                "&userid=" +
                userID.ToString() +
                "&newsletterid=" +
                newsletterID.ToString() +
                "\">click here</a> to confirm your e-mail address.";

            System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient();
            smtpClient.Send(mailMsg);
        }
    }
}
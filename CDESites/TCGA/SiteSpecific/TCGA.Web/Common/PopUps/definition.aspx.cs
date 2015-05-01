using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NCI.Web.CDE;
using System.Collections;
using System.Text.RegularExpressions;
using System.Data;
using System.Configuration;
using NCI.Util;
using CancerGov.CDR.TermDictionary;
using CancerGov.UI;
using CancerGov.Common;
using CancerGov.UI.HTML;
using NCI.Logging;
namespace TCGA.Web.Common.PopUps
{
    public partial class Definition : PopUpPage
    {
        private IRenderer content;
        protected string strSendPrinter = "Send to Printer";
        //protected string strHeading = "<h3 class='popup-definition'>Definition from NCI's Dictionary of Cancer Terms</h3>";
        protected string strHeading = "<div class=\"heading\">Definition:</div>";
        #region Page properties

        public IRenderer Content
        {
            get { return content; }
            set { content = value; }
        }

        #endregion

        /// <summary>
        /// Default web form class constructor
        /// </summary>
        public Definition()
        {
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            //base.OnLoad(e);
            string input_term=string.Empty;
            string term = string.Empty;
            string id = string.Empty;
            string mediaHtml = "";
            PDQVersion version;
            string pronunciation = string.Empty;
            string termDefinition = string.Empty;

            DisplayLanguage dl = new DisplayLanguage();

            if (Request.QueryString["language"] == "English")
                dl = DisplayLanguage.English;
            else if (Request.QueryString["language"] == "Spanish")
                dl = DisplayLanguage.Spanish;
            else
                dl = DisplayLanguage.English;


            try
            {

                //include page title
                this.pageHtmlHead.Title = "Definition - National Cancer Institute";
                input_term = Strings.Clean(Request.Params["term"]);
                id = Strings.IfNull(Strings.Clean(Request.Params["id"]), Strings.Clean(Request.Params["cdrid"]));
                //version = PDQVersion.Patient;
            }
            catch(Exception ex)
            {
                Logger.LogError("TCGA:Definition.cs:PageLoad", "", NCIErrorLevel.Error, ex);

            }
            version = PDQVersionResolver.GetPDQVersion(Strings.Clean(Request.Params["version"]));

            ArrayList result = null;
            try
            {
                term = "Error";
                pronunciation = "invalid input";
                termDefinition = string.Empty;

                if (input_term == null && id == null)
                {
                    termDefinition = "You have not specified either a CDRID nor a term name.";
                }
                else
                {
                    if (input_term != null && input_term.Length > 0)
                    {
                        result = get_definition("term", input_term, version, dl);
                        if (result == null && input_term.EndsWith("s"))
                        {
                            result = get_definition("term", input_term.Substring(0, input_term.Length - 1), version, dl);
                            if (result == null && input_term.EndsWith("es"))
                            {
                                result = get_definition("term", input_term.Substring(0, input_term.Length - 2), version, dl);
                                if (result == null && input_term.EndsWith("ies"))
                                {
                                    result = get_definition("term", input_term.Substring(0, input_term.Length - 3) + "y", version, dl);
                                    if (result == null && input_term.EndsWith("um"))
                                    {
                                        result = get_definition("term", input_term.Substring(0, input_term.Length - 2) + "a", version, dl);
                                        if (result == null && input_term.EndsWith("ly"))
                                        {
                                            result = get_definition("term", input_term.Substring(0, input_term.Length - 2), version, dl);
                                            if (result == null && input_term.EndsWith("ii"))
                                            {
                                                result = get_definition("term", input_term.Substring(0, input_term.Length - 2) + "us", version, dl);
                                            }

                                        }

                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        id = Regex.Replace(id, "^CDR0+", "", RegexOptions.Compiled);
                        result = get_definition("id", id, version, dl);
                    }

                    if (result == null)
                    {
                        term = "term not found";
                        pronunciation = "";
                        termDefinition = "The term you are looking for does not exist in the glossary.";
                    }
                    else
                    {
                        term = result[0].ToString();
                        pronunciation = result[1].ToString();
                        termDefinition = result[2].ToString();
                        mediaHtml = result[3].ToString();
                        mediaHtml = mediaHtml.Replace("[__imagelocation]", ConfigurationSettings.AppSettings["CDRImageLocation"]);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("TCGA:Definition.cs:PageLoad", "", NCIErrorLevel.Error, ex);

            }
            if (dl == DisplayLanguage.Spanish)
            {
                strSendPrinter = "Imprima esta página";
                pronunciation = String.Empty;
                //strHeading = "<h3 class='popup-definition'>Definición del Diccionario de cáncer del NCI</h3>";
                strHeading = "<div class=\"heading\">Definición:</div>";
            }

            content = new HtmlSegment("<div class=\"audioPronounceLink\">" 
                + String.Format("<span class=\"term\">{0}</span>", term) 
                + String.Format("<span class=\"pronunciation\">{0}</span>", ((Strings.Clean(pronunciation) != null) ? " " + pronunciation : "")) 
                + "</div>" 
                + String.Format("<div class=\"definition\">{0}</div>", termDefinition) 
                + String.Format("<div class=\"definitionImage\">{0}</div>", mediaHtml)
                );
                
                
                
                
                //String.Format("<span class=\"black-text-b\">{0}</span>", term) + ((Strings.Clean(pronunciation) != null) ? " " + pronunciation : "") + "<p>" + termDefinition + "<p>" + mediaHtml);

        }

        private ArrayList get_definition(string type, string param, PDQVersion pdqVersion, DisplayLanguage language)
        {
            string lng = string.Empty;
            if (language == DisplayLanguage.English)
            {
                lng = "English";
            }
            else
            {
                lng = "Spanish";
            }

            ArrayList returnvalue = new ArrayList(3);
            returnvalue = CancerGov.CDR.TermDictionary.TermDictionaryManager.GetDefinition(type, param, pdqVersion, lng);
            return returnvalue;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
        }

        #region Web Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using NCI.Web.CDE;
using CancerGov.UI;
using TCGA.Web;
using NCI.Web.CDE.Application;

namespace TCGA.Web.Common.PopUps
{
    public partial class popImage : PopUpPage
    {
        private string imageName = "";
        private string caption = "";

        public string ImageName
        {
            get { return imageName; }
        }

        public string Caption
        {
            get { return caption; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Object o = Request.Params["imageName"];

            if (o != null)
                imageName = o.ToString();
            if (imageName == "")
            {
                ErrorPageDisplayer.RaisePageNotFound(this.GetType().ToString());
            }

            o = Request.Params["caption"];
            //if (o != null)
            //    caption = o.ToString();

            //This is a hack for SCR30157, which IIS7 has some bugs in encoding non-ascii character.

            Encoding unicode = Encoding.UTF8;
            Encoding latin1Code = Encoding.GetEncoding(28591);

            // Convert the string into a byte[] from UTF8 to western european 
            byte[] unicodeBytes = unicode.GetBytes(Request.RawUrl.ToString());
            byte[] latinBytes = Encoding.Convert(unicode, latin1Code, unicodeBytes);
            string latincap = unicode.GetString(latinBytes); //Get back to Unicode string.

            //use regular expression to get caption out
            Regex r = new Regex(@"caption=(([^&]*))?");
            Match match = r.Match(latincap);

            latincap = HttpUtility.UrlDecode(match.Groups[1].Value);

            if (o != null)
                caption = latincap.ToString();

            caption = HttpUtility.HtmlEncode(caption);

            string bottomCaption = string.Empty;
            try
            {
                // This server variables is not always present in the request.
                if (!isStringHtmlEncoded(caption))
                {
                    // This is really wierd accessing the params or query string 'caption=' value here, you will
                    // find that the values have the funny square chars in them. but some how accessing the rawurl 
                    // property does not have the same issue. 
                    bottomCaption = Request.RawUrl.Substring(Request.RawUrl.IndexOf("?"));
                    bottomCaption = bottomCaption.Substring(bottomCaption.IndexOf("caption=") + "caption=".Length);
                }
            }
            catch { bottomCaption = string.Empty; }
            if (bottomCaption != string.Empty)
                caption = bottomCaption.Replace("%20", " ");
        }

        //"C&#233;lulas sangu&#237;neas. La sangre contiene muchos tipos de c&#233;lulas: gl&#243;bulos blancos (monolitos, linfocitos, neutr&#243;filos, eosin&#243;filos, bas&#243;filos y macr&#243;fagos), gl&#243;bulos rojos (eritrocitos) y plaquetas. La sangre circula por el cuerpo a trav&#233;s de las arterias y las venas."
        private bool isStringHtmlEncoded(string val)
        {
            string checkForEncoded = string.Empty;
            if (val.IndexOf("&#237;") >= 0 || val.IndexOf("&#192;") >= 0 || val.IndexOf("&#193;") >= 0 || val.IndexOf("&#194;") >= 0 ||
                val.IndexOf("&#195;") >= 0 || val.IndexOf("&#196;") >= 0 || val.IndexOf("&#197;") >= 0 || val.IndexOf("&#198;") >= 0 || val.IndexOf("&#199;") >= 0 ||
                val.IndexOf("&#200;") >= 0 ||
                val.IndexOf("&#201;") >= 0 ||
                val.IndexOf("&#202;") >= 0 ||
                val.IndexOf("&#203;") >= 0 ||
                val.IndexOf("&#204;") >= 0 ||
                val.IndexOf("&#205;") >= 0 ||
                val.IndexOf("&#206;") >= 0 ||
                val.IndexOf("&#207;") >= 0 ||
                val.IndexOf("&#208;") >= 0 ||
                val.IndexOf("&#209;") >= 0 ||
                val.IndexOf("&#210;") >= 0 ||
                val.IndexOf("&#211;") >= 0 ||
                val.IndexOf("&#212;") >= 0 ||
                val.IndexOf("&#213;") >= 0 ||
                val.IndexOf("&#214;") >= 0 ||
                val.IndexOf("&#216;") >= 0 ||
                val.IndexOf("&#217;") >= 0 ||
                val.IndexOf("&#218;") >= 0 ||
                val.IndexOf("&#219;") >= 0 ||
                val.IndexOf("&#220;") >= 0 ||
                val.IndexOf("&#221;") >= 0 ||
                val.IndexOf("&#222;") >= 0 ||
                val.IndexOf("&#223;") >= 0 ||
                val.IndexOf("&#224;") >= 0 ||
                val.IndexOf("&#225;") >= 0 ||
                val.IndexOf("&#226;") >= 0 ||
                val.IndexOf("&#227;") >= 0 ||
                val.IndexOf("&#228;") >= 0 ||
                val.IndexOf("&#229;") >= 0 ||
                val.IndexOf("&#230;") >= 0 ||
                val.IndexOf("&#231;") >= 0 ||
                val.IndexOf("&#232;") >= 0 ||
                val.IndexOf("&#233;") >= 0 ||
                val.IndexOf("&#234;") >= 0 ||
                val.IndexOf("&#235;") >= 0 ||
                val.IndexOf("&#236;") >= 0 ||
                val.IndexOf("&#237;") >= 0 ||
                val.IndexOf("&#238;") >= 0 ||
                val.IndexOf("&#239;") >= 0 ||
                val.IndexOf("&#240;") >= 0 ||
                val.IndexOf("&#241;") >= 0 ||
                val.IndexOf("&#242;") >= 0 ||
                val.IndexOf("&#243;") >= 0 ||
                val.IndexOf("&#244;") >= 0 ||
                val.IndexOf("&#245;") >= 0 ||
                val.IndexOf("&#246;") >= 0 ||
                val.IndexOf("&#248;") >= 0 ||
                val.IndexOf("&#249;") >= 0 ||
                val.IndexOf("&#250;") >= 0 ||
                val.IndexOf("&#251;") >= 0 ||
                val.IndexOf("&#252;") >= 0 ||
                val.IndexOf("&#253;") >= 0)
                return true;
            return false;
        }
    }
}

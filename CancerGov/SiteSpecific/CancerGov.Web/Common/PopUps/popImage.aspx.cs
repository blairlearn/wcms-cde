using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using CancerGov.Web;
namespace Www.Common.PopUps
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
                Response.Redirect(ConfigurationSettings.AppSettings["NotFoundPage"], true);
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
        }
    }
}

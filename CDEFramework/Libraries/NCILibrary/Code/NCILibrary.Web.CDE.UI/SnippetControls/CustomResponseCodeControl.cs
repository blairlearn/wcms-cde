using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Common.Logging;

namespace NCI.Web.CDE.UI.SnippetControls
{
    public class CustomResponseCodeControl : SnippetControl
    {
        static ILog log = LogManager.GetLogger(typeof(CustomResponseCodeControl));

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            dynamic config = JObject.Parse(this.SnippetInfo.Data);
    
            try
            {
                HttpContext.Current.Response.StatusCode = config.code;
                HttpContext.Current.Response.StatusDescription = config.description;
                HttpContext.Current.Response.TrySkipIisCustomErrors = true;
            }
            catch (Exception ex)
            {
                log.WarnFormat("Could not set response code {0} or description {1} for page.", ex, config.code, config.description);
            }

            this.Visible = false;
        }
    }
}

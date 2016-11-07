using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;

namespace NCI.Web.CDE.UI.SnippetControls
{
    public class FilterTitleSnippet : SnippetControl
    {
        private DateTime filterDate = DateTime.MinValue;
        private String formattedDate = String.Empty;
        static private String englishDateFormat = "{0} - {1}";
        static private String espanolDateFormat = "{0} - {1}";
        static private String englishArchiveFormat = "{0} Archive";
        static private String espanolArchiveFormat = "{0} Archivo";

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            fillFormattedDate();

            //set the page title as the protocol title
            PageInstruction.AddFieldFilter("long_title", (fieldName, data) =>
            {
                if (formattedDate == String.Empty)
                {
                    data.Value = String.Format((PageInstruction.Language == "es" ? espanolArchiveFormat : englishArchiveFormat),
                        (String.IsNullOrEmpty(PageInstruction.GetField("short_title")) ? data.Value : PageInstruction.GetField("short_title")));
                }
                else
                {
                    data.Value = String.Format((PageInstruction.Language == "es" ? espanolDateFormat : englishDateFormat),
                        (String.IsNullOrEmpty(PageInstruction.GetField("short_title")) ? data.Value : PageInstruction.GetField("short_title")), formattedDate);
                }
            });
        }

        private void fillFormattedDate()
        {
            int year = 0;
            Dictionary<string, string> filters = GetUrlFilters();
            if (filters.ContainsKey("year"))
            {
                try
                {
                    year = Int32.Parse(filters["year"]);
                    if (filters.ContainsKey("month"))
                    {
                        try
                        {
                            int month = Int32.Parse(filters["month"]);
                            filterDate = new DateTime(year, month, 1);
                            formattedDate = filterDate.ToString("MMMM yyyy", CultureInfo.CurrentUICulture.DateTimeFormat);
                        }
                        catch
                        {
                            NCI.Web.CDE.Application.ErrorPageDisplayer.RaisePageByCode("BaseSearchSnippet", 404, "Invalid month parameter in dynamic list filter");
                        }
                    }
                    else
                    {
                        filterDate = new DateTime(year, 1, 1);
                        formattedDate = filterDate.ToString("yyyy");
                    }
                }
                catch
                {
                    NCI.Web.CDE.Application.ErrorPageDisplayer.RaisePageByCode("BaseSearchSnippet", 404, "Invalid year parameter in dynamic list filter");
                }
            }
        }

        private Dictionary<string, string> GetUrlFilters()
        {
            Dictionary<string, string> urlParams = new Dictionary<string, string>();
            Regex pattern = new Regex(@"filter\[([^]]*)\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            foreach (string key in HttpContext.Current.Request.QueryString.AllKeys)
            {
                if (pattern.IsMatch(key))
                {
                    Match match = pattern.Match(key);
                    urlParams.Add(match.Groups[1].Value, HttpContext.Current.Request.QueryString[match.Value]);
                }
            }

            return urlParams;
        }
    }
}

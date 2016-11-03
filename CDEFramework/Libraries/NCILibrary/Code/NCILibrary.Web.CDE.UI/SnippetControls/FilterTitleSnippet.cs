using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;

namespace NCI.Web.CDE.UI.SnippetControls
{
    public class FilterTitleSnippet : SnippetControl
    {
        private String formattedDate = String.Empty;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            fillFormattedDate();

            //set the page title as the protocol title
            PageInstruction.AddFieldFilter("long_title", (fieldName, data) =>
            {
                if (formattedDate == String.Empty)
                {
                    data.Value = PageInstruction.GetField("short_title") + " Archive";
                }
                else
                {
                    data.Value = PageInstruction.GetField("short_title") + ": " + formattedDate + " Archive";
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
                            //int lastDay = DateTime.DaysInMonth(year, month);
                            //startDate = new DateTime(year, month, 1);
                            //endDate = new DateTime(year, month, lastDay);
                            formattedDate = month.ToString() + "/" + year.ToString();
                        }
                        catch
                        {
                            NCI.Web.CDE.Application.ErrorPageDisplayer.RaisePageByCode("BaseSearchSnippet", 404, "Invalid month parameter in dynamic list filter");
                        }
                    }
                    else
                    {
                        //startDate = new DateTime(year, 1, 1);
                        //endDate = new DateTime(year, 12, 31);
                        formattedDate = year.ToString();
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

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace NCI.Web.UI.WebControls
{
    /// <summary>
    /// This is a web control for the blog Archive in the Right Rail
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:BlogSeriesArchiveWebControl runat=server></{0}:BlogSeriesArchiveWebControl>")]
    public class BlogSeriesArchiveControl : WebControl
    {
        public BlogSeriesArchiveControl()
        {
            Years = 1;
        }
        
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public int Years
        {
            get {
                object temp = ViewState["Years"];
                return temp == null ? 0 : 1;
            }
            set { ViewState["Years"] = value; }
        }

        public DataTable results { get; set; }

        #region Protected Methods

        protected override void RenderContents(HtmlTextWriter output)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<div class=\"blog-archive-accordion managed list\"><p id=\"archive\" class=\"title\">Archive</p><div class=\"archive-accordion-expand global-expand\">+</div></div>");
            sb.Append("<div class=\"blog-archive-accordion-panel archive-panel\">");
            sb.Append("<section id=\"select\">");
            sb.Append("<div class=\"archives-list\">");
            
            // Structure for the list is created above, now iterate over the rows
            // and group the dates by year
            List<BlogListItem> monthList = new List<BlogListItem>();
            for(var i = 0; i< results.Rows.Count; i++){
                var row = results.Rows[i];
                monthList.Add(new BlogListItem(Convert.ToInt32(row[0]), Convert.ToInt32(row[1]), Convert.ToInt32(row[2])));                
            }
            var returnedYears = monthList.Select(e => e.Year).Distinct();

            foreach (var year in returnedYears)
            {
                // Get all the months within the row collection for this year.
                List<BlogListItem> rowsToPrint = monthList.Where(e => e.Year == year).ToList();
                rowsToPrint.Sort((x, y) => y.Month.CompareTo(x.Month));

                // We will iterate over all the months within the year only if there are
                // Months within that year that actually have blog pots.
                if (rowsToPrint.Any(month => month.Quantity > 0))
                {
                    sb.Append("<div class=\"blog-archive-year-container\">");
                    sb.Append("<div class=\"blog-archive-accordion\"><div class=\"archive-year-header\">" + year + "</div><div class=\"archive-accordion-expand\">+</div></div>");
                    sb.Append("<div class=\"blog-archive-accordion-panel archive-year-panel\">");
                    sb.Append("<ul>");
                    foreach (var value in rowsToPrint)
                    {
                        var rowYear = value.Year;
                        var month = value.Month;                        
                        var quantity = value.Quantity;
                        string monthStr = "";
                        switch (month)
                        {
                            case 1: monthStr = "January";
                                break;
                            case 2: monthStr = "February";
                                break;
                            case 3: monthStr = "March";
                                break;
                            case 4: monthStr = "April";
                                break;
                            case 5: monthStr = "May";
                                break;
                            case 6: monthStr = "June";
                                break;
                            case 7: monthStr = "July";
                                break;
                            case 8: monthStr = "August";
                                break;
                            case 9: monthStr = "September";
                                break;
                            case 10: monthStr = "October";
                                break;
                            case 11: monthStr = "November";
                                break;
                            case 12: monthStr = "December";
                                break;
                            default: monthStr = "";
                                break;
                        }

                        sb.Append("<li class=\"month\">");

                        if(quantity > 0) // Print the month as a link
                            sb.Append("<a class href=\"/news-events/cancer-currents-blog/archive?filter[year]=" + year + "&filter[month]=" + month + "\">" + monthStr + "</a>");
                        else
                            sb.Append(monthStr);
                        sb.Append(" (" + quantity + ")");
                        sb.Append("</li>");
                    }
                    sb.Append("</ul>");
                    sb.Append("</div>"); // End the blog-archive-accordion-panel div
                    sb.Append("</div>"); // End the blog-archive-year-container div
                }                
            }
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</section>");
            sb.Append("</div>");
            output.Write(sb.ToString());
            
        }

        #endregion

        #region Private Methods


       

        #endregion


        private class BlogListItem
        {
            public int Year, Month, Quantity;
            public BlogListItem(int year, int month, int quantity)
            {
                Year = year;
                Month = month;
                Quantity = quantity;
            }
        }
    }
}

        
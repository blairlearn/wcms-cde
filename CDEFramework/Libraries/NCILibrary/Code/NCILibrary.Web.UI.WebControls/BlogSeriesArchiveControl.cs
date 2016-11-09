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
        public BlogSeriesArchiveControl(string language, string groupBy)
        {
            Language = language;
            GroupBy = groupBy;
        }

        private string Language;
        private string GroupBy;
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public int Years
        {
            get
            {
                object temp = ViewState["Years"];
                return temp == null ? 0 : (int)temp;
            }
            set { ViewState["Years"] = value; }
        }


        public DataTable results { get; set; }

        #region Protected Methods

        protected override void RenderContents(HtmlTextWriter output)
        {
            if (GroupBy.ToLower().Equals("year"))
            {
                output.Write(RenderAccordionOnYear());
            }
            else if (GroupBy.ToLower().Equals("month"))
            {
                output.Write(RenderAccordionOnMonth());
            }
        }

        #endregion

        #region Private Methods

        private string RenderAccordionOnYear()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=\"desktop\">" +
                      "<div id=\"blog-archive-accordion\">");
            sb.Append("<h3 id=\"archive\" class=\"blog-archive-header\">Archive</h3>");
            sb.Append("<div id=\"blog-archive-accordion-year\">");
            sb.Append("<ul>");
            // Structure for the list is created above, now iterate over the rows
            // and group the dates by year
            List<BlogListItem> monthList = new List<BlogListItem>();
            for (var i = 0; i < results.Rows.Count; i++)
            {
                var row = results.Rows[i];
                monthList.Add(new BlogListItem(Convert.ToInt32(row[0]), Convert.ToInt32(row[1]), Convert.ToInt32(row[2])));
            }
            var returnedYears = monthList.Select(e => e.Year).Distinct();

            foreach (var year in returnedYears)
            {
                // Get all the months within the row collection for this year.
                var blogCount = monthList.Where(e => e.Year == year).ToList().Count();

                sb.Append("<li class=\"year\">");
                if (blogCount > 0) // Print the month as a link
                    sb.Append("<a class href=\"/news-events/cancer-currents-blog/archive?filter[year]=" + year + "\">" + year + "</a>");
                else
                    sb.Append(year);
                sb.Append(" (" + blogCount + ")");
                sb.Append("</li>");
            }
            sb.Append("</ul>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            return sb.ToString();
        }

        private string RenderAccordionOnMonth()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=\"desktop\">" +
                      "<div id=\"blog-archive-accordion\">");
            sb.Append("<h3 id=\"archive\" class=\"blog-archive-header\">Archive</h3>");
            sb.Append("<div id=\"blog-archive-accordion-year\">");

            // Structure for the list is created above, now iterate over the rows
            // and group the dates by year
            List<BlogListItem> monthList = new List<BlogListItem>();
            for (var i = 0; i < results.Rows.Count; i++)
            {
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
                    sb.Append("<h4>" + year + "</h4>");
                    sb.Append("<ul>");
                    foreach (var value in rowsToPrint)
                    {
                        var month = value.Month;
                        var quantity = value.Quantity;
                        string monthStr = GetMonthString(month);


                        sb.Append("<li class=\"month\">");

                        if (quantity > 0) // Print the month as a link
                            sb.Append("<a class href=\"/news-events/cancer-currents-blog/archive?filter[year]=" + year + "&filter[month]=" + month + "\">" + monthStr + "</a>");
                        else
                            sb.Append(monthStr);
                        sb.Append(" (" + quantity + ")");
                        sb.Append("</li>");
                    }
                    sb.Append("</ul>");
                }
            }
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");
            return sb.ToString();
        }

        
        private string GetMonthString(int month)
        {
            var monthStr = "";
            var isEnglish = Language == "en";
            switch (month)
            {
                case 1:
                    if (isEnglish)
                        monthStr = "January";
                    else
                        monthStr = "enero";
                    break;
                case 2:
                    if (isEnglish)
                        monthStr = "February";
                    else
                        monthStr = "febrero";
                    break;
                    break;
                case 3:
                    if (isEnglish)
                        monthStr = "March";
                    else
                        monthStr = "marzo";
                    break;
                case 4:
                    if (isEnglish)
                        monthStr = "April";
                    else
                        monthStr = "abril";
                    break;
                case 5:
                    if (isEnglish)
                        monthStr = "May";
                    else
                        monthStr = "mayo";
                    break;
                case 6:
                    if (isEnglish)
                        monthStr = "June";
                    else
                        monthStr = "junio";
                    break;
                case 7:
                    if (isEnglish)
                        monthStr = "July";
                    else
                        monthStr = "julio";
                    break;
                case 8:
                    if (isEnglish)
                        monthStr = "August";
                    else
                        monthStr = "agosto";
                    break;
                case 9:
                    if (isEnglish)
                        monthStr = "September";
                    else
                        monthStr = "septiembre";
                    break;
                case 10:
                    if (isEnglish)
                        monthStr = "October";
                    else
                        monthStr = "octubre";
                    break;
                case 11:
                    if (isEnglish)
                        monthStr = "November";
                    else
                        monthStr = "noviembre";
                    break;
                case 12:
                    if (isEnglish)
                        monthStr = "December";
                    else
                        monthStr = "deciembre";
                    break;
                default: monthStr = "";
                    break;
            }
            return monthStr;
        }
       

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

        
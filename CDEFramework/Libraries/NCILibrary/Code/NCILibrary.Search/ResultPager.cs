using System;
using System.Text;

namespace NCI.Search
{
    ///<summary>
    ///Construct to manage the navigation and paging of search results<br/>
    ///<br/>
    ///<b>Author</b>:  Greg Andres<br/>
    ///<b>Date</b>:  8-8-2001<br/>
    ///<br/>
    ///<b>Revision History</b>:<br/>
    ///<br/>
    ///</summary>
    #region ResultPager class

    public class ResultPager
    {
        private int showPages = 10;
        private int currentPage = 0;
        private int recordsPerPage = 10;
        private int recordCount = 0;
        private string pageBaseUrlFormat = "javascript:page('{0}');";

        #region Properties

        /// <summary>
        /// Property sets index of current page view
        /// </summary>
        public int CurrentPage
        {
            get { return currentPage; }
            set { currentPage = value; }
        }

        /// <summary>
        /// Property sets number of records per page view
        /// </summary>
        public int RecordsPerPage
        {
            get { return recordsPerPage; }
            set { recordsPerPage = value; }
        }

        /// <summary>
        /// Property sets total number of records
        /// </summary>
        public int RecordCount
        {
            get { return recordCount; }
            set { recordCount = value; }
        }

        public int ShowPages
        {
            get { return showPages; }
            set { showPages = value; }
        }

        #endregion

        /// <summary>
        /// Default class constructor
        /// </summary>
        public ResultPager() { }

        public ResultPager(string pageBaseUrl, int pageIndex, int pageSize, int pageCount, int itemCount)
        {
            this.currentPage = pageIndex;
            this.recordCount = itemCount;
            this.recordsPerPage = pageSize;
            this.showPages = pageCount;
            this.pageBaseUrlFormat = pageBaseUrl + "&first={0}&page={1}";
        }

        /// <summary>
        /// Method that builds HTML paging constructs based on class properties
        /// </summary>
        /// <returns>Paging HTML links</returns>
        public string RenderPager()
        {
            string result = "";
            int startIndex = 0;
            int endIndex = 0;
            int pages = 0;

            //Get number of pages
            if (recordsPerPage > 0)
            {
                pages = recordCount / recordsPerPage;
                if (recordCount % recordsPerPage > 0)
                {
                    pages += 1;
                }
            }

            if (pages > 1)
            {
                startIndex = currentPage - showPages > 0 ? currentPage - showPages : 1;
                endIndex = currentPage + showPages > pages ? pages : currentPage + showPages;

                for (int i = startIndex; i <= endIndex; i++)
                {
                    if (currentPage != i)
                    {
                        result += "<a href=\"" + String.Format(pageBaseUrlFormat, (((i - 1) * this.recordsPerPage) + 1).ToString(), i) + "\">" + i.ToString() + "</a>&nbsp;";
                    }
                    else
                    {
                        result += "<b>" + i.ToString() + "</b>&nbsp;";
                    }
                }

                if (currentPage > 1)
                {
                    result = "<a href=\"" + String.Format(pageBaseUrlFormat, (((currentPage - 2) * this.recordsPerPage) + 1).ToString(), (currentPage - 1).ToString()) + "\">&lt;&nbsp;Previous</a>&nbsp;&nbsp;" + result;
                }
                if (currentPage < pages)
                {
                    result += "&nbsp;&nbsp;<a href=\"" + String.Format(pageBaseUrlFormat, (((currentPage) * this.recordsPerPage) + 1).ToString(), (currentPage + 1).ToString()) + "\">Next&nbsp;&gt;</a>";
                }
            }

            return result;
        }
    }

    #endregion
}

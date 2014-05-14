using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCI.Web.UI.WebControls
{
    public class BlogPager : SimplePager
    {

        protected override void RenderContents(System.Web.UI.HtmlTextWriter output)
        {
            int pages = 0;


            if (this.RecordsPerPage > 0)
            {
                pages = (this.RecordCount / this.RecordsPerPage) + ((this.RecordCount % this.RecordsPerPage > 0) ? 1 : 0);
            }

            if (pages > 1)
            {
                //NOTE: this has been reversed from what is the in the Simple Pager method because of ordering of links in html
                //for screen readers
                //Next Page Link
                if (this.CurrentPage < pages)
                {
                    RenderNextLink(output);
                }

                //Previous Page Link
                if (this.CurrentPage > 1)
                {
                    RenderPrevLink(output);
                }


            }

        }
    }
}

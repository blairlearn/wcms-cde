using System;
using System.Text;

namespace CancerGov.UI.HTML
{
    /// <summary>
    /// Summary description for HtmlSegment.
    /// </summary>
    public class HtmlSegment : IRenderer
    {
        private StringBuilder sbContent;

        public HtmlSegment(string strHtml)
        {
            sbContent = new StringBuilder(strHtml);
        }

        public string Render()
        {
            return sbContent.ToString();
        }
    }
}

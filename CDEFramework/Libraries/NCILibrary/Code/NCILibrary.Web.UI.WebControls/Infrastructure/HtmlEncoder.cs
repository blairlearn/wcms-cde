using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace NCI.Web.UI.WebControls.Infrastructure
{
    public static class HtmlEncoder
    {
        public static bool IsHtmlEncoded(string text)
        {
            return (HttpUtility.HtmlDecode(text) != text);
        }

        public static String HtmlDecode(string text)
        {
            return HttpUtility.HtmlDecode(text);
        }

        public static void HtmlDecode(string text, TextWriter output)
        {
            HttpUtility.HtmlDecode(text, output);
        }

        public static String HtmlEncode(string text)
        {
            return HttpUtility.HtmlEncode(text);
        }

        public static void HtmlEncode(string text, TextWriter output)
        {
            HttpUtility.HtmlEncode(text, output);
        }
    }
}

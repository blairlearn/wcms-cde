using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCI.Web.CDE
{
    internal class Utility
    {
        static public bool IgnoreWebResource(string url)
        {
            url = url.ToLower();
            return url.IndexOf(".axd") != -1 || 
                   url.IndexOf(".css") != -1 || 
                   url.IndexOf(".eot") != -1 || 
                   url.IndexOf(".gif") != -1 || 
                   url.IndexOf(".ico") != -1 || 
                   url.IndexOf(".jpg") != -1 || 
                   url.IndexOf(".js") != -1 || 
                   url.IndexOf(".png") != -1 || 
                   url.IndexOf(".svg") != -1 || 
                   url.IndexOf(".ttf") != -1 || 
                   url.IndexOf(".woff") != -1;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Globalization;
using NCI.Util;

namespace NCI.Web.CancerGov.Apps
{
    public class AppsBaseUserControl : UserControl
    {
        virtual protected string GetResource(string key)
        {
            if( string.IsNullOrEmpty(key) )
                return "";
            object localizedObject = GetLocalResourceObject(key);
            if (localizedObject == null)
                return "key:" + key + "not localized";
            return localizedObject as string;
        }
    }
}
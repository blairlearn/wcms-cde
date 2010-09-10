using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Text.RegularExpressions;
using NCI.Web.CDE.Configuration;
using System.Web;

namespace NCI.Web.CDE
{
    public interface IMultiPageAssemblyInstruction : IPageAssemblyInstruction
    {
        Boolean ContainsURL(string requestedURL);

    }
}

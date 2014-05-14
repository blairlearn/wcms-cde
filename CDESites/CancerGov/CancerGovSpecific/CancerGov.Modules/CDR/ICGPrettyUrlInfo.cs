using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CancerGov.Modules.CDR
{
    public interface ICGPrettyUrlInfo
    {
        Guid ObjectID { get; }

        string PrettyUrl { get; }

        string RealUrl { get; }

        string RedirectUrl { get; }
    }
}

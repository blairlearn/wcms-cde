using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace NCI.Services.Dictionary.BusinessObjects
{
    internal interface IJsonizable
    {
        void Jsonize(Jsonizer builder);
    }
}
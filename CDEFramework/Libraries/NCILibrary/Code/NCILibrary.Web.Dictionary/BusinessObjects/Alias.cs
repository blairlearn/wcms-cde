using System;
using System.Runtime.Serialization;

namespace NCI.Web.Dictionary.BusinessObjects
{
   
    public class Alias
    {
        /// <summary>
        /// The other name
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// The type other name
        /// </summary>
        public String Type { get; set; }
    }
}

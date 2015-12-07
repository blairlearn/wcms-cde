using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace NCI.Services.Dictionary.BusinessObjects
{
    /// <summary>
    /// Used in combination with Jsonizer to convert the values in an object
    /// hierarchy to a JSON string.
    /// </summary>
    internal interface IJsonizable
    {
        /// <summary>
        /// Hook for IJsonizable objects to store data members in the
        /// builder parameter by calling the various AddMember() overloads.
        /// </summary>
        /// <param name="builder">The Jsonizer instance to use for storing data members.</param>
        void Jsonize(Jsonizer builder);
    }
}
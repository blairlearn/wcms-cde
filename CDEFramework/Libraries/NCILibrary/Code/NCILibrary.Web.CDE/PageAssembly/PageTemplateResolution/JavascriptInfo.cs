using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace NCI.Web.CDE
{
    public class JavascriptInfo
    {

        /// <summary>
        /// Gets or sets the Javascript path.
        /// </summary>
        /// <value>The javascript path.</value>
        [XmlElement(Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public string JavaScriptPath { get; set; }

        [XmlAttribute(Form = XmlSchemaForm.Unqualified)]
        public string Beginning{ get; set; }
        
        [XmlAttribute(Form = XmlSchemaForm.Unqualified)]
        public string End { get; set; }

        [XmlAttribute(Form = XmlSchemaForm.Unqualified)]
        public string Type { get; set; }

        public override bool Equals(object obj)
        {
            JavascriptInfo target = obj as JavascriptInfo;

            if (target == null)
                return false;

            if (JavaScriptPath != target.JavaScriptPath)
                return false;

            return true;
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            //This object may change, so it would be bad for the hash code to change,
            //so use the base implementation
            return base.GetHashCode();
        }
    }
}

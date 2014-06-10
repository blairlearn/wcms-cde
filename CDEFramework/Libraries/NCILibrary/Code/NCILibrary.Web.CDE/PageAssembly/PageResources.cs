using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace NCI.Web.CDE
{
    /// <summary>
    /// Represents an instance of a PageResources XML item, which encapsulates CSS and JS.
    /// </summary>
    public sealed class PageResources
    {
        /// <summary>
        /// Gets or sets the style sheets of this PageTemplateInfo.
        /// </summary>
        /// <value>The style sheets.</value>
        [XmlArray(ElementName = "StyleSheets", Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        [XmlArrayItem(ElementName = "StyleSheetInfo", Form = XmlSchemaForm.Unqualified)]
        public StyleSheetInfo[] StyleSheets { get; set; }

        /// <summary>
        /// Gets or sets the Javascripts of this PageTemplateInfo.
        /// </summary>
        /// <value>The Javascripts</value>
        [XmlArray(ElementName = "Javascripts", Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        [XmlArrayItem(ElementName = "JavascriptInfo", Form = XmlSchemaForm.Unqualified)]
        public JavascriptInfo[] Javascripts { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageTemplateInfo"/> class.
        /// </summary>
        public PageResources()
        {
            StyleSheets = new StyleSheetInfo[]{};
            Javascripts = new JavascriptInfo[] { };

        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj)
        {
            PageTemplateInfo target = obj as PageTemplateInfo;

            if (target == null)
                return false;

            if (
                (StyleSheets == null && target.StyleSheets != null) ||
                (StyleSheets != null && target.StyleSheets == null)
                )
            {
                return false;
            }

            if (
                (Javascripts == null && target.Javascripts != null) ||
                (Javascripts != null && target.Javascripts == null)
                )
            {
                return false;
            }


            if (StyleSheets != null && target.StyleSheets != null)
            {

                if (StyleSheets.Length != target.StyleSheets.Length)
                    return false;

                for (int i = 0; i < StyleSheets.Length; i++)
                {
                    if (StyleSheets[i] == null)
                    {
                        if (target.StyleSheets[i] != null)
                            return false;

                        //If we did not return then we know that target.Stylesheets[i] is also null
                    }
                    else
                    {
                        if (!StyleSheets[i].Equals(target.StyleSheets[i]))
                            return false;
                    }
                }
            }
            if (Javascripts != null && target.Javascripts != null)
            {

                if (Javascripts.Length != target.Javascripts.Length)
                    return false;

                for (int i = 0; i < Javascripts.Length; i++)
                {
                    if (Javascripts[i] == null)
                    {
                        if (target.Javascripts[i] != null)
                            return false;

                        //If we did not return then we know that target.Stylesheets[i] is also null
                    }
                    else
                    {
                        if (!Javascripts[i].Equals(target.Javascripts[i]))
                            return false;
                    }
                }
            }

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

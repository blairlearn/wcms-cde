using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace NCI.Web.CDE
{
    /// <summary>
    /// Defines a Page Template Theme, which allows us to use different physical page templates across the
    /// web site based on a theme name for a given logical page template.
    /// </summary>
    public class TemplateThemeInfo
    {

        /// <summary>
        /// Gets or sets the name of this CDE Page Template Theme.
        /// </summary>
        /// <value>The name of the template.</value>
        [XmlAttribute(AttributeName = "Name", Form = XmlSchemaForm.Unqualified)]
        public string ThemeName { get; set; }

        /// <summary>
        /// Gets or sets a collection of CDE Page Templates.
        /// </summary>
        /// <value>The page template collections.</value>
        [XmlArray(ElementName = "PageTemplateCollections", Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        [XmlArrayItem(ElementName="PageTemplateCollection", Form=XmlSchemaForm.Unqualified)]
        public PageTemplateCollection[] PageTemplateCollections { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageTemplateConfiguration"/> class.
        /// </summary>
        public TemplateThemeInfo()
        {
            PageTemplateCollections = new PageTemplateCollection[] { };
        }

        /// <summary>
        /// Gets the PageTemplateInfo for a given template and Display Version.
        /// </summary>
        /// <param name="templateName">The template we are looking for </param>
        /// <param name="version">The Display Version to get the PageTemplateInfo</param>
        /// <returns>The PageTemplateInfo for the given Display Version if defined, otherwise null
        /// </returns>
        public PageTemplateInfo GetPageTemplateInfo(string templateName, DisplayVersions version)
        {
            PageTemplateCollection col = PageTemplateCollections.FirstOrDefault(ptc => ptc.TemplateName == templateName);
            //If we could not find the page template col then return null, otherwise, ask the PTC for the template info
            return col == null ? null : col.GetPageTemplateInfo(version);
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
            TemplateThemeInfo target = obj as TemplateThemeInfo;

            if (target == null)
                return false;

            if (
                (PageTemplateCollections == null && target.PageTemplateCollections != null) ||
                (PageTemplateCollections != null && target.PageTemplateCollections == null)
                )
            {
                return false;
            }

            if (PageTemplateCollections.Length != target.PageTemplateCollections.Length)
                return false;

            for (int i = 0; i < PageTemplateCollections.Length; i++)
            {
                if (PageTemplateCollections[i] == null)
                {
                    if (target.PageTemplateCollections[i] != null)
                        return false;

                    //If we did not return then we know that target.PageTemplateCollections[i] is also null
                }
                else
                {
                    if (!PageTemplateCollections[i].Equals(target.PageTemplateCollections[i]))
                        return false;
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

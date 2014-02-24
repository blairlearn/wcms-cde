using System;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Configuration;
namespace NCI.Web.CDE
{
    public class SocialMetadata
    {
        /// <summary>
        /// Gets the enabled state of comments on the page.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public bool IsCommentingAvailable { get; set; }

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
            SocialMetadata target = obj as SocialMetadata;

            if (IsCommentingAvailable != target.IsCommentingAvailable)
                return false;
            
            return true;
        }
    }
}

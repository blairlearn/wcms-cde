using System;
using System.Text;
using CancerGov.UI;
using CancerGov.UI.HTML;
using NCI.Web.CDE;

namespace CancerGov.UI.PageObjects
{
	/// <summary>
	/// Summary description for ReturnToTopBar.
	/// </summary>
	public class ReturnToTopAnchor : HtmlAnchor, IRenderer
	{
		public ReturnToTopAnchor() : base("#top", "Back to Top")
		{
			SetReturnToTop(DisplayVersions.Image, DisplayLanguage.English);
		}
		
		public ReturnToTopAnchor(DisplayInformation displayInfo) : base("#top", "Back to Top")
		{
			SetReturnToTop(displayInfo.Version, displayInfo.Language);			
		}

		private void SetReturnToTop(DisplayVersions version, DisplayLanguage language)
		{
			this.Class = "backtotop-link";
			
			switch(language)
			{
				case DisplayLanguage.Spanish:
					this.InnerHtml = "Volver arriba";
					break;
			}

			if(version != DisplayVersions.Text)
			{			
				HtmlImage backToTopArrow = new HtmlImage("/images/backtotop_red.gif", this.InnerHtml);
				backToTopArrow.Border = "0";
				this.InnerHtml = backToTopArrow.Render() + this.InnerHtml;					
			}			
		}
	}
}

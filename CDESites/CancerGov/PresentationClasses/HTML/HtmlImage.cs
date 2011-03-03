using System;
using System.Text;
using System.Collections;
using CancerGov.DataAccessClasses.UI;
using CancerGov.Text;
//using System.Web.UI.WebControls;
using NCI.Web.CDE;


namespace CancerGov.UI.HTML
{
	/// <summary>
	/// Summary description for HtmlImage.
	/// </summary>
	public class HtmlImage : BaseHtmlHashtable, IRenderer
	{
		#region Class Properties

		public string ID
		{
			get {return (string)this["id"];}
			set {SetAttribute("id", value);}			
		}

		public string Src
		{
			get {return (string)this["src"];}
			set {SetAttribute("src", value);}			
		}

		public string Name
		{
			get {return (string)this["name"];}
			set {SetAttribute("name", value);}
		}

		public string AltText
		{
			get {return (string)this["alt"];}
			set {SetAttribute("alt", value);}
		}

		public string TextSrc
		{
			get {return (string)this["textsrc"];}
			set {SetAttribute("textsrc", value);}
		}

		public string Url
		{
			get {return (string)this["url"];}
			set {SetAttribute("url", value);}
		}

		public string Width
		{ 
			get {return (string)this["width"];}
			set {SetAttribute("width", value);}
		}

		public string Height
		{
			get {return (string)this["height"];}
			set {SetAttribute("height", value);}
		}

		public string Border
		{
			get {return (string)this["border"];}
			set {SetAttribute("border", value);}
		}

		public string UseMap
		{ 
			get {return (string)this["usemap"];}
			set {SetAttribute("usemap", value);}
		}

		public string IsMap
		{ 
			get {return (string)this["ismap"];}
			set {SetAttribute("ismap", value);}
		}

		public string CssClass
		{
			get {return (string)this["class"];}
			set {SetAttribute("class", value);}
		}
		
		public string Style
		{
			get {return (string)this["style"];}
			set {SetAttribute("style", value);}
		}

		public string Align
		{
			get {return (string)this["align"];}
			set {SetAttribute("align", value);}
		}

		public DisplayInformation DisplayInfo
		{
			get {return this.Display;}
			set {this.Display = value;}
		}

		#endregion


		public HtmlImage()
		{
			this.HtmlAttributes = new string[]{"id","src","name","alt","url","width","height","border","usemap","ismap","class","style","align"};
		}

		public HtmlImage(string src, string alt)
		{
			this.HtmlAttributes = new string[]{"id","src","name","alt","url","width","height","border","usemap","ismap","class","style","align"};
			this.Src = src;
			this.AltText = alt;
			this.Border = "0";
		}

		public HtmlImage(string src, string alt, string strWidth, string strHeight) : this(src, alt)
		{
			this.Width = strWidth;
			this.Height = strHeight;
		}

		public HtmlImage(Image img, DisplayInformation displayInfo)
		{
			this.HtmlAttributes = new string[]{"id","src","name","alt","url","width","height","border","usemap","ismap","class","style","align"};
			this.Src = img.Src;
			this.Name = img.Name;
			this.ID = img.Name;
			this.AltText = img.AltText;
			this.Border = (img.Border == -1) ? "0" : img.Border.ToString();
			this.Height = (img.Height == -1) ? null : img.Height.ToString();
			this.Width = (img.Width == -1) ? null : img.Width.ToString();
			this.TextSrc = img.TextSrc;	
			this.Display = displayInfo;
            
		}

		public string Render()
		{
			string imgFormat = "<img{0}>";

			switch(this.Display.Version)
			{
				case DisplayVersions.Image:
					return String.Format(imgFormat, RenderAttributes(HtmlAttributes));
				case DisplayVersions.Text:
					return Strings.IfNull(this.TextSrc, "");
				case DisplayVersions.Print:
					return String.Format(imgFormat, RenderAttributes(HtmlAttributes));
				default:
					return String.Format(imgFormat, RenderAttributes(HtmlAttributes));
			}
		}
	}
}

using System;
using CancerGov.Text;

namespace CancerGov.UI.HTML 
{
	/// <summary>
	/// Summary description for HtmlAnchor.
	/// </summary>
	public class HtmlAnchor : BaseHtmlHashtable, IRenderer 
	{
		#region Class Properties

		public string HRef 
		{
			get {return (string)this["href"];}
			set {SetAttribute("href", value);}
		}
		
		public string ID 
		{
			get {return (string)this["id"];}
			set {SetAttribute("id", value);}
		}
		
		public string Name 
		{
			get {return (string)this["name"];}
			set {SetAttribute("name", value);}
		}
		
		public string Title 
		{
			get {return (string)this["title"];}
			set {SetAttribute("title", value);}
		}
		
		public string Class 
		{
			get {return (string)this["class"];}
			set {SetAttribute("class", value);}
		}
		
		public string Style 
		{
			get {return (string)this["style"];}
			set {SetAttribute("style", value);}
		}
		
		public string NoTab 
		{
			get {return (string)this["notab"];}
			set {SetAttribute("notab", value);}
		}
		
		public string TabIndex 
		{
			get {return (string)this["tabindex"];}
			set {SetAttribute("tabindex", value);}
		}
		
		public string Shape 
		{
			get {return (string)this["shape"];}
			set {SetAttribute("shape", value);}
		}
		
		public string Coords 
		{
			get {return (string)this["coords"];}
			set {SetAttribute("coords", value);}
		}
		
		public string OnMouseOver 
		{
			get {return (string)this["onmouseover"];}
			set {SetAttribute("onmouseover", value);}
		}
		
		public string OnMouseOut 
		{
			get {return (string)this["onmouseout"];}
			set {SetAttribute("onmouseout", value);}
		}

		public string OnClick 
		{
			get {return (string)this["onclick"];}
			set {SetAttribute("onclick", value);}
		}

		public string InnerHtml 
		{
			get {return (string)this["innerhtml"];}
			set {SetAttribute("innerhtml", value);}
		}

		#endregion
		
		public HtmlAnchor() 
		{
			this.TagFormat = "<a{0}>{1}</a>";
			this.HtmlAttributes = new string[] {"href","id","name","title","class","style","notab","tabindex","shape","coords","onmouseover","onmouseout","onclick"};
		}

		public HtmlAnchor(string name) 
		{
			this.TagFormat = "<a{0}>{1}</a>";
			this.HtmlAttributes = new string[] {"href","id","name","title","class","style","notab","tabindex","shape","coords","onmouseover","onmouseout","onclick"};
			this.Name = name;
		}

		public HtmlAnchor(string hRef, string innerHtml) 
		{
			this.TagFormat = "<a{0}>{1}</a>";
			this.HtmlAttributes = new string[] {"href","id","name","title","class","style","notab","tabindex","shape","coords","onmouseover","onmouseout","onclick"};
			HRef = hRef;
			InnerHtml = innerHtml;
		}

		public HtmlAnchor(string hRef, HtmlImage img) 
		{
			this.TagFormat = "<a{0}>{1}</a>";
			this.HtmlAttributes = new string[] {"href","id","name","title","class","style","notab","tabindex","shape","coords","onmouseover","onmouseout","onclick"};
			HRef = hRef;
			InnerHtml = img.Render();
		}

		public string Render() 
		{
			return String.Format(this.TagFormat, this.RenderAttributes(this.HtmlAttributes), Strings.IfNull((string)this["innerhtml"], ""));
		}
	}
}

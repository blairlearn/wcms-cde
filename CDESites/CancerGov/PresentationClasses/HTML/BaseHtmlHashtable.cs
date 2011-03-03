using System;
using System.Text;
using System.Collections;
using CancerGov.DataAccessClasses.UI;
using CancerGov.Text;
using NCI.Web.CDE;

namespace CancerGov.UI.HTML
{
	/// <summary>
	/// Summary description for HtmlImage.
	/// </summary>
	public class BaseHtmlHashtable : Hashtable
	{
		private DisplayInformation display;
		private string[] htmlAttributes;
		private string tagFormat = "{0}";
		
		public BaseHtmlHashtable(){}

		#region Class Properties

		public object this[string key]
		{
			get {return base[key.ToString().Trim().ToLower()];}
			set {base[key.ToString().Trim().ToLower()] = value;}
		}

		public string TagFormat
		{
			get {return tagFormat;}
			set {tagFormat = value;}
		}

		public DisplayInformation Display
		{
			get {return display;}
			set {display = value;}
		}

		public string[] HtmlAttributes
		{
			get {return htmlAttributes;}
			set {htmlAttributes = value;}
		}

		#endregion

		#region Class Methods

		public string RenderAttributes()
		{
			StringBuilder attributes = new StringBuilder();

			foreach(string key in this.Keys)
			{
				if (key.ToLower().Equals("alt") && Strings.Clean((string)this[key]) == null)
				{
					attributes.Append(key.PadLeft(1, ' '));
					attributes.Append("=");
				}
				else
				{
					if(Strings.Clean((string)this[key]) != null)
					{
						attributes.Append(key.PadLeft(1, ' '));
						attributes.Append("=");
						attributes.Append(this[key].ToString());
					}
				}
			}

			return attributes.ToString();
		}

		/// <summary>
		/// Renders filtered subset of attributes
		/// </summary>
		/// <param name="attributeKeys"></param>
		/// <returns></returns>
		public string RenderAttributes(string[] attributeKeys)
		{
			StringBuilder attributes = new StringBuilder();

			foreach(string key in attributeKeys)
			{
				if (key.ToLower().Equals("alt") && Strings.Clean((string)this[key]) == null)
				{
					attributes.Append(key.PadLeft(key.Length + 1, ' '));
					attributes.Append("=");
					attributes.Append("\"\"");
				}
				else
				{
					if(Strings.Clean((string)this[key]) != null)
					{
						attributes.Append(key.PadLeft(key.Length + 1, ' '));
						attributes.Append("=");
						attributes.Append("\"");
						attributes.Append(this[key].ToString());
						attributes.Append("\"");
					}
				}
			}

			return attributes.ToString();
		}

		public void SetAttribute(string key, string val)
		{
			if(Strings.Clean(val) != null)
			{
				if(!this.ContainsKey(key.Trim().ToLower()))
				{
					this.Add(key.Trim().ToLower(), val);
				}
				else
				{
					this[key.Trim().ToLower()] = val;
				}
			}
		}

		public void SetAttributes(Hashtable attributes)
		{
			foreach(string key in attributes.Keys)
			{
				SetAttribute(key, (string)attributes[key]);
			}
		}

		#endregion
	}
}

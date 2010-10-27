using System;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Configuration;

namespace CancerGov.Common
{
	/// <summary>
	///Defines the Xml-Html Transform object, employs Xsl transform to convert Xml to Html<br/>
	///<br/>
	///<b>Author</b>:  Greg Andres<br/>
	///<b>Date</b>:  6-5-2002<br/>
	///<br/>
	///<b>Revision History</b>:<br/>
	///<br/>
	///</summary>
	public class Transform
	{
		public Transform()
		{
		}

		public string GetHtml(XmlDocument xmlSource, string xslUri, XsltArgumentList xslArguments)
		{
			XslTransform xslt = new XslTransform();
			XmlDocument resultDoc = new XmlDocument();
			XmlReader resultReader;
			string html = "";

			xslt.Load(xslUri);
			resultReader = xslt.Transform(xmlSource.CreateNavigator(), xslArguments);
			resultDoc.Load(resultReader);
			
			html = resultDoc.OuterXml;
			
			resultReader.Close();
			resultDoc.RemoveAll();
			
			return html;			
		}

		public string GetHtml(XmlDocument xmlSource, XmlDocument xslStylesheet, XsltArgumentList xslArguments)
		{
			XslTransform xslt = new XslTransform();
			XmlDocument resultDoc = new XmlDocument();
			XmlReader resultReader;
			string html = "";
			
			xslt.Load(xslStylesheet.CreateNavigator());
			resultReader = xslt.Transform(xmlSource.CreateNavigator(), xslArguments);
			resultDoc.Load(resultReader);
			
			html = resultDoc.OuterXml;

			resultReader.Close();
			resultDoc.RemoveAll();
	
			return html;			
		}
	}
}

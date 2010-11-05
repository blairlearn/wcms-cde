using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using CancerGov.Text;

namespace CancerGov.DataAccessClasses.UI
{
	/// <summary>
	/// Summary description for Image.
	/// </summary>
	public class Image
	{
		//member variables
		private Guid imageId;
		private string src;
		private string name;
		private string altText;
		private string textSrc;
		private string url;
		private int width;
		private int height;
		private int border;
		private bool isClickLogged;

		public Image(string imageName)
		{
			DataTable dbTable = new DataTable();

			SqlDataAdapter dbAdapter = null;

			try {
				dbAdapter = new SqlDataAdapter("usp_GetImageViewObject", ConfigurationSettings.AppSettings["DbConnectionString"]);
				dbAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
				dbAdapter.SelectCommand.Parameters.Add("@ImageName",imageName);
				dbAdapter.Fill(dbTable);
			}
			catch(SqlException) {
				//CancerGovError.LogError("", this.ToString(), ErrorType.DbUnavailable, sqlE);
			} finally {
				if (dbAdapter != null){
					dbAdapter.Dispose();
				}
			}

			if(dbTable.Rows.Count > 0)
			{
				this.Populate(dbTable.Rows[0]);				
			}
			else
			{
				//no data
			}	
			
			if (dbTable != null) {
				dbTable.Dispose();
			}
		}

		public Image(Guid gImageID) 
		{
			DataTable dbTable = new DataTable();
			SqlDataAdapter dbAdapter = null;

			try {
				dbAdapter = new SqlDataAdapter("usp_GetImage", ConfigurationSettings.AppSettings["DbConnectionString"]);
				dbAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
				dbAdapter.SelectCommand.Parameters.Add("@ImageID",gImageID);
				dbAdapter.Fill(dbTable);
			}
			catch(SqlException) {
				//CancerGovError.LogError("", this.ToString(), ErrorType.DbUnavailable, sqlE);
			} finally {
				if (dbAdapter != null) {
					dbAdapter.Dispose();
				}
			}

			if(dbTable.Rows.Count > 0)
			{
				this.Populate(dbTable.Rows[0]);
			}
			else
			{
				//no data
			}			

			if (dbTable != null) {
				dbTable.Dispose();
			}
		}

		#region Class Properties
        
		public Guid ImageId
		{
			get {return imageId;}
		}

		public string Src
		{
			get {return src;}
		}

		public string Name
		{
			get {return name;}
		}

		public string AltText
		{
			get {return altText;}
		}

		public string TextSrc
		{
			get {return textSrc;}
		}

		public string Url
		{
			get {return url;}
		}

		public int Width
		{ 
			get {return width;}
		}

		public int Height
		{
			get {return height;}
		}

		public int Border
		{
			get {return border;}
		}

		public bool IsClickLogged 
		{
			get {return isClickLogged;}
		}

		#endregion

		#region Class Methods

		private void Populate(DataRow dr)
		{			
			imageId = Strings.ToGuid(dr["ImageId"].ToString());
			name = Strings.Clean(dr["ImageName"].ToString());
			src = Strings.Clean(dr["ImageSource"].ToString());
			altText = Strings.Clean(dr["ImageAltText"].ToString());
			textSrc = Strings.Clean(dr["TextSource"].ToString());
			url = Strings.Clean(dr["Url"].ToString());
			width = Strings.ToInt(dr["width"].ToString());
			height = Strings.ToInt(dr["height"].ToString());
			border = Strings.ToInt(dr["border"].ToString());
			isClickLogged = Strings.ToBoolean(dr["ClickloggingEnabled"].ToString());
		}

		#endregion

	}
}


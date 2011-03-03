using System;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using CancerGov.Common.ErrorHandling;

namespace CancerGov.UI
{
	/// <summary>
	/// Purpose:	This HttpHanlder handles the cross-summary reference url.
	///				Converts http://cancer.gov/cancertopics/pdq/treatment/unusual-cancers-childhood/patient/5.cdr#section_5
	///					or /cancertopics/pdq/treatment/unusual-cancers-childhood/patient/5.cdr#section_5
	///					to  /templates/doc.aspx?viewid=4c22eba0-85a6-429e-a8f8-ad1a73278b04&sectionid=35&version=1
	/// </summary>
	public class CDRHttpHandler : IHttpHandler
	{
		private HttpResponse Response;
		private HttpRequest Request;
		
		public CDRHttpHandler()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        public void ProcessRequest(HttpContext ctx)
        {
            Response = ctx.Response;
            Request = ctx.Request;

            //Get relative url from requrest
            string relativeUrl = Request.Url.AbsolutePath.ToLower(); //Absolute path  /cancertopics/pdq/treatment/unusual-cancers-childhood/patient/5.cdr

            string baseURL = relativeUrl.Substring(0, relativeUrl.LastIndexOf("/")); //Get pretty url before section /cancertopics/pdq/treatment/unusual-cancers-childhood/patient

            int length = relativeUrl.IndexOf(".cdr"); //Get .cdr's index value

            if (length < 0) //if no cdr exists, return
                return;

            string sectionID = relativeUrl.Substring(relativeUrl.LastIndexOf("/") + 1, length - relativeUrl.LastIndexOf("/") - 1); //Get section ID

            //use baseurl to get viewID and version to create a correct real url 
            string redirectURL = "";
            string pageSection = "";
            Guid summaryID = Guid.Empty;
            try
            {
                SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString);

                SqlCommand myCommand = new SqlCommand("usp_GetSummaryRealURL", myConnection);

                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.Parameters.Add("@PrettyURL", baseURL);

                myConnection.Open();
                SqlDataReader myReader;
                myReader = myCommand.ExecuteReader();
                // Always call Read before accessing data.
                while (myReader.Read())
                {
                    redirectURL = myReader.GetString(0);
                    summaryID = myReader.GetGuid(1);
                }

                myReader.Close();
                myConnection.Close();

                SqlConnection myConnection1 = new SqlConnection(ConfigurationManager.ConnectionStrings["CDRDbConnectionString"].ConnectionString);

                SqlCommand myCommand1 = new SqlCommand("usp_GetSummaryURLSection", myConnection1);

                myCommand1.CommandType = CommandType.StoredProcedure;
                myCommand1.Parameters.Add("@SummaryID", summaryID);
                myCommand1.Parameters.Add("@SectionID", sectionID);

                myConnection1.Open();
                SqlDataReader myReader1;
                myReader1 = myCommand1.ExecuteReader();
                // Always call Read before accessing data.
                while (myReader1.Read())
                {
                    pageSection = myReader1.GetString(0);
                }

                myReader1.Close();
                myConnection1.Close();

            }
            catch (SqlException sqlE)
            {
                Response.Write(sqlE.Message);
                //CancerGovError.LogError("CDRHttpHandler", "CDRHttpHandler", ErrorType.InvalidArgument, sqlE);
                return;
            }

            //No try catch --both Response.Redirect and Server.Transfer method throw an exception to end the current page execution. 
            //The main thread is getting aborted so the new thread can start for the target page. 
            //http://support.microsoft.com/kb/817266


            //try
            //{
                if (redirectURL == string.Empty)
                {
                    Response.Redirect(baseURL);
                }
                else
                {

                    redirectURL = string.Format(redirectURL + "&sectionid={0}", pageSection);
                    ctx.Server.Transfer(redirectURL);

                    //redirectURL = string.Format(redirectURL +"&sectionid={0}#section_{1}",  pageSection, sectionID);
                    //ctx.Response.Redirect(redirectURL);
                    //ctx.RewritePath(redirectURL);
                    //IHttpHandler handler = PageParser.GetCompiledPageInstance(
                    //    "/Templates", ctx.Server.MapPath("/Templates/doc.aspx"), ctx);

                    //IHttpHandler handler = PageParser.GetCompiledPageInstance(
                    //    baseURL, ctx.Server.MapPath("/Templates/doc.aspx"), ctx);
                    //handler.ProcessRequest(ctx);
                }
            //}
            //catch (Exception ex)
            //{
            //    CancerGovError.LogError("CDRHttpHandler", "CDRHttpHandler", ErrorType.InvalidArgument, ex);
            //}

        }// void ProcessRequest(HttpContext ctx)


		public bool IsReusable { get { return true; } }

	} // Class
} // namespace

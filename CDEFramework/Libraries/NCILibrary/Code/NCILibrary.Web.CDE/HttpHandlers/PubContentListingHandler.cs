using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;

using Newtonsoft.Json;

using NCI.Web.CDE.Configuration;


namespace NCI.Web.CDE.HttpHandlers
{
    /// <summary>
    /// This HttpHandler allows a user or computer to peruse a set of allowed published content
    /// folders programatically.  While you can jump directly to a piece of published content,
    /// you cannot see all of the published content.  We do want to make sure this is limited
    /// to a select group of folders AND only certain file types.
    /// </summary>
    public class PubContentListingHandler : IHttpHandler
    {
        private static readonly string _HANDLER_PATH = "/PublishedContent/List";
        private static readonly string _FORMAT_KEY = "fmt";
        private static readonly string _ROOT_KEY = "root";
        private static readonly string _PATH_KEY = "path";

        enum ResponseFormat
        {
            JSON,
            HTML
        }

        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            //Let's check on the format.  We default to JSON, but allow HTML.
            ResponseFormat format = ResponseFormat.JSON;
            if (
                !string.IsNullOrWhiteSpace(context.Request.QueryString[_FORMAT_KEY])
                && context.Request.QueryString[_FORMAT_KEY].Trim().ToUpper() == "HTML"
            ){
                format = ResponseFormat.HTML;
            }

            //Set the response content type
            if (format == ResponseFormat.HTML)
            {
                context.Response.ContentType = "text/html";
            }
            else
            {
                context.Response.ContentType = "application/json";
            }

            //First let's get the name of the allowed path to show the files for
            string name = string.IsNullOrWhiteSpace(context.Request.QueryString[_ROOT_KEY]) ? null : context.Request.QueryString[_ROOT_KEY].Trim();

            try
            {
                if (name == null)
                {
                    //There is no name so we want to show a listing of possible paths
                    ListAvailablePaths(context, format);
                }
                else
                {
                    GetItemsForPath(context, format, name);
                }
            }
            catch (System.Threading.ThreadAbortException)
            {
                //This is thrown from response.end.  It is an ok exception to bubble up.
                throw;
            }
            catch (Exception ex)
            {
                ErrorResponse(context, format, ex);
            }
        }

        /// <summary>
        /// Helper function to display all the available paths
        /// </summary>
        /// <param name="context"></param>
        /// <param name="format"></param>
        private void ListAvailablePaths(HttpContext context, ResponseFormat format)
        {
            List<RootPathItem> configPaths = new List<RootPathItem>();

            foreach(PublishedContentListingPathElement elem in ContentDeliveryEngineConfig.PublishedContentListing.ListablePaths) 
            {
                configPaths.Add(new RootPathItem()
                {
                    DisplayName = elem.DisplayName,
                    Url = string.Format(
                        "{0}?{1}={2}&{3}={4}",
                        _HANDLER_PATH,
                        _ROOT_KEY, elem.Name,
                        _FORMAT_KEY, format == ResponseFormat.HTML ? "html" : "json")
                });
            }

            if (format == ResponseFormat.HTML)
            {
                StringBuilder builder = new StringBuilder();
                
                using (StringWriter sw = new StringWriter(builder)) {
                    using (HtmlTextWriter writer = new HtmlTextWriter(sw))
                    {
                        writer.RenderBeginTag(HtmlTextWriterTag.Ul);
                        foreach (var item in configPaths)
                        {
                            writer.RenderBeginTag(HtmlTextWriterTag.Li);
                            writer.AddAttribute(HtmlTextWriterAttribute.Href, item.Url);
                            writer.RenderBeginTag(HtmlTextWriterTag.A);
                            writer.Write(item.DisplayName);
                            writer.RenderEndTag();
                            writer.RenderEndTag();
                        }
                        writer.RenderEndTag();
                    }
                }
                

                context.Response.Write(
                    GetFullHTML(
                        "Published Content Folders",
                        builder.ToString()
                    )
                );
            }
            else
            {
                context.Response.Write(JsonConvert.SerializeObject(configPaths));
            }

            context.Response.End();
        }

        /// <summary>
        /// Helper function to display all the items in a folder.  This really just validates the
        /// request and sets up variables for the response.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="format"></param>
        /// <param name="name"></param>
        private void GetItemsForPath(HttpContext context, ResponseFormat format, string rootPath)
        {
            //Step 1. Validate the rootPath to make sure it is an allowed path.
            PublishedContentListingPathElement pathConfig = ContentDeliveryEngineConfig.PublishedContentListing.ListablePaths[rootPath];
            if (pathConfig == null)
            {
                throw new HttpException(404, String.Format("Root path, {0}, does not exist", rootPath));
            }

            //Step 2. Pull the path param to get the subfolder under this root. 
            // Treat no path param as wanting to see the contents of the root folder.
            string subfolder = string.IsNullOrWhiteSpace(context.Request.QueryString[_PATH_KEY]) ? "/" : context.Request.QueryString[_PATH_KEY].Trim();

            //Step 2a. Validate the path.  Note, : is not actually a invalid path char for windows,
            //but it is not really valid since it is only for the drive...
            if ((subfolder.IndexOfAny(Path.GetInvalidPathChars()) != -1) || (subfolder.IndexOfAny(new char[] { ':' }) != -1))
                throw new HttpException(400, String.Format("The path parameter, {0}, is not a valid path format", subfolder));

            //Step 3. Let's turn that subfolder into a valid, usable path
            //3a, let's get the physical path for the configured rootPath
            string rootPhysicalPath = pathConfig.Path;
            rootPhysicalPath = context.Server.MapPath(rootPhysicalPath);
            
            //3b. let's try and build a real path with the subfolder.
            string requestedPath = rootPhysicalPath;

            string fullPath = "";

            try
            {
                List<string> paths = new List<string>();
                paths.Add(requestedPath);
                paths.AddRange(
                    subfolder.Split(
                        new char[]{
                            Path.DirectorySeparatorChar,
                            Path.AltDirectorySeparatorChar
                        },
                        StringSplitOptions.RemoveEmptyEntries
                    )
                );
                requestedPath = Path.Combine(paths.ToArray());

                //Normalize the path
                fullPath = Path.GetFullPath(requestedPath);


            }
            catch (Exception ex)
            {
                throw new HttpException(400, String.Format("The path parameter, {0}, is not a valid path format", subfolder));
            }

            // At this point fullPath should have been cleaned up.  So let's make sure that
            // the path has not left the root.
            if (fullPath.IndexOf(rootPhysicalPath) != 0)
            {
                throw new HttpException(403, String.Format("The path, {0}, would be outside of the allowed paths and is not allowed.", subfolder));
            }

            if (!Directory.Exists(fullPath))
            {
                throw new HttpException(404, String.Format("The directory, {0}, was not found", subfolder));
            }

            //Get the server path so we can give the full web site URL to the file.
            string serverPath = VirtualPathUtility.ToAbsolute(pathConfig.Path);

            RespondItemsForPath(context, format, rootPath, serverPath, subfolder, pathConfig.GetAllowedTypesAsArray(), fullPath);
            
        }

        private void RespondItemsForPath(
            HttpContext context, 
            ResponseFormat format, 
            string rootPath, 
            string serverPath,
            string subfolder,
            string[] allowedFileTypes,
            string fullPath)
        {

            var output = new
            {
                RootPath = rootPath,
                Subfolder = subfolder,
                FullPath = fullPath
            };


            var subDirectories = 
                from dir in Directory.GetDirectories(fullPath)
                select dir.Replace(fullPath + "\\", "");

            var files = from file in Directory.GetFiles(fullPath)
                        where (allowedFileTypes.Length == 0 || allowedFileTypes.Contains(Path.GetExtension(file)))
                        select new { 
                            FullWebPath = serverPath + file.Replace(fullPath + "\\", "/ADD_SUBFOLDER_HERE/"), //need the subfolder too...
                            FileName = file.Replace(fullPath + "\\", ""),
                            CreationTime = File.GetCreationTime(file),
                            LastWriteTime = File.GetLastWriteTime(file)
                        };

            if (format == ResponseFormat.HTML)
            {
                StringBuilder builder = new StringBuilder();

                using (StringWriter sw = new StringWriter(builder))
                {
                    using (HtmlTextWriter writer = new HtmlTextWriter(sw))
                    {
                        writer.RenderBeginTag(HtmlTextWriterTag.Ul);
                        foreach (var dir in subDirectories)
                        {
                            string url = string.Format(
                                "{0}?{1}={2}&{3}={4}&{5}={6}",
                                _HANDLER_PATH,
                                _ROOT_KEY, rootPath,
                                _PATH_KEY, subfolder + "/" + dir,
                                _FORMAT_KEY, format == ResponseFormat.HTML ? "html" : "json");

                            writer.RenderBeginTag(HtmlTextWriterTag.Li);
                            writer.AddAttribute(HtmlTextWriterAttribute.Href, url);
                            writer.RenderBeginTag(HtmlTextWriterTag.A);
                            writer.Write(dir);
                            writer.RenderEndTag();
                            writer.RenderEndTag();
                        }
                        foreach (var file in files)
                        {

                            writer.RenderBeginTag(HtmlTextWriterTag.Li);
                            writer.AddAttribute(HtmlTextWriterAttribute.Href, file.FullWebPath);
                            writer.RenderBeginTag(HtmlTextWriterTag.A);
                            writer.Write(file.FileName);
                            writer.RenderEndTag();
                            writer.Write(" (Last Modified: " + file.LastWriteTime.ToString() + ")");
                            writer.RenderEndTag();
                        }

                        writer.RenderEndTag();
                    }
                }


                context.Response.Write(
                    GetFullHTML(
                        "Published Content Folders",
                        builder.ToString()
                    )
                );

            }
            else
            {
                context.Response.Write(JsonConvert.SerializeObject(new
                {
                    Directories = subDirectories,
                    Files = files
                }));
            }

            context.Response.End();
        }


        /// <summary>
        /// Helper for returning errors to client.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="format"></param>
        /// <param name="ex"></param>
        private void ErrorResponse(HttpContext context, ResponseFormat format, Exception ex)
        {
            string message = "Errors have occurred. ";

            // Slight assumption here, only we will raise HTTPExceptions.  
            if (ex is HttpException)
            {
                context.Response.StatusCode = ((HttpException)ex).GetHttpCode();
                message = ex.Message;
            }
            else
            {
                //Unhandled exception. :(  Lets not show the raw message, it may be bad.
                context.Response.StatusCode = 500;
                message += ex.GetType().FullName;

                //TODO: Log Error!!
            }

            if (format == ResponseFormat.HTML)
            {
                context.Response.Write(GetFullHTML("Error Message", System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(message, false)));
            }
            else
            {
                context.Response.Write(JsonConvert.SerializeObject(
                    new
                    {
                        ErrorMessage = message
                    }
                ));
            }

            //Make sure you set the skip IIS errors to prevent pretty error pages from being shown.
            context.Response.TrySkipIisCustomErrors = true;
            context.Response.End();

        }

        /// <summary>
        /// Helper to build full HTML page.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        private string GetFullHTML(string title, string content)
        {
            string htmlTemplate = @"
                    <html>
                        <head>
                            <title>{0}</title>
                        </head>
                        <body>{1}</body>
                    </html>
                ";

            return string.Format(htmlTemplate, title, content);
        } 

        #endregion

        /// <summary>
        /// Represents a path item when getting all root paths defined.
        /// This is because we cannot do silly dynamic stuff.
        /// </summary>
        class RootPathItem
        {
            public string DisplayName { get; set; }
            public string Url { get; set; }
        }
    }
}

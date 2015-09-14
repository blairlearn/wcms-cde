using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web; 
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using NCI.Web.CDE.Modules;
using NCI.DataManager;
using NCI.Web.UI.WebControls;

namespace NCI.Web.CDE.UI.SnippetControls
{
    public class DynamicListHelper : BaseSearchSnippet
    {
        ///This is completely dirty and really a hack, but it gets this done.  This should be
        ///fixed in a future release. --BryanP 2/10/2015

        /*
         * Sets text according to the language of the containing page.
         */
        public string languageStrings()
        {
            String pageLanguage = @"
			#set($videoContent = ""Video"")##
			#set($carouselContent = ""Video Playlist"")##
			#set($infographicContent = ""Infographic"")##
			#set($postedString = ""Posted"")##
			#set($updatedString = ""Updated"")##
			#set($reviewedString = ""Reviewed"")##
            #set($newsString = ""All NCI news"")##
            #set($continueReading = ""Continue Reading"")##
            #set($by = ""by"")##
			";
            if (PageAssemblyContext.Current.PageAssemblyInstruction.GetField("Language") == "es")
            {
                pageLanguage = @"
				#set($videoContent = ""Video"")##
				#set($carouselContent = ""Lista de reproducci&oacute;n de videos"")##
				#set($infographicContent = ""Infograf&iacute;a"")##
				#set($postedString = ""Publicaci&oacute;n"")##
				#set($updatedString = ""Actualizaci&oacute;n"")##
				#set($reviewedString = ""Revisi&oacute;n"")##
                #set($newsString = ""Todas las noticias del NCI"")##
                #set($continueReading = ""Siga leyendo"")##
                #set($by = ""por"")##
				";
            }
            return pageLanguage;
        }


        /*
         * Opening tags for dynamic list. Also sets variables for any File Content Type data.
         */
        public string openList()
        {
            string open = @"
            <ul class=""list no-bullets"">##
            #foreach($resultItem in $DynamicSearch.Results)
				#if($resultItem.ContentType == ""rx:nciFile"")##
					##
					## Set file size
					##
					#if($resultItem.FileSize < 1000)##
						#set($fileSize = ""($resultItem.FileSize B)"")##
					#else##
						#set($kbFileSize = ($resultItem.FileSize / 1000))##
						#if($kbFileSize < 1000)##
							#set($fileSize = ""($kbFileSize KB)"")##
						#else##
							#set($mbFileSize = ($kbFileSize / 1000))##
							#set($fileSize = ""($mbFileSize MB)"")##
						#end##
					#end##
					##
					## Set mime type
					##							
					#set($printOutExt="""")##
					#set($fileType = $resultItem.MimeType)##
					#if($fileType == ""application/vnd.ms-excel"" || $fileType == ""application/excel"" || $fileType == ""application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"")##
					    #set($fileClass = ""list-excel"")##
					    #set($fileType_safe = ""excel"")##
					#elseif($fileType == ""application/mspowerpoint"" || $fileType== ""application/vnd.ms-powerpoint"" || $fileType == ""application/vnd.openxmlformats-officedocument.presentationml.presentation"")##
						#set($fileClass = ""list-powerpoint"")##
						#set($fileType_safe = ""ppt"")##
					#elseif($fileType == ""application/msword"" || $fileType == ""application/vnd.openxmlformats-officedocument.wordprocessingml.document"")##
						#set($fileClass = ""list-word"")##
						#set($fileType_safe = ""word"")##
					#elseif($fileType == ""application/pdf"")##
						#set($fileClass = ""list-pdf"")##
						#set($fileType_safe = ""pdf"")##
					#elseif($fileType == ""application/octet-stream"" || $fileType == ""application/x-compressed"" || $fileType == ""application/x-msdownload"")##
						#set($fileClass = ""list-execute"")##
						#set($fileType_safe = ""exe"")##
					#elseif($fileType == ""application/epub+zip"" || $fileType == ""application/x-mobipocket-ebook"")##
						#set($fileClass = ""list-ebook"")##
						#set($fileType_safe = ""ebook"")##
					#elseif($fileType == ""text/plain"")##
						#set($fileClass = ""list-txt"")##
						#set($fileType_safe = ""txt"")##
					#else##
                        #set($fileClass = ""list-item-link link"")##
                        #set($fileType_safe = ""unknown"")##
                        #set($printOutExt = $fileType)##
					#end##
				<li class=""general-list-item file $fileType_safe list-item $fileClass"">##
				#else##
				<li class=""general-list-item list-item"">##
				#end##";
            return open;
        }

        /*
         * Output image if attached to content item.
         */
        public string imageString()
        {
            string image = @"
                ##
                ## Display image
                ##
                <div class=""list-item-image image container"">##
                    #set($alt = ""image"")##
                    #if($resultItem.Alt)## 
                        #set($alt = ""$resultItem.Alt"")##
                    #elseif($resultItem.AltText)## 
                        #set($alt = ""$resultItem.AltText"")##
                    #else##
                        #set($alt = ""image"")##
                    #end##
                    #if($resultItem.ThumbnailURL)## 
                        <img src=""$resultItem.ThumbnailURL"" class=""item-image image"" alt=""$alt"" align=""left"">
                    #else##
                        &nbsp;##
                    #end##
                </div>##";
            return image;
        }

        /*
         * Output Long Title of content item plus additional info for file and media types.
         */
        public string openListItem()
        {
            string title = @"
            <div class=""title container"">##
                ##
                ## Display title
                ##
                <span class=""title"">##
                    <a href=""$resultItem.Href"" onclick=""NCIAnalytics.SearchResults(this,$resultItem.RecNumber);"" class=""$fileClass title"">##						
                    $resultItem.LongTitle##
                    #if($resultItem.ContentType == ""rx:gloVideo"")##
                        </a> ($videoContent)##
                    #elseif($resultItem.ContentType == ""rx:gloVideoCarousel"")##
                        </a> ($carouselContent)##
                    #elseif($resultItem.ContentType == ""rx:cgvInfographic"")##
                        </a> ($infographicContent)##
                    #elseif($resultItem.ContentType == ""rx:nciFile"")##
                        <span class=""filesize"">$fileSize $printOutExt</span><span class=""filetype $fileType_safe""><span class=""hidden"">$fileType_safe file</span></span></a>##
                    #else##
                        </a>##
                    #end##
                </span>##
            ";
            return title;
        }

        /*
         * Output dates.
         */
        public string dateString()
        {
            string dateForLists = @"$resultItem.DateForLists";
            if(PageAssemblyContext.Current.PageAssemblyInstruction.Language == "es")
                dateForLists = @"$resultItem.DateForListsEs";

            string dates = @"
                <p class=""date dynamic-date"">##
                ##
                ## Display dates
                ##
                " + dateForLists + 
                @"</p>##";
            return dates;
        }

        /*
         * Output long description if exists.
         */
        public string descString()
        {
            string desc = @"
                <p class=""description dynamic-description"">
                ##
                ## Display description
                ##
                    #if($resultItem.LongDescription)##
                        $resultItem.LongDescription ##
                    #else##
                        &nbsp;##
                    #end##
                </p>##";
            return desc;
        }

        /*
         * Closing tags for Dynamic list.
         */
        public string closeListItem()
        {
            string close = @"
                    </div>## Close description and title div class
            ";
            return close;
        }

        /*
         * Closing tags for Dynamic list.
         */
        public string closeList()
        {
            string close = @"
                </li>## End list item
            #end## End foreach search results loop
            </ul>## End list
            ";
            return close;
        }

        /*
         * Closing tags for News/Events Dynamic list. Same a closeList() but with tag for "show all" link
         */
        public string closeNews()
        {
            string close = @"
                </li>
            #end
				<li>
					<div class=""image container"">&nbsp;</div>
					<div class=""title container"">
						  <a class=""arrow-link news-footer"" href=""#"">$newsString</a>
					</div>
				</li>
            </ul>
            ";
            return close;
        }

        /*
         * Output formatted list of Blog posts for Blog Landing Page Dynamic List
         */
        public string blogBodyString()
        {
            // Display comment count if comments have been allowed for this blog series
            string commentCount = "";
            foreach (SnippetInfo snippet in PageAssemblyContext.Current.PageAssemblyInstruction.Snippets)
            {
                if (snippet.SnippetTemplatePath.Contains("BlogLandingDynamicList.ascx") &&
                  snippet.Data.Contains("isCommentingAvailable=true"))
                {
                    commentCount = 
                    @"#set($identifier = ${resultItem.ContentType} + ""-""+${resultItem.ContentID})
                        <a class=""comment-count"" href=""${prettyUrl}#disqus_thread"" data-disqus-identifier=""$identifier"">0 Comments</a>";
                }
            }

            // Format date according to language
            string date = @"<span>$resultItem.DateForBlogs</span> ";
            if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "es")
                date = @"<span>$resultItem.DateForBlogsEs</span> ";

            // Put the whole blog snippet template together
            string blogBody = @"
            <div class=""blog-list"">   
            #foreach($resultItem in $DynamicSearch.Results)##
                #set($prettyUrl = $resultItem.HRef)##
                <div class=""row blog-post"">
                #if($resultItem.ThumbnailURL.length() > 0)##
	                <div class=""medium-3 columns post-thumb"">
		                <a href=""$prettyUrl"" title=""$resultItem.LongTitle"">
			                <img src=""$resultItem.ThumbnailURL"" alt="""" />
		                </a>						  
	                </div>
                #end##
	                <div class=""medium-9 columns post-info"">
		                <div class=""post-title clearfix""><h3><a href=""$prettyUrl"">$resultItem.LongTitle</a></h3>"
                        + commentCount + 
		                @"</div>
		                <div class=""date-author"">"
			                + date + @"
                            #if($resultItem.Author.length()>0)##
                                $by $resultItem.Author##
                            #else##
                                &nbsp;##
                            #end##
		                </div>
		                <div>
                #if($resultItem.BlogBody.length()>0)##
	                $resultItem.BlogBody
                #elseif($resultItem.LongDescription.length()>0)##
	                $resultItem.LongDescription
                #else##
	                &nbsp;##
                #end##
		                </div>
		                <p>
			                <a href=""$prettyUrl"">$continueReading &gt;</a>
		                </p>
	                </div>
                </div>
                #set($itemType= $resultItem.type)##
                #set($identifier = $itemType + ""-""+$resultItem.ContentID)
            #end
            </div>
            <script type=""text/javascript"">
            /* * * CONFIGURATION VARIABLES: EDIT BEFORE PASTING INTO YOUR WEBPAGE * * */
            var disqus_shortname = '$DynamicSearch.DisqusShortname'; // required: replace example with your forum shortname
            /* * * DON'T EDIT BELOW THIS LINE * * */
            (function () {
            var s = document.createElement('script'); s.async = true;
            s.type = 'text/javascript';
            s.src = 'http://' + disqus_shortname + '.disqus.com/count.js';
            (document.getElementsByTagName('HEAD')[0] || document.getElementsByTagName('BODY')[0]).appendChild(s);
            }());
            </script>";
            return blogBody;
        }
    }
}
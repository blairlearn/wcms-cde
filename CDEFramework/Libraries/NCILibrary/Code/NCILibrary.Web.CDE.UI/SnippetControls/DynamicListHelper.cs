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
        public string languageStrings()
        {
            String pageLanguage = @"
			#set($videoContent = ""Video"")##
			#set($carouselContent = ""Video Playlist"")##
			#set($infographicContent = ""Infographic"")##
			#set($fileContent = ""File"")##
			#set($postedString = ""Posted"")##
			#set($updatedString = ""Updated"")##
			#set($reviewedString = ""Reviewed"")##
            #set($newsString = ""All NCI news"")##
			";
            if (PageAssemblyContext.Current.PageAssemblyInstruction.GetField("Language") == "es")
            {
                pageLanguage = @"
				#set($videoContent = ""Video"")##
				#set($carouselContent = ""Lista de reproducci&oacute;n"")##
				#set($infographicContent = ""Infograf&iacute;a"")##
    			#set($fileContent = ""Fila"")##
				#set($postedString = ""Publicaci&oacute;n"")##
				#set($updatedString = ""Actualizaci&oacute;n"")##
				#set($reviewedString = ""Revisi&oacute;n"")##
                #set($newsString = ""Todas las noticias del NCI"")##
				";
            }
            return pageLanguage;
        }

        //This is completely dirty and really a hack, but it gets this done.  This should be
        //fixed in a future release. --BryanP 2/10/2015
        public string openList()
        {
            string open = @"
            <ul class=""list no-bullets"">##
            #foreach($resultItem in $DynamicSearch.Results)
                <li class=""general-list-item general"">##";
            return open;
        }

        public string imageString()
        {
            string image = @"
                ##
                ## Display image
                ##
                <div class=""list-item-image image container"">##
                    #if($resultItem.ThumbnailURL)## 
                        <img src=""$resultItem.ThumbnailURL"" class=""item-image image"" align=""left"">
                    #else##
                        &nbsp;##
                    #end##
                </div>##";
            return image;
        }

        public string titleString()
        {
            string title = @"
                    <div class=""title-and-desc title desc container"">##
                        ##
                        ## Display title
                        ##
                        <a href=""$resultItem.Href"" onclick=""NCIAnalytics.SearchResults(this,$resultItem.RecNumber);"" class=""title"">
                            #if($resultItem.ContentType == ""rx:nciFile"")##
                                $fileContent $resultItem.LongTitle $resultItem.MimeType $resultItem.FileSize##
                            #else
                                $resultItem.LongTitle##
                                #if($resultItem.ContentType == ""rx:gloVideo"")##
                                    ($videoContent)##
                                #elseif($resultItem.ContentType == ""rx:gloVideoCarousel"")##
                                    ($carouselContent)##
                                #elseif($resultItem.ContentType == ""rx:cgvInfographic"")##
                                    ($infographicContent)##
                                #end##
                            #end##
                        </a>##
                        <p class=""description date"">";
            return title;
        }

        public string dateString()
        {
            string dates = @"
                        ##
                        ## Display dates
                        ##

                            <span class=""date"">
								#if ($resultItem.DateDisplayMode == 1)##
									($postedString: $resultItem.PostedDate) ##	
								#elseif ($resultItem.DateDisplayMode == 2)##
									($updatedString: $resultItem.UpdatedDate) ##	
								#elseif ($resultItem.DateDisplayMode == 3)##
									($postedString: $resultItem.PostedDate, $updatedString: $resultItem.UpdatedDate) ##
								#elseif ($resultItem.DateDisplayMode == 4)##
									($reviewedString: $resultItem.ReviewedDate) ##	
								#elseif ($resultItem.DateDisplayMode == 5)##
									($postedString: $resultItem.PostedDate, $reviewedString: $resultItem.ReviewedDate) ##
								#elseif ($resultItem.DateDisplayMode == 6)##
									($updatedString: $resultItem.UpdatedDate, $reviewedString: $resultItem.ReviewedDate) ##
								#elseif ($resultItem.DateDisplayMode == 7)##
									($postedString: $resultItem.PostedDate, $updatedString: $resultItem.UpdatedDate, $reviewedString: $resultItem.ReviewedDate) ##
								#end
							</span><br/>##";
            return dates;
        }

        public string descString()
        {
            string desc = @"
                            ##
                            ## Display description
                            ##
                            <span>##
                                #if($resultItem.LongDescription)##
                                    $resultItem.LongDescription ##
                                #else##
                                    &nbsp;##
                                #end##
                            </span>##";
            return desc;
        }

        public string closeList()
        {
            string close = @"
                        </p>
                    </div>
                </li>
            #end
            </ul>
            ";
            return close;
        }

        public string closeNews()
        {
            string close = @"
                        </p>
                    </div>
                </li>
            #end
            <li><a class=""arrow-link news-footer"" href=""#"">$newsString</a></li>
            </ul>
            ";
            return close;
        }
    }
}

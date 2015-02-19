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
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:DynamicListDescImgDate runat=server></{0}:DynamicListDescImgDate>")]
    public class DynamicListDescImgDate : BaseSearchSnippet
    {
        override protected SearchList SearchList
        {
            get
            {
                if (base.SearchList == null)
                {
                    base.SearchList = ModuleObjectFactory<DynamicList>.GetModuleObject(SnippetInfo.Data);

                    //This is completely dirty and really a hack, but it gets this done.  This should be
                    //fixed in a future release. --BryanP 2/10/2015

                    //Set content type and date labels according to locale of page, not the search results
					String pageLanguage = @"
					#set($videoContent = ""Video"")##
					#set($carouselContent = ""Video Playlist"")##
					#set($infographicContent = ""Infographic"")##
					#set($postedString = ""Posted"")##
					#set($updatedString = ""Updated"")##
					#set($reviewedString = ""Reviewed"")##
					";
					if(PageAssemblyContext.Current.PageAssemblyInstruction.GetField("Language") == "es")
					{
						pageLanguage = @"
						#set($videoContent = ""Video"")##
						#set($carouselContent = ""Lista de reproducci&oacute;n"")##
						#set($infographicContent = ""Infograf&iacute;a"")##
						#set($postedString = ""Publicaci&oacute;n"")##
						#set($updatedString = ""Actualizaci&oacute;n"")##
						#set($reviewedString = ""Revisi&oacute;n"")##
						";
					}

                    base.SearchList.ResultsTemplate = base.SearchList.ResultsTemplate = pageLanguage + 
                    @" 
                    <ul class=""list no-bullets"">##
                    #foreach($resultItem in $DynamicSearch.Results)
                        <li class=""general-list-item general"">##
	                        ##
	                        ## Display image
	                        ##
                            <div class=""list-item-image image container"">##
                                #if($resultItem.ThumbnailURL)## 
	                                <img src=""$resultItem.ThumbnailURL"" class=""item-image image"" align=""left"">
                                #else##
                                    &nbsp;##
                                #end##
                            </div>##
                            <div class=""title-and-desc title desc container"">##
                                ##
                                ## Display title
                                ##
                                <a href=""$resultItem.Href"" onclick=""NCIAnalytics.SearchResults(this,$resultItem.RecNumber);"" class=""title"">
                                    $resultItem.LongTitle
                                    #if($resultItem.ContentType == ""rx:gloVideo"")##
                                        ($videoContent)##
                                    #elseif($resultItem.ContentType == ""rx:gloVideoCarousel"")##
                                        ($carouselContent)##
                                    #elseif($resultItem.ContentType == ""rx:cgvInfographic"")##
                                        ($infographicContent)##
                                    #end##
                                </a>##
	                            ##
	                            ## Display dates
    	                        ##
                                <p class=""description"">
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
									</span>##
                                    ##
	                                ## Display description
	                                ##
                                    <span>##
                                        #if($resultItem.LongDescription)##
    	                                    $resultItem.LongDescription ##
                                        #else##
                                            &nbsp;##
                                        #end##
                                    </span>##
                                </p>
                            </div>
                        </li>
                    #end
                    </ul>
                    ";
                }
                return base.SearchList;
            }
        }
    }
}

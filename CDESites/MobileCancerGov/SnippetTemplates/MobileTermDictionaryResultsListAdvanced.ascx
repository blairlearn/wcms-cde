<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MobileTermDictionaryResultsListAdvanced.ascx.cs" Inherits="MobileCancerGov.Web.SnippetTemplates.MobileTermDictionaryResultsListAdvanced" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN"
  "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"> 
   <script type="text/javascript" src="test_js/js/jquery-1.4.2.min.js"></script>
   <script type="text/javascript" src="test_js/js/jquery.scrollExtend.min.js"></script>
   <script type="text/javascript">

	jQuery(document).ready(
		function($) {
		
			$('.scroll_container').data('pageNumber', 1);
			$('.scroll_container').onScrollBeyond(function(target) {
					//Add loading indicator to dom...
										
					ajaxSettings = {
						'success': function(data, textStatus) {
							
							$(data.TermDictionaryItems).each(
								function() { // output block structure for each row
									var localContainer = $("<div>");
									localContainer.appendTo($('.scroll_container'));
									
									$("<a>")
										.attr('href', '/foo?id=' + this.id)
										.html(this.item)
										.appendTo(localContainer);
									
									$("<p>")
										.html(this.TermDictionaryDetail.DefinitionHTML)
										.appendTo(localContainer);
								}
							);
							$('.scroll_container').data().pageNumber++;
						},
						'dataType': 'json',
						'url': 'http://localhost:7001/TermDictionary.svc/GetTermDictionaryListJSON/English?searchTerm=tumor&Contains=false&MaxRows=10&PageNumber=' + $('.scroll_container').data().pageNumber,
					}
					
					jQuery.ajax( ajaxSettings );
					
				}
			);		
		}
	);			
   </script>
<div class="scroll_container">

    <ul>
                <li><a name="tumor"></a>
                <a href="/mtd?cdrid=46634&language=english"
                onclick="NCIAnalytics.TermsDictionaryResults(this,'1');">
                tumor</a>
                </br>
                An abnormal
          mass of tissue that results when cells divide more than
          they should or do not die when...</li>
                
                <li><a name="tumor antigen vaccine"></a>

                <a href="/mtd?cdrid=44927&language=english"
                onclick="NCIAnalytics.TermsDictionaryResults(this,'2');">
                tumor antigen vaccine</a>
                </br>
                A vaccine made of cancer cells, parts of cancer cells, or pure tumor antigens (substances isolated from...</li>
                
                <li><a name="tumor board review"></a>
                <a href="/mtd?cdrid=322893&language=english"
                onclick="NCIAnalytics.TermsDictionaryResults(this,'3');">
                tumor board review</a>

                </br>
                A treatment planning approach in which a number of doctors who are experts in different specialties...</li>
                
                <li><a name="tumor burden"></a>
                <a href="/mtd?cdrid=44627&language=english"
                onclick="NCIAnalytics.TermsDictionaryResults(this,'4');">
                tumor burden</a>
                </br>
                Refers to the number of cancer cells, the size of a tumor, or the amount of cancer in the body. Also called tumor...</li>

                
                <li><a name="tumor debulking"></a>
                <a href="/mtd?cdrid=46635&language=english"
                onclick="NCIAnalytics.TermsDictionaryResults(this,'5');">
                tumor debulking</a>
                </br>
                Surgical removal of as much of a tumor as possible. Tumor debulking may increase the chance that chemotherapy...</li>
                
                <li><a name="tumor infiltrating lymphocyte"></a>
                <a href="/mtd?cdrid=45329&language=english"
                onclick="NCIAnalytics.TermsDictionaryResults(this,'6');">
                tumor infiltrating lymphocyte</a>

                </br>
                A white blood cell that has left the bloodstream and migrated into a tumor.</li>
                
                <li><a name="tumor initiation"></a>
                <a href="/mtd?cdrid=390314&language=english"
                onclick="NCIAnalytics.TermsDictionaryResults(this,'7');">
                tumor initiation</a>
                </br>
                A process in which normal cells are changed so that they are able to form tumors. Substances that cause...</li>

                
                <li><a name="tumor load"></a>
                <a href="/mtd?cdrid=44804&language=english"
                onclick="NCIAnalytics.TermsDictionaryResults(this,'8');">
                tumor load</a>
                </br>
                Refers to the number of cancer cells, the size of a tumor, or the amount of cancer in the body. Also called tumor...</li>
                
                <li><a name="tumor lysis syndrome"></a>
                <a href="/mtd?cdrid=626342&language=english"
                onclick="NCIAnalytics.TermsDictionaryResults(this,'9');">
                tumor lysis syndrome</a>

                </br>
                A condition that can occur after treatment of a fast-growing cancer, especially certain leukemias and...</li>
                
                <li><a name="tumor marker"></a>
                <a href="/mtd?cdrid=46636&language=english"
                onclick="NCIAnalytics.TermsDictionaryResults(this,'10');">
                tumor marker</a>
                </br>
                A substance that may be found in tumor tissue or released from a tumor into the blood or other body fluids. A...</li>

                <li><a name="tumor debulking"></a>
                <a href="/mtd?cdrid=46635&language=english"
                onclick="NCIAnalytics.TermsDictionaryResults(this,'5');">
                tumor debulking</a>
                </br>
                Surgical removal of as much of a tumor as possible. Tumor debulking may increase the chance that chemotherapy...</li>
                
                <li><a name="tumor infiltrating lymphocyte"></a>
                <a href="/mtd?cdrid=45329&language=english"
                onclick="NCIAnalytics.TermsDictionaryResults(this,'6');">
                tumor infiltrating lymphocyte</a>

                </br>
                A white blood cell that has left the bloodstream and migrated into a tumor.</li>
                
                <li><a name="tumor initiation"></a>
                <a href="/mtd?cdrid=390314&language=english"
                onclick="NCIAnalytics.TermsDictionaryResults(this,'7');">
                tumor initiation</a>
                </br>
                A process in which normal cells are changed so that they are able to form tumors. Substances that cause...</li>

                
                <li><a name="tumor load"></a>
                <a href="/mtd?cdrid=44804&language=english"
                onclick="NCIAnalytics.TermsDictionaryResults(this,'8');">
                tumor load</a>
                </br>
                Refers to the number of cancer cells, the size of a tumor, or the amount of cancer in the body. Also called tumor...</li>
                
                <li><a name="tumor lysis syndrome"></a>
                <a href="/mtd?cdrid=626342&language=english"
                onclick="NCIAnalytics.TermsDictionaryResults(this,'9');">
                tumor lysis syndrome</a>

                </br>
                A condition that can occur after treatment of a fast-growing cancer, especially certain leukemias and...</li>
                
                <li><a name="tumor marker"></a>
                <a href="/mtd?cdrid=46636&language=english"
                onclick="NCIAnalytics.TermsDictionaryResults(this,'10');">
                tumor marker</a>
                </br>
                A substance that may be found in tumor tissue or released from a tumor into the blood or other body fluids. A...</li>

                
            
        <br />
    </ul>
</div>
<div style="clear:both;"></div>


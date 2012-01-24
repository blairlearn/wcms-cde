<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MobileTermDictionaryResultsListAdvanced.ascx.cs" Inherits="MobileCancerGov.Web.SnippetTemplates.MobileTermDictionaryResultsListAdvanced" %>
<%@ Register assembly="NCILibrary.Web.UI.WebControls" namespace="NCI.Web.UI.WebControls" tagprefix="NCI" %>
<asp:Literal runat="server" ID="litPageUrl" Visible="false"></asp:Literal>
<asp:Literal runat="server" ID="litSearchBlock"></asp:Literal>
<asp:Literal runat="server" ID="litSearchStr" Visible="false"></asp:Literal>

   <script type="text/javascript" src="http://localhost:7069/test_js/js/jquery.scrollExtend.min.js"></script>
   <style type="text/css">

	div.scroll_container {
		background-color: #EEEEEE;
		width: 500px;
		margin-left: auto;
		margin-right: auto;
		border: 1px solid #CCCCCC;
		text-align: center;
	}


	div.list_item {
		height: 200px;
		background-color: #FEFEFE;
		margin-bottom: 5px;
		width: 90%;
		border: 1px solid #999999;
		margin-left: auto;
		margin-right: auto;
	}

	div.more_content {
		height: 100px;
		width: 500px;
		background-color: #E1ABCE;
		border: 1px solid blue;
	}

   </style>
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
						'url': 'http://localhost:7069/TermDictionary.svc/GetTermDictionaryListJSON/English?searchTerm=tumor&Contains=false&MaxRows=10&PageNumber=' + $('.scroll_container').data().pageNumber,
					}
					
					jQuery.ajax( ajaxSettings );
					
				}
			);		
		}
	);			
   </script>

  <div class="scroll_container" style="max-height:200px;">
     <div id="scroll_items">
        <div class="list_item">
	        Scroll beyond this container to automatically load more content
     	</div>
        <div class="list_item">
	        <a name="tumor"></a>
                <a href="/mtd?cdrid=46634&language=english"
                onclick="NCIAnalytics.TermsDictionaryResults(this,'1');">
                tumor</a>
                </br>
                An abnormal
          mass of tissue that results when cells divide more than
          they should or do not die when...
     	</div>

        <div class="list_item">
	        <a name="tumor antigen vaccine"></a>

                <a href="/mtd?cdrid=44927&language=english"
                onclick="NCIAnalytics.TermsDictionaryResults(this,'2');">
                tumor antigen vaccine</a>
                </br>
                A vaccine made of cancer cells, parts of cancer cells, or pure tumor antigens (substances isolated from...
                
	</div>

        <div class="list_item">
	  <a name="tumor board review"></a>
                <a href="/mtd?cdrid=322893&language=english"
                onclick="NCIAnalytics.TermsDictionaryResults(this,'3');">
                tumor board review</a>

                </br>
                A treatment planning approach in which a number of doctors who are experts in different specialties...

     	</div>

        <div class="list_item">
	       <a name="tumor burden"></a>
                <a href="/mtd?cdrid=44627&language=english"
                onclick="NCIAnalytics.TermsDictionaryResults(this,'4');">
                tumor burden</a>
                </br>
                Refers to the number of cancer cells, the size of a tumor, or the amount of cancer in the body. Also called tumor...

     	</div>

        <div class="list_item">
	       <a name="tumor lysis syndrome"></a>
                <a href="/mtd?cdrid=626342&language=english"
                onclick="NCIAnalytics.TermsDictionaryResults(this,'9');">
                tumor lysis syndrome</a>

                </br>
                A condition that can occur after treatment of a fast-growing cancer, especially certain leukemias and...

     	</div>

     </div>
  </div>
    <div style="clear:both;"></div>


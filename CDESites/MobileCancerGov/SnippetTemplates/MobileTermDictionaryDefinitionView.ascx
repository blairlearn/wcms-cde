<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MobileTermDictionaryDefinitionView.ascx.cs" Inherits="MobileCancerGov.Web.SnippetTemplates.MobileTermDictionaryDefinitionView" %>
<%@ Register Assembly="NCILibrary.Web.ContentDeliveryEngine.UI" Namespace="NCI.Web.CDE.UI.WebControls" TagPrefix="NCI" %>
<script type="text/javascript">
    //Hookup JPlayer for Audio
    if (jQuery.jPlayer) {
        jQuery(document).ready(function($) {
            var my_jPlayer = $("#dictionary_jPlayer");

            my_jPlayer.jPlayer({
                swfPath: "/PublishedContent/files/global/flash/", //Path to SWF File Used by jPlayer
                //errorAlerts: true,
                supplied: "mp3" //The types of files which will be used.
            });

            //Attach a click event to the audio link
            $(".CDR_audiofile").click(function() {
                my_jPlayer.jPlayer("setMedia", {
                    mp3: $(this).attr("href") // Defines the m4v url
                }).jPlayer("play");

                return false;
            });
        });
    }
</script>
<div id="dictionary_jPlayer"></div>
<asp:Literal runat="server" ID="litPageUrl" Visible="false"></asp:Literal>
<asp:Literal runat="server" ID="litSearchBlock"></asp:Literal>
<h3><% =TermName %></h3>
<% if(!String.IsNullOrEmpty(AudioPronounceLink)) { %>
<div class="audioPronounceLink">
<% =AudioPronounceLink %>
</div>
<br />
<% } %>

<div class="definition">
<% =DefinitionHTML %>
</div>
<br />

<% if(!String.IsNullOrEmpty(ImageLink)) { %>
<div class="imageLink">
<% =ImageLink %>
</div>
<br />
<% } %>

<% if(!String.IsNullOrEmpty(ImageCaption)) { %>
<div class="imageCaption">
<% =ImageCaption %>
</div>
<% } %>
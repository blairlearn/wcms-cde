<%@ Page Language="c#" CodeBehind="definition.aspx.cs" AutoEventWireup="True" Inherits="CancerGov.Web.Common.PopUps.Definition" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head id="header" runat="server">

    <script src="/PublishedContent/js/popEvents.js" type="text/javascript"></script>
    <script type="text/javascript" language="JavaScript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.5.1/jquery.min.js"></script>
    <script src="/PublishedContent/js/jquery.jplayer.min.js" type="text/javascript"></script>
    <link href="/PublishedContent/Styles/nvcg.css" rel="stylesheet" />

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
                $("a.CDR_audiofile").click(function() {
                    my_jPlayer.jPlayer("setMedia", {
                        mp3: $(this).attr("href") // Defines the m4v url
                    }).jPlayer("play");

                    return false;
                });
            });
        }
           
    </script>


</head>

<body>
    <div id='dictionary_jPlayer'></div>
    <div class="heading">
        <%=strHeading%>
    </div>
    <div class="audioPronounceLink">
        <%=this.Content.Render()%>
    </div>
    <asp:Literal ID="litOmniturePageLoad" Mode="PassThrough" runat="server" />
</body>
</html>

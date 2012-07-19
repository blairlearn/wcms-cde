<%@ Page Language="c#" CodeBehind="definition.aspx.cs" AutoEventWireup="True" Inherits="CancerGov.Web.Common.PopUps.Definition" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head id="header" runat="server">

    <script src="/PublishedContent/js/popEvents.js" type="text/javascript"></script>
    <script type="text/javascript" language="JavaScript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.5.1/jquery.min.js"></script>
    <script src="/PublishedContent/js/jquery.jplayer.min.js" type="text/javascript"></script>
    <link href="/PublishedContent/Styles/nci.css" rel="stylesheet" />

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

<body leftmargin="0" topmargin="0" marginwidth="0" marginheight="0" class="popup">
    <div align="center">
    <div id='dictionary_jPlayer'></div>
        <p>
            <table border="0" cellpadding="10" cellspacing="0" width="100%">
                <tr>
                    <td align="left" valign="top">
                        <div style="width: 100%;">
                            <%=strHeading%>
                            <div class="audioPronounceLink">
                            <%=this.Content.Render()%>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <div style="width: 100%;">
                            <a href="Javascript:window.print();">
                                <%=strSendPrinter%></a>
                        </div>
                    </td>
                </tr>
            </table>
            <p>
    </div>
    <asp:Literal ID="litOmniturePageLoad" Mode="PassThrough" runat="server" />
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="popDefinition.aspx.cs"
    Inherits="TCGA.Web.Common.PopUps.PopDefinition" %>

<%@ Import Namespace="NCI.Web.Dictionary.BusinessObjects" %>
<!DOCTYPE html>
<html>
<head id="header" runat="server">
    <title>Dictionary of Cancer Terms</title>
    <link rel="stylesheet" href="/PublishedContent/Styles/nci.css" type="text/css" />
    <link rel="stylesheet" href="/PublishedContent/Styles/nvcg.css" type="text/css" />
    <meta content="text/html;charset=ISO-8859-1" http-equiv="content-type" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />

    <script src="/PublishedContent/js/modernizr.custom.2.7.1.js" type="text/javascript"></script>

    <script src="/PublishedContent/js/popEvents.js" type="text/javascript"></script>

    <script type="text/javascript" language="JavaScript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.5.1/jquery.min.js"></script>

    <script src="/PublishedContent/js/jquery.jplayer.min.js" type="text/javascript"></script>

    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.5.1/jquery.min.js"></script>

    <script type="text/javascript">
        //Hookup JPlayer for Audio
        if (jQuery.jPlayer && !Modernizr.touch) {
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

    <script type="text/javascript">
        $(window).load(function() {
            $(window).resize();
        });
    </script>

</head>
<body leftmargin="0" topmargin="0" marginheight="0" marginwidth="0">
    <!--[if lt IE 9]>
            <script src="/PublishedContent/js/respond.js"></script>
        <![endif]-->
    <div class="popup">
        <div class="clearfix">
            <div class="nci-logo">
                <a>NATIONAL CANCER INSTITUTE
                    <br />
                    <span>at the National Institutes of Health</span></a>
            </div>
            <div class="popup-close">
                <a href="javascript:window.parent.window.close();"><span class="hidden" id="closeWindowText"
                    runat="server" /></a>
            </div>
        </div>
        <p>
        </p>
        <div id='dictionary_jPlayer'>
        </div>
        <asp:PlaceHolder ID="phDefinition" runat="server">
            <div class="heading">
                <asp:Literal ID="definitionLabel" runat="server" Text="Definition:" />
            </div>
            <asp:Repeater ID="termDictionaryDefinitionView" runat="server" OnItemDataBound="termDictionaryDefinitionView_OnItemDataBound">
                <ItemTemplate>
                    <div class="audioPronounceLink">
                        <span class="term">
                            <%# ((DictionaryTerm)(Container.DataItem)).Term%></span>
                        <asp:PlaceHolder ID="phPronunciation" runat="server">
                            <asp:Label ID="pronunciationKey" runat="server" CssClass="pronunciation" />
                        </asp:PlaceHolder>
                    </div>
                    <div class="definition">
                        <%# ((DictionaryTerm)(Container.DataItem)).Definition.Text%></div>
                    <asp:Panel runat="server" ID="pnlRelatedInfo">
                        <div class="definitionImage">
                            <asp:Repeater ID="relatedImages" runat="server" Visible="false" OnItemDataBound="relatedImages_OnItemDataBound">
                                <ItemTemplate>
                                    <figure class="image-left-medium">
                                    <a id="termEnlargeImage" runat="server" target="_blank" class="article-image-enlarge no-resize">Enlarge</a>
                                    <img id="termImage" runat="server" src="" alt="" />
                                        <figcaption>
                                            <div class="caption-container no-resize">
                                            <p><%# ((ImageReference)(Container.DataItem)).Caption  %></p>
                                            </div>
                                        </figcaption>
                                 </figure>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </asp:Panel>
                </ItemTemplate>
            </asp:Repeater>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="phNoResult" runat="server" Visible="false">
            <div class="definition">The term you are looking for does not exist in the glossary.</div>
        </asp:PlaceHolder>
        <asp:Literal ID="litOmniturePageLoad" Mode="PassThrough" runat="server" />
    </div>
</body>
</html>

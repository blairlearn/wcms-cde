<%@ Page Language="c#" CodeBehind="definition.aspx.cs" AutoEventWireup="True" Inherits="CancerGov.Web.Common.PopUps.Definition" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head id="header" runat="server">

<link href="/PublishedContent/Styles/nci.css" rel="stylesheet" />
<link href="/PublishedContent/Styles/nci-new.css" rel="stylesheet" />
<link href="/PublishedContent/Styles/nciplus.css" rel="stylesheet" />
<link href="/PublishedContent/Styles/emergency_IE.css" rel="stylesheet" />
<link href="/PublishedContent/Styles/jquery-ui-1.8.5.custom.css" rel="stylesheet" />
<script src="/PublishedContent/js/imgEvents.js" type="text/javascript"></script>
<script src="/PublishedContent/js/popEvents.js" type="text/javascript"></script>
<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.5.1/jquery.min.js" type="text/javascript"></script>
<script src="/PublishedContent/js/jquery-widgets.js" type="text/javascript"></script>
<script src="/PublishedContent/js/jquery-ui-1.8.5.custom.min.js" type="text/javascript"></script>
<script src="/PublishedContent/js/JSLoader.js" type="text/javascript"></script>
<script src="/PublishedContent/js/modernizr-1.7.min.js" type="text/javascript"></script>
<script src="/PublishedContent/js/jquery.ui.position.js" type="text/javascript"></script>
<script src="/PublishedContent/js/jquery.ui.autocomplete.js" type="text/javascript"></script>
<script src="/PublishedContent/js/sw-autocomplete.js" type="text/javascript"></script>
<script src="https://ajax.googleapis.com/ajax/libs/swfobject/2.2/swfobject.js" type="text/javascript"></script>
<script src="/PublishedContent/js/NCIGeneralJS.js" type="text/javascript"></script>
<script src="/PublishedContent/js/jquery.jplayer.min.js" type="text/javascript"></script>




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
                            <span class="CDR_audiofile">
                            <%=this.Content.Render()%>
                            </span>
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

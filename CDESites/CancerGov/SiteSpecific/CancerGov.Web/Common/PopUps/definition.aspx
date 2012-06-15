<%@ Page Language="c#" CodeBehind="definition.aspx.cs" AutoEventWireup="True" Inherits="CancerGov.Web.Common.PopUps.Definition" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head id="header" runat="server">
    <script src="/PublishedContent/js/imgEvents.js" type="text/javascript"></script>
    <script src="/PublishedContent/js/popEvents.js" type="text/javascript"></script>
    <script src="/PublishedContent/js/modernizr-1.7.min.js" type="text/javascript"></script>
    <script src="/PublishedContent/js/wcmsAudio.js" type="text/javascript"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.5.1/jquery.min.js" type="text/javascript"></script>
	<link href="/PublishedContent/Styles/nci.css" rel="stylesheet" type="text/css" />
</head>

<body leftmargin="0" topmargin="0" marginwidth="0" marginheight="0" class="popup">
    <div align="center">
        <p>
            <table border="0" cellpadding="10" cellspacing="0" width="100%">
                <tr>
                    <td align="left" valign="top">
                        <div style="width: 100%;">
                            <%=strHeading%>
                            <%=this.Content.Render()%>
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

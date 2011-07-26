<%@ Page Language="c#" CodeBehind="definition.aspx.cs" AutoEventWireup="True" Inherits="CancerGov.Web.Common.PopUps.Definition" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head id="header" runat="server">

    <script type="text/javascript" language="JavaScript" src="/JS/imgEvents.js"></script>
    <script type="text/javascript" language="JavaScript" src="/JS/popEvents.js"></script>
    <script type="text/javascript" language="JavaScript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.5.1/jquery.min.js"></script>
    <script type="text/javascript" language="JavaScript" src="https://ajax.googleapis.com/ajax/libs/swfobject/2.2/swfobject.js"></script>
    <script type="text/javascript" language="JavaScript" src="/PublishedContent/js/modernizr-1.7.min.js"></script>
    <script type="text/javascript" language="JavaScript" src="/PublishedContent/js/wcmsAudio.js"></script>
    <link rel="stylesheet" href="/stylesheets/nci.css" type="text/css" />
</head>

<body leftmargin="0" topmargin="0" marginwidth="0" marginheight="0">
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

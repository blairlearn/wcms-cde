<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cbComments.ascx.cs"
    Inherits="CancerGov.Web.SnippetTemplates.CancerBulletin.cbComments" %>
<style>
    .GoodText
    {
        font-weight: bold;
        color: #484bd0;
        font-family: Trebuchet MS, Tahoma, Verdana, Arial, sans-serif;
    }
    .BadText
    {
        font-weight: bold;
        color: #a8364d;
        font-family: Trebuchet MS, Tahoma, Verdana, Arial, sans-serif;
    }
    .InfoText
    {
        font-weight: bold;
        font-size: 19px;
        color: #333367;
        font-family: Trebuchet MS, Tahoma, Verdana, Arial, sans-serif;
    }
    .HeaderText
    {
        font-family: Times New Roman, Serif;
        font-size: 19px;
        color: #C5080B;
    }
</style>

<script src="/Scripts/JSLoader/JSLoader.js" type="text/javascript"></script>

<table border="0" cellpadding="0" cellspacing="0" width="571" valign="top">
    <tr>
        <td valign="top">
            <a name="skiptocontent"></a>
            <table border="0" cellpadding="10" cellspacing="0">
                <tr runat="server" id="trForm">
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <div style="font-family: Arial; margin-bottom: 3px; margin-top: 0px;">
                            Share your comments about the <i>NCI Cancer Bulletin</i>.</div>
                        <p>
                            <!-- We welcome your ideas and suggestions for the Director's Updates. Please use the form below to submit your comments, including topics you would like addressed in future Director's Updates.-->
                        </p>
                        <div style="font-family: Arial;">
                            <%=this.strError%></div>
                        <br />
                        <form runat="server" id="frmComments" method="post">
                        <label for="txtComment" class="hidden">
                            comments</label>
                        <textarea name="txtComment" id="txtComment" rows="15" wrap="soft" cols="65" style="font-size: 12px;
                            width: 550px;"></textarea>
                        <br />
                        <input type="submit" value="Submit" style="font-size: 12px;">
                        </form>
                    </td>
                </tr>
                <tr runat="server" id="trThanks">
                    <td valign="top">
                        <table border="0" cellpadding="10" cellspacing="0">
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td valign="top">
                                    <%=this.strPostResponse%>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>

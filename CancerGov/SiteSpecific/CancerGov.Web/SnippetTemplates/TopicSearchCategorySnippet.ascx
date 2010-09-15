<%@ Control Language="C#" AutoEventWireup="true" Inherits="CancerGov.Modules.TopicSearch.UI.TopicSearchCategorySnippet" %>
<form id="Form1" runat="server">
<table cellspacing="0" cellpadding="1" border="0" width="571" class="gray-border">
	<tbody><tr>
			<td valign="top">
<table cellspacing="0" cellpadding="0" border="0" bgcolor="#ffffff" width="569">
<tbody><tr>
<td valign="top" class="box-title"><img height="25" border="0" width="7" alt="" src="$rxs_imageBase/spacer.gif"></td>
<td valign="middle" class="box-title" colspan="2">INSERT TABLE TITLE HERE</td>
</tr>
<tr>
<td valign="top" class="gray-border" colspan="3"><img height="1" border="0" width="1" alt="" src="$rxs_imageBase/spacer.gif"></td>
</tr>
<tr>
<td valign="top"><img height="7" border="0" width="8" alt="" src="$rxs_imageBase/spacer.gif"></td>
<td valign="top"><img height="1" border="0" width="553" alt="" src="$rxs_imageBase/spacer.gif"></td>
<td valign="top"><img height="1" border="0" width="8" alt="" src="$rxs_imageBase/spacer.gif"></td>
</tr>
<tr>
<td valign="top"><img height="1" border="0" width="7" alt="" src="$rxs_imageBase/spacer.gif"></td>
<td valign="top" class="leftnav">
<table cellspacing="0" cellpadding="0" border="0" width="553">
<%-- --%>
<tbody><tr><td height="10" colspan="4"></td></tr><tr><td width="20"></td><td valign="top"><b>Select Subcategory:</b>
<%--Display topic search radio buttons.--%>
<p>
      <asp:radiobuttonlist id="rblTopicSearchList" runat="server" RepeatLayout="Flow">
      </asp:radiobuttonlist>
<br></p></td>
<%--Display timeframe buttons.--%>
<td width="150" valign="top"><b>Select Timeframe:</b><p><b></b>
      <asp:radiobuttonlist id="rblTimeframeList" runat="server" RepeatLayout="Flow">
        <asp:listitem id="all" runat="server" value="All (No Date Limit)" />
        <asp:listitem id="thirty" runat="server" value="Last 30 Days" />
        <asp:listitem id="sixty" runat="server" value="Last 60 Days" />
        <asp:listitem id="ninety" runat="server" value="Last 90 Days" />
      </asp:radiobuttonlist>
<br></p></td>
<%--Display search button.--%>
<td width="100"></td></tr><tr><td height="3" colspan="4"></td></tr><tr><td width="20"></td><td colspan="3"><br><br>
	<asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click"/>
</td></tr><tr><td height="10" colspan="4"></td></tr>
<%----%>
</tbody></table>
</td>
<td valign="top"><img height="1" border="0" width="7" alt="" src="$rxs_imageBase/spacer.gif"></td>
</tr>
<tr>
<td valign="top" colspan="3"><img height="7" border="0" width="1" alt="" src="$rxs_imageBase/spacer.gif"></td>
</tr>
</tbody></table>
</td>
</tr>
</tbody></table>
</form>
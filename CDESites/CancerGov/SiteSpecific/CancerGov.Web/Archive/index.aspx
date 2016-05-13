<%@ Page language="c#" Codebehind="index.aspx.cs" AutoEventWireup="True" Inherits="www.Archive.index" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>arch_index</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<ul>
				<li>
					<asp:HyperLink ID="lnkProtocol" Runat="server" NavigateUrl="arch_protocols.aspx">Protocols</asp:HyperLink></li>
				<li>
					<asp:HyperLink ID="lnkSummary" Runat="server" NavigateUrl="arch_summaries.aspx">Summaries</asp:HyperLink></li>
				<li>
					<asp:HyperLink ID="lnkDictionary" Runat="server" NavigateUrl="arch_dictionary.aspx">Dictionary</asp:HyperLink></li>
				<li><asp:HyperLink ID="lnkDrugInfoSummary" runat="server" NavigateUrl="arch_druginfosummaries.aspx">Drug Info Summaries</asp:HyperLink></li>
				<li><asp:HyperLink ID="lnkDrugDictionary" runat="server" NavigateUrl="arch_drugdictionary.aspx">Drug Dictionary</asp:HyperLink></li>
			</ul>
		</form>
	</body>
</HTML>

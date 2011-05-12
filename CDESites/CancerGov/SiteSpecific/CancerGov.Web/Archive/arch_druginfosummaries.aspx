<%@ Page Language="C#" CodeBehind="arch_druginfosummaries.aspx.cs" AutoEventWireup="true" Inherits="www.Archive.arch_druginfosummaries" %>
<%@ Register TagPrefix="CancerGovWww" TagName="AlphaListBox" Src="/Common/UserControls/AlphaListBox.ascx"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Drug Info Summaries</title>
	<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
	<meta content="C#" name="CODE_LANGUAGE" />
	<meta content="JavaScript" name="vs_defaultClientScript" />
	<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
	<link rel="stylesheet" href="/PublishedContent/Styles/nci.css" type="text/css" />
	<script src="/JS/JSLoader/JSLoader.js" type="text/javascript"></script>
</head>
<body>
		<h1>Drug Info Summaries</h1>
		<hr />

		<asp:DataGrid id="drugDictionaryList" CssClass="black-text" runat="server" AutoGenerateColumns="False"
			EnableViewState="False" AlternatingItemStyle-BackColor="LemonChiffon" HeaderStyle-BackColor="Gold"
			HeaderStyle-Font-Bold="True" HeaderStyle-HorizontalAlign="Center">
			<Columns>
				<asp:TemplateColumn HeaderText="ID">
					<ItemTemplate>
						<asp:HyperLink id="HyperLink1" Text='<%# DataBinder.Eval(Container.DataItem,"DrugInfoSummaryID") %>' NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"PrettyURL") + "?print=1" %>' Target="_blank" runat="server" />
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:BoundColumn DataField="Title" HeaderText="Name"></asp:BoundColumn>
			</Columns>
		</asp:DataGrid>
		<br />
		<hr />
</body>
</html>

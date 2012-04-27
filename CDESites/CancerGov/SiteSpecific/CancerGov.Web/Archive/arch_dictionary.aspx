<%@ Page language="c#" Codebehind="arch_dictionary.aspx.cs" AutoEventWireup="True" Inherits="www.Archive.arch_dictionary" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>arch_dictionary</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link rel="stylesheet" href="/PublishedContent/Styles/nci.css" type="text/css">
		<script src="/JS/JSLoader/JSLoader.js" type="text/javascript"></script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<h1>Dictionary</h1>
		<asp:panel id="alphaListPanel" Runat="server" HorizontalAlign="Center">
			<asp:Label id="alphaList" runat="server" CssClass="navigation-dark-red-link"></asp:Label>
		</asp:panel>
		<hr>
		<asp:datalist id="wordList" runat="server" EnableViewState="false" CellSpacing="10" AlternatingItemStyle-BackColor="LemonChiffon">
			<ItemTemplate>
				<a name='<%# DataBinder.Eval(Container.DataItem, "TermName") %>' ></a>
				<asp:Label ID="TermName" CssClass="header-B" Runat="server">
					<%# DataBinder.Eval(Container.DataItem, "TermName") %>
				</asp:Label>
				<asp:Label ID="TermPronunciation" CssClass="black-text" Runat="server">
					<%# DataBinder.Eval(Container.DataItem, "TermPronunciation") %>
				</asp:Label>
				[<asp:Label ID="Language" CssClass="black-text" Runat="server">
					<%# DataBinder.Eval(Container.DataItem, "Language") %>
				</asp:Label>
				/
				<asp:Label ID="Audience" CssClass="black-text" Runat="server">
					<%# DataBinder.Eval(Container.DataItem, "Audience") %>
				</asp:Label>
				/
				<asp:Label ID="Dictionary" CssClass="black-text" Runat="server">
					<%# DataBinder.Eval(Container.DataItem, "Dictionary") %>
				</asp:Label>]
				<br>
				<asp:Label ID="Definition" CssClass="black-text" Runat="server">
					<%# DataBinder.Eval(Container.DataItem, "DefinitionHTML") %>
				</asp:Label>
			</ItemTemplate>
		</asp:datalist>
	</body>
</HTML>

<%@ Page language="c#" Codebehind="arch_summaries.aspx.cs" AutoEventWireup="True" Inherits="www.Archive.arch_summaries" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Summary</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	    <script src="/Scripts/JSLoader/JSLoader.js" type="text/javascript"></script>
	</HEAD>
	<body>
		<h1>Summaries</h1>
		<hr>
		<asp:DataGrid id="summaryList" CssClass="black-text" runat="server" AutoGenerateColumns="False"
			EnableViewState="False" AlternatingItemStyle-BackColor="LemonChiffon" HeaderStyle-BackColor="Gold"
			HeaderStyle-Font-Bold="True" HeaderStyle-HorizontalAlign="Center">
			<Columns>
				<asp:TemplateColumn HeaderText="ID">
					<ItemTemplate>
						<asp:HyperLink id="docLink" Text='<%# DataBinder.Eval(Container.DataItem,"SummaryID") %>' NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"prettyurl") %>' Target="_blank" runat="server" />
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:BoundColumn DataField="Type" HeaderText="Type"></asp:BoundColumn>
				<asp:BoundColumn DataField="Audience" HeaderText="Audience"></asp:BoundColumn>
				<asp:BoundColumn DataField="Language" HeaderText="Language"></asp:BoundColumn>
				<asp:BoundColumn DataField="Title" HeaderText="Title"></asp:BoundColumn>
			</Columns>
		</asp:DataGrid>
		<br>
		<hr>
	</body>
</HTML>

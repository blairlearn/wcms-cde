<%@ Page Language="C#" CodeBehind="arch_drugdictionary.aspx.cs" AutoEventWireup="true" Inherits="www.Archive.arch_drugdictionary" %>
<%@ Register TagPrefix="CancerGovWww" TagName="AlphaListBox" Src="/Common/UserControls/AlphaListBox.ascx"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Drug Dictionary</title>
	<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
	<meta content="C#" name="CODE_LANGUAGE" />
	<meta content="JavaScript" name="vs_defaultClientScript" />
	<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
	<link rel="stylesheet" href="/stylesheets/nci.css" type="text/css" />
	<script src="/Scripts/JSLoader/JSLoader.js" type="text/javascript"></script>
</head>
<body>
		<h1>Drug Dictionary</h1>
		<hr />

        <table style="width: 300px; font-size: small; margin: 0px auto;">
            <CancerGovWww:AlphaListBox runat="server" id="alphaListBox"
                    BaseUrl="/archive/arch_drugdictionary.aspx" NumericItems="true" ShowAll="false"
                    CssClass="navigation-dark-red-link" />
        </table>

		<asp:DataGrid id="drugDictionaryList" CssClass="black-text" runat="server" AutoGenerateColumns="False"
			EnableViewState="False" AlternatingItemStyle-BackColor="LemonChiffon" HeaderStyle-BackColor="Gold"
			HeaderStyle-Font-Bold="True" HeaderStyle-HorizontalAlign="Center">
			<Columns>
				<asp:TemplateColumn HeaderText="ID">
					<ItemTemplate>
						<asp:HyperLink id="HyperLink1" Text='<%# DataBinder.Eval(Container.DataItem,"TermID") %>' NavigateUrl='<%# "/Templates/drugdictionary.aspx?page=1&print=1&cdrid=" + DataBinder.Eval(Container.DataItem,"TermID") %>' Target="_blank" runat="server" />
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:BoundColumn DataField="PreferredName" HeaderText="Name"></asp:BoundColumn>
				<asp:BoundColumn DataField="DefinitionHTML" HeaderText="Definition"></asp:BoundColumn>
			</Columns>
		</asp:DataGrid>
		<br />
		<hr />
</body>
</html>

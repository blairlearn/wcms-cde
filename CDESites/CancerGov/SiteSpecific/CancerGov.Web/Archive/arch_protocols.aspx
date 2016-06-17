<%@ Page language="c#" Codebehind="arch_protocols.aspx.cs" AutoEventWireup="True" Inherits="www.Archive.arch_protocols" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Protocol</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	</HEAD>
	<body>
		<h1>Protocols</h1>
		<pre>"_P" for Patient, "_H" for Health Professional</pre>
		<hr>
		<asp:DataGrid id="protocolList" CssClass="black-text" runat="server" AutoGenerateColumns="False"
			EnableViewState="False" AlternatingItemStyle-BackColor="LemonChiffon" HeaderStyle-BackColor="Gold"
			HeaderStyle-Font-Bold="True" HeaderStyle-HorizontalAlign="Center">
			<Columns>
				<asp:TemplateColumn HeaderText="ID">
					<ItemTemplate>
						<asp:HyperLink id="patientLink" Text='<%# DataBinder.Eval(Container.DataItem,"ProtocolID") + "_P" %>' NavigateUrl='<%# "/ClinicalTrials/search/view?cdrid=" + DataBinder.Eval(Container.DataItem,"ProtocolID") + "&version=patient&print=1" %>' Target="_blank" runat="server" />
						<asp:HyperLink id="profLink" Text='<%# DataBinder.Eval(Container.DataItem,"ProtocolID") + "_H" %>' NavigateUrl='<%# "/ClinicalTrials/search/view?cdrid=" + DataBinder.Eval(Container.DataItem,"ProtocolID") + "&version=healthprofessional&print=1" %>' Target="_blank" runat="server" />
					</ItemTemplate>
				</asp:TemplateColumn>
				<asp:BoundColumn DataField="CurrentStatus" HeaderText="Status"></asp:BoundColumn>
				<asp:BoundColumn DataField="HealthProfessionalTitle" HeaderText="Title"></asp:BoundColumn>
			</Columns>
		</asp:DataGrid>
		<br>
		<hr>
	</body>
</HTML>

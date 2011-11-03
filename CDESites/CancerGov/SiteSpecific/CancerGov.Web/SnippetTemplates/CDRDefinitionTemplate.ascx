<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CDRDefinitionTemplate.ascx.cs" Inherits="CancerGov.Web.SnippetTemplates.CDRDefinitionTemplate" %>
<script type="text/javascript"> 
function toggle() {
    var DefMore = document.getElementById("<% =spDefinitionTextMore.ClientID %>");
    var spDef = document.getElementById("<% =spDefinitionText.ClientID %>");

    var text = document.getElementById("<% =moreLink.ClientID %>");
    var Definition = document.getElementById("<% =ltDefinitionText.ClientID %>");
    if (DefMore.style.display == "block") {
        DefMore.style.display = "none";
		text.innerHTML = "";
  	}
  	else {
  	    spDef.style.display = "none"
  	    DefMore.style.display = "block";
		text.innerHTML = "";
		spDefinitionText.innerHTML = divDefinitionTextMore.innerHTML;
	}
} 
</script>
<span id="spDefinitionText" style="display: block" runat="server">
<asp:Literal runat="server" ID="ltDefinitionText"></asp:Literal>&nbsp;<a visible="false" name="moreLink" id="moreLink" href="javascript:toggle();" runat="server"></a></span>
<span id="spDefinitionTextMore" style="display: none" runat="server"></span>




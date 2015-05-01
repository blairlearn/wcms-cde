<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GeneticsTermDictionaryDefinitionView.ascx.cs" Inherits="CancerGov.Web.SnippetTemplates.GeneticsTermDictionaryDefintionView" %>
<%@ Register Assembly="NCILibrary.Web.ContentDeliveryEngine.UI" Namespace="NCI.Web.CDE.UI.WebControls" TagPrefix="NCI" %>

<asp:Literal runat="server" ID="litPageUrl" Visible="false"></asp:Literal>
<div id="welcomeDiv">
    <p>Welcome to the NCI Dictionary of Genetics Terms, which contains technical definitions for more than 150 terms related to genetics. These definitions were developed by the <a href="/cancertopics/pdq/cancer-genetics-board">PDQ® Cancer Genetics Editorial Board</a> to support the evidence-based, peer-reviewed <a href="/cancertopics/pdq/genetics">PDQ cancer genetics information summaries</a>.</p>
</div>
<asp:Literal runat="server" ID="litSearchBlock"></asp:Literal>
<asp:PlaceHolder ID="phSearchBox" runat="server" />
<dl><dt><dfn><% =TermName %></dfn></dt>
<% if(!String.IsNullOrEmpty(AudioPronounceLink)) { %>
<dd class="pronunciation">
<% =AudioPronounceLink %>
</dd>
<% } %>

<dd class="definition">
<% =DefinitionHTML %>
</dd>


<dd class="relatedInfo">
<% =RelatedInfoHTML%>
</dd>


<% if(!String.IsNullOrEmpty(MediaHTML)) { %>
   <dd class="imageLink">
      <% =MediaHTML %>
   </dd>
<% } %>
</dl>
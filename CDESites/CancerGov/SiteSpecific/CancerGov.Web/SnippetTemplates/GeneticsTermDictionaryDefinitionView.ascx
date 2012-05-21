<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GeneticsTermDictionaryDefinitionView.ascx.cs" Inherits="CancerGov.Web.SnippetTemplates.GeneticsTermDictionaryDefintionView" %>
<%@ Register Assembly="NCILibrary.Web.ContentDeliveryEngine.UI" Namespace="NCI.Web.CDE.UI.WebControls" TagPrefix="NCI" %>

<asp:Literal runat="server" ID="litPageUrl" Visible="false"></asp:Literal>
<asp:Literal runat="server" ID="litSearchBlock"></asp:Literal>
<asp:PlaceHolder ID="phSearchBox" runat="server" />
<h3><% =TermName %></h3>
<% if(!String.IsNullOrEmpty(AudioPronounceLink)) { %>
<div class="audioPronounceLink">
<% =AudioPronounceLink %>
</div>
<br />
<% } %>

<div class="definition">
<% =DefinitionHTML %>
</div>
<br />

<div class="relatedInfo">
<% =RelatedInfoHTML%>
</div>
<br />

<% if(!String.IsNullOrEmpty(ImageLink)) { %>
   <div class="imageLink">
      <% =ImageLink %>
      <br />
      <% if(!String.IsNullOrEmpty(ImageCaption)) { %>
         <div class="caption">
            <% =ImageCaption %>
         </div>
      <% } %>
   </div>
<% } %>
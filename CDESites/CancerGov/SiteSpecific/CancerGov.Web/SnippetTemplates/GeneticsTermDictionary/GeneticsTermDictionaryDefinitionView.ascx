<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GeneticsTermDictionaryDefinitionView.ascx.cs" Inherits="CancerGov.Web.SnippetTemplates.GeneticsTermDictionaryDefintionView" %>
<%@ Register Assembly="NCILibrary.Web.ContentDeliveryEngine.UI" Namespace="NCI.Web.CDE.UI.WebControls" TagPrefix="NCI" %>

<asp:Literal runat="server" ID="litPageUrl" Visible="false"></asp:Literal>
<asp:Literal runat="server" ID="litSearchBlock"></asp:Literal>
<asp:PlaceHolder ID="phSearchBox" runat="server" />
<dl><dt><dfn><% =TermName %></dfn></dt>
<% if(!String.IsNullOrEmpty(AudioPronounceLink)) { %>
<dd class="audioPronounceLink">
<% =AudioPronounceLink %>
</dd>
<% } %>

<dd class="definition">
<% =DefinitionHTML %>
</dd>


<dd class="relatedInfo">
<% =RelatedInfoHTML%>
</dd>


<% if(!String.IsNullOrEmpty(ImageLink)) { %>
   <dd class="imageLink">
      <% =ImageLink %>
      <% if(!String.IsNullOrEmpty(ImageCaption)) { %>
         <div class="caption">
            <% =ImageCaption %>
         </div>
      <% } %>
   </dd>
<% } %>
</dl>
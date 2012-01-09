<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MobileTermDictionaryDefinitionView.ascx.cs" Inherits="MobileCancerGov.Web.SnippetTemplates.MobileTermDictionaryDefinitionView" %>
<asp:Literal runat="server" ID="litPageUrl" Visible="false"></asp:Literal>
<asp:Literal runat="server" ID="litSearchBlock"></asp:Literal>
<h3><% =TermName %></h3>
<!-- addthis block -->
<div class="addthis_toolbox addthis_container addthis_default_style addthis_32x32_style" id="AddThisButtonList1">
    <a class="addthis_button_email"></a>
    <a class="addthis_button_facebook"></a>
    <a class="addthis_button_twitter"></a>
    <a class="addthis_button_plus.google.com"></a>
    <a class="addthis_button_compact"></a>
</div>
<script src="http://s7.addthis.com/js/250/addthis_widget.js#pubid=xa-4ed7cc9f006efaac" type="text/javascript"></script>
<!-- end addthis block -->
<br/>

<% if(!String.IsNullOrEmpty(AudioPronounceLink)) { %>
<div class="audioPronounceLink">
<% =AudioPronounceLink %>
</div>
<br/>
<% } %>

<div class="definition">
<% =DefinitionHTML %>
</div>
<br />

<% if(!String.IsNullOrEmpty(ImageLink)) { %>
<div class="imageLink">
<% =ImageLink %>
</div>
<br />
<% } %>

<% if(!String.IsNullOrEmpty(ImageCaption)) { %>
<div class="imageCaption">
<% =ImageCaption %>
</div>
<% } %>
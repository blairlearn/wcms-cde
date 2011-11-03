<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DocTitleBlock.ascx.cs"
    Inherits="CancerGov.Web.SnippetTemplates.DocTitleBlock" %>
<asp:PlaceHolder ID="phWeb" runat="server" Visible="false">
    <!-- Begin Document Title Block -->
    <div class="document-title-block" <% =TableColor %> >
        <% if (ModuleData.TitleDisplay == "DocTitleBlockBodyField")
           {
               Response.Write(ModuleData.ContentField); %>
           <% } else{ %>
        <asp:Image ID="imgImage" BorderWidth="0" runat="server" AlternateText="" GenerateEmptyAlternateText="true" />
        <h1>
            <% =Title%></h1>
        <% if (!string.IsNullOrEmpty(ModuleData.SubTitle))
           { %><span class="subtitle"><% =ModuleData.SubTitle%>
           </span>
        <% } %>
        <% if (!string.IsNullOrEmpty(ModuleData.LinkTitle))
           { %> 
        <span class="backtomain"><a href="<% =ModuleData.LinkUrl %>">
            <% =ModuleData.LinkTitle%></a></span><% } %>
            
        <%} %>
    </div>
    <!-- END Document Title Block -->
</asp:PlaceHolder>
<asp:PlaceHolder ID="phPrint" runat="server" Visible="false">
    <div class="document-title-block" <% =TableColor %> >
        <h1>
            <% =Title %></h1>
        <% if (!string.IsNullOrEmpty(ModuleData.SubTitle))
           { %><span class="subtitle"><% =ModuleData.SubTitle%>
           </span>
        <% } %>
        <span class="document-title">
            <asp:Literal ID="litAudienceTitle" runat="server" /></span>
        <div style="float: right">
            <asp:Literal ID="litPrintDate" runat="server" />
        </div>
    </div>
</asp:PlaceHolder>
<asp:PlaceHolder ID="phPrintNoAudience" runat="server" Visible="false">
    <div class="document-title-block" <% =TableColor %> >
        <h1>
            <% =Title %></h1>
        <% if (!string.IsNullOrEmpty(ModuleData.SubTitle))
           { %><span class="subtitle"><% =ModuleData.SubTitle%>
           </span>
        <% } %>
    </div>
</asp:PlaceHolder>

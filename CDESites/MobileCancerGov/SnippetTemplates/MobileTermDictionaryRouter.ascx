<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MobileTermDictionaryRouter.ascx.cs" Inherits="MobileCancerGov.Web.SnippetTemplates.MobileTermDictionaryRouter" %>
<!-- Temporary location for page CSS 1 -->
<style type="text/css">
    .term
    {
        font-family: Arial,Helvetica,sans-serif;
        font-size: 21px;
        font-weight: bold;
    }
    .audio
    {
        font-family: Arial,Helvetica,sans-serif;
    }
    .pronounce
    {
        font-family: Arial,Helvetica,sans-serif;
    }
    .definition
    {
        font-family: Arial,Helvetica,sans-serif;
    }
    .media
    {
        font-family: Arial,Helvetica,sans-serif;
     }    
    .imageCaption 
    {
        font-family: Arial,Helvetica,sans-serif;
        font-size: 14px;
        font-style:italic;
     }    
     .az
    {
        font-family: Arial,Helvetica,sans-serif;
        font-size: 21px;
     }
     .heading1
     {
     	font-family: Arial,Helvetica,sans-serif;
        font-size: 21px;
        font-weight: bold;
     }
     .wait 
     { 
     	cursor: wait !important; 
     }
</style>
<!-- End Temporary location for page CSS -->
<asp:PlaceHolder ID="phMobileTermDictionary" runat="server" />

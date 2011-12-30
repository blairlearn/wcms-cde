<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MobileTermDictionaryRouter.ascx.cs" Inherits="MobileCancerGov.Web.SnippetTemplates.MobileTermDictionaryRouter" %>
<!-- Mobile Term Dictionary CSS -->
<style type="text/css">
    .mtd_term
    {
        font-family: Arial,Helvetica,sans-serif;
        font-size: 21px;
        font-weight: bold;
    }
    .mtd_audio
    {
        font-family: Arial,Helvetica,sans-serif;
    }
    .mtd_pronounce
    {
        font-family: Arial,Helvetica,sans-serif;
    }
    .mtd_definition
    {
        font-family: Arial,Helvetica,sans-serif;
    }
    .mtd_media
    {
        font-family: Arial,Helvetica,sans-serif;
     }    
    .mtd_imageCaption 
    {
        font-family: Arial,Helvetica,sans-serif;
        font-size: 14px;
        font-style:italic;
     }    
     .mtd_az
    {
        font-family: Arial,Helvetica,sans-serif;
        font-size: 21px;
     }
     .mtd_heading1
     {
     	font-family: Arial,Helvetica,sans-serif;
        font-size: 21px;
        font-weight: bold;
     }
     .mtd_wait 
     { 
     	cursor: wait !important; 
     }
     .mtd_imageCaption
     {
     	width: 200px;
     }
     .mtd_spacer1
    {
        height: 8px;
    }
</style>
<!-- End Mobile Term Dictionary CSS -->
<asp:PlaceHolder ID="phMobileTermDictionary" runat="server" />

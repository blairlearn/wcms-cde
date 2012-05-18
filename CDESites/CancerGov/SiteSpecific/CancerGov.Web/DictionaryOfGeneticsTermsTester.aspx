<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DictionaryOfGeneticsTermsTester.aspx.cs" Inherits="CancerGov.Web.DictionaryOfGeneticsTermsTester" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="CancerGovWww" TagName="GeneticsTermDictionary" Src="~/SnippetTemplates/GeneticsTermDictionaryRouter.ascx"%>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<title>Dictionary of Genetics Terms Tester</title>
</head>
<body>

    <CancerGovWww:GeneticsTermDictionary runat="server" />

    

   
</body>
</html>

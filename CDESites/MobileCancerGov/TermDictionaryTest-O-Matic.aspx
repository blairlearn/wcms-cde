<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TermDictionaryTest-O-Matic.aspx.cs" Inherits="MobileCancerGov.Web.TermDictionaryTest_O_Matic" %>

<%@ Register src="UserControls/TermDictionary.ascx" tagname="TermDictionary" tagprefix="uc1" %>

<%@ Register src="SnippetTemplates/MobileTermDictionaryDefinitionView.ascx" tagname="MobileTermDictionaryDefinitionView" tagprefix="uc2" %>

<%@ Register src="SnippetTemplates/MobileTermDictionaryResultsList.ascx" tagname="MobileTermDictionaryResultsList" tagprefix="uc3" %>

<%@ Register src="SnippetTemplates/MobileTermDictionaryRouter.ascx" tagname="MobileTermDictionaryRouter" tagprefix="uc4" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>Mobile Dictionary of Cancer Terms - National Cancer Institute</title>
    <!-- CGov Mobile Header Info -->
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" /> 
    <!-- CGov Mobile Header Info End-->    
    <link href="/PublishedContent/Styles/jquery.mobile-1.0rc2.css" rel="stylesheet" />
    <link href="/PublishedContent/Styles/nci-mobile.css" rel="stylesheet" />
    <script src="/PublishedContent/js/jquery-1.6.4.min.js" type="text/javascript"></script>
    <script src="/PublishedContent/js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="/PublishedContent/js/ajax-linking.js" type="text/javascript"></script>
    <script src="/PublishedContent/js/jquery.mobile-1.0rc2.js" type="text/javascript"></script>
    <script src="/PublishedContent/js/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="/PublishedContent/js/jquery.ui.position.js" type="text/javascript"></script>
    <script src="/PublishedContent/js/jquery.ui.autocomplete.js" type="text/javascript"></script>
    <script src="/PublishedContent/js/sw-autocomplete.js" type="text/javascript"></script>
    <script src="/PublishedContent/js/search.js" type="text/javascript"></script>
    <script src="/PublishedContent/js/NCIGeneralJS.js" type="text/javascript"></script>
    <link rel="canonical" href="http://imaging.cancer.gov/" />
    <meta name="keywords" />
    <meta name="description" />
    <meta name="content-language" content="en" />
    <meta name="english-linking-policy" />
    <meta name="espanol-linking-policy" />
</head>
<body>
    <form id="form1" runat="server">
<!--
Calling Page Top---------------------------------------------------------------------------<br/>    
-->
    <uc4:MobileTermDictionaryRouter ID="MobileTermDictionaryRouter2" 
        runat="server" />
    </form>
<!-- 
Calling Page Bottom------------------------------------------------------------------<br/>
-->

</body>
</html>

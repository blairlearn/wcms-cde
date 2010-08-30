<%@ Page Language="C#" AutoEventWireup="true" Inherits="NCI.Web.CDE.UI.WebPageAssembler" %>
<%@ Register Src="../UserControls/Headers/MainPageHeader.ascx" TagName="MainPageHeader" TagPrefix="NCI" %>

<!doctype html public "-//w3c//dtd html 4.0 transitional//en">
<!-- saved from url=(0026)http://www.cancer.gov/i131 -->
<html>
<head>
    <link href="/StyleSheets/nci.css" rel="Stylesheet" />
    <link href="/StyleSheets/nci_general_browsers.css" rel="Stylesheet" />
	<script type="text/javascript" language="JavaScript" src="/scripts/imgEvents.js"></script>
	<script type="text/javascript" language="JavaScript" src="/scripts/popEvents.js"></script>
</head>
<body leftmargin=0 topmargin=0 marginwidth="0" marginheight="0"><!-- site banner -->
    <div id="bannerdiv" align="center">
        <nci:mainpageheader runat="server" id="mainpageheader">
        </nci:mainpageheader>
    </div>
<div align=center></div><!-- content header -->
<div id=headerzone align=center></div><!-- main area -->
<div align=center>
<table border=0 cellspacing=0 cellpadding=0 width=771>
  <tbody>
  <tr>
    <td valign=top><img border=0 alt="" 
      src="radioactive%20i-131%20from%20fallout%20-%20national%20cancer%20institute_files/spacer.gif" 
      width=9 height=1></td><!-- left nav column -->
    <td id=leftzone valign=top>
      <table border=0 cellspacing=0 cellpadding=0 width=178>
        <tbody>
        <tr>
          <td valign=top align=left>
            <table class=gray-border border=0 cellspacing=0 cellpadding=1 
            width=164>
              <tbody>
              <tr>
                <td valign=top>
                <nci:templateslot id="quicklinkslot" runat="server" />
                  </td></tr></tbody></table>
            <table border=0 cellspacing=0 cellpadding=0 width=1>
              <tbody>
              <tr>
                <td><img border=0 alt="" 
                  src="radioactive%20i-131%20from%20fallout%20-%20national%20cancer%20institute_files/spacer.gif" 
                  width=1 height=6></td></tr></tbody></table>
            <nci:templateslot id="questionsslot" runat="server" cssclass="leftnavslot" />
            <table border=0 cellspacing=0 cellpadding=0 width=1>
              <tbody>
              <tr>
                <td><img border=0 alt="" 
                  src="radioactive%20i-131%20from%20fallout%20-%20national%20cancer%20institute_files/spacer.gif" 
                  width=1 height=6></td></tr></tbody></table>
            <nci:templateslot id="quitsmokingslot" runat="server"  />            
            <table border=0 cellspacing=0 cellpadding=0 width=1>
              <tbody>
              <tr>
                <td><img border=0 alt="" 
                  src="radioactive%20i-131%20from%20fallout%20-%20national%20cancer%20institute_files/spacer.gif" 
                  width=1 height=6></td></tr></tbody></table>
             
            <nci:templateslot id="ncihighlightsslot" runat="server"  />            
            <table border=0 cellspacing=0 cellpadding=0 width=1>
              <tbody>
              <tr>
                <td><img border=0 alt="" 
                  src="radioactive%20i-131%20from%20fallout%20-%20national%20cancer%20institute_files/spacer.gif" 
                  width=1 height=6></td></tr></tbody></table></td>
          <td valign=top><img border=0 alt="" 
            src="radioactive%20i-131%20from%20fallout%20-%20national%20cancer%20institute_files/spacer.gif" 
            width=16 height=1></td></tr></tbody></table></td><!-----------------------><!-- red line -->
            <!-- main content area goes here -->
            <nci:templateslot id="maincontentslot" runat="server"  />
            <!----------------------->
    <td valign=top><img border=0 alt="" 
      src="radioactive%20i-131%20from%20fallout%20-%20national%20cancer%20institute_files/spacer.gif" 
      width=10 height=1></td></tr></tbody></table></div><!-- end main area --><!-- footer -->
    <!-- footer -->
    <div id="footerzone" align="center">
        <nci:templateslot id="footer" runat="server" removeifempty="false" />    
    </div>
    <!-- end foooter-->
</body></html>

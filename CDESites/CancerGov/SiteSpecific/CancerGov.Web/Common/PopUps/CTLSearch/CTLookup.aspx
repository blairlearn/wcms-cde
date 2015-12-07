<%@ Page language="c#" Codebehind="CTLookup.aspx.cs" AutoEventWireup="True" Inherits="CancerGov.Web.CTLookup" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
  <head>
    <title>Cancer.gov</title>
    <link rel="stylesheet" href="/PublishedContent/Styles/nvcg.css" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
  </head>
  <frameset rows="325,*,65" id="Lookup" frameborder="no" border="0">
	<frame name="search" title="Search" src="/Common/PopUps/CTLSearch/CTLookupSearch.aspx?fld=<%=Request.Params["fld"]%>&type=<%=Request.Params["type"]%>&title=<%=Title%>" scrolling="no" noresize/>
    <frame name="results" title="Results" src="/Common/PopUps/CTLSearch/CTLookupResults.aspx?fld=<%=Request.Params["fld"]%>" scrolling="auto" frameborder="0" />
    <frame name="footer" title="Footer" src="/Common/PopUps/CTLSearch/CTFooter.htm" scrolling="no" noresize>
    <frame name="select" title="Select" src="/Common/PopUps/CTLSearch/blank.htm" scrolling="no" noresize>

</frameset>
</html>

<%@ Page language="c#" Codebehind="CTLookup.aspx.cs" AutoEventWireup="True" Inherits="CancerGov.Web.CTLookup" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
  <head>
    <title>Cancer.gov</title>
    <link rel="stylesheet" href="/stylesheets/nci.css" />
  </head>
  <frameset rows="275,*,40" id="Lookup" frameborder="no" border="0">
	<frame name="search" title="Search" src="/Common/popups/CTLSearch/CTLookupSearch.aspx?fld=<%=Request.Params["fld"]%>&title=<%=Title%>" scrolling="no" noresize/>
	<frame name="results" title="Results" src="/Common/popups/CTLSearch/CTLookupResults.aspx?fld=<%=Request.Params["fld"]%>" scrolling="auto" frameborder="0" />
	<frame name="select" title="Select" src="/Common/popups/CTLSearchblank.htm" scrolling="no" noresize>	
  </frameset>
</html>

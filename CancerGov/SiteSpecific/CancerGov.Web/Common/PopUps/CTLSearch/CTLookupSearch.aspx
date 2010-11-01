<%@ Page language="c#" Codebehind="CTLookupSearch.aspx.cs" AutoEventWireup="True" Inherits="CancerGov.Web.CTLookupSearch" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
    <head>
        <link rel="stylesheet" href="/stylesheets/nci.css" />
		<script language="javascript">
			function doSubmit()
			{
			    userAg = navigator.userAgent;
                bName = navigator.appName;
                bVer  = parseInt(navigator.appVersion);

                window.parent.frames[2].location.href = '/common/popups/CTLSearch/CTLookupSelect.aspx?fld=<%=Request.Params["fld"]%>';
				document.lookupSearch.submit();
			}
		</script>
    </head>
  
	<body class="cts-az-search">
	
        <!-- Top Header Section -->
        <table width="100%" cellspacing="0" cellpadding="0" border="0" bgcolor="#A80101">
        <tr>
         <td valign="top">
              <table width="681" cellspacing="0" cellpadding="0" border="0" bgcolor="#A80101">
              <tr>
                 <td valign="top" rowspan="2"><img src="/images/header_popup.gif" width="328" height="54" alt="National Cancer Institute" border="0"></td>
	              <td valign="top" rowspan="2"><img src="/images/spacer.gif" width="205" height="54" alt="" border="0"></td>
	              <td valign="top"><img src="/images/spacer.gif" width="149" height="34" alt="" border="0"></td>
              </tr>
              <tr>
                 <td valign="top"><a href="#" onclick="javascript:window.parent.close();"><img src="/images/popup_close_window.gif" width="149" height="20" alt="Close Window" border="0"></a></td>
              </tr>
              </table>
          </td>
          <td valign="top" width="100%" bgcolor="#A80101"><img src="/images/spacer.gif" width="1" height="1" alt="" border="0"></td>
        </tr>
        </table>	
        <!-- end Top Header Section -->

        <form name="lookupSearch" method="get" action="/common/popups/CTLSearch/CTLookupResults.aspx" target="results" onsubmit="javascript: document.forms[0].alphaIndex.value=''; doSubmit();">
	        <input type="hidden" name="title" value="<%=Title%>">
	        <input type="hidden" name="alphaIndex" value="<%=InputAlphaIndex%>">
	        <input type="hidden" name="fld" value="<%=Request.Params["fld"]%>">

            <h1><%=Title%></h1>   
            <p id="caption"><%=Caption%></p>
            <p id="alphaSearch"><span>Click a Letter/#:</span><%=AlphaIndexLinks%></p>
            <div class="cts-az-search-textsearch">
                <label for="SearchBox"><%=TextInputPrompt%>:</label>
                <input type="text" id="SearchBox" name="keyword" value="<%=InputKeyword%>" />
                <input type="image" src="/images/search_red.gif" alt="Search" title="Search" />
            </div>
         </form>
	</body>
</html>

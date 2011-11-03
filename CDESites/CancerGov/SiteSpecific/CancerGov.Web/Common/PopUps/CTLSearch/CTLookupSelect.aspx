<%@ Page debug="true"  language="c#" Codebehind="CTLookupSelect.aspx.cs" AutoEventWireup="True" Inherits="CancerGov.Web.CTLookupSelect" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
  <head>
        <link rel="stylesheet" href="/stylesheets/nci.css" />
		<title>lookup_select</title>				
		<script language="javascript">
			function doSubmit(fld)
			{
			    var itemsSelected = false;
				var returnControl = eval('window.parent.opener.window.' + fld + "_obj");
                var selectedText;
                var selectedValue;

			    var chkIds='';
				var chkValues = '';
				var chkInputs = window.parent.results.document.forms[0].elements;
				
				for(i = 0; i < chkInputs.length; i++)
				{
					if(chkInputs[i] != null)
					{
						if(chkInputs[i].checked == true)
						{
							if(chkInputs[i].value != '')
							{
//								if(chkValues != '')
//								{
//									chkIds +=	",";							
//									chkValues += "; ";
//								}


					    		var fIndex = chkInputs[i].value.indexOf("{");
				    			var lIndex = chkInputs[i].value.indexOf("}");
			    				if (fIndex > 0)
		    					{
	    						    selectedText = chkInputs[i].value.substring(0,fIndex);
        							selectedValue = chkInputs[i].value.substring(fIndex+1,lIndex);
//								    chkValues += chkInputs[i].value.substring(0,fIndex);
//							    	chkIds += chkInputs[i].value.substring(fIndex+1,lIndex);
								}
						    	else
					    		{
				    			    selectedText = chkInputs[i].value;
			    				    selectedValue = "0";
//		    						chkValues += chkInputs[i].value;
//	    							chkIds += "0";
			    				}
								returnControl.AddEntry(selectedText, selectedValue);
								itemsSelected = true;
							}
						}
					}
				}
				
				if(itemsSelected)
    				RevealParentListArea('<%=Request.Params["fld"]%>');

				window.parent.window.close();				
			}

            function RevealParentListArea(field){
			    switch(field){
			        case "drug":
			            window.parent.opener.window.showDrugList();
                        break;
                    case "intervention":
			            window.parent.opener.window.showInterventionList();
                        break;
                    case "investigator":
			            window.parent.opener.window.showInvestigatorList();
                        break;
                    case "leadOrg":
			            window.parent.opener.window.showLeadOrgList();
                        break;
                    case "institution":
			            window.parent.opener.window.showInstitutionList();
			    }
            }
		</script>	
  </head>
  <body leftmargin="0" topmargin="0" marginwidth="0" marginheight="0" style="background:none">
	
	<table border="0" cellpadding="0" cellspacing="0" width="100%">
	<tr>
	 <td><img src="/images/spacer.gif" width="10" height="15" alt=""></td>
	 <td></td>
	<tr>
	 
	 <td valign="top" width="100%">
	 <table><tr><td>
    <form>
      <input type="image" src="/images/ctsearch/add-selected-btn.gif" name="selectchecked" onclick="doSubmit('<%=Request.Params["type"]%>');" alt="Add Selected" title="Add Selected" />
     </form>
     </td></tr>
     <tr>
	 <td><img src="/images/spacer.gif" width="10" height="15" alt=""></td>
	 <td></td>
	<tr>
     
     </table>
     
     
     </td>
   </tr> 
     <tr><td bgcolor="#e4e4e3" width="100%">
                 <a href="#" onclick="javascript:window.parent.close();"><img src="/images/pop_close.gif" width="117" height="26" alt="Close Window" border="0"></a>

     </td></tr>

	</table>
  </body>
</html>

<%@ Page debug="true"  language="c#" Codebehind="CTLookupSelect.aspx.cs" AutoEventWireup="True" Inherits="CancerGov.Web.CTLookupSelect" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
  <head>
        <link rel="stylesheet" href="/PublishedContent/Styles/nci.css" />
		<title>lookup_select</title>				
		<script language="javascript">
		    function doSubmit(fld) {
		        var parentDeleteList = window.parent.opener.window.jQuery(fld);
		        var chkInputs = window.parent['results'].document.forms['resultsForm'].elements['chkItem'];

		        for (var i = 0; i < chkInputs.length; i++) {
		            if (chkInputs[i].checked && chkInputs[i].value !== '') {
		                // check if the deletelist has already been created
		                if (typeof parentDeleteList.data('nci-deletelist') === 'undefined') {
		                    parentDeleteList.deletelist();
		                }

		                var selectedArray = checkedItems[i].value.split(/[{}]/),
			                deleteListItem = {};

		                if (selectedArray.length === 3) {
		                    deleteListItem.name = selectedArray[0];
		                    deleteListItem.value = selectedArray[1];
		                } else {
		                    deleteListItem.name = selectedArray[0];
		                    deleteListItem.value = '0';
		                }

		                parentDeleteList.deletelist('addItem', deleteListItem);
		                RevealParentListArea('<%=Request.Params["fld"]%>');
		            }
		        }

		        window.parent.window.close();
		    }

		    function RevealParentListArea(field) {
		        switch (field) {
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
                <td valign="top" width="100%">
                <div class="popup-add-selected">
                <form>
      <input type="image" src="/images/ctsearch/add-selected-btn.gif" name="selectchecked" onClick="doSubmit('<%=Request.Params["type"]%>');" alt="Add Selected" title="Add Selected" />
     </form>
</div>
     </td>
   </tr> 
     <tr><td bgcolor="#e4e4e3" width="100%">
                 <a href="#" onclick="javascript:window.parent.close();"><img src="/images/pop_close.gif" width="117" height="26" alt="Close Window" border="0"></a>

     </td></tr>

                </table>
  </body>

</html>

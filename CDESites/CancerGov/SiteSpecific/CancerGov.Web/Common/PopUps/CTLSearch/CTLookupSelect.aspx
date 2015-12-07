<%@ Page debug="true"  language="c#" Codebehind="CTLookupSelect.aspx.cs" AutoEventWireup="True" Inherits="CancerGov.Web.CTLookupSelect" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
  <head>
        <link rel="stylesheet" href="/PublishedContent/Styles/nvcg.css" />
		<title>lookup_select</title>				
		<script language="javascript">
		    function doSubmit(fld) {
		        var parentDeleteList = window.parent.opener.window.jQuery('#' + fld);
		        var chkInputs = window.parent['results'].document.forms['resultsForm'].elements['chkItem'];

                //If there is only one elem, this will not be an array.
		        if (chkInputs && typeof chkInputs.length === "undefined") {
		            chkInputs = [chkInputs];
		        }
		        
		        for (var i = 0; i < chkInputs.length; i++) {
		            if (chkInputs[i].checked && chkInputs[i].value !== '') {
		                // check if the deletelist has already been created
		                if (typeof parentDeleteList.data('nci-deletelist') === 'undefined') {
		                    parentDeleteList.deletelist();
		                }

		                var selectedArray = chkInputs[i].value.split(/[{}]/),
			                deleteListItem = {};

		                if (selectedArray.length === 3) {
		                    deleteListItem.name = selectedArray[0];
		                    deleteListItem.value = selectedArray[1];
		                } else {
		                    deleteListItem.name = selectedArray[0];
		                    deleteListItem.value = '0';
		                }

		                parentDeleteList.deletelist('addItem', JSON.stringify(deleteListItem));
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
    <body class="popup">
        <form>
            <div class="row">
                <div class="small-12 columns">
                    <p>
                        <button name="selectchecked" class="button submit" onclick="doSubmit('<%=Request.Params["type"]%>');">Add Selected</button>
                        <button class="button reset"onclick="window.parent.close();">Close Window</button>
                    </p>
                </div>
            </div>
        </form>
  </body>

</html>

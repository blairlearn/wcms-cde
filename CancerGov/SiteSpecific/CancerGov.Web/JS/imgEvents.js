<!--
function MM_swapImgRestore() 
{
	var i;
	var x;
	var a = document.MM_sr; 

	for(i = 0; a && i < (a.length) && (x = a[i]) && x.oSrc; i++)
	{
		x.src = x.oSrc;		
	}
}

function MM_preloadImages() 
{ 
	var d = document; 
  
	if(d.images)
	{
		if(!d.MM_p)
		{
			d.MM_p = new Array();
		}

		var i;
		var j = d.MM_p.length;
		var a = MM_preloadImages.arguments; 

		for(i = 0; i < a.length; i++)
		{
			if (a[i].indexOf("#") != 0)
			{ 
				d.MM_p[j] = new Image; 
				d.MM_p[j++].src = a[i];			
			}
		}
	}
}

function MM_findObj(n, d) { //v3.0
  var p,i,x;  if(!d) d=document; if((p=n.indexOf("?"))>0&&parent.frames.length) {
    d=parent.frames[n.substring(p+1)].document; n=n.substring(0,p);}
  if(!(x=d[n])&&d.all) x=d.all[n]; for (i=0;!x&&i<d.forms.length;i++) x=d.forms[i][n];
  for(i=0;!x&&d.layers&&i<d.layers.length;i++) x=MM_findObj(n,d.layers[i].document); return x;
}

function MM_swapImage() 
{	
	var i;
	var j = 0;
	var x;
	var a = MM_swapImage.arguments; 
  
	document.MM_sr = new Array;
  
	for(i = 0; i < (a.length - 1); i += 2)
	{		
		if ((x = MM_findObj(a[i])) != null)
		{
			document.MM_sr[j++] = x; 
			x.oSrc = x.src; 
			x.src=a[i + 1];
		}		
	}
}



//Validates RightNav Search Control input
function siteSearchSubmit(bSpanish)
{
	if(trim(document.siteSearchForm.swKeyword.value) != "")
	{
		return true;
	}
	else
	{
		if(bSpanish != null && bSpanish)
			alert("Se necesita t" + String.fromCharCode(233) + "rmino de b" + String.fromCharCode(250) + "squeda.");
		else
			alert("You must enter a search value.");
		document.siteSearchForm.swKeyword.focus();	
	}	
	return false;
}

//Validates RightNav Search Control input
function siteResultSearchSubmit(bSpanish)
{
	if(trim(document.resultForm.txtSWRKeyword.value) != "")
	{
		return true;
	}
	else
	{
		if(bSpanish != null && bSpanish)
			alert("Se necesita t" + String.fromCharCode(233) + "rmino de b" + String.fromCharCode(250) + "squeda.");
		else
			alert("You must enter a search value.");
		document.resultForm.txtSWRKeyword.focus();	
	}	
	return false;
}

function CBSearchSubmit()
{
	if(trim(document.CBSearchForm.cbkeyword.value) == "" || document.CBSearchForm.cbkeyword.value =="Enter Keyword")
	{
		alert("You must enter a search value.");
		document.CBSearchForm.cbkeyword.value="";
		document.CBSearchForm.cbkeyword.focus();
		return false;		
	}
	else{
	    return true; 
	}
}

function CBSetSearchType(e)
{
        // if search is performed by hitting enter in the keyword textbox, set searchType 
        if (window.event) { e = window.event; }
        if (e.keyCode == 13)
        {
                searchType=0;
        }
}


//no need to validate just yet
//Validates RightNav Date Range Search Control input (For newscenter)
function navDateRangeSearchSubmit()
{
//	if(trim(document.navDateRangeSearchForm.keyword.value) != "")
//	{
		document.navDateRangeSearchForm.submit();
//	}
//	else
//	{
//		alert("You must enter a search value.");
//		document.navDateRangeSearchForm.keyword.focus();		
//	}
}

//This is for content_nav
function page(pagenum)
{
    var count = parseInt(document.pageContent.count.value);
	pagenum = parseInt(pagenum);
				
	document.pageContent.first.value = ((pagenum - 1)* count) + 1;
	document.pageContent.selectedPage.value = pagenum;
	document.pageContent.submit();
}

//Removes All Spaces From String
function trim(val)
{
	return val.replace(/\s+/g , '');
}

function CTSimpleSearchSubmit(fm)
{
//	ntptAddPair("cancerType", fm.cancerType.value);
//	ntptAddPair("cancerStage", fm.cancerStage.value);
//	ntptAddPair("trialType", fm.trialType.value);
	AddSelectTextMultiple("cancerType", fm.cancerType);
	AddSelectTextMultiple("cancerStage", fm.cancerStage);
	AddSelectTextMultiple("trialType", fm.trialType);
	ntptAddPair("zipCode", fm.zipCode.value);
	ntptAddPair("zipCodeProximity", fm.zipCodeProximity.value);
	ntptAddPair("nihOnly", fm.nihOnly.value);
	return ntptSubmitTag(fm);
}
  

function CTAdvancedSearchSubmit(fm)
{
//	ntptAddPair("cancerType", fm.cancerType.value);
//	ntptAddPair("cancerStage", fm.cancerStage.value);
//	ntptAddPair("trialType", fm.trialType.value);
	AddSelectTextMultiple("cancerType", fm.cancerType);
	AddSelectTextMultiple("cancerStage", fm.cancerStage);
	AddSelectTextMultiple("trialType", fm.trialType);
	if(fm.trialStatus[0].checked) 	ntptAddPair("trialStatus", "1");
	else 	ntptAddPair("trialStatus", "0");
	ntptAddPair("protocolID", fm.protocolID.value);
	ntptAddPair("zipCode", fm.zipCode.value);
	ntptAddPair("zipCodeProximity", fm.zipCodeProximity.value);
	ntptAddPair("city", fm.city.value);
//	ntptAddPair("state", fm.state.text);
//	ntptAddPair("country", fm.country.text);
	AddSelectTextMultiple("state", fm.state);
	AddSelectTextMultiple("country", fm.country);
	ntptAddPair("institution", fm.institution.value);
	ntptAddPair("nihOnly", fm.nihOnly.value);
	ntptAddPair("newOnly", fm.newOnly.value);
//	ntptAddPair("treatmentType", fm.treatmentType.value);
	AddSelectTextMultiple("treatmentType", fm.treatmentType);
	ntptAddPair("drug", fm.drug.value);
	ntptAddPair("cbDrugFormula", fm.cbDrugFormula.value);
//	ntptAddPair("trialPhase", fm.trialPhase.value);
	AddSelectTextMultiple("trialPhase", fm.trialPhase);
//	ntptAddPair("sponsor", fm.sponsor.value);
	AddSelectValueMultiple("sponsor", fm.sponsor);
//	ntptAddPair("specialCategory", fm.specialCategory.value);
	AddSelectValueMultiple("specialCategory", fm.specialCategory);
	ntptAddPair("investigator", fm.investigator.value);
	ntptAddPair("leadOrg", fm.leadOrg.value);
	return ntptSubmitTag(fm);
}

function AddSelectValueMultiple(keyword, sb)
{ 
	var i;
	var value = "";
	for (i=0;i<sb.options.length;i++)
	{
		var current = sb.options[i];
		if(current.selected)
		{
			if(value != "")
			{
				value=value + ";" + current.value;
			}
			else
			{
				value=current.value;
			}
		}
	}
	ntptAddPair(keyword, value);
	return;
}


function AddSelectTextMultiple(keyword, sb)
{ 
	var i;
	var value = "";
	for (i=0;i<sb.options.length;i++)
	{
		var current = sb.options[i];
		if(current.selected)
		{
			if(value != "")
			{
				value=value + ";" + current.text;
			}
			else
			{
				value=current.text;
			}
		}
	}
	ntptAddPair(keyword, value);
	return;
}


//-->
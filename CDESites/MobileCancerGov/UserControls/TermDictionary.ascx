<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TermDictionary.ascx.cs" Inherits="MobileCancerGov.Web.UserControls.TermDictionary" %>

<!-- Begin Header --> 
	<div class="header"> 

		<div class="site-banner"> 
				<a href="/m/index.html" data-ajax="false"><img src="/m/images/nci-logo.png" height="33"></a>
        </div>
    
  
      
        <div class="searchDiv">
       	 	<a href="/m/index.html" data-ajax="false"><img src="/m/images/icon-home.png" height="35" class="home-icon"></a>

        	<a id="displayText" href="javascript:toggle();" data-ajax="false"><img src="/m/images/icon-search.png" height="35"></a>
        </div>
         
   
        
<div id="toggleText" class="slidingDiv">
<form action="" method="post" name="search">
<input name="" type="text" class="searchInput" id="search" placeholder="Search NCI" />
<a href="/m/global/search-results.html"><img src="/m/images/go-button.png" class="go"></a>
</form>
</div>  

  
</div>

<!-- End Header -->



<div data-role="content" class="content-general">
	<!-- Language Toggle Start --> 
		<div class="langContainer">
          	
			<div class="languageToggle">
				<a href="../es/diccionario/index.html">Español</a>
			</div> 
		</div>
	<!-- Language Toggle End --> 

<!-- Begin Content --> 

	<!-- Page Title Start --> 
			<div class="pageTitle">

				<h2>Dictionary of Cancer Terms</h2>
			</div> 
	<!-- Page Title End -->
<table border="0" cellpadding="5" cellspacing="0" width="100%">
<tbody>
<tr>
<td width="85%">
<input name="" type="text" placeholder="Search Dictionary" style="width:90%" />
</td>
<td>
<div>
 
    <div style="width:35px; float:left;">
    <img src="/m/images/icon-question-mark-red.png" /> 
    </div>
  
    <div class="callBtn">
        <a href="tel:18004226237">Search</a>
    </div>

</div>


</td>
</tr>
</tbody>

</table>



<table border="0" cellpadding="5" cellspacing="0" width="100%">
<tbody>
<tr>
  <td><strong><a href="#">#</a></strong></td>
  <td><strong><a href="#">A</a></strong></td>
  <td><strong><a href="b.html">B</a></strong></td>
  <td><strong><a href="c.html">C</a></strong></td>

  <td><strong><a href="#">D</a></strong></td>
  <td><strong><a href="#">E</a></strong></td>
  <td><strong>F</strong></td>
  <td><strong><a href="#">G</a></strong></td>
  <td><strong><a href="#">H</a></strong></td></tr><tr>
    <td><strong><a href="#">I</a></strong></td>

    <td><strong>J</strong></td>
    <td><strong><a href="#" data-ajax="false">K</a></strong></td>
    <td><strong><a href="l.html">L</a></strong></td>
    <td><strong><a href="#" data-ajax="false">M</a></strong></td>
    <td><strong><a href="#">N</a></strong></td>
    <td><strong><a href="#" data-ajax="false">O</a></strong></td>

    <td><strong><a href="p.html">P</a></strong></td>
    <td><strong>Q</strong></td></tr><tr>
      <td><strong><a href="#" data-ajax="false">R</a></strong></td>
      <td><strong><a href="s.html">S</a></strong></td>
      <td><strong><a href="#">T</a></strong></td>
      <td><strong><a href="u.html">U</a></strong></td>

      <td><strong><a href="#">V</a></strong></td>
      <td><strong><a href="#">W</a></strong></td>
      <td><strong>X</strong></td>
      <td><strong><a href="#">Y</a></strong></td>
<td><strong>Z</strong></td>
</tr>
</tbody>
</table>  
 <!-- Begin Footer --> 

<!--<div style="box-shadow: 0px 5px 5px 2px #888;"> -->

<div style="margin:15px;">
 
    <div style="width:35px; float:left;">
    <img src="/m/images/icon-question-mark-red.png" /> 
    </div>
    
    <div>

    <strong>Questions about cancer?</strong> <a href="#">Talk to a NCI Information Specialist </a>
    </div>
    
    <div class="callBtn">
      <img src="/m/images/phone-icon.png">
        <a href="tel:18004226237">1-800-4-CANCER </a>
    </div>

</div>

<!--</div> -->
 
<div class="footer">
	<ul>
    	<li><a href="/m/index.html" data-ajax="false"><img src="/m/images/icon-home.png"></a></li>
         <li><a href="#top" data-ajax="false"><img src="/m/images/icon-top.png"></a></li>
         <li class="line"><a href="http://www.cancer.gov/" data-ajax="false"><img src="/m/images/icon-full-site.png"></a></li>
    </ul>
    
    <div class="followUs"><strong>Follow us:</strong></div>

    

</div>    
 
<!-- End Footer --> 
<!-- End Content -->     

</div>
<asp:Panel id="basic" runat="server" Visible="false">
<p>This is Basic</p>
</asp:Panel>
<asp:Panel id="advanced" runat="server" Visible="true">
<p>This is advanced</p>
</asp:Panel>








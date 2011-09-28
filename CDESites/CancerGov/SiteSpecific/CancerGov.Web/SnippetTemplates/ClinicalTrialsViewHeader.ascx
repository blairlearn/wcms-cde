<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClinicalTrialsViewHeader.ascx.cs"
    Inherits="CancerGov.Web.SnippetTemplates.ClinicalTrialsViewHeader" %>

<div class="language-dates">
  <div class="version-language"> 
    <ul>
    <% if ( PatientVersion ){ %>
      <li class="one active">Patient Version</li>
      <li class="two"><a href="<% =strPageUrl %>">Health Professional Version</a></li>
      <% } %>
      
    <% if ( HPVersion ) {%>
      <li class="one"><a href="<% =strPageUrl %>">Patient Version</a></li>
      <li class="two active">Health Professional Version</li>
      <% } %>
      
    </ul>
  </div>
  <div class="document-dates"> 
  <ul>
  <% if(!string.IsNullOrEmpty(pvFirstPublished)) {%>
    <li><strong>First Published:</strong><% =pvFirstPublished%></li> 
    <%} %>
  <% if (!string.IsNullOrEmpty(pvLastModified)) { %>
  <li><strong>Last Modified:</strong><% =pvLastModified%> </li> 
  <% } %>
  </ul>
  </div>
</div>


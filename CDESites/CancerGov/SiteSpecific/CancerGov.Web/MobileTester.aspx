<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<!--
<%@ Import Namespace="NCI.Web.CDE.CapabilitiesDetection" %>
<%@ Import Namespace="WURFL" %>
<%@ Page language="c#" %>
-->
<head>
    <title>CancerGov Mobile Tester</title>

    <script language="c#" runat="server">
    void Page_Load(object sender, System.EventArgs e) 
    {
        GridView1.DataSource = DisplayDeviceDetector.DisplayDeviceCapabilitiesList;
        GridView1.DataBind(); 
    } 
  </script>
</head>
<body>
    <form id="form1" runat="server">
    
    <p>This is the CancerGov Mobile Tester</p>
    <p>Display Device: <% =DisplayDeviceDetector.DisplayDeviceString %></p>
    <p>Screen Height: <% =DisplayDeviceDetector.ScreenHeight %></p>
    <p>Screen Width: <% =DisplayDeviceDetector.ScreenWidth %></p>

    <asp:GridView ID="GridView1" runat="server">
    </asp:GridView>
       <script language="CS" runat="server"></script>
    
    </form>
</body>
</html>
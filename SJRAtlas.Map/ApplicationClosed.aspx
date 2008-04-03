<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ApplicationClosed.aspx.cs" Inherits="ApplicationClosed" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Web Mapping Application</title>
</head>
<body style="margin: 0px 0px 0px 0px; background-color: white;  width: 100%; font-family: Verdana; font-size: 8pt; color: DarkGray; " >
    <form id="form1" runat="server">
    <div style="position: absolute; left: 0px; top: 0px; width: 100%; height: 100%; overflow: hidden;">
  <%-- Page Header --%>
            <div id="PageHeader"  class="MapViewer_TitleBannerStyle" style="height: 50px; width: 100%; position: relative;" >
                &nbsp;<asp:Label ID="MapViewer_TitleTextShadowLabel" runat="server" Text="ESRI Web Mapping Application" Font-Size="12pt" Font-Names="Verdana" ForeColor="White" Font-Bold="True" style="position: absolute; left: 5px; top: 5px; z-index: 100;"></asp:Label>
                <div style="top: 0px; float: right;">
                    <asp:HyperLink ID="CloseHyperLink" runat="server" Style="color: White;" NavigateUrl="JavaScript: CloseOut()" Visible="False" ToolTip="Close Application" >Close</asp:HyperLink>&nbsp;&nbsp;
                </div>
            </div>
 <%-- Link and Tool bar --%>
           <div id="LinkBar" class="MapViewer_TaskbarStyle" style="height: 30px; width: 100%">
                <table cellpadding="0" cellspacing="0" style="width: 100%; font-family: Verdana; font-size: 8pt;"><tr>
                    <td style="height: 30px; padding-right: 5px" align="right">
                        <asp:Menu ID="TitleMenu" runat="server" BackColor="Transparent" Orientation="Horizontal" Font-Size="8pt" DataSourceID="SiteMapDataSource1" OnDataBound="TitleMenu_DataBound" SkinID="TaskBarSkin">
                            <StaticMenuItemStyle ItemSpacing="4px" />
                        </asp:Menu>
                        <asp:SiteMapDataSource ID="SiteMapDataSource1" runat="server" ShowStartingNode="false" SiteMapProvider="AspnetXmlSiteMapProvider" />
                        
                    </td>
                </tr></table>
           
           </div>  
          
          <div style="text-align: center; width: 100%; font-size: medium; color: Black; "  >
                <br />
                <br />
                The web application is now closed.
          </div> 
    </div>      
  </form>
</body>
</html>

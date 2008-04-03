<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Map.aspx.cs" Inherits="WebMapApplication" %>

<%@ Register Assembly="ESRI.ArcGIS.ADF.Web.UI.WebControls, Version=9.2.4.1420, Culture=neutral, PublicKeyToken=8fc3cc631e44ad86" Namespace="ESRI.ArcGIS.ADF.Web.UI.WebControls" TagPrefix="esri" %>
<%@ Register Assembly="ESRI.ArcGIS.ADF.Tasks, Version=9.2.4.1420, Culture=neutral, PublicKeyToken=8fc3cc631e44ad86" Namespace="ESRI.ArcGIS.ADF.Tasks" TagPrefix="esriTasks" %>
<%@ Register Src="Measure.ascx" TagName="Measure" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title runat="server" id="HeaderTitle">Web Mapping Application</title>
</head>
<body style="margin: 0px 0px 0px 0px; background-color: white;  width: 100%; font-family: Verdana; font-size: 8pt; color: #A9A9A9; " >
    <form id="form1" runat="server">
    <script language="javascript" type="text/javascript" src="javascript/WebMapApp.js"></script> 
 
 
  <%-- Page Header --%>
            <asp:Panel runat="server" ID="PageHeader" CssClass="MapViewer_TitleBannerStyle" Width="100%" Height="50px" style="position: relative; ">
                <div style="top: 0px; float: right;">
                    <asp:HyperLink ID="CloseHyperLink" runat="server" Style="color: White; font-family: Verdana; font-size: 8pt;" NavigateUrl="JavaScript: CloseOut()" Visible="False" ToolTip="Close Application" >Close</asp:HyperLink>&nbsp;&nbsp;
                </div>
                &nbsp;<asp:Label ID="PageTitle" runat="server" Text="Web Mapping Application" Font-Size="12pt" Font-Names="Verdana" ForeColor="White" Font-Bold="True" style="position: absolute; left: 5px; top: 5px;"></asp:Label>
           </asp:Panel> 
 <%-- Link and Tool bar --%>
            <asp:Panel runat="server" ID="LinkBar" CssClass="MapViewer_TaskbarStyle" Width="100%" Height="30px">
                <table cellpadding="0" cellspacing="0" style="width: 100%; font-family: Verdana; font-size: 8pt;"><tr>
                    <td align="left" style="height: 30px; padding-left: 5px;" valign="middle">
                          <esri:Toolbar ID="Toolbar1" runat="server" BuddyControlType="Map" Group="Toolbar1_Group" Height="28px" 
                        ToolbarItemDefaultStyle-BackColor="Transparent" ToolbarItemDefaultStyle-Font-Names="Arial" 
                        ToolbarItemDefaultStyle-Font-Size="Smaller" ToolbarItemDisabledStyle-BackColor="Transparent" 
                        ToolbarItemDisabledStyle-Font-Names="Arial" ToolbarItemDisabledStyle-Font-Size="Smaller" 
                        ToolbarItemDisabledStyle-ForeColor="Gray" ToolbarItemHoverStyle-BackColor="Transparent" 
                        ToolbarItemHoverStyle-Font-Bold="True" ToolbarItemHoverStyle-Font-Italic="True" 
                        ToolbarItemHoverStyle-Font-Names="Arial" ToolbarItemHoverStyle-Font-Size="Smaller" 
                        ToolbarItemSelectedStyle-BackColor="WhiteSmoke" ToolbarItemSelectedStyle-Font-Bold="True" 
                        ToolbarItemSelectedStyle-Font-Names="Arial" ToolbarItemSelectedStyle-Font-Size="Smaller" 
                        ToolbarStyle="ImageOnly" WebResourceLocation="/aspnet_client/ESRI/WebADF/" 
                        Width="190px" ToolbarItemHoverStyle-BorderColor="Black" ToolbarItemHoverStyle-BorderStyle="Solid" ToolbarItemHoverStyle-BorderWidth="1px" ToolbarItemSelectedStyle-BorderColor="Black" ToolbarItemSelectedStyle-BorderStyle="Solid" ToolbarItemSelectedStyle-BorderWidth="1px" CurrentTool="MapPan">
                            <ToolbarItems>
<esri:Tool DefaultImage="esriZoomIn.png" JavaScriptFile="" ServerActionAssembly="ESRI.ArcGIS.ADF.Web.UI.WebControls" HoverImage="esriZoomIn.png" ServerActionClass="ESRI.ArcGIS.ADF.Web.UI.WebControls.Tools.MapZoomIn" ClientAction="DragRectangle" ToolTip="Zoom In" SelectedImage="esriZoomIn.png" Name="MapZoomIn" Text="Zoom In"></esri:Tool>
<esri:Tool DefaultImage="esriZoomOut.png" JavaScriptFile="" ServerActionAssembly="ESRI.ArcGIS.ADF.Web.UI.WebControls" HoverImage="esriZoomOut.png" ServerActionClass="ESRI.ArcGIS.ADF.Web.UI.WebControls.Tools.MapZoomOut" ClientAction="DragRectangle" ToolTip="Zoom Out" SelectedImage="esriZoomOut.png" Name="MapZoomOut" Text="Zoom Out"></esri:Tool>
<esri:Tool DefaultImage="esriPan.png" JavaScriptFile="" ServerActionAssembly="ESRI.ArcGIS.ADF.Web.UI.WebControls" HoverImage="esriPan.png" ServerActionClass="ESRI.ArcGIS.ADF.Web.UI.WebControls.Tools.MapPan" ClientAction="DragImage" ToolTip="Pan" SelectedImage="esriPan.png" Name="MapPan" Text="Pan"></esri:Tool>
<esri:Command JavaScriptFile="" ServerActionAssembly="ESRI.ArcGIS.ADF.Web.UI.WebControls" HoverImage="esriZoomFullExtent.png" ClientAction="" ToolTip="Full Extent" SelectedImage="esriZoomFullExtent.png" ServerActionClass="ESRI.ArcGIS.ADF.Web.UI.WebControls.Tools.MapFullExtent" Name="MapFullExtent" DefaultImage="esriZoomFullExtent.png" Text="Full Extent"></esri:Command>
<esri:Tool DefaultImage="esriIdentify.png" JavaScriptFile="" HoverImage="esriIdentify.png" ClientAction="MapIdentify('Map1');" ToolTip="Identify (Ctrl-MouseClick)" SelectedImage="esriIdentify.png" Name="MapIdentify" Text="Identify"></esri:Tool>
<esri:Tool DefaultImage="esriMeasure.png" JavaScriptFile="" HoverImage="esriMeasure.png" ClientAction="startMeasure()" ToolTip="Measure" SelectedImage="esriMeasure.png" Name="Measure" Text="Measure"></esri:Tool>
<esri:Command JavaScriptFile="" ClientAction="toggleMagnifier()" ToolTip="Magnifier" Name="Magnifier" DefaultImage="esriShow-Magnify.png" Text="Magnifier"></esri:Command>
</ToolbarItems>
                            <BuddyControls>
<esri:BuddyControl Name="Map1"></esri:BuddyControl>
</BuddyControls>
                        </esri:Toolbar>
                        </td> 
                     <td style="height: 30px; padding-right: 5px" align="right" >
                         &nbsp;
                        
                    </td>
               </tr></table>          
          </asp:Panel> 
 
 <%-- Page content area ..... Left panel and Map display --%>            
           <div id="PageContent" style="width: 100%; position: relative; " onmouseover="webMapAppCheckPanelScroll()">
<%-- Map Display --%>
                          <div id="Map_Panel" style="width: 512px; height: 512px; position: absolute; top: 0px; left: 260px; overflow: hidden;">
                                <esri:Map ID="Map1" runat="server" MapResourceManager="MapResourceManager1" style="" Height="100%" Width="100%" >
                                </esri:Map>
                         </div>
<%-- Left Panel ..... for tasks, results, toc, overview map, etc --%> 
                        <table id="LeftPanelCell" cellpadding="0" cellspacing="0" style="position: absolute; left: 0px; top: 0px; background-color: White;">
                            <tr><td id="LeftPanelTableCell" style="position: relative;">
                        
                            <div id="LeftPanelScrollDiv" style="position: relative; width: auto;">
                           <div id="LeftPanelCellDiv" style="width: 250px;  border: solid 1px #999999; position: relative; " >
                         <!-- TOC ... Map Contents -->
                                         <esri:FloatingPanel ID="Toc_Panel" runat="server" BackColor="White" BorderColor="LightGray"
                                            BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black"
                                            Height="150px" Style="position:relative" Draggable="False"
                                            Title="Map Contents" TitleBarColor="White" TitleBarHeight="20px" TitleBarSeparatorLine="True"
                                            Transparency="0" Width="100%" HeightResizable="True" Font-Bold="True" CloseButton="False" TitleBarForeColor="DarkGray"
                                            WidthResizable="False" ShowDockButton="True" ShowDockedContextMenu="True">
                                                <esri:Toc ID="Toc1" runat="server" Font-Bold="False" Width="100%" Height="100%" Style="left: 0px; width: 100%; position: absolute; top: 0px; height: 100%; overflow:auto;" BuddyControl="Map1" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" RenderOnDemand="True"/>
                                        </esri:FloatingPanel>

                                 <!-- Navigation -->
                                         <esri:FloatingPanel ID="Navigation_Panel" runat="server" BackColor="White" BorderColor="White"
                                            BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black"
                                            Style="position:relative; margin-bottom:0px; overflow: auto;" Draggable="False"
                                            Title="Navigation" TitleBarColor="White" TitleBarHeight="20px" TitleBarSeparatorLine="True"
                                            Transparency="0" Width="100%" HeightResizable="True" Font-Bold="True" CloseButton="False" TitleBarForeColor="DarkGray" WidthResizable="False" ShowDockButton="True" ShowDockedContextMenu="True">
                                              <table id="Navigation_Table" cellpadding="2" cellspacing="0" style="width: 100%" ><tr>
                                              <td align="center" valign="middle" >
                                                 <esri:Navigation ID="Navigation1" runat="server" Map="Map1" Height="100px" Width="100%" ForeColor="Black" BackColor="White" ImageFormat="PNG8" Speed="3">
                                                        <DisplayCharacter CharacterIndex="58" FontName="ESRI North" />
                                                    </esri:Navigation>
                                                </td>
                                                <td align="center" valign="middle" >
                                                  <esri:ZoomLevel ID="ZoomLevel1" runat="server" Map="Map1" />
                                             </td>
                                          </tr></table>
                                       </esri:FloatingPanel>  
                         
                                  <!-- Tasks -->
                                            <esri:FloatingPanel ID="Tasks_Menu_Panel" runat="server" BackColor="White" BorderColor="White"
                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black"
                                                Height="75px" Style="position:relative; margin-bottom:0px;" Draggable="False"
                                                Title="Tasks" TitleBarColor="White" TitleBarHeight="20px" TitleBarSeparatorLine="True"
                                                Transparency="0" Width="100%" Font-Bold="True" CloseButton="False" TitleBarForeColor="DarkGray"
                                                WidthResizable="False" ShowDockButton="True" ShowDockedContextMenu="True" Expanded="False">
                                                <asp:Menu ID="TaskMenu" runat="server" Style="left: 0px; position: relative;
                                                    top: 0px" Orientation="Vertical" BackColor="#F7F6F3" DynamicHorizontalOffset="2" Font-Names="Verdana" Font-Size="8pt" ForeColor="#7C6F57" StaticSubMenuIndent="10px">
                                                    <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                                                    <DynamicHoverStyle BackColor="#7C6F57" ForeColor="White" />
                                                    <DynamicMenuStyle BackColor="#F7F6F3" />
                                                    <StaticSelectedStyle BackColor="#5D7B9D" />
                                                    <DynamicSelectedStyle BackColor="#5D7B9D" />
                                                    <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                                                    <StaticHoverStyle BackColor="#7C6F57" ForeColor="White" />
                                                </asp:Menu>
                                            </esri:FloatingPanel>
                                      
                                  <!-- Results -->
                                            <esri:FloatingPanel ID="Results" runat="server" BackColor="White" BorderColor="White"
                                                BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black"
                                                Height="150px" Style="position:relative; margin-bottom:0px;" Draggable="False"
                                                Title="Results" TitleBarColor="White" TitleBarHeight="20px" TitleBarSeparatorLine="True"
                                                Transparency="0" Width="100%" HeightResizable="True" Font-Bold="True" CloseButton="False" TitleBarForeColor="DarkGray"
                                                Expanded="False" WidthResizable="False" ShowDockButton="True" ShowDockedContextMenu="True">
                                                    <esri:TaskResults ID="TaskResults1" runat="server" BackColor="#ffffff" Font-Names="Verdana" Font-Size="8pt" Font-Bold="False" ForeColor="#000000" Height="200px" Style="left: 0px; width: 100%; position: absolute; top: 0px; height: 100%; overflow:auto;" Width="200px" Map="Map1" />
                                            </esri:FloatingPanel>                                                                                
                                                                   
                                   </div>
                                </div>
                             </td>
                             
                             <td> 
 <%-- Toggle Bar ..... toggles left panel visibility --%>
                                <div id="ToggleCell" style="overflow: hidden; width: 10px; border-style: solid; border-color: #999999; border-bottom-width: 0px; border-left-width: 1px; border-right-width: 1px; border-top-width: 0px; background-color: White; position: relative ">
                                    <table id="ToggleCellTable" cellpadding="0" cellspacing="0" style="position: relative; height: 100%; width: 100%;">
                                        <tr>
                                            <td id="PanelSlider" onmousedown="startWebMapAppDockDrag(event); return false;" style="cursor: e-resize; background-color: White; height: 45%" ></td>
                                        </tr> 
                                        <tr>
                                            <td style="height: 24px;"  ><img id="CollapseImage" src="images/collapse_left.gif" alt="Collapse" onmousedown="togglePanelDock()"  style="cursor: pointer" height="20px" /></td>
                                        </tr> 
                                        <tr>
                                            <td id="PanelSliderBottom" onmousedown="startWebMapAppDockDrag(event); return false;" style="cursor: e-resize; background-color: White; height: 50%" ></td>
                                        </tr> 
                                    </table>
                                </div>
                                
                            </td>
                        </tr>
                   </table>  
                               
    </div>
         

    
        <esri:MapResourceManager ID="MapResourceManager1" runat="server" Style="
            left: 483px; position: absolute; top: 133px; z-index: 102;">
        </esri:MapResourceManager>
 
         <esri:ScaleBar ID="ScaleBar1" runat="server" BarHeight="8" Height="30px" Map="Map1"
            Style=" left: 278px; position: absolute; top: 485px; "
            Width="175px" />
        <asp:Panel ID="CopyrightTextHolder" runat="server" Style="position: absolute; left: 625px; top: 580px; padding: 3px; cursor: pointer; -moz-opacity: 0.75; filter: alpha(opacity=75);" BorderColor="DarkGray" BorderStyle="Solid" BorderWidth="1px" ToolTip="Display Copyright Information" Font-Underline="True" BackColor="White" Font-Size="XX-Small">
            Copyright
        </asp:Panel>
        <esri:FloatingPanel ID="CopyrightText_Panel" runat="server" BackColor="White" BorderColor="Gray"
            BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black"
            Height="200px" Style="left: 295px; position: absolute; top: 517px; overflow: auto;"
            Title="Copyright" TitleBarColor="WhiteSmoke" TitleBarHeight="20px"
            TitleBarSeparatorLine="False" Transparency="35" Width="250px" Visible="False" Docked="False">
           <div id="CopyrightTextContents" style="width: 100%" >Copyright Information</div>
       </esri:FloatingPanel>
     
    <script language="javascript" type="text/javascript">
         setPageElementSizes();
    </script>

        &nbsp;
        
  
         <esri:Magnifier ID="Magnifier1" runat="server" style="position: absolute; left: 567px; top: 304px;" Font-Names="Verdana,Sans Serif" Font-Size="8pt" MagnifierMapResource="MapResourceItem0" Map="Map1" MapResourceManager="MapResourceManager1" TitleBarColor="White" Transparency="50" Visible="False" BackColor="White" BorderColor="DarkGray" BorderStyle="Solid" BorderWidth="1px" Font-Bold="True" ForeColor="Black" TitleBarForeColor="DarkGray" ToolTip="Magnifier">
        </esri:Magnifier>
                
        <uc1:Measure ID="Measure1" runat="server" AreaUnits="Sq_Miles" MapBuddyId="Map1" MapUnits="Resource_Default" MeasureUnits="Miles" NumberDecimals="3"  />

        <esri:TaskManager ID="TaskManager1" runat="server" BuddyControl="TaskMenu" Font-Names="Verdana"
            Font-Size="8pt" ForeColor="Black" Height="17px" Style="left: 570px;
            position: absolute; top: 236px" Width="204px">&nbsp; &nbsp;&nbsp; </esri:TaskManager>
        &nbsp;&nbsp;
        
  
        
<%--
        <asp:TextBox ID="MapDebugBox" runat="server" Height="200px" Width="935px" TextMode="MultiLine" style="left: 5px; position: absolute; top: 549px" Visible="true">Debug box... 
      This is useful for debugging modifications or additions to the Map Viewer application. 
      Set Visible property to true to display Map Viewer requests and responses on web page.
      By default, the property is set to false, and the box will not be rendered on the web page.
       </asp:TextBox>
        <input id="Button1" style="left: 749px; position: absolute; top: 516px" type="button"
            value="Clear Debug Box"  onmousedown="document.forms[0].MapDebugBox.value=''" />

--%>
    </form>
 
    <script language="javascript" type="text/javascript">
        newLoad = <%=m_newLoad %>; 
        webMapAppCloseCallback = "<%=m_closeOutCallback %>";
        webMapAppCopyrightCallback = "<%=m_copyrightCallback %>";

        if (window.addEventListener) window.addEventListener("load",startUp,false);
        else if (window.attachEvent) window.attachEvent("onload",startUp);
    </script> 
</body>
</html>

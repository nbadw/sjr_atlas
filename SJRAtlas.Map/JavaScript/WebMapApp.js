
// Javascript file for Web Mapping Application

var reloadTimer;
var webMapAppLeftPanelWidth = 262;
var webMapAppToggleWidth = 10;
var webMapAppTopBannerHeight = 80;
var newLoad = false;
var webMapAppCloseCallback = "";
var webMapAppCopyrightCallback = "";
var webMapAppMoveFunction = null;
var webMapAppMapDisplay = null;
var webMapAppPanelDisplay = null;
var webMapAppPanelDisplayCell = null;
var webMapAppPanelDisplayTableCell = null;
var webMapAppPanelScrollDiv = null;
var webMapAppToggleDisplay = null;
var webMapAppSpacerDiv = null;
var webMapAppPanelBottomSlider = null;
var webMapAppScaleBar = null;
var webMapAppCopyrightText = null;
var webMapAppWindowWidth = 500;
var webMapAppLeftOffsetX = 0;
var webMapAppRightOffsetX = 0
var webMapAppDefaultMinDockWidth = 125;
var webMapAppMinDockWidth = webMapAppDefaultMinDockWidth;
var webMapAppMapLeft = 262;
var webMapAppHasScroll = false;
var webMapAppLastHasScroll = false;
var webMapAppFirstScalebarPosition = true;
var webMapAppDockMoving = false;
var m_measureToolbarId = "";

// function to set initial sizes of page elements
function setPageElementSizes() {
        // set body style 
        if (document.documentElement) {
            document.documentElement.style.overflow = "hidden";
            document.documentElement.style.height = "100%"; 
        } else {
            document.body.style.overflow = "hidden";
            document.body.style.height = "100%";
        }  
        // get necessary elements
        webMapAppMapDisplay = document.getElementById("Map_Panel");
        webMapAppPanelDisplay = document.getElementById("LeftPanelCellDiv");
        webMapAppPanelDisplayCell = document.getElementById("LeftPanelCell");
        webMapAppPanelScrollDiv = document.getElementById("LeftPanelScrollDiv");
        webMapAppToggleDisplay = document.getElementById("ToggleCell");
        webMapAppPanelSlider = document.getElementById("PanelSlider");
        webMapAppPanelDisplayTableCell = document.getElementById("LeftPanelTableCell");
        webMapAppPanelBottomSlider = document.getElementById("PanelSliderBottom");
        webMapAppScaleBar = document.getElementById("ScaleBar1");
        webMapAppCopyrightText = document.getElementById("CopyrightTextHolder");
        var headerDisplay = document.getElementById("PageHeader");
        var linkDisplay = document.getElementById("LinkBar");
        // set scroll on Dock
        webMapAppPanelScrollDiv.style.overflowY = "auto";
        if (isIE) {
            webMapAppPanelDisplay.style.overflow = "hidden";
        } 
        // get the set widths and heights
        webMapAppLeftPanelWidth = webMapAppPanelDisplay.clientWidth;
        webMapAppToggleWidth = parseInt(webMapAppToggleDisplay.style.width);
        webMapAppTopBannerHeight = headerDisplay.clientHeight + linkDisplay.clientHeight;
        // get browser window dimensions
        var sWidth = getWinWidth();
        var sHeight = getWinHeight();
        // set map display dimensions
        var mWidth = sWidth - webMapAppLeftPanelWidth - webMapAppToggleWidth;
        var mHeight = sHeight - webMapAppTopBannerHeight;
        webMapAppMapLeft = webMapAppLeftPanelWidth + webMapAppToggleWidth;
        webMapAppMapDisplay.style.width =  mWidth + "px";
        webMapAppMapDisplay.style.height = mHeight  + "px";
        if (webMapAppScaleBar!=null) {
            //webMapAppScaleBar.style.width = "auto";
            //webMapAppScaleBar.style.height = "auto";
       
            positionScalebar(); 

        } 
        if (webMapAppCopyrightText!=null) {
            webMapAppCopyrightText.onmousedown = webMapAppGetCopyrightText;
            var crtHeight = webMapAppCopyrightText.clientHeight;
            webMapAppCopyrightText.style.left = (webMapAppMapLeft + 10) + "px";
            webMapAppCopyrightText.style.top = (sHeight - crtHeight - 10) + "px";
        }
        // set heights of left panel and toggle bar
        webMapAppToggleDisplay.style.height = mHeight  + "px";
        webMapAppPanelScrollDiv.style.height = mHeight  + "px";
        esriMaxFloatingPanelDragRight = sWidth - 15;
        esriMaxFloatingPanelDragBottom = sHeight - 15;

}

// function to toggle Dock visibility
function togglePanelDock() {
    if (webMapAppPanelDisplay.style.display=="none") {
        expandPanelDock();
    } else {
        collapsePanelDock();
    }     
}

function expandPanelDock() {
    var image = document.images["CollapseImage"];
    webMapAppPanelDisplay.style.display = "block";
    image.src = "images/collapse_left.gif";
    image.alt = "Collapse";
    webMapAppPanelSlider.style.cursor = "e-resize";
    webMapAppPanelBottomSlider.style.cursor = "e-resize"; 
    webMapAppMapLeft = webMapAppLeftPanelWidth + webMapAppToggleWidth;
    webMapAppMapDisplay.style.left =  webMapAppMapLeft + "px";
    AdjustMapSize(); 
}

function collapsePanelDock() {
    var image = document.images["CollapseImage"];
    dockWidthString = webMapAppPanelDisplayCell.clientWidth + "px";
    webMapAppPanelDisplay.style.display = "none";
    //webMapAppPanelDisplayCell.style.width = "1px";
    image.src = "images/expand_right.gif";
    image.alt = "Expand";
    webMapAppPanelSlider.style.cursor = "default"; 
    webMapAppPanelBottomSlider.style.cursor = "default"; 
    webMapAppMapLeft = webMapAppToggleWidth; 
    webMapAppMapDisplay.style.left =  webMapAppMapLeft + "px";  
    AdjustMapSize();  
   
}  

// function for adjusting element sizes when brower is resized
function AdjustMapSize() {
   // set element widths 
    webMapAppPanelDisplay.style.width =  webMapAppLeftPanelWidth + "px";
    webMapAppToggleDisplay.style.width = webMapAppToggleWidth + "px";
   // get browser window dimensions 
    var sWidth = getWinWidth();
    var sHeight = getWinHeight();
    // calc dimensions needed for map
    var mWidth = sWidth - webMapAppPanelDisplayCell.clientWidth;
    var mHeight = sHeight - webMapAppTopBannerHeight;
    if (mWidth<5) mWidth = 5;
    if (mHeight<5) mHeight = 5;  
    webMapAppMapDisplay.style.width =  mWidth + "px";
    webMapAppMapDisplay.style.left =  webMapAppPanelDisplayCell.clientWidth + "px"; 
   // set heights on elements 
    webMapAppMapDisplay.style.height = mHeight  + "px";
    webMapAppToggleDisplay.style.height = mHeight  + "px";
    webMapAppPanelScrollDiv.style.height = mHeight  + "px";
    if (webMapAppScaleBar!=null) {
        var sbWidth = webMapAppScaleBar.clientWidth;
        var sbHeight = webMapAppScaleBar.clientHeight;
        if (webMapAppFirstScalebarPosition) {
            window.setTimeout("positionScalebar();", 1000);
            webMapAppFirstScalebarPosition = false; 
        } else
            positionScalebar(); 
    } 
   // resize the map 
    window.setTimeout("resizeTheMap(" + mWidth + ", " + mHeight + ", false);", 500);
   // update map properties 
     
    var box = calcElementPosition("Map_Panel"); 
    map.containerLeft = box.left;
	map.containerTop = box.top;
    if (webMapAppCopyrightText!=null) {
        var crtHeight = webMapAppCopyrightText.clientHeight;
        webMapAppCopyrightText.style.left = (box.left + 10) + "px";
        webMapAppCopyrightText.style.top = (sHeight - crtHeight - 10) + "px";
    }
    return false;
}

// function for resizing map in Web Map App
function resizeTheMap(width, height, resizeExtent) {
    if (resizeExtent==null) resizeExtent = true;
    map.resize(width, height, resizeExtent);
    var div = document.getElementById("LeftPanelCellDiv"); 
    // update overview, if doc panel is expanded 
    if (ov!=null && div.style.display!="none") { 
        var argument = "ControlType=OverviewMap&EventArg=OverviewZoom";
        var context = ov.controlName; 
        eval(ov.callBackFunctionString);
    }
    return false; 
}

// handler for window resize
function AdjustMapSizeHandler(e) {
    window.clearTimeout(reloadTimer);
	reloadTimer = window.setTimeout("AdjustMapSize();",1000);
}

// function run at startup
function startUp() {
        // set up identify mode for javascript
        map.ctrlMode = "MapIdentify";
        map.ctrlAction = "Point";
        map.ctrlCursor = "pointer";
        map.ctrlFunction = "MapIdClick(e)";
        if (newLoad) {
            // execute only on intial load.... not callbacks
            map.divObject.style.cursor = "wait";
            // move magnifier and measure toolbar to top left corner of map display 
            var box = calcElementPosition("Map_Panel"); 
            var mag = document.getElementById("Magnifier1");
            if (mag!=null && typeof(esriMagnifiers)!="undefined" && esriMagnifiers!=null) { 
                    floatingPanel = esriMagnifiers["Magnifier1"].floatingPanel; 
                    if (floatingPanel!=null) moveTo(box.left, box.top);
            }
            var tb = document.getElementById(m_measureToolbarId);
			if (tb!=null) {
			    tb.style.left = box.left + "px";
			    tb.style.top = box.top + "px";
			}
			var fp = document.getElementById("CopyrightText_Panel");
			if (fp!=null) {
			    floatingPanel = fp;
			    moveTo(box.left, box.top);
			}
            if (webMapAppScaleBar!=null) window.setTimeout("positionScalebar();", 1000);
        } 
        // set window resize event handler
        window.onresize = AdjustMapSizeHandler;
       for (var fp in FloatingPanels) {
            FloatingPanels[fp].onDockFunction = scrollDockToPanel;
       }   
}  

// function to request closing of session items.... only called if at least one resource is local non-pooled
function CloseOut() {
	var argument = "ControlID=Map1&ControlType=Map&EventArg=CloseOutApplication";
	var context = map.controlName;
	eval(webMapAppCloseCallback);
}

// response function to close out browser ... request sent to server by CloseOut()
function CloseOutResponse(response, context) {
    window.close(); 
    // if user selects Cancel in close dialog, send to close page 
    document.location = response; 
}


function startWebMapAppDockDrag(e) {
    if (!webMapAppDockMoving) {
        webMapAppMoveFunction = document.onmousemove;
        webMapAppDockMoving = true;
    }
    document.onmouseup = stopWebMapAppDocDrag;
    if (webMapAppPanelDisplay.style.display!="none") {
        webMapAppWindowWidth = getWinWidth();
        getXY(e); 
        webMapAppLeftOffsetX = mouseX - webMapAppPanelDisplay.clientWidth;
        var box = calcElementPosition("Map_Panel");
        webMapAppRightOffsetX = box.left - mouseX; 
        document.onmousemove = moveWebMapAppDockDrag;
        var ovPanel = document.getElementById("OverviewMap_Panel_BodyRow");
        var ovDisplay =  document.getElementById("OVDiv_OverviewMap_Panel_OverviewMap1");
        // because the panel cell will be as wide as the overview map image, keep the element width larger than the image. 
        if (FloatingPanels["OverviewMap_Panel"]!=null &&FloatingPanels["OverviewMap_Panel"].docked)
            webMapAppMinDockWidth = parseInt(ovDisplay.style.width) + 20;
        else    
            webMapAppMinDockWidth = webMapAppDefaultMinDockWidth; 
    }
    return false;  
}

function moveWebMapAppDockDrag(e) {
    getXY(e);
    var theButton = (isIE) ? event.button : e.which;      
    if (theButton==0) stopWebMapAppDocDrag(e)
    webMapAppLeftPanelWidth =  mouseX - webMapAppLeftOffsetX;
    var sWidth = getWinWidth();
    if (webMapAppLeftPanelWidth>sWidth-webMapAppToggleDisplay.clientWidth-2) webMapAppLeftPanelWidth=sWidth-webMapAppToggleDisplay.clientWidth-2;
    if (webMapAppLeftPanelWidth < webMapAppMinDockWidth) webMapAppLeftPanelWidth = webMapAppMinDockWidth; 
    var mapLeftString =  (webMapAppLeftPanelWidth + webMapAppToggleDisplay.clientWidth) + "px";
    var widthString =  webMapAppLeftPanelWidth + "px";
    webMapAppPanelDisplay.style.width = widthString;
    var width =  webMapAppWindowWidth - webMapAppPanelDisplayCell.clientWidth; 
    if (width<1) width = 1; 
    webMapAppMapDisplay.style.width = width + "px";
    webMapAppMapDisplay.style.left = mapLeftString;
    return false;
}

function stopWebMapAppDocDrag(e) {
    document.onmousemove = webMapAppMoveFunction;
    document.onmouseup = null;
    webMapAppDockMoving = false;    
    webMapAppCheckPanelWidths();
    AdjustMapSize();  
    return false;
}

function OpenWindow(url) {
    window.open(url);
}

function webMapAppCheckPanelScroll() {
    if (webMapAppPanelScrollDiv.scrollHeight>webMapAppPanelScrollDiv.clientHeight) {
        webMapAppHasScroll = true;
    }  else {
        webMapAppHasScroll = false;
    }  
    //webMapAppCheckPanelWidths(); 
    if (webMapAppHasScroll!=webMapAppLastHasScroll)
        AdjustMapSize();
    webMapAppLastHasScroll = webMapAppHasScroll;    
    return false; 
}

function webMapAppCheckPanelWidths() {
    var maxWidth = 0;
    var node; 
    for (var i=0; i< webMapAppPanelDisplay.childNodes.length; i++) {
        if (webMapAppPanelDisplay.childNodes[i].tagName=="TABLE") {
            node = webMapAppPanelDisplay.childNodes[i];
            if (node.clientWidth>maxWidth) maxWidth = node.clientWidth; 
        }
    }  
     webMapAppPanelDisplay.style.width = maxWidth + "px";
    return false;
}

function webMapAppGetCopyrightText() {
	var argument = "ControlID=Map1&ControlType=Map&EventArg=GetCopyrightText";
	var context = map.controlName;
	eval(webMapAppCopyrightCallback);
	showFloatingPanel('CopyrightText_Panel');
}

function scrollDockToPanel(panelElement) {
    if (panelElement==null) return;
    var yPos = panelElement.offsetTop;
    webMapAppPanelScrollDiv.scrollTop = yPos;  
}

function toggleMagnifier() {
    var mag = document.getElementById("Magnifier1");
    if (mag!=null) { 
            toggleFloatingPanelVisibility('Magnifier1'); 
    } else 
        alert("Magnifier is not available"); 
    
}

function positionScalebar() {
    var sWidth = getWinWidth();
    var sHeight = getWinHeight();
    var sbWidth = webMapAppScaleBar.clientWidth;
    var sbHeight = webMapAppScaleBar.clientHeight;
    webMapAppScaleBar.style.left = (sWidth - sbWidth - 10) + "px";
    webMapAppScaleBar.style.top = (sHeight - sbHeight - 10) + "px";
}

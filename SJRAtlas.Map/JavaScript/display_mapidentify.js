// display_mapidentify.js


var identifyFilePath = "";
var identifyImageType = "png";

// Set up Identify tool
function MapIdentify(divid) {
    map = Maps[divid];
    MapPoint(map.controlName, "MapIdentify", false);
    map.divObject.onmousedown = MapIdClick;
}

// Event handler for Identify
function MapIdClick(e) {
    map.cursor = map.divObject.style.cursor;
	//map.divObject.style.cursor = "wait";
	getXY(e);
	var box = calcElementPosition(map.containerDivId);
	zleft = mouseX - box.left;
	ztop = mouseY - box.top;

	map.xMin=zleft;
	map.yMin=ztop;
	var div = document.getElementById("IdentifyLocation");
	if (div==null) {
	    addIdentifyLocation();
	}
	map.getTopLeftTile();
	var fpBody = document.getElementById('Results_TaskResults1');
	var html = fpBody.innerHTML;
	fpBody.innerHTML = "<div><img src='images/callbackActivityIndicator.gif' align='middle'/> Getting Information. . .</div>" + html;
	showFloatingPanel('Results');
	fpBody=document.getElementById('Results_BodyRow');
	if (fpBody.style.display=="none")
	    toggleFloatingPanelState('Results','images/collapse.gif','images/expand.gif');

	var message = "ControlID=Map1&ControlType=Map&EventArg=MapIdentify&Map1_mode=MapIdentify&minx=" + zleft + "&miny=" + ztop;
	var context = map.controlName;
	eval(map.identifyCallbackFunctionString);
	var div = document.getElementById("IdentifyLocation");
	// point is bottom center... 2 pixels up for shadow
	var cWidth = Math.floor(div.clientWidth / 2);
	var cHeight = div.clientHeight;
	//alert(cWidth + " x " + cHeight); // width and height might not be available on first time.... if so, approximate size needed
	if (cWidth==0) cWidth = 12;
	if (cHeight==0) cHeight = 29;
	var idLeft = zleft - parseInt(map.divObject.style.left) - cWidth;
	var idTop = ztop - parseInt(map.divObject.style.top) - cHeight + 2; // add two back for icon bottom
//	if (isIE) {
//	    idTop+=2; 
//	    //idLeft+=2;
//	}
	window.setTimeout('moveLayer("IdentifyLocation", ' + idLeft + ', ' + idTop + '); showLayer("IdentifyLocation");', 0);
	map.mode = map.tempMode;
	map.actionType = map.tempAction;
	map.cursor = map.tempCursor;
	return false;
	
}

function addIdentifyLocation() {
    var content = '<div id="IdentifyLocation" style="position: absolute; left: 0px; top: 0px; visibility: hidden;">';
    if (isIE  && ieVersion < 7 && (identifyImageType.toLowerCase()=="png")) 
	    content += '<img src="' + identifyFilePath + 'images/blank.gif" alt="" border="0"  hspace="0" vspace="0" style="filter:  progid:DXImageTransform.Microsoft.AlphaImageLoader(src=\'' + identifyFilePath + 'images/identify-map-icon.png\');" />\n';
	else
	    content += '<img src="' + identifyFilePath + 'images/identify-map-icon.png" alt="" border="0"  hspace="0" vspace="0" />\n';
    content += '</div>';
    map.overlayObject.insertAdjacentHTML("BeforeEnd", content);
}
// display_measure.js

//

var m_currentMeasureToolbarTool = "polyline";
var m_measureToolbarImagePath = "images/";
var m_measureToolbarImageExtension = ".gif";
var m_measureDisplay = "MeasureDisplay";
var m_measureToolbarId = "MeasureToolbar";
var m_measureLengthsTotal = 0.0;
var m_measureAreasTotal = 0.0;
var m_measureXOffset = 0;
var m_measureYOffset = 0;
var m_MeasureTypes = new Array();
m_MeasureTypes[0] = "point";
m_MeasureTypes[1] = "polyline";
m_MeasureTypes[2] = "polygon";
var m_measureMoveFunction = null;

var m_measureToolbar = null;

function checkMeasureToolbarBorder(cell, type) {
    if (type.toLowerCase()==m_currentMeasureToolbarTool)
        cell.style.borderColor = "Black";
    else
        cell.style.borderColor = "White";  
}

// set current measure tool
function setMeasureToolbarTool(type) {
	m_currentMeasureToolbarTool = type.toLowerCase();
	var cellObj;
	var buttonId = "";
	for (var i=0; i<m_MeasureTypes.length; i++) {
		buttonId = "MeasureToolbarButton_" + m_MeasureTypes[i];
		cellObj = document.getElementById(buttonId);
		if (cellObj!=null) {
			if (m_MeasureTypes[i]==m_currentMeasureToolbarTool) {
				cellObj.style.borderColor = "Black";
				cellObj.style.backgroundColor = "#EEEEEE";
				startMeasure();
			}
			else {
				cellObj.style.borderColor = "White";
				cellObj.style.backgroundColor = "White";
			}
		}
	}
}


// Polyline Measure action ... for distances
function MeasurePolyline(divid) {
	map = Maps[divid];
	if (map!=null) {
        map.vectorCallbackFunctionString = measureVectorCallbackFunctionString;
        vectortoolbar = "MeasureToolbar";
		map.setTool("Measure", false, "ClickShape", "crosshair", 1, "visible", 
		"Measure-Polyline - Click to start line. Click again to add vectors. Double-click to add last vector and complete polyline.", false, measureVectorCallbackFunctionString);
	}
}

// Polygon Measure action ... for areas
function MeasurePolygon(divid) {
	map = Maps[divid];
	if (map!=null) {
        map.vectorCallbackFunctionString = measureVectorCallbackFunctionString;
        vectortoolbar = "MeasureToolbar";
		map.setTool("Measure", false, "ClickShape", "crosshair", 2, "visible", 
		"Measure-Polygon - Click to start line. Click again to add vectors. Double-click to add last vector and complete polygon.", false, measureVectorCallbackFunctionString);
	}
}

// Point Measure action ... for location coordinates
function MeasurePoint(divid) {
	map = Maps[divid];
	if (map!=null) {
        vectortoolbar = "MeasureToolbar";
        //MapPoint(map.controlName, "Measure", false, "pointer");
        map.setTool("Measure", false, "Point", "pointer", -1, "visible","");
        map.divObject.onmousedown = MapCoordsClick;
        map.mode = "MeasurePoint";
        var vo = map.vectorObject;
        showLayer(vo.divId);
        vo.clear();
        vo.draw();
	}    
}

// Handler for MeasurePoint clicks
function MapCoordsClick(e) {
	var vo = map.vectorObject;
	var pix = vo.pixelObject;
	var xycoord = vo.xyCoord;
	getXY(e);
	zleft = mouseX - map.containerLeft;
	ztop = mouseY - map.containerTop;
	vo.clear();
	vo.crosshair(zleft, ztop);
	vo.draw();

	map.xMin=zleft;
	map.yMin=ztop;
	map.getTopLeftTile();
    coordString = + zleft + ":" + ztop;
    var argument = "ControlID=" + map.controlName + "&EventArg=Point&ControlType=Map&coords=" + coordString + "&VectorMode=Measure&VectorAction=Coordinates&minx=" + zleft + "&miny=" + ztop;
    if (checkForFormElement(document, 0, "MeasureUnits")) argument += "&MeasureUnits=" + document.forms[0].MeasureUnits.value;
    if (checkForFormElement(document, 0, "AreaUnits")) argument += "&AreaUnits=" + document.forms[0].AreaUnits.value;
    if (checkForFormElement(document, 0, "MapUnits")) argument += "&MapUnits=" + document.forms[0].MapUnits.options[document.forms[0].MapUnits.selectedIndex].value;
    
    var context = map.controlName + ",Point";
    
    map.vectorCallbackFunctionString = measureVectorCallbackFunctionString;
    eval(map.vectorCallbackFunctionString);

}

// measure tool is selected... call current type (polyline for distance, polygon for area)
function startMeasure() {
    var md;
    if (m_measureDisplay!=null) {
        md = document.getElementById(m_measureDisplay);
    }
	if (m_currentMeasureToolbarTool=="point") {
		//alert("MeasurePoint");
        if (md!=null) md.innerHTML = "Click on the map to return the coordinate location of the point.<br />";
		MeasurePoint(map.controlName);
	} else if (m_currentMeasureToolbarTool=="polyline") {
		//alert("MeasurePolyline");
        if (md!=null) md.innerHTML = "Click on the map and draw a line. Double-click to end the line.<br />";
		MeasurePolyline(map.controlName);
	} else {
		//alert("MeasurePolygon");
        if (md!=null) md.innerHTML = "Click on the map and draw a polygon. Double-click to end the polygon.<br />";
		MeasurePolygon(map.controlName);
	}
}

function closeMeasureToolbarTool(id) {
    if (id!=null) m_measureToolbarId = id;
    m_measureToolbar = document.getElementById(m_measureToolbarId);
    if (m_measureToolbar!=null) {
        m_measureToolbar.style.visibility = "hidden";
    }
    map.vectorObject.clear();
    map.vectorObject.draw(); 
 
}

// update distance unit settings... request new totals from server
function changeMeasureUnits() {
    var f = document.forms[docFormID];
    var i = f.MeasureUnits2.selectedIndex;
    var m = f.MeasureUnits2.options[i].value;
    f.MeasureUnits.value = m; 
    coordString = map.coords;
    if (coordString==null) coordString="";
    var argument = "ControlID=" + map.controlName + "&EventArg=" + m_currentMeasureToolbarTool + "&ControlType=Map&coords=" + coordString + "&VectorMode=" + map.mode + "&VectorAction=AddPoint&MeasureUnits=" + m;
   if (checkForFormElement(document, 0, "AreaUnits")) {
        argument += "&AreaUnits=" + f.AreaUnits.value;
   } 
    var context = map.controlName + "," + m_currentMeasureToolbarTool;

    	// Debug stuff to be removed, or at least commented out
   	if (checkForFormElement(document, 0, "MapDebugBox")) document.forms[0].MapDebugBox.value += ("Vector Request: " + context + ": MeasureUnits=" + m + "\n"); 
		                
    eval(map.vectorCallbackFunctionString);    
}

// update area unit settings... request new totals from server
function changeAreaUnits() {
    var f = document.forms[docFormID];
    var i = f.AreaUnits2.selectedIndex;
    var a = f.AreaUnits2.options[i].value;
    f.AreaUnits.value = a
    coordString = map.coords;
    if (coordString==null) coordString="";
    var argument = "ControlID=" + map.controlName + "&EventArg=" + m_currentMeasureToolbarTool + "&ControlType=Map&coords=" + coordString + "&VectorMode=" + map.mode + "&VectorAction=AddPoint&AreaUnits=" + a;
   if (checkForFormElement(document, 0, "MeasureUnits")) {
        argument += "&MeasureUnits=" + f.MeasureUnits.value;
   } 
    var context = map.controlName + "," + m_currentMeasureToolbarTool;

    	// Debug stuff to be removed, or at least commented out
   	if (checkForFormElement(document, 0, "MapDebugBox")) document.forms[0].MapDebugBox.value += ("Vector Request: " + context + ": AreaUnits=" + a + "\n"); 
		                
	eval(map.vectorCallbackFunctionString);    
}

// event handler for starting to drag toolbar around... mouse down
function dragMeasureToolbarStart(e, id) {
    if (id!=null) m_measureToolbarId = id;
    m_measureToolbar = document.getElementById(m_measureToolbarId);
    if (m_measureToolbar!=null) {
        getXY(e);
        var box = calcElementPosition(m_measureToolbarId);
        m_measureXOffset = mouseX - box.left;
        m_measureYOffset = mouseY - box.top;
    }
    m_measureMoveFunction = document.onmousemove; 
    document.onmousemove = dragMeasureToolbarMove;
    document.onmouseup = dragMeasureToolbarStop;
    return false;
}

// event handler for toolbar drag movement... mousemove
function dragMeasureToolbarMove(e) {
    getXY(e);
    m_measureToolbar.style.left = (mouseX-m_measureXOffset) + "px";;
    m_measureToolbar.style.top = (mouseY-m_measureYOffset) + "px";
    return false;
}

// event handler for end of toolbar drag movement... mouseup
function dragMeasureToolbarStop(e) {
    document.onmousemove = m_measureMoveFunction;
    document.onmouseup = null;
    return false;
}

// set up the images for transparency in IE6
function setIE6MeasureToolbarImages() {
    var imageId = "";
    var imgSrc = ""; 
    var imgObj = document.images["MeasureToolbar_CloseButton"];
    if (imgObj!=null) {
        imgObj.src = "images/blank.gif";
        imgObj.style.filter =  "progid:DXImageTransform.Microsoft.AlphaImageLoader(src=/aspnet_client/ESRI/WebADF/images/dismiss.png)";
    }
    for (var i=0; i<m_MeasureTypes.length; i++) {
	    imageId = "ToolbarImage_" + m_MeasureTypes[i];
	    imgObj = document.images[imageId];
	    if (imgObj!=null) {
	        imgSrc = imgObj.src;
            imgObj.src = "images/blank.gif";
            imgObj.style.filter =  "progid:DXImageTransform.Microsoft.AlphaImageLoader(src=" + imgSrc + ")";
	    }
    }

}




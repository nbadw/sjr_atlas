var $dom = YAHOO.util.Dom;
var $evt = YAHOO.util.Event;
var $log = function() { };

function init()
{
    $log('initializing advanced search form');
    //init_watershed_tree();
    init_watershed_autocomplete();
    init_waterbody_autocomplete();
    
    $evt.addListener('options_dataset', 'change', function(evt)
    {
        // check if it's valid first   
        $dom.setStyle('aoi-toggles', 'display', 'block');
    });
}

function init_watershed_tree()
{
    $log('creating watershed tree');
    var tree = new YAHOO.widget.TreeView('watershed-unit-tree');
    var root = tree.getRoot();
    var topnode = { label: "Select a Watershed Unit:", code: "" };
    var nodestyle = 1;
    tree.setDynamicLoad(load_watershed_children, nodestyle);
    new YAHOO.widget.TextNode(topnode, root, false);    
    tree.draw();
}

function init_watershed_autocomplete()
{
    $log('creating watershed autocomplete');
    var schema = ["Watersheds", "UnitName", "Id"];
    var datasource = new YAHOO.widget.DS_XHR("http://gis.mektekdev.com/sjratlas/watershed/autocomplete.rails", schema);
    datasource.queryMatchCase = false;
    datasource.queryMatchContains = true;
    var autocomplete = new YAHOO.widget.AutoComplete('options_watershed', 'watershed-autocomplete', datasource);
    
    // This function returns markup that bolds the original query,
    // and also displays to additional pieces of supplemental data.
    autocomplete.formatResult = function(result, query) 
    {
        var name = result[0] || 'Unnamed Watershed Unit';    
        var drainageCode = result[1];
        var key = name + " (" + drainageCode + ")";
                    
        var index = key.toLowerCase().indexOf(query.toLowerCase());        
        var preKeyQuery = key.substring(0, index);
        var keyQuery = key.substring(index, index + query.length);
        var postKeyQuery = key.substring(index + query.length, key.length);

        var aMarkup = ["<div id='ysearchresult'>",
          preKeyQuery,
          '<span style="font-weight: bold;">',
          keyQuery,
          '</span>',
          postKeyQuery,
          "</div>"];
        return (aMarkup.join(""));
    };
    
    //define your itemSelect handler function:
    var itemSelectHandler = function(sType, aArgs) 
    {
	    YAHOO.log(sType); //this is a string representing the event;
	    var oSelf = aArgs[0]; // your AutoComplete instance
	    var elItem = aArgs[1]; //the <li> element selected in the suggestion container
	    var oData = aArgs[2]; //array of the data for the item as returned by the DataSource	    
	    var oTextbox = oSelf._oTextbox;
	    
	    oTextbox.focus();
        // First clear text field
        oTextbox.value = "";
        oTextbox.value = oData[0] + " (" + oData[1] + ")";

        // scroll to bottom of textarea if necessary
        if(oTextbox.type == "textarea") 
        {
            oTextbox.scrollTop = oTextbox.scrollHeight;
        }

        // move cursor to end
        var end = oTextbox.value.length;
        this._selectText(oTextbox, end, end);
    };

    //subscribe your handler to the event, assuming
    //you have an AutoComplete instance myAC:
    autocomplete.itemSelectEvent.subscribe(itemSelectHandler);
}

function init_waterbody_autocomplete()
{
    $log('creating waterbody autocomplete');
    var schema = ["WaterBodies", "Name", "Id", "Abbreviation"];
    var datasource = new YAHOO.widget.DS_XHR("http://gis.mektekdev.com/sjratlas/waterbody/autocomplete.rails", schema);
    //var datasource = new YAHOO.widget.DS_JSFunction(waterbody_autocomplete_data);
    datasource.queryMatchCase = false;
    datasource.queryMatchContains = true;
    var autocomplete = new YAHOO.widget.AutoComplete('options_waterbody', 'waterbody-autocomplete', datasource);
    
    // This function returns markup that bolds the original query,
    // and also displays to additional pieces of supplemental data.
    autocomplete.formatResult = function(result, query) 
    {
        var name = result[0] || 'Unnamed Water Body';    
        var id = result[1];
        var abbrev = result[2];      
        
        var key = id + ' - ' + name;
        if("" != abbrev)
            key += " (" + abbrev + ")";
            
        var index = key.toLowerCase().indexOf(query.toLowerCase());        
        var preKeyQuery = key.substring(0, index);
        var keyQuery = key.substring(index, index + query.length);
        var postKeyQuery = key.substring(index + query.length, key.length);

       var aMarkup = ["<div id='ysearchresult'>",
          preKeyQuery,
          '<span style="font-weight: bold;">',
          keyQuery,
          '</span>',
          postKeyQuery,
          "</div>"];
        return (aMarkup.join(""));
    };
    
    //define your itemSelect handler function:
    var itemSelectHandler = function(sType, aArgs) 
    {
	    YAHOO.log(sType); //this is a string representing the event;
	    var oSelf = aArgs[0]; // your AutoComplete instance
	    var elItem = aArgs[1]; //the <li> element selected in the suggestion container
	    var oData = aArgs[2]; //array of the data for the item as returned by the DataSource	    
	    var oTextbox = oSelf._oTextbox;
	    var name = oData[0] || 'Unnamed Water Body';    
        var id = oData[1];
        var abbrev = oData[2];      
        
        var key = id + ' - ' + name;
        if("" != abbrev)
            key += " (" + abbrev + ")";
	    
	    oTextbox.focus();
        // First clear text field
        oTextbox.value = "";
        oTextbox.value = key;

        // scroll to bottom of textarea if necessary
        if(oTextbox.type == "textarea") 
        {
            oTextbox.scrollTop = oTextbox.scrollHeight;
        }

        // move cursor to end
        var end = oTextbox.value.length;
        this._selectText(oTextbox, end, end);
    };

    //subscribe your handler to the event, assuming
    //you have an AutoComplete instance myAC:
    autocomplete.itemSelectEvent.subscribe(itemSelectHandler);
}

function load_watershed_children(node, fnLoadComplete)  
{	
	//prepare URL for XHR request:
	var url = "/watershed/list.rails?code=" + encodeURI(node.data.code);
	
	//prepare our callback object
	var callback = 
	{	
		//if our XHR call is successful, we want to make use
		//of the returned data and create child nodes.
		success: function(response) 
		{
			$log("watershed data request was successful");
			var results = eval("(" + response.responseText + ")");
			for(var i=0; i < results.length; i++)
			{
			    var label = results[i].UnitName;
			    var code = results[i].DrainageCode;
			    
			    // remove extra 00 values from drainage code
			    var cutoff = code.indexOf('-00');
			    if(cutoff != -1)
			        code = code.substring(0, code.indexOf('00') - 1);
			    
			    var obj = { label: label, code: code }
				var newNode = new YAHOO.widget.TextNode(obj, node, false);
			}
			response.argument.fnLoadComplete();
		},
		
		failure: function(response) 
		{
			$log("watershed data request failed");
			response.argument.fnLoadComplete();
		},
		
		//our handlers for the XHR response will need the same
		//argument information we got to loadNodeData, so
		//we'll pass those along:
		argument: 
		{
			"node": node,
			"fnLoadComplete": fnLoadComplete
		},
		
		//timeout -- if more than 7 seconds go by, we'll abort
		//the transaction and assume there are no children:
		timeout: 7000
	};
	
	//With our callback object ready, it's now time to 
	//make our XHR call using Connection Manager's
	//asyncRequest method:
	YAHOO.util.Connect.asyncRequest('GET', url, callback);
}

function toggle_area_of_interest(area_of_interest)
{
    // reset the toggle link styles 
    $dom.getElementsByClassName('aoi-toggle', 'a', 'aoi-toggles', function(elm)
    {
        $dom.setStyle(elm, 'color', '#425CBD');
    });
    
    var current_toggle = $dom.get('aoi-' + area_of_interest);
    if(current_toggle)
    {        
        $dom.setStyle(current_toggle, 'color', 'red');
    }
    
    $dom.getElementsByClassName('area-of-interest', 'div', 'form1', function(elm)
    {
        $dom.setStyle(elm, 'display', 'none');
    });
    $dom.setStyle(area_of_interest, 'display', 'block');
}

$evt.onDOMReady(init);
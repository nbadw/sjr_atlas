var Search = { Results: [] };

Ext.onReady(function() {    
    var results = new ResultsPanel();
    var map = new MapPanel();
    
    results.on('resultSelect', function(result) {
        map.zoomTo(result);
    });
    
    var panel = new Ext.Panel({
        layout: 'border',
        applyTo: 'search-results',
        items: [results, map]
    });
    
    results.addResults(Search.Results);   
    map.addMarkers(Search.Results); 
    map.makeAllMarkersVisible();   
});

ResultsPanel = function() { 
    ResultsPanel.superclass.constructor.call(this, {
        id: 'results-panel',
        region: 'west',
        title: 'Which Location Did You Mean?',
        collapsible: true,
        split: true,
        autoScroll: true,
        width: 250,
        minSize: 200,
        lines: false,
        rootVisible: false,
        root: new Ext.tree.TreeNode('Search Results'),
        collapseFirst: false
    });
    
    this.results = this.root;
    
    this.getSelectionModel().on({
        'beforeselect': function(sm, node) {
            return node.isLeaf();
        },
        'selectionchange': function(sm, node) {
            if(node) {
                this.fireEvent('resultSelect', node.attributes);
            }
        },
        scope: this
    });
};

Ext.extend(ResultsPanel, Ext.tree.TreePanel, {
    addResult: function(attrs) {        
        var group_name = attrs['type'].gsub(/\s\(\d+\)$/, '');    
        var group_node = this.results.findChild('group_name', group_name);
        if(!group_node)
        {
            group_node = new Ext.tree.TreeNode({
                text: group_name,
                group_name: group_name,
                cls: 'results-group-node',
                leaf: false,
                expanded: true
            });
            this.results.appendChild(group_node);
        }
        
        Ext.apply(attrs, {
            leaf: true,
            cls: 'result-node',
            text: attrs['name']
        });
        var node = new Ext.tree.TreeNode(attrs);        
        group_node.appendChild(node);
        return node;
    },
    
    addResults: function(results) {
        for(var i=0; i < results.length; i++) {
            this.addResult(results[i]);
        }
    }
});

MapPanel = function() {       
    this.map_canvas = document.createElement('div'); 
    this.map_canvas.id = 'map-canvas'; 
    this.map_canvas.style.height = '100%';
    this.map_canvas.style.width  = '100%';   
    
    this.map = new GMap2(this.map_canvas);
    this.map.setCenter(new GLatLng(37.4419, -122.1419), 13);
    this.map.addControl(new GLargeMapControl());
    
    this.markers = {};
     
    MapPanel.superclass.constructor.call(this, {
        id: 'map-panel',
        region: 'center',
        contentEl: this.map_canvas
    });
    
    this.on({
        'resize': function(container, width, height, rawWidth, rawHeight) {
            this.map.checkResize();
        },
        scope: this 
    });
    
    Event.observe(window.document.body, 'unload', function(e) { 
        GUnload() 
    });
};

Ext.extend(MapPanel, Ext.Panel, {
    zoomTo: function(result) {
        for(var key in this.markers) {
            this.markers[key].hide();
        }
        var activeMarker = this.markers[result['cgndb_key']];
        activeMarker.show();
        this.map.panTo(activeMarker.getLatLng());
    },
    
    makeAllMarkersVisible: function() {
        if(this.markers.length == 0)
            return;
        
        var bounds = new GLatLngBounds();        
        for(var key in this.markers) {
            bounds.extend(this.markers[key].getLatLng());
        }
        this.map.setCenter(
            bounds.getCenter(), 
            this.map.getBoundsZoomLevel(bounds)
        );        
    },
    
    addMarker: function(result) { 
        var marker = new GMarker(
            new GLatLng(
                result['latitude'],
                result['longitude']
            )
        );
        this.map.addOverlay(marker);
        this.markers[result['cgndb_key']] = marker;
    },
    
    addMarkers: function(results) {
        for(var i=0; i < results.length; i++) {
            this.addMarker(results[i]);
        }        
    }
});
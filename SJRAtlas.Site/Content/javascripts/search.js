/**
 * @author colin
 **/
YAHOO.widget.Logger.enableBrowserConsole();

SJRSearch = function()
{
    var self = {}; // returnable object...
    
    /**
     * private variables
     */ 
    var url;
    // shorthand references to YUI utilities:
	var evt = YAHOO.util.Event, dom = YAHOO.util.Dom, conn = YAHOO.util.Connect;
    
    /**
     * private methods
     */      
    var log = function(msg, cat, src)
    {
        if(!cat) cat = 'info';
        if(!src) src = 'SJRSearch';
        
        YAHOO.log(msg, cat, src);
    }
    
    var start = function()
    {
        log('Search request starting...');
        show_loading();
        var search_url = url + (/&$/.test(url) ? '' : '&') + 'layout=false';
        conn.asyncRequest('GET', search_url, 
        {
            success: search_complete,
            failure: search_failed                       
        });
    }
    
    var search_complete = function(response)
    {
        log('Search Complete');    
        
        var xml = response.responseXML;
        
            
        dom.get('results').innerHTML = response.responseText;
        hide_loading();     
        dom.setStyle('results', 'display', 'block');   
    }
    
    var search_failed = function(response)
    {
        log('Search Failed');    
        dom.get('error_details').innerHTML = response.statusText;
        hide_loading();    
        dom.setStyle('error_message', 'display', 'block');        
    }
    
    var show_loading = function()
    {
        log('Showing loading message', 'debug');
        dom.setStyle('loading_message', 'display', 'block');
    }
    
    var hide_loading = function()
    {
        log('Hiding loading message', 'debug');
        dom.setStyle('loading_message', 'display', 'none');        
    }
    
    /**
     * public variables
     */
    // self.public_var = "Public Variable";
    
    /**
     * public methods
     */
    self.init = function(search_url)
    {
        // if called by the DOMReady event handler, the method signature is actually ("DOMReady", [], url)
        if(search_url == 'DOMReady') 
        { 
            search_url = arguments[2]; 
        }
        
        log("Initializing with URL=" + search_url + "...");  
        url = search_url;
        
        start();                
    }
    
    return self;
}();
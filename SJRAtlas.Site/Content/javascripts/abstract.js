// should automatically shorten p.abstract elements to a pre-defined height (150px?)
// and add a 'read more' link that shows the rest of the abstract
var dom = YAHOO.util.Dom;
var evt = YAHOO.util.Event;
var max_height = 18 * 5;

function maybe_shorten_abstract()
{
    var region = dom.getRegion(this);   
    var height = Math.abs(region.top - region.bottom); 
    if(height <= max_height)  
    {
        return;
    }  
        
    dom.setStyle(this, 'height', max_height + 'px');
    dom.setStyle(this, 'overflow', 'hidden');
    dom.generateId(this, 'abstract-');
    dom.addClass(this, 'hidden');
    
    var read_more_control = document.createElement('div');
    read_more_control.innerHTML = '<a id="' + this.id + '-toggle" href="javascript:toggle_read_more(\'' + this.id + '\');" class="option">Show Full Abstract</a>';
    
    var parent = this.parentNode;
    if(parent.lastchild == this)
    {
        parent.appendChild(read_more_control);
    }
    else
    {
        parent.insertBefore(read_more_control, this.nextSibling);
    }
}

function toggle_read_more(abstract_id)
{
    var abstract_text = dom.get(abstract_id);
    var abstract_toggle = dom.get(abstract_id + '-toggle');
    if(dom.hasClass(abstract_text, 'hidden'))
    {
        dom.removeClass(abstract_text, 'hidden');
        dom.setStyle(abstract_text, 'height', '');
        abstract_toggle.innerHTML = 'Hide Full Abstract';
    }
    else
    {
        dom.addClass(abstract_text, 'hidden');
        dom.setStyle(abstract_text, 'height', max_height + 'px');
        abstract_toggle.innerHTML = 'Show Full Abstract';
    }
}

evt.onDOMReady( function() { dom.getElementsByClassName('abstract', 'p', 'content', maybe_shorten_abstract) } );
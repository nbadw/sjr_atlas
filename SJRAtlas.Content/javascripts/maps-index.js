var $dom = YAHOO.util.Dom, $evt = YAHOO.util.Event, $eff = YAHOO.widget.Effects;
var animating = false;
$evt.onDOMReady(init);

function init()
{
    $dom.getElementsByClassName('toggle', 'a', 'map-controls', function(elm)
    {
        $evt.addListener(elm, 'click', toggle_clicked);
    });  
}

function toggle_clicked(evt)
{        
    YAHOO.util.Event.stopEvent(evt); 
    
    if(animating)
        return;
       
    var toggles = $dom.getElementsByClassName('toggle', 'a', 'map-controls');
    var new_active = this;
    var active;
    var direction; // blind left or blind right depending on toggles.index(active) < toggles.index(new_active)
    for(var i=0; i < toggles.length; i++)
    {
        if($dom.hasClass(toggles[i], 'active'))
            active = toggles[i];
    }
    
    $dom.removeClass(active, 'active');
    $dom.addClass(new_active, 'active');
    
    var fade = new $eff.Fade(active.href.substring(active.href.lastIndexOf('#') + 1, active.href.length),
                            { seconds: 0.3 });
    fade.animate();
    fade.onEffectComplete.subscribe(function() 
    { 
        var appear = new $eff.Appear(new_active.href.substring(new_active.href.lastIndexOf('#') + 1, new_active.href.length),
                                    { seconds: 0.3 });
        appear.onEffectComplete.subscribe(function() { animating = false; });
        appear.animate();
    });
    animating = true;
    fade.animate();
    
    //$dom.setStyle(active.href.substring(active.href.lastIndexOf('#') + 1, active.href.length), 'display', 'none');
    //$dom.setStyle(new_active.href.substring(new_active.href.lastIndexOf('#') + 1, new_active.href.length), 'display', 'block');
}

function show_large_preview(img)
{
    var preview = $dom.get('large-preview-img');
    var message = $dom.get('preview-message');  

    if(img == null || img == '')
    {
        preview.src = '';
        message.innerHTML = 'Sorry, no large preview available for this map';
        $dom.setStyle(preview, 'display', 'none');
        $dom.setStyle(message, 'display', 'block');
    }
    else
    {    
        preview.src = img;
        $dom.setStyle(preview, 'display', 'block');
        $dom.setStyle(message, 'display', 'none');
    }
}

function remove_large_preview()
{
    var preview = $dom.get('large-preview-img');
    var message = $dom.get('preview-message');

    preview.src = '';
    message.innerHTML = 'Place the mouse over a map to view a larger preview image';
    
    $dom.setStyle(preview, 'display', 'none');
    $dom.setStyle(message, 'display', 'block');
}
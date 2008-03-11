var MAX_ABSTRACT_HEIGHT = 18 * 3; // 5 lines at 18px a line

Event.observe(window, 'load', function() {    
    $('content').select('p.abstract').each(function(elm) {
        var paragraph = elm;
        var height = paragraph.getHeight();
        if(height <= MAX_ABSTRACT_HEIGHT)
            return;
            
        elm.setStyle({
            height: MAX_ABSTRACT_HEIGHT + 'px',
            overflow: 'hidden'
        });
        
        var readMoreToggle = $(document.createElement('a'));
        readMoreToggle.href = '#';
        readMoreToggle.innerHTML = 'Show Full Abstract';
        Event.observe(readMoreToggle, 'click', function(evt) {                       
            evt.preventDefault();
            var a = $(this);
            var target = paragraph;
            
            if(a.hasClassName('on')) {
                a.removeClassName('on');
                a.innerHTML = 'Show Full Abstract';
                target.setStyle({ height: MAX_ABSTRACT_HEIGHT + 'px' })
            }
            else {
                a.addClassName('on');
                a.innerHTML = 'Hide Full Abstract';
                target.setStyle({ height: '' });
            }
        });
                
        var readMore = $(document.createElement('div'));
        readMore.addClassName('abstract-toggle');
        readMore.appendChild(readMoreToggle);        
        var parent = paragraph.parentNode;
        if(parent.lastchild == this) {
            parent.appendChild(readMore);
        }
        else {
            parent.insertBefore(readMore, paragraph.nextSibling);
        }
    });
});


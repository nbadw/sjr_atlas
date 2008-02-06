Ext.onReady(function() {
    Ext.get('waterbody-box').boxWrap();

    var waterbody_datastore = new Ext.data.Store({
        url: '/waterbody/autocomplete.castle',
        root: 'waterbodies',
        fields: ['']
    });
    
    var waterbody_result_template = new Ext.XTemplate(
        '<tpl for="."><div class="waterbody-search-item">',
            '<h3>{name}</h3>',
            '<p>Id: {id}</p>',
            '<p>Also known as: {alt_name}</p>',
        '</div></tpl>'
    );
    
    var waterbody_search = new Ext.form.ComboBox({
        store: waterbody_datastore,
        displayField: 'title',
        typeAhead: false,
        loadingText: 'Searching...',
        width: 500,
        pageSize: 10,
        hideTrigger: true,
        tpl: waterbody_result_template,
        applyTo: 'options_waterbody',
        itemSelector: 'div.waterbody-search-item'
    });
});
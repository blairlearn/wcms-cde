DeleteList = Class.create();
Object.extend(DeleteList,{
    _deleteListsOnPage : {},
    // Static fields and methods
    _valueListRoot : "ValueList",       // Hidden list of values
    _textListRoot : "NameList",         // Hidden list of item names.
    _deletedListRoot : "DeletedList",   // Hidden list of flags for which item to delete.
    _emptyListMessageRoot : "EmptyMessage",
    _displayListRoot : "DisplayList",   // The visible list of items.
    

    _deleteButtonRoot : "Delete",

    _valueSeparator : "|",
    _nameSeparator : "_",

    BuildMarkerButton : function(parentID, offset, deleteIconUrl)
    {
        var buttonName = parentID + DeleteList._nameSeparator + DeleteList._deleteButtonRoot;
        var buttonHtml = "<input alt=\"Delete.\" name=\"" + buttonName + "\" src=\"" + deleteIconUrl + "\" type=\"image\" value=\"" + offset + "\" />";
        return buttonHtml;
    },

    ClearAll : function(controlID)
    {
        control = eval(controlID + DeleteList._nameSeparator + "obj");
        control.ClearAll();
    },
    
    GetInstance : function(controlID) {
        return DeleteList._deleteListsOnPage[controlID];
    },

    RegisterInstance : function(controlID, instance) {
        DeleteList._deleteListsOnPage[controlID] = instance;
    }

});

Object.extend(DeleteList.prototype, {
    _control : false,
    _deleteIconUrl : false,

    // Instance fields and methods.
    _valueListField : false,    // Hidden list of values
    _textListField : false,     // Hidden list of item names.
    _deletedListField : false,  // Hidden list of flags for which item to delete.
    _emptyListMessageField : false,
    _displayList : false,       // The visible list of items.
    _itemCount : false,

    initialize : function(controlID, deleteIconUrl)
    {        
        this._control = $(controlID);
        
        DeleteList.RegisterInstance(controlID, this);
        
        //In order to make things easier in getting the selected text,
        //we are going to add a method to the instance of the HTML element
        //we converting into a DeleteList.  This is just a helper method that
        //queries the DeleteList "static" object to get the instance of the
        //DeleteList object we want to get the selected text for.
        Object.extend(this._control, {
            SelectedTextList : function(delimiter)  {
                var instance = DeleteList.GetInstance(this.id);
                if (instance) {
                    return instance.SelectedTextListInternal(delimiter);
                } else {
                    return '';
                }
            }
        });

        
        this._deleteIconUrl = deleteIconUrl;

        this._valueListField = $(controlID + DeleteList._nameSeparator + DeleteList._valueListRoot);
        this._textListField = $(controlID + DeleteList._nameSeparator + DeleteList._textListRoot);
        this._deletedListField = $(controlID + DeleteList._nameSeparator + DeleteList._deletedListRoot);
        this._emptyListMessageField = $(controlID + DeleteList._nameSeparator + DeleteList._emptyListMessageRoot);
        this._displayList = $(controlID + DeleteList._nameSeparator + DeleteList._displayListRoot);

        // Attach handlers for deletion clicks.
        var listItems = $(controlID).select("li");
        var length = listItems.length;
        this._itemCount = listItems.length;
        for(var i = 0; i < length; ++i)
        {
            // If it exists (and it should) itemMarker is an array of at least one span.
            // The first span (and this is tied to the rendering code) contains the marker
            // for deleting the item.
            var itemMarker = listItems[i].down().down();
            if(itemMarker)
                itemMarker.observe("click", this.ClickHandler.bindAsEventListener(this, listItems[i], i));
        }
        
        // If the list is empty, activate the empty list message.
        if( length < 0 )
            this._emptyListMessageField.show();
    },

    AddEntry : function (text, value)
    {
        if(!value)
            value = text;

        var itemEntry = this._BuildItemEntry(text);
        this._AddToList(itemEntry, text, value);
        ++this._itemCount;

        // Definitely don't need to show this.
        this._emptyListMessageField.hide();
    },

    ClearAll : function()
    {
        var deletionFlags = this._deletedListField.value.split(DeleteList._valueSeparator);
        var itemList = this._displayList.childElements();
        var size = deletionFlags.length;
        for( var i = 0; i < size; i++)
        {
            deletionFlags[i] = true;
            itemList[i].hide();
        }
        this._deletedListField.value = deletionFlags.join(DeleteList._valueSeparator);

        // List is empty.
        this._itemCount = 0;
        this._emptyListMessageField.show();
    },

    // "private" methods.

    _BuildItemEntry : function(text)
    {
        var itemOffset = this._displayList.childElements().length;
        var markerContent = DeleteList.BuildMarkerButton(this._control.identify(), itemOffset, this._deleteIconUrl);
        var itemMarker = new Element("span").update(markerContent);
        var itemEntry = new Element("li").update(itemMarker);
        itemEntry.insert(" " + text);

        Event.observe(itemMarker, "click", this.ClickHandler.bindAsEventListener(this, itemEntry, itemOffset));

        return itemEntry;
    },

    _AddToList : function(itemEntry, text, value)
    {
        // Update the visual elements
        this._displayList.insert(itemEntry);

        // Update hidden fields.
        var separator = "|"
        if(this._valueListField.value == "")
            separator = "";
        this._valueListField.value += separator + value;
        this._textListField.value += separator + text;
        this._deletedListField.value += separator + "false";
    },

    // Event Handler

    ClickHandler : function (event, listItem, offset)
    {
        listItem.hide();
        var deletionFlags = this._deletedListField.value.split(DeleteList._valueSeparator);
        deletionFlags[offset] = true;

        // Check whether the list still has any active items.
        // If all items have been deleted, display the empty message.
        this._itemCount--;
        if(this._itemCount < 1)
            this._emptyListMessageField.show();

        this._deletedListField.value = deletionFlags.join(DeleteList._valueSeparator);
        Event.stop(event);
    },
    
    SelectedTextListInternal : function(delimiter)  {
    
	    var textlist = this._textListField.value.split('|');
	    var deletedlist = this._deletedListField.value.split('|');
	    var output = '';
	    
	    if (delimiter == null) 
	        delimiter = ','; 
	    
	    for (var i=0; i < textlist.length; i++) {
	        if (deletedlist[i] == 'false')  {
	            if (output.length > 0)
	                output += ' ' + delimiter + ' ';
	            output += textlist[i];
	        }
	    }
        return output; 
    }

});
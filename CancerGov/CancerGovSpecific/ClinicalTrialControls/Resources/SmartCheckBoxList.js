SmartCheckBoxList = Class.create();
Object.extend(SmartCheckBoxList,{
    // Static fields and methods

    _valueSeparator : "|",
    _nameSeparator : "_",

    _allOffset : 0,

  Create : function(controlID)
  {
      // The contructor binds the object to the individual
      // check boxes.  Since there are no public methods,
      // we can just let obj go out of scope.
      var obj = new SmartCheckBoxList(controlID);
  }

});

Object.extend(SmartCheckBoxList.prototype, {

    // Instance fields and methods.
    _control : false,
    _allCheckBox : false,

    initialize : function(controlID)
    {
        this._control = $(controlID);
        if(this._control){
            // Attach click handlers.
            var listItems = this._control.select("input[type=checkbox]");
            this._allCheckBox = listItems[SmartCheckBoxList._allOffset];
            var length = listItems.length;
            for(var i = 0; i < length; ++i)
            {
                if(listItems[i])
                    listItems[i].observe("click", this.ClickHandler.bindAsEventListener(this, listItems[i], i));
            }
            Object.extend(this._control, {
                SelectedTextList : function(delimiter)  {
                    var listItems = this.select("input[type=checkbox]:checked");
                    var length = listItems.length;
                    var output = "";
                    var item;

	                if (delimiter == null) 
	                    delimiter = ","; 

                    for(var i = 0; i < length; ++i)
                    {
                        if (output.length > 0) {
                            output += " " + delimiter + " ";
                        }
                        item = this.select("label[for=" +  listItems[i].id + "]");
                        output += item[0].innerHTML;
                    }
                    return output; 
                }       
            });
        }
    },

    // Event Handler

    ClickHandler : function (event, listItem, offset)
    {
        // Offset 0 is always the "All" item.  If the "All" item is selected,
        // everything else is cleared.  If anything else is selected, "All" is cleared.

        if(offset == SmartCheckBoxList._allOffset )
        {
            var listItems = this._control.select("input[type=checkbox]:checked");
            var length = listItems.length;

            // NOTE: Deliberately starting at 1.
            for(var i = 1; i < length; ++i)
            {
                if(listItems[i])
                    listItems[i].checked = false;
            }
        }
        else
        {
            if(this._allCheckBox)
                this._allCheckBox.checked = false;
        }

        //Event.stop(event);
    }

});
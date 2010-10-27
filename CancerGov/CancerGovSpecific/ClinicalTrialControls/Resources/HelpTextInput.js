HelpTextInput = Class.create();
Object.extend(HelpTextInput,{
    // Static fields and methods

    _validState : "valid",
    _invalidState : "invalid",

    _valueSeparator : "|",
    _nameSeparator : "_",

  Create : function(controlID, stateID, helpText, helpTextColor)
  {
      // The contructor binds the object to the input field.
      // Since there are no public methods, we can just let obj go out of scope.
      var obj = new HelpTextInput(controlID, stateID, helpText, helpTextColor);
  }

});

Object.extend(HelpTextInput.prototype, {

    // Instance fields and methods.
    _control : false,
    _controlState: false,
    _helpText : false,
    _helpTextColor : false,
    _normalTextColor : false,

    initialize : function(controlID, stateID, helpText, helpTextColor)
    {
        this._control = $(controlID);
        this._controlState = $(stateID);
        this._helpText = helpText;
        this._helpTextColor = helpTextColor;
        this._normalTextColor = this._control.getStyle("color");

        // Initialize the text field.
        if( this._control.value == "" || this._controlState.value ==  HelpTextInput._invalidState )
        {
            this._control.value = helpText;
            this._control.setStyle({color : this._helpTextColor});
            this._controlState.value = HelpTextInput._invalidState;
        }
        else
        {
            this._controlState.value = HelpTextInput._validState;
        }

        this._control.observe("focus", this.FocusHandler.bindAsEventListener(this, this._control));
        this._control.observe("blur", this.BlurHandler.bindAsEventListener(this, this._control));
    },

    // Event Handlers

    // If the control was invalid before we got there, any content
    // must be helper text.  Clear it out. (Else, leave it alone.)

    FocusHandler : function (event)
    {
        if(this._controlState.value == HelpTextInput._invalidState)
        {
            this._control.value = "";
            this._control.setStyle({color : this._normalTextColor});
        }
    },

    // If the control has content as we leave, it must have been
    // newly entered.  Leave it alone.
    BlurHandler : function (event)
    {
        if(this._control.value != "")
        {
            this._controlState.value = HelpTextInput._validState
        }
        else
        {
            this._control.setStyle({color : this._helpTextColor});
            this._control.value = this._helpText;
            this._controlState.value = HelpTextInput._invalidState;
        }
        
        return;
    }

});
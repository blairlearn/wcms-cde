using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCI.Web.CDE
{
    /// <summary>
    /// Display Devices is used for targeting specific devices like mobile devices 
    /// </summary>    
    public enum DisplayDevices
    {
        /// <summary>
        /// This is the standard Display Device for Desktop and laptop browsers
        /// </summary>
        Desktop = 1,
        /// <summary>
        /// This is for Tablet devices like the iPad
        /// </summary>
        Tablet = 2,
        /// <summary>
        /// This is for advanced mobile devices that can do things like AJAX
        /// </summary>
        AdvancedMobile  = 3,
        /// <summary>
        /// This is for basic mobile devices with a browser but can't do things like AJAX
        /// </summary>
        BasicMobile = 4,
    }
}


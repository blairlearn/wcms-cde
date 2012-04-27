using System;
using CancerGov.UI;

//This file Defines interfaces for objects, if it becomes too unweildly, then
//it should be split up

namespace CancerGov.UI
{

    public interface IRenderer
    {
        string Render();
    }

}

namespace CancerGov.UI.PageObjects
{

    public interface IBanner : IRenderer { }

    public interface IFooter : IRenderer { }


}

namespace CancerGov.UI.Search
{

    public interface ISearchControl : IRenderer { }

}
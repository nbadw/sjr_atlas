using System;
using SJRAtlas.Models;
using Castle.MonoRail.Framework;

namespace SJRAtlas.Site
{
    public interface IPresentationViewHandler
    {
        void RenderViewFor(Presentation presentation, IRailsEngineContext context);
    }
}

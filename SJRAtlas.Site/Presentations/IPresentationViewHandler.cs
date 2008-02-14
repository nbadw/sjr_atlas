using System;
using Castle.MonoRail.Framework;
using SJRAtlas.Models.Atlas;

namespace SJRAtlas.Site.Presentations
{
    public interface IPresentationViewHandler
    {
        void RenderViewFor(Presentation presentation, IRailsEngineContext context);
        void RegisterDynamicActions(Controller controller);
    }
}

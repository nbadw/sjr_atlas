using System;
using System.Collections.Generic;
using Castle.MonoRail.Framework;
using SJRAtlas.Models.Atlas;
using SJRAtlas.Site.Controllers;

namespace SJRAtlas.Site.Presentations
{
    public class PresentationViewResolver
    {
        private IDictionary<Type, IPresentationViewHandler> presentationTypesToViewHandlers;

        public PresentationViewResolver(IDictionary<Type, IPresentationViewHandler> presentationTypesToViewHandlers)
        {
            this.presentationTypesToViewHandlers = presentationTypesToViewHandlers;
        }        

        public IPresentationViewHandler GetHandler(Type type)
        {
            return presentationTypesToViewHandlers[type];
        }

        public void RegisterDynamicActions(Controller controller)
        {
            foreach (IPresentationViewHandler handler in presentationTypesToViewHandlers.Values)
            {
                handler.RegisterDynamicActions(controller);
            }
        }
    }
}

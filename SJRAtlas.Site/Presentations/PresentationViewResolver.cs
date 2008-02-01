using System;
using System.Collections.Generic;
using Castle.MonoRail.Framework;
using SJRAtlas.Models.Atlas;

namespace SJRAtlas.Site.Presentations
{
    public class PresentationViewResolver
    {        
        public PresentationViewResolver(IDictionary<Type, IPresentationViewHandler> presentationTypesToViewHandlers)
        {
            this.presentationTypesToViewHandlers = presentationTypesToViewHandlers;
        }

        private IDictionary<Type, IPresentationViewHandler> presentationTypesToViewHandlers;

        public IPresentationViewHandler GetHandler(Type type)
        {
            return presentationTypesToViewHandlers[type];
        }
    }
}

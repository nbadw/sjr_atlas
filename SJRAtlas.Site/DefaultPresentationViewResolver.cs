using System;
using System.Collections.Generic;
using SJRAtlas.Models;
using Castle.MonoRail.Framework;

namespace SJRAtlas.Site
{
    public class DefaultPresentationViewHandler : IPresentationViewHandler
    {        
        public DefaultPresentationViewHandler(IDictionary<Type, string> presentationsToViews)
        {
            this.presentationsToViews = presentationsToViews;
        }

        private IDictionary<Type, string> presentationsToViews;	

        #region IPresentationViewHandler Members

        public void RenderViewFor(Presentation presentation, IRailsEngineContext context)
        {
            string view = presentationsToViews[presentation.GetType()];
            context.CurrentController.RenderSharedView(view);
        }

        #endregion
    }
}

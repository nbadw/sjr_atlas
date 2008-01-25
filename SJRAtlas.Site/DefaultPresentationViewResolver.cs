using System;
using System.Collections.Generic;
using SJRAtlas.Models;

namespace SJRAtlas.Site
{
    public class DefaultPresentationViewResolver : IPresentationViewResolver
    {        
        public DefaultPresentationViewResolver(IDictionary<Type, string> presentationsToViews)
        {
            this.presentationsToViews = presentationsToViews;
        }

        private IDictionary<Type, string> presentationsToViews;	

        #region IPresentationViewResolver Members

        public string GetViewFor(Presentation presentation)
        {
            return presentationsToViews[presentation.GetType()];
        }

        #endregion
    }
}

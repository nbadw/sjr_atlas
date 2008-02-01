using System;
using System.Collections.Generic;
using Castle.Core.Logging;
using Castle.MonoRail.Framework;
using SJRAtlas.Models;
using NHibernate.Expression;
using SJRAtlas.Models.Atlas;
using SJRAtlas.Site.Presentations;

namespace SJRAtlas.Site.Controllers
{
    public class PresentationController : BaseController
    {        
        public PresentationController(PresentationViewResolver presentationViewResolver)
        {
            this.presentationViewResolver = presentationViewResolver; 
        }

        private PresentationViewResolver presentationViewResolver;

        public PresentationViewResolver PresentationViewResolver
        {
            get { return presentationViewResolver; }
        }

        public void View(int id, string drainageCode, int waterbodyId)
        {
            Presentation presentation = AtlasMediator.Find<Presentation>(id);            
            // what if presentation == null ???

            IPresentationViewHandler viewHandler = PresentationViewResolver.GetHandler(presentation.GetType());
            if (viewHandler == null)
                throw new Exception(String.Format("No implementation of IPresentationViewHandler has been registered to handle presentations of type {0}", presentation.GetType()));

            viewHandler.RenderViewFor(presentation, Context);
        }
    }
}

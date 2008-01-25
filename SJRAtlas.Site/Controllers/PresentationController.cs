using System;
using System.Collections.Generic;
using Castle.Core.Logging;
using Castle.MonoRail.Framework;
using SJRAtlas.Models;
using NHibernate.Expression;

namespace SJRAtlas.Site.Controllers
{
    public class PresentationController : BaseController
    {        
        public PresentationController(IPresentationViewResolver presentationViewResolver)
        {
            this.presentationViewResolver = presentationViewResolver;   
        }

        private IPresentationViewResolver presentationViewResolver;

        public IPresentationViewResolver PresentationViewResolver
        {
            get { return presentationViewResolver; }
            set { presentationViewResolver = value; }
        }
	

        public void View(int id)
        {
            Presentation presentation = AtlasMediator.Find<Presentation>(id);
            RenderSharedView(PresentationViewResolver.GetViewFor(presentation));
        }
    }
}

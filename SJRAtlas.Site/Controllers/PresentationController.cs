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
        public PresentationController()
        {
             
        }

        private IPresentationViewHandler presentationViewHandler;

        public IPresentationViewHandler PresentationViewHandler
        {
            get { return presentationViewHandler; }
            set { presentationViewHandler = value; }
        }	

        public void View(int id)
        {
            Presentation presentation = AtlasMediator.Find<Presentation>(id);            
            PresentationViewHandler.RenderViewFor(presentation, Context);
        }
    }
}

using System;
using Castle.Core.Logging;
using Castle.MonoRail.Framework;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using SJRAtlas.Site.Helpers;
using SJRAtlas.Models.Atlas;
using SJRAtlas.Models;
using SJRAtlas.Site.Filters;

namespace SJRAtlas.Site.Controllers
{
    [Layout("sjratlas")]
    [Rescue("friendlyerror")]
    [Helper(typeof(AtlasHelper), "Atlas")]
    [Filter(ExecuteEnum.AfterAction, typeof(EscapeToUnicodeFilter))]
    public class BaseController : SmartDispatcherController
    {
        public BaseController()
            : this(new AtlasMediator())
        {
        }

        public BaseController(AtlasMediator atlasMediator)
        {
            if (atlasMediator == null)
                throw new ArgumentNullException("atlasMediator");

            this.atlasMediator = atlasMediator;
        }

        private AtlasMediator atlasMediator;

        public AtlasMediator AtlasMediator
        {
            get { return atlasMediator; }
            set { atlasMediator = value; }
        }

        protected IList<T> GetPublicationsByType<T>(IList<Publication> publications) where T : Publication
        {
            IList<T> publicationsByType = new List<T>();
            foreach (Publication publication in publications)
            {
                if (publication is T)
                    publicationsByType.Add((T)publication);
            }
            return publicationsByType;
        }
    }
}

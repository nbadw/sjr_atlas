using System;
using Castle.MonoRail.Framework;
using SJRAtlas.Models.Atlas;

namespace SJRAtlas.Site.Components
{
    [ViewComponentDetails("publication")]
    public class PublicationComponent : ViewComponent
    {
        private Publication publication;

        [ViewComponentParam(Required = true)]
        public Publication Publication
        {
            get { return publication; }
            set { publication = value; }
        }

        public override void Initialize()
        {
            if (publication == null)
            {
                throw new ViewComponentException("The PublicationComponent requires a view component " +
                    "parameter named 'Publication' which should contain a 'Publication' instance");
            }
            base.Initialize();
        }

        public override void Render()
        {
            PropertyBag["publication"] = Publication;
            PropertyBag["title"] = Publication.Title;
            PropertyBag["abstract"] = Publication.Abstract;
            PropertyBag["origin"] = Publication.Origin;
            PropertyBag["url_for_publication"] = "#";
            RenderSharedView("publication/publication");
        }
    }
}

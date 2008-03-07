using System;
using Castle.ActiveRecord;

namespace SJRAtlas.Models.Atlas
{
    public abstract class PublicationPresentation : Presentation
    {
        private Publication publication;

        [BelongsTo("publication_id")]
        public Publication Publication
        {
            get { return publication; }
            set { publication = value; }
        }
    }
}

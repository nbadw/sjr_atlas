using System;
using Castle.MonoRail.Framework;
using System.Collections;
using System.Collections.Generic;
using SJRAtlas.Models.Atlas;

namespace SJRAtlas.Site.Controllers
{
    public class PublicationController : BaseController
    {
        public void View(int id)
        {
            Publication publication = AtlasMediator.Find<Publication>(id);
            Uri resource = new Uri(publication.Uri);
            if (resource.IsFile)
            {
                CancelLayout();
                CancelView();
                Response.ContentType = !String.IsNullOrEmpty(publication.MimeType) ?
                    publication.MimeType : "application/octet-stream";
                Response.WriteFile(resource.LocalPath);
            }
            else
            {
                Redirect(publication.Uri);
            }
        }
    }
}

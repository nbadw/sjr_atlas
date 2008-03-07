using System;
using SJRAtlas.Models.Atlas;
using System.Reflection;
using Castle.MonoRail.Framework;

namespace SJRAtlas.Site.Presentations
{
    public class PublicationPresentationViewHandler : IPresentationViewHandler
    {
        #region IPresentationViewHandler Members

        public void RegisterDynamicActions(Controller controller)
        {

        }

        public void RenderViewFor(Presentation presentation, IRailsEngineContext context)
        {
            PublicationPresentation publicationPresentation = presentation as SummaryReportPresentation;
            Publication publication = publicationPresentation.Publication;
            Controller controller = context.CurrentController;

            Uri resource = new Uri(publication.Uri);
            if (resource.IsFile)
            {
                controller.CancelLayout();
                controller.CancelView();
                controller.Response.ContentType = !String.IsNullOrEmpty(publication.MimeType) ? 
                    publication.MimeType : "application/octet-stream";
                controller.Response.WriteFile(resource.LocalPath);
            }
            else
            {
                controller.Redirect(publication.Uri);
            }
        }

        #endregion
    }
    
}

using System;
using SJRAtlas.Models.Atlas;
using System.Xml;
using System.Xml.Xsl;
using System.Reflection;
using System.IO;

namespace SJRAtlas.Site.Controllers
{
    public class MetadataController : BaseController
    {
        public void View(int owner, string type)
        {
            Metadata metadata = Metadata.FindByOwner(owner, type);
            if (metadata == null)
            {
                RenderSharedView("shared/404");
                return;
            }

            Uri location = new Uri(metadata.Uri);
            if (location.IsFile && location.AbsolutePath.EndsWith(".xml"))
            {
                CancelLayout();
                CancelView();
                string htmlFile = location.LocalPath.Replace(".xml", ".html");
                if(File.Exists(htmlFile))
                {
                    Response.WriteFile(htmlFile);
                }
                else
                {
                    Context.Response.ContentType = "text/xml";
                    RenderText(metadata.Content);
                }
            }
            else
            {
                Redirect(metadata.Uri);
            }          
        }
    }
}

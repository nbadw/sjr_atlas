using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SJRAtlas.Models.Atlas;

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
            }
            else
            {
                Context.Response.ContentType = "text/xml";
                RenderText(metadata.Content);
            }
        }
    }
}

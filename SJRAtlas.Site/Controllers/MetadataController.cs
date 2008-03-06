using System;
using SJRAtlas.Models.Atlas;
using System.Xml;
using System.Xml.Xsl;
using System.Reflection;

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
            if (location.IsFile)
            {
                //XmlDocument xml = new XmlDocument();
                //xml.LoadXml(metadata.Content);
                //using(XmlReader reader = XmlReader.Create(
                //    Assembly.GetExecutingAssembly().GetManifestResourceStream(
                //        "SJRAtlas.Site.FGDC.xsl"))) 
                //{
                //    XslCompiledTransform xsl = new XslCompiledTransform();
                //    xsl.Load(reader);                    
                //    xsl.Transform(xml, null, Context.Response.OutputStream);
                //}  
                Context.Response.ContentType = "text/xml";
                RenderText(metadata.Content);
            }
            else
            {
                Redirect(metadata.Uri);
            }          
        }
    }
}

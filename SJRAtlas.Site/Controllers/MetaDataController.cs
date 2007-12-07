namespace SJRAtlas.Site.Controllers
{
	using System;

	using Castle.MonoRail.Framework;
    using System.Collections.Generic;
    using SJRAtlas.Site.Models;

	[Layout("sjratlas"), Rescue("generalerror")]
	public class MetaDataController : SJRAtlasController
	{
        //public void View(int id, string output)
        //{
        //    View(MetadataLookup.Find(id), output);
        //}

        //public void View(IMetadata metadata, string output)
        //{
        //    if (metadata == null)
        //        throw new ArgumentNullException("metadata", "The requested metadata file could not be found.");

        //    object[] o = metadata.Resources;
        //    //switch (output)
        //    //{
        //    //    case "xml":
        //    //        CancelLayout();
        //    //        CancelView();
        //    //        Response.ContentType = "text/xml";
        //    //        metadata.ToXml(Response.OutputStream);
        //    //        break;
        //    //    case "pdf":
        //    //        CancelLayout();
        //    //        CancelView();
        //    //        Response.ContentType = "application/pdf";
        //    //        metadata.ToPdf(Response.OutputStream);
        //    //        break;
        //    //    default:
        //    //        PropertyBag["metadata"] = metadata;
        //    //        break;
        //    //}
        //    PropertyBag["metadata"] = metadata;
        //}
	}
}

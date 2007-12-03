namespace SJRAtlas.Site.Controllers
{
	using System;

	using Castle.MonoRail.Framework;
    using System.Collections.Generic;
    using SJRAtlas.Site.Models;

	[Layout("sjratlas"), Rescue("generalerror")]
	public class AboutController : SJRAtlasController
	{
		public void Index()
		{
            Logger.Info("About/Index action called");
		}
	}
}

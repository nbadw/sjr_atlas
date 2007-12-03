namespace SJRAtlas.Site.Controllers
{
	using System;

	using Castle.MonoRail.Framework;
    using System.Collections.Generic;
    using SJRAtlas.Site.Models;

	[Layout("sjratlas"), Rescue("generalerror")]
	public class FormsController : SJRAtlasController
	{
		public void Index()
		{

		}
	}
}

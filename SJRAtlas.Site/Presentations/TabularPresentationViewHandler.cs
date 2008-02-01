using System;
using SJRAtlas.Models.Atlas;
using Castle.MonoRail.Framework;
using System.Collections.Generic;

namespace SJRAtlas.Site.Presentations
{
    public class TabularPresentationViewHandler : IPresentationViewHandler
    {
        #region IPresentationViewHandler Members

        public void RenderViewFor(Presentation presentation, IRailsEngineContext context)
        {
            TabularPresentation tabularPresentation = presentation as TabularPresentation;
            Controller controller = context.CurrentController;

            IList<Table> tables = tabularPresentation.GetTables();
            foreach (Table table in tables)
            {
                table.FilterByAgencyCode("UNK");
            }

            controller.PropertyBag["tables"] = tables;
            controller.RenderSharedView("presentation/tables");
        }

        #endregion
    }
}

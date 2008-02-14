using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;

namespace SJRAtlas.Models.Atlas
{
    [ActiveRecord(DiscriminatorValue = "TabularPresentation")]
    public class TabularPresentation : Presentation
    {
        private IList<TabularQuery> tables;

        protected IList<TabularQuery> Tables
        {
            get { return tables; }
            set { tables = value; }
        }

        public virtual IList<TabularQuery> GetQueries()
        {
            return TabularQuery.FindQueriesForTabularPresentation(this.Id);
        }       
    }
}

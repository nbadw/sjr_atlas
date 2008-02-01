using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;

namespace SJRAtlas.Models.Atlas
{
    [ActiveRecord(DiscriminatorValue = "TabularPresentation")]
    public class TabularPresentation : Presentation
    {
        private IList<Table> tables;

        protected IList<Table> Tables
        {
            get { return tables; }
            set { tables = value; }
        }

        public virtual IList<Table> GetTables()
        {
            return Table.FindTablesForTabularPresentation(this.Id);
        }       
    }
}

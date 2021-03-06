using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;

namespace SJRAtlas.Models.Atlas
{
    [ActiveRecord("web_presentations", DiscriminatorColumn = "type", DiscriminatorType = "String", DiscriminatorValue = "Presentation")]
    public abstract class Presentation : ActiveRecordBase<Presentation>
    {
        #region Active Record Properties

        private int id;

        [PrimaryKey(PrimaryKeyType.Increment)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        
        #endregion
    }   
}

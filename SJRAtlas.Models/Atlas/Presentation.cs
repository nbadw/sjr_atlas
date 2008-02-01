using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;

namespace SJRAtlas.Models.Atlas
{
    [ActiveRecord("presentations", DiscriminatorColumn = "type", DiscriminatorType = "String", DiscriminatorValue = "Presentation")]
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
        
        private string title;

        [Property]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private string description;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        
        #endregion


    }
}

using System;
using Castle.ActiveRecord;

namespace SJRAtlas.Models.Atlas
{
    public partial class MapService
    {
        private bool visible = true;

        public bool Visible
        {
            get { return this.visible; }
            set { this.visible = value; }
        }
    }
}

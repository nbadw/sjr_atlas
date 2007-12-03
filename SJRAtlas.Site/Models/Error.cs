using System;

namespace SJRAtlas.Site.Models
{
    public partial class Error
    {
        private Error[] innerExceptions;

        public void LoadChildren()
        {
            innerExceptions = Error.FindAllByProperty("ParentId", this.Id);
        }
    }
}

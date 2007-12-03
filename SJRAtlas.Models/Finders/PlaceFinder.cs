using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace SJRAtlas.Models.Finders
{
    public class PlaceFinder : IEntityFinder<Place>
    {
        #region IEntityFinder<Place> Members

        public Place Find(object id)
        {
            return Place.Find(id);
        }

        public Place[] FindByQuery(string query, params object[] positionalParameters)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Place[] FindByDefaultQuery(object queryParameter)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}

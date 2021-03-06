using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;
using NHibernate.Expression;
using SJRAtlas.Models.DataWarehouse;

namespace SJRAtlas.Models.Atlas
{
    [ActiveRecord("web_closest_watershed_to_place", Mutable = false)]
    public class ClosestWatershedToPlace : ActiveRecordBase<ClosestWatershedToPlace>
    {
        private int id;

        [PrimaryKey("id")]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private Place place;

        [BelongsTo("cgndb_key")]
        public Place Place
        {
            get { return place; }
            set { place = value; }
        }

        private Watershed watershed;

        [BelongsTo("DrainageCd")]
        public Watershed Watershed
        {
            get { return watershed; }
            set { watershed = value; }
        }

        public virtual bool IsWithinBasin()
        {
            if (Watershed == null)
                return false;

            return Watershed.IsWithinBasin();
        }

        public static ClosestWatershedToPlace FindByCgndbKey(string cgndbKey)
        {
            DetachedCriteria criteria = DetachedCriteria.For<ClosestWatershedToPlace>();
            Place place = new Place();
            place.CgndbKey = cgndbKey;
            criteria.Add(Expression.Eq("Place", place));
            return ClosestWatershedToPlace.FindOne(criteria);
        }
    }
}

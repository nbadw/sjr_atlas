using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;

namespace SJRAtlas.Models
{
    [ActiveRecord("closest_watershed_to_place", Mutable = false)]
    public class ClosestWatershedToPlace : ActiveRecordBase<ClosestWatershedToPlace>
    {
        private int id;

        [PrimaryKey("OBJECTID")]
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

        internal static ClosestWatershedToPlace FindByCgndbKey(string CgndbKey)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;

namespace SJRAtlas.Models
{
    public class AtlasMediator
    {
        public virtual T Find<T>(object id) where T : ActiveRecordBase<T>
        {
            return (T)ActiveRecordBase<T>.Find(id);
        }

        public virtual IList<InteractiveMap> FindAllBasinMaps()
        {
            return InteractiveMap.FindAllBasinMaps();
        }

        public virtual bool WaterBodyExistsForCgndbKeyOrAltCgndbKey(string cgndbKey)
        {
            return WaterBody.ExistsForCgndbKeyOrAltCgndbKey(cgndbKey);
        }

        public virtual bool WatershedExistsForCgndbKey(string cgndbKey)
        {
            return Watershed.ExistsForCgndbKey(cgndbKey);
        }

        public virtual InteractiveMap FindInteractiveMapByTitle(string title)
        {
            return InteractiveMap.FindByTitle(title);
        }

        public virtual IList<Place> FindAllPlacesByQuery(string query)
        {
            return Place.FindAllByQuery(query);
        }

        public virtual IList<Watershed> FindAllWatershedsByQuery(string query)
        {
            return Watershed.FindAllByQuery(query);
        }
    }
}

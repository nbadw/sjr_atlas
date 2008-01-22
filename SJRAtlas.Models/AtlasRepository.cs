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

        public virtual IList<T> FindAll<T>() where T : ActiveRecordBase<T>
        {
            return (IList<T>)ActiveRecordBase<T>.FindAll();
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

        public virtual IList<InteractiveMap> FindAllInteractiveMapsByTitles(params string[] titles)
        {
            return InteractiveMap.FindAllByTitles(titles);
        }

        public virtual IList<Place> FindAllPlacesByQuery(string query)
        {
            return Place.FindAllByQuery(query);
        }

        public virtual IList<Watershed> FindAllWatershedsByQuery(string query)
        {
            return Watershed.FindAllByQuery(query);
        }

        public virtual IList<PublishedMap> FindAllPublishedMaps()
        {
            IList<Publication> publications = PublishedMap.FindAll();
            List<PublishedMap> publishedMaps = new List<PublishedMap>(publications.Count);
            foreach (Publication publication in publications)
            {
                publishedMaps.Add((PublishedMap)publication);
            }
            return publishedMaps;
        }
    }
}

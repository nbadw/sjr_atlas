using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;
using SJRAtlas.Models.Atlas;
using SJRAtlas.Models.DataWarehouse;

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
            List<PublishedMap> publishedMaps = new List<PublishedMap>();
            foreach (Publication publication in publications)
            {
                if(publication is PublishedMap)
                    publishedMaps.Add((PublishedMap)publication);
            }
            publishedMaps.Sort(delegate(PublishedMap pm1, PublishedMap pm2) { return pm1.Title.CompareTo(pm2.Title); });
            return publishedMaps;
        }

        public virtual IList<PublishedReport> FindAllPublishedReports()
        {
            IList<Publication> publications = PublishedReport.FindAll();
            List<PublishedReport> publishedReports = new List<PublishedReport>();
            foreach (Publication publication in publications)
            {
                if (publication is PublishedReport)
                    publishedReports.Add((PublishedReport)publication);
            }
            publishedReports.Sort(delegate(PublishedReport pr1, PublishedReport pr2) { return pr1.Title.CompareTo(pr2.Title); });
            return publishedReports;
        }

        public virtual Watershed FindWatershedByCgndbKey(string cgndbKey)
        {
            return Watershed.FindByCgndbKey(cgndbKey);
        }

        public virtual IList<DataSet> FindAllDataSetsByQuery(string q)
        {
            return DataSet.FindAllByQuery(q);
        }

        public virtual IList<InteractiveMap> FindAllInteractiveMapsByQuery(string q)
        {
            return InteractiveMap.FindAllByQuery(q);
        }

        public virtual IList<Publication> FindAllPublicationsByQuery(string q)
        {
            return Publication.FindAllByQuery(q);
        }
    }
}

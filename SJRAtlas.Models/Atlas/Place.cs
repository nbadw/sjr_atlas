using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;
using NHibernate.Expression;

namespace SJRAtlas.Models.Atlas
{
    [ActiveRecord("web_places")]
    public class Place : ActiveRecordBase<Place>
    {
        #region ActiveRecord Properties

        private string cgndbKey;

        [PrimaryKey(PrimaryKeyType.Assigned, "cgndb_key")]
        public string CgndbKey
        {
            get { return cgndbKey; }
            set { cgndbKey = value; }
        }

        private string name;

        [Property("geoname")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string region;

        [Property("region_name")]
        public string Region
        {
            get { return region; }
            set { region = value; }
        }

        private string county;

        [Property("location")]
        public string County
        {
            get { return county; }
            set { county = value; }
        }

        private string nameStatus;

        [Property("status_term")]
        public string NameStatus
        {
            get { return nameStatus; }
            set { nameStatus = value; }
        }

        private string genericTerm;

        [Property("generic_term")]
        public string GenericTerm
        {
            get { return genericTerm; }
            set { genericTerm = value; }
        }

        private string conciseTerm;

        [Property("concise_term")]
        public string ConciseTerm
        {
            get { return conciseTerm; }
            set { conciseTerm = value; }
        }

        private double latitude;

        [Property("latitude")]
        public double Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

        private double longitude;

        [Property("longitude")]
        public double Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }

        private string coordAccM;

        [Property("coord_acc_m")]
        public string CoordAccM
        {
            get { return coordAccM; }
            set { coordAccM = value; }
        }

        private string ntsMap;

        [Property("nts_map")]
        public string NtsMap
        {
            get { return ntsMap; }
            set { ntsMap = value; }
        }

        private string datum;

        [Property("datum")]
        public string Datum
        {
            get { return datum; }
            set { datum = value; }
        }

        private string featureId;

        [Property("feature_id")]
        public string FeatureId
        {
            get { return featureId; }
            set { featureId = value; }
        }

        private string conciseType;

        [Property("concise_type")]
        public string ConciseType
        {
            get { return conciseType; }
            set { conciseType = value; }
        }
        
        #endregion

        private bool closestWatershedToPlaceAlreadyLoaded = false;
        private ClosestWatershedToPlace closestWatershedToPlace;

        public ClosestWatershedToPlace ClosestWatershedToPlace
        {
            get
            {
                if (!closestWatershedToPlaceAlreadyLoaded)
                {
                    closestWatershedToPlace = ClosestWatershedToPlace.FindByCgndbKey(CgndbKey);
                    closestWatershedToPlaceAlreadyLoaded = true;
                }

                return closestWatershedToPlace;
            }
            set
            {
                closestWatershedToPlace = value;
                closestWatershedToPlaceAlreadyLoaded = true;
            }
        }

        private IList<InteractiveMap> interactiveMaps;

        public virtual IList<InteractiveMap> RelatedInteractiveMaps
        {
            get 
            {
                if (interactiveMaps == null)
                {
                    interactiveMaps = IsWithinBasin() ?
                        new List<InteractiveMap>(InteractiveMap.FindAllBasinMaps()) :
                        new List<InteractiveMap>();
                }

                return interactiveMaps; 
            }
        }

        private IList<Publication> publications;

        public virtual IList<Publication> RelatedPublications
        {
            get 
            {
                if (publications == null)
                {
                    publications = Publication.FindAllByQuery(String.Format("%{0}%", Name));
                }

                return publications; 
            }
        }

        private LatLngCoord coordinate;

        public virtual LatLngCoord GetCoordinate()
        {
            if (coordinate == null)
            {
                coordinate = new LatLngCoord(Latitude, Longitude);
            }

            return coordinate;
        }

        public virtual bool IsWithinBasin()
        {
            if (ClosestWatershedToPlace == null)
                return false;

            return ClosestWatershedToPlace.IsWithinBasin();
        }

        public static IList<Place> FindAllByQuery(string query)
        {
            if (query == null)
                throw new ArgumentNullException("query");

            DetachedCriteria criteria = DetachedCriteria.For<Place>();
            criteria.Add(Expression.Like("Name", query.Trim(), MatchMode.Start));
            return Place.FindAll(criteria, new Order[] { Order.Asc("Name") });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;
using SJRAtlas.Models.Finders;

namespace SJRAtlas.Models
{
    [ActiveRecord("places")]
    public class Place : ActiveRecordBase<Place>, IPlace, ICoordinateAware, IEntity
    {
        public Place() : this(new NullAtlasRepository())
        {
        }

        public Place(IAtlasRepository repository)
        {
            if (repository == null)
                throw new ArgumentNullException("repository");

            this.repository = repository;
        }

        private IAtlasRepository repository;

        public IAtlasRepository Repository
        {
            get { return repository; }
            set 
            {
                if (value == null)
                    throw new ArgumentNullException("repository");

                repository = value; 
            }
        }

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

                    closestWatershedToPlace = Repository.GetFinder<ClosestWatershedToPlaceFinder>()
                        .FindByCgndbKey(CgndbKey);

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

        public IList<InteractiveMap> RelatedInteractiveMaps
        {
            get 
            {
                if (interactiveMaps == null)
                {
                    interactiveMaps = FindAllRelatedInteractiveMaps();                    
                }

                return interactiveMaps; 
            }
        }

        protected virtual IList<InteractiveMap> FindAllRelatedInteractiveMaps()
        {
            return InteractiveMap.FindAllByQuery(String.Format("%{0}%", Name));
        }

        protected virtual IList<InteractiveMap> FindAllRelatedInteractiveMaps(string query)
        {
            return InteractiveMap.FindAllByQuery(query);
        }

        private IList<IPublication> publications;

        public IList<IPublication> RelatedPublications
        {
            get 
            {
                if (publications == null)
                {
                    publications = FindAllRelatedPublications();
                }

                return publications; 
            }
        }

        protected virtual IList<IPublication> FindAllRelatedPublications()
        {
            return FindAllRelatedPublications(String.Format("%{0}%", Name));
        }

        protected virtual IList<IPublication> FindAllRelatedPublications(string query)
        {
            return Publication.FindAllByQuery(query);
        }

        public bool IsWithinBasin()
        {
            if (ClosestWatershedToPlace == null)
                return false;

            return ClosestWatershedToPlace.IsWithinBasin();
        }

        #region ICoordinateAware Members

        private LatLngCoord coordinate;

        public virtual LatLngCoord GetCoordinate()
        {
            if (coordinate == null)
            {
                coordinate = new LatLngCoord(Latitude, Longitude);
            }

            return coordinate;
        }

        #endregion

        #region IEntity Members

        public object GetId()
        {
            return CgndbKey;
        }

        #endregion

    }
}

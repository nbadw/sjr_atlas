using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Castle.ActiveRecord;

namespace SJRAtlas.Models
{
    [ActiveRecord("tblDrainageUnit", Mutable = false)]
    public class Watershed : ActiveRecordBase<Watershed>, IPlace, ICoordinateAware
    {
        public Watershed()
        {
        }

        public Watershed(IAtlasRepository repository) : this(repository, null)
        {
        }

        public Watershed(IAtlasRepository repository, IPlace place)
        {
            this.repository = repository;
            this.place = place;
        }

        private IAtlasRepository repository;

        public IAtlasRepository Repository
        {
            get { return repository; }
            set { repository = value; }
        }

        private IPlace place;
        private bool hasPlaceAlreadyBeenLoaded = false;

        public IPlace Place
        {
            get
            {
                // lazy load the place
                if (!hasPlaceAlreadyBeenLoaded && place == null)
                {
                    place = Repository.Find<Place>(CgndbKey);
                    hasPlaceAlreadyBeenLoaded = true;
                }

                return place;
            }
            set { place = value; }
        }

        private string drainageCode;

        [PrimaryKey("DrainageCd", Generator = PrimaryKeyType.Assigned)]
        public string DrainageCode
        {
            get { return drainageCode; }
            set { drainageCode = value; }
        }

        public string FlowsInto
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
        
        public string TributaryOf
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public string DrainsInto
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string cgndbKey;

        public string CgndbKey
        {
            get { return cgndbKey; }
            set { cgndbKey = value; }
        }

        public WaterBody[] WaterBodies
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        public DataSet[] DataSets
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        #region IPlace Members
        
        public string ConciseTerm
        {
            get { return Place.ConciseTerm; }
            set { Place.ConciseTerm = value; }
        }

        public string ConciseType
        {
            get { return Place.ConciseType; }
            set { Place.ConciseType = value; }
        }

        public string CoordAccM
        {
            get { return Place.CoordAccM; }
            set { Place.CoordAccM = value; }
        }

        public string County
        {
            get { return Place.County; }
            set { Place.County = value; }
        }

        public string Datum
        {
            get { return Place.Datum; }
            set { Place.Datum = value; }
        }

        public string FeatureId
        {
            get { return Place.FeatureId; }
            set { Place.FeatureId = value; }
        }

        public string GenericTerm
        {
            get { return Place.GenericTerm; }
            set { Place.GenericTerm = value; }
        }

        public bool IsWithinBasin()
        {
            Regex re = new Regex(@"01-[\d]{2}-[\d]{2}-[\d]{2}-[\d]{2}-[\d]{2}");
            return re.IsMatch(DrainageCode);
        }

        public double Latitude
        {
            get { return Place.Latitude; }
            set { Place.Latitude = value; }
        }

        public double Longitude
        {
            get { return Place.Longitude; }
            set { Place.Longitude = value; }
        }

        public string NameStatus
        {
            get { return Place.NameStatus; }
            set { Place.NameStatus = value; }
        }

        public string NtsMap
        {
            get { return Place.NtsMap; }
            set { Place.NtsMap = value; }
        }

        public string Region
        {
            get { return Place.Region; }
            set { Place.Region = value; }
        }

        public InteractiveMap[] RelatedInteractiveMaps
        {
            get 
            { 
                throw new Exception("The method or operation is not implemented."); 
            }
        }

        public IPublication[] RelatedPublications
        {
            get 
            { 
                throw new Exception("The method or operation is not implemented."); 
            }
        }

        #endregion

        #region ICoordinateAware Members

        public LatLngCoord GetCoordinate()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
